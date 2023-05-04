using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.MSBuild;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Reflection.Metadata;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Linq;

namespace StaticAnalyzatorForCSharp
{
    internal class TestingStaticAnalyzator
    {
        private static MSBuildWorkspace workspace;
        private static double progressBar;

        public static void Start(string path, ListBox listWarnings)
        {
            //workspace = null;

            const string ifWarningMessage =
                "if и else приводят к одному результату! Файл: {0}, строка: {1}";
            const string isThrowWarningMessage =
                "Cоздаётся экземпляр класса, унаследованного от 'System.Exception', но при этом никак не используется! Файл: {0}, строка: {1}";
            const string isUpperSymbolInMethodMessage =
                "Метод: '{0}' объявлена с маленькой буквы. Файл: {1}, строка: {2}";
            const string isLowerSymbolInVariableMessage =
                "Переменная: '{0}' объявлена с заглавной буквы. Файл: {1}, строка: {2}";

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
                progressBar = 15;
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
                                listWarnings.Invoke(new Action(() => ListboxStringsAdd(listWarnings, counterWarnings, ifWarningMessage, document.FilePath, lineNumber)));
                            }
                        }

                        if (ifStatementNodes.Count() != 0)
                            progressBar = (double)100 / countWarningsForProgressBar;
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

                                listWarnings.Invoke(new Action(() => ListboxStringsAdd(listWarnings, counterWarnings, isThrowWarningMessage, document.FilePath, lineNumber)));
                            }
                        }
                        if (throwStatementNodes.Count() != 0)
                            progressBar += (double)100 / countWarningsForProgressBar;
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

                                listWarnings.Invoke(new Action(() => ListboxStringsAdd(listWarnings, counterWarnings, isUpperSymbolInMethodMessage, methodName, document.FilePath, lineNumber)));
                            }
                        }

                        if (methodStatementNodes.Count() != 0)
                            progressBar += (double)100 / countWarningsForProgressBar;
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

                                listWarnings.Invoke(new Action(() => ListboxStringsAdd(listWarnings, counterWarnings, isLowerSymbolInVariableMessage, methodName, document.FilePath, lineNumber)));
                            }
                        }
                        if (variableStatementsNodes.Count() != 0)
                            progressBar += (double)100 / countWarningsForProgressBar;
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

        internal static void StartProgressBar(ProgressBar progress)
        {
            while (progressBar <= 100)
            {
                progress.Invoke(new Action(() => progress.Value = (int)progressBar));

                if (progressBar == 100)
                    break;
            }
        }
    }
}
