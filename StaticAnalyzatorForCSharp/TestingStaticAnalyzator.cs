using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.MSBuild;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticAnalyzatorForCSharp
{
    internal class TestingStaticAnalyzator
    {
        public static string Start(string path)
        {
            string logPath = "";

            StringBuilder warnings = new StringBuilder();

            const string warningMessageFormat =
              "'if' with equal 'then' and 'else' blocks is found in file {0} at line {1}";

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
                    var correctA = tree.GetRoot()
                                       .DescendantNodesAndSelf()
                                       .OfType<VariableDeclarationSyntax>();


                    foreach (var ifStatement in ifStatementNodes)
                    {
                        if (ApplyRule(ifStatement))
                        {
                            int lineNumber = ifStatement.GetLocation()
                                                        .GetLineSpan()
                                                        .StartLinePosition.Line + 1;

                            warnings.AppendLine(String.Format(warningMessageFormat,
                                                              document.FilePath,
                                                              lineNumber));
                        }
                    }
                }

                if (warnings.Capacity != 0)
                {
                    return "1";
                }
                else
                {
                    return "2";
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

        static bool ApplyRule(IfStatementSyntax ifStatement)
        {
            if (ifStatement?.Else == null)
                return false;

            StatementSyntax thenBody = ifStatement.Statement;
            StatementSyntax elseBody = ifStatement.Else.Statement;

            return SyntaxFactory.AreEquivalent(thenBody, elseBody);
        }
    }
}
