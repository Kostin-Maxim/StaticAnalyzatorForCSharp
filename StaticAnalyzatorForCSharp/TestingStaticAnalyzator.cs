using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.MSBuild;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StaticAnalyzatorForCSharp
{
    internal class TestingStaticAnalyzator
    {
        public static void Start(string path, ref ListBox listWarnings)
        {
            StringBuilder warnings = new StringBuilder();

            const string ifWarningMessage =
                "if и else приводят к одному результату! Файл: {0}, строка: {1}";
            const string isThrowWarningMessage =
                "Cоздаётся экземпляр класса, унаследованного от 'System.Exception', но при этом никак не используется! Файл: {0}, строка: {1}";

            MSBuildLocator.RegisterDefaults();
            using (var workspace = MSBuildWorkspace.Create())
            {
                int counterWarnings = 0;
                Project currProject = GetProjectFromSolution(path, workspace);
                foreach (var document in currProject.Documents)
                {
                    var tree = document.GetSyntaxTreeAsync().Result;
                    var ifStatementNodes = tree.GetRoot()
                                               .DescendantNodesAndSelf()
                                               .OfType<IfStatementSyntax>();
                    var throwStatementNodes = tree.GetRoot()
                                               .DescendantNodes()
                                               .OfType<ObjectCreationExpressionSyntax>();

                    foreach (var ifStatement in ifStatementNodes)
                    {
                        if (Rules.IfElseRule(ifStatement))
                        {
                            counterWarnings++;
                            int lineNumber = ifStatement.GetLocation()
                                                        .GetLineSpan()
                                                        .StartLinePosition.Line + 1;

                            listWarnings.Items.Add(String.Format(counterWarnings + ". " + ifWarningMessage,
                                                              document.FilePath,
                                                              lineNumber));
                        }
                    }

                    Compilation compilation = currProject.GetCompilationAsync().Result;
                    foreach (var throwStatement in throwStatementNodes)
                    {
                        if (Rules.IsMissingThrowOperatorRule(compilation.GetSemanticModel(tree), throwStatement))
                        {

                            counterWarnings++;
                            int lineNumber = throwStatement.GetLocation()
                            .GetLineSpan()
                            .StartLinePosition.Line + 1;

                            listWarnings.Items.Add(String.Format(counterWarnings + ". " + isThrowWarningMessage,
                                                              document.FilePath,
                                                              lineNumber));
                        }
                    }
                    
                    
                }

            }

        }
        
        static Project GetProjectFromSolution(String solutionPath,
                                      MSBuildWorkspace workspace)
        {
            Solution currSolution = workspace.OpenSolutionAsync(solutionPath)
                                             .Result;
            return currSolution.Projects.Single();
        }
    }
}
