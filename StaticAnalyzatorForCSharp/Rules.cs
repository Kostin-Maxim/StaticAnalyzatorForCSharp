using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Globalization;
using System.Linq;

namespace StaticAnalyzatorForCSharp
{
    internal class Rules
    {
        internal static bool IfElseRule(IfStatementSyntax ifStatement)
        {
            if (ifStatement?.Else == null)
                return false;

            StatementSyntax thenBody = ifStatement.Statement;
            StatementSyntax elseBody = ifStatement.Else.Statement;

            return SyntaxFactory.AreEquivalent(thenBody, elseBody);
        }

        internal static bool IsMissingThrowOperatorRule(SemanticModel model, ObjectCreationExpressionSyntax node)
        {
            if (IsReferenceUsed(model, node.Parent))
                return false;

            return true;
        }

        internal static bool IsReferenceUsed(SemanticModel model, SyntaxNode parentNode)
        {
            if (parentNode.IsKind(SyntaxKind.ExpressionStatement))
                return false;

            if (parentNode is LambdaExpressionSyntax)
                return (model.GetDeclaredSymbol(parentNode) as IMethodSymbol)
                         ?.ReturnsVoid == false;

            return true;
        }

        internal static bool MethodUpperSymbolRule(MethodDeclarationSyntax methodDeclaration, out string methodName)
        {
            methodName = methodDeclaration.Identifier.Text;
            return methodName.Length != 0 && !char.IsUpper(methodName[0]);
        }

        internal static bool VariableLowerSymbolRule(VariableDeclarationSyntax variableDeclaration, out string variableName)
        {
            variableName = variableDeclaration.Variables.ToString();
            return variableName.Length != 0 && !char.IsLower(variableName[0]);
        }

        internal static bool CorrectNameFor(ForStatementSyntax interpolationFormat)
        {
            var operatorChange = interpolationFormat.Incrementors.ToString().First();
            var operatorVariable = interpolationFormat.Declaration.Variables.ToString().First();
            return operatorChange == operatorVariable;
        }

        internal static bool IfStateEquals(BinaryExpressionSyntax ifStatement)
        {
            return SyntaxFactory.AreEquivalent(ifStatement.Left, ifStatement.Right);
        }

        internal static bool IfStateImpossible(BinaryExpressionSyntax binaryExpression)
        {
            string leftStatement = binaryExpression.Left.ToString();
            string rightStatement = binaryExpression.Right.ToString();
            if (leftStatement.IndexOfAny(new char[] { '>', '<' }) == -1 || rightStatement.IndexOfAny(new char[] { '>', '<' }) == -1)
                return true;

            if (leftStatement.Substring(0, leftStatement.IndexOfAny(new char[] { '>', '<' })).Replace(" ", "") == rightStatement.Substring(0, rightStatement.IndexOfAny(new char[] { '>', '<' })).Replace(" ", ""))
            {
                string leftNumericString = CuteNumericForString(leftStatement.Substring(leftStatement.IndexOfAny(new char[] { '>', '<' }) + 1));
                string rightNumericString = CuteNumericForString(rightStatement.Substring(rightStatement.IndexOfAny(new char[] { '>', '<' }) + 1));

                double maxValue, minValue;
                CultureInfo ciEnUs = new CultureInfo("en-us");

                maxValue = double.Parse(leftNumericString, ciEnUs) >= double.Parse(rightNumericString, ciEnUs) ? double.Parse(leftNumericString, ciEnUs) : double.Parse(leftNumericString, ciEnUs);
                minValue = double.Parse(rightNumericString, ciEnUs) >= double.Parse(leftNumericString, ciEnUs) ? double.Parse(rightNumericString, ciEnUs) : double.Parse(leftNumericString, ciEnUs);
            }

           


            

           
            return true;
        }

        private static string CuteNumericForString(string stringNum)
        {
            string localString = "";

            foreach (var word in stringNum)
            {
                if (word > 44 && word < 57)
                {
                    localString += word;
                }
            }

            return localString;
        }
    }
}
