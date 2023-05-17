using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.MSBuild;
using System;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace StaticAnalyzatorForCSharp
{
    internal class TestingStaticAnalyzator
    {
        private static MSBuildWorkspace workspace;

        public static void Start(string path, ListBox listWarnings)
        {
            if (!MSBuildLocator.IsRegistered)
            {
                MSBuildLocator.RegisterDefaults();
            }

            using (workspace = MSBuildWorkspace.Create())
            {
                int countWarningsForProgressBar = 0;

                foreach (var rule in Properties.Settings.Default.PropertyValues)
                {
                    var currentRule = (SettingsPropertyValue)rule;
                    if ((bool)currentRule.PropertyValue)
                        countWarningsForProgressBar++;
                }

                int counterWarnings = 0;
                Project currProject = GetProjectFromSolution(path, workspace);
                foreach (var document in currProject.Documents)
                {
                    var tree = document.GetSyntaxTreeAsync().Result;

                    if (SettingsRules.GetDictionary(SettingsRules.NamesErrors.ifWarningMessage))
                    {
                        var ifStatementNodes = tree.GetRoot()
                                                   .DescendantNodesAndSelf()
                                                   .OfType<IfStatementSyntax>();
                        foreach (var ifStatement in ifStatementNodes)
                        {
                            if (Rules.IfElseRule(ifStatement))
                            {
                                counterWarnings++;
                                int lineNumber = ifStatement.GetLocation()
                                                            .GetLineSpan()
                                                            .StartLinePosition.Line + 1;
                                listWarnings.Invoke(new Action(() => ListboxStringsAdd(listWarnings, counterWarnings, NamesMessage.IfWarningMessage, document.FilePath, lineNumber)));
                            }
                        }

                        if (ifStatementNodes.Count() != 0)
                        {
                            ProgressBarWork.SetProgress += (double)100 / countWarningsForProgressBar;
                        }
                    }

                    if (SettingsRules.GetDictionary(SettingsRules.NamesErrors.isThrowWarningMessage))
                    {
                        var throwStatementNodes = tree.GetRoot()
                                                      .DescendantNodes()
                                                      .OfType<ObjectCreationExpressionSyntax>();
                        Compilation compilation = currProject.GetCompilationAsync().Result;
                        foreach (var throwStatement in throwStatementNodes)
                        {
                            if (Rules.IsMissingThrowOperatorRule(compilation.GetSemanticModel(tree), throwStatement))
                            {
                                counterWarnings++;
                                int lineNumber = throwStatement.GetLocation()
                                                               .GetLineSpan()
                                                               .StartLinePosition.Line + 1;

                                listWarnings.Invoke(new Action(() => ListboxStringsAdd(listWarnings, counterWarnings, NamesMessage.IsThrowWarningMessage, document.FilePath, lineNumber)));
                            }
                        }

                        if (throwStatementNodes.Count() != 0)
                        {
                            ProgressBarWork.SetProgress += (double)100 / countWarningsForProgressBar;
                        }
                    }

                    if (SettingsRules.GetDictionary(SettingsRules.NamesErrors.isUpperSymbolInMethodMessage))
                    {
                        var methodStatementNodes = tree.GetRoot()
                                                  .DescendantNodesAndSelf()
                                                  .OfType<MethodDeclarationSyntax>();
                        foreach (var methodDeclaration in methodStatementNodes)
                        {
                            if (Rules.MethodUpperSymbolRule(methodDeclaration, out var methodName))
                            {
                                counterWarnings++;
                                int lineNumber = methodDeclaration.GetLocation()
                                                                  .GetLineSpan()
                                                                  .StartLinePosition.Line + 1;

                                listWarnings.Invoke(new Action(() => ListboxStringsAdd(listWarnings, counterWarnings, NamesMessage.IsUpperSymbolInMethodMessage, methodName, document.FilePath, lineNumber)));
                            }
                        }

                        if (methodStatementNodes.Count() != 0)
                        {
                            ProgressBarWork.SetProgress += (double)100 / countWarningsForProgressBar;
                        }
                    }

                    if (SettingsRules.GetDictionary(SettingsRules.NamesErrors.isLowerSymbolInVariableMessage))
                    {
                        var variableStatementsNodes = tree.GetRoot()
                                                         .DescendantNodesAndSelf()
                                                         .OfType<VariableDeclarationSyntax>();
                        foreach (var variableDeclaration in variableStatementsNodes)
                        {
                            if (Rules.VariableLowerSymbolRule(variableDeclaration, out var methodName))
                            {
                                counterWarnings++;
                                int lineNumber = variableDeclaration.GetLocation()
                                .GetLineSpan()
                                .StartLinePosition.Line + 1;

                                listWarnings.Invoke(new Action(() => ListboxStringsAdd(listWarnings, counterWarnings, NamesMessage.IsLowerSymbolInVariableMessage, methodName, document.FilePath, lineNumber)));
                            }
                        }

                        if (variableStatementsNodes.Count() != 0)
                        {
                            ProgressBarWork.SetProgress += (double)100 / countWarningsForProgressBar;
                        }
                    }

                    if (SettingsRules.GetDictionary(SettingsRules.NamesErrors.correctNameVariableInFor))
                    {
                        var formatNodes = tree.GetRoot()
                                                    .DescendantNodesAndSelf()
                                                    .OfType<ForStatementSyntax>();
                        foreach (var formatNode in formatNodes)
                        {
                            if (Rules.CorrectNameFor(formatNode))
                            {
                                counterWarnings++;
                                int lineNumber = formatNode.GetLocation()
                                .GetLineSpan()
                                .StartLinePosition.Line + 1;

                                listWarnings.Invoke(new Action(() => ListboxStringsAdd(listWarnings, counterWarnings, NamesMessage.СorrectNameVariableInForMessage, document.FilePath, lineNumber)));
                            }
                        }

                        if (formatNodes.Count() != 0)
                        {
                            ProgressBarWork.SetProgress += (double)100 / countWarningsForProgressBar;
                        }
                    }

                    if (SettingsRules.GetDictionary(SettingsRules.NamesErrors.ifStateEquals))
                    {
                        var binaryStatementNodes = tree.GetRoot()
                                                   .DescendantNodesAndSelf()
                                                   .OfType<BinaryExpressionSyntax>();
                        foreach (var binaryStatement in binaryStatementNodes)
                        {
                            if (Rules.IfStateEquals(binaryStatement))
                            {
                                counterWarnings++;
                                int lineNumber = binaryStatement.GetLocation()
                                                            .GetLineSpan()
                                                            .StartLinePosition.Line + 1;
                                listWarnings.Invoke(new Action(() => ListboxStringsAdd(listWarnings, counterWarnings, NamesMessage.IfStateEqualsMessage, document.FilePath, lineNumber)));
                            }
                        }

                        if (binaryStatementNodes.Count() != 0)
                        {
                            ProgressBarWork.SetProgress += (double)100 / countWarningsForProgressBar;
                        }
                    }

                    if (SettingsRules.GetDictionary(SettingsRules.NamesErrors.ifStateImpossible))
                    {
                        var binaryStatementNodes = tree.GetRoot()
                                                   .DescendantNodesAndSelf()
                                                   .OfType<BinaryExpressionSyntax>();
                        foreach (var binaryStatement in binaryStatementNodes)
                        {
                            if (Rules.IfStateImpossible(binaryStatement))
                            {
                                counterWarnings++;
                                int lineNumber = binaryStatement.GetLocation()
                                                            .GetLineSpan()
                                                            .StartLinePosition.Line + 1;
                                listWarnings.Invoke(new Action(() => ListboxStringsAdd(listWarnings, counterWarnings, NamesMessage.IsImpossibleIfMessage, document.FilePath, lineNumber)));
                            }
                        }

                        if (binaryStatementNodes.Count() != 0)
                        {
                            ProgressBarWork.SetProgress += (double)100 / countWarningsForProgressBar;
                        }
                    }
                }
            }
        }

        private static void ListboxStringsAdd(ListBox listWarnings, int counter, string ruleMessage, string path, int lineNumber)
        {
            listWarnings.Items.Add(String.Format(counter + ". " + ruleMessage, path, lineNumber));
        }

        private static void ListboxStringsAdd(ListBox listWarnings, int counter, string ruleMessage, string methodName, string path, int lineNumber)
        {
            listWarnings.Items.Add(String.Format(counter + ". " + ruleMessage, methodName, path, lineNumber));
        }

        private static Project GetProjectFromSolution(String solutionPath,
                                      MSBuildWorkspace workspace)
        {
            Solution currSolution = workspace.OpenSolutionAsync(solutionPath)
                                             .Result;
            return currSolution.Projects.Single();
        }
    }
}
