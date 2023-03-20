using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.MSBuild;
using System;
using System.Collections.Generic;
using System.Linq;
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

            const string warningMessageFormat =
              "if и else приводят к одному результату! Файл: {0}, строка: {1}";

            MSBuildLocator.RegisterDefaults();
            using (var workspace = MSBuildWorkspace.Create())
            {
                Project currProject = GetProjectFromSolution(path, workspace);
                foreach (var document in currProject.Documents)
                {
                    var tree = document.GetSyntaxTreeAsync().Result;
                    var ifStatementNodes = tree.GetRoot()
                                               .DescendantNodesAndSelf()
                                               .OfType<IfStatementSyntax>();

                    foreach (var ifStatement in ifStatementNodes)
                    {
                        int counter = 0;
                        if (Rules.IfElseRule(ifStatement))
                        {
                            counter++;
                            int lineNumber = ifStatement.GetLocation()
                                                        .GetLineSpan()
                                                        .StartLinePosition.Line + 1;

                            listWarnings.Items.Add(String.Format(counter.ToString() + ". " + warningMessageFormat,
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
