using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;

namespace StaticAnalyzatorForCSharp
{
    internal class Rules
    {
        private CultureInfo ciEnUs = new CultureInfo("en-us");
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

            // Переменные полученные из условия
            double leftNumeric = 0;
            double rightNumeric = 0;

            // Переменны для запоминание знака < >
            bool isLeftStatementGreater = false;
            bool isRightStatementGreater = false;

            // Переменны для определения местонахождения числа
            bool isRightStatementLeft = false;
            bool isRightStatementRight = false;

            const string greater = "GreaterThanExpression";
            const string less = "LessThanExpression";

            if (binaryExpression.Left.Kind().ToString() == greater)
            {
                AnalysisBorderGreater(binaryExpression.Left.ToString(), ref leftNumeric, ref isRightStatementLeft);
                isLeftStatementGreater = true;
            }
            else if (binaryExpression.Left.Kind().ToString() == less)
            {
                AnalysisBorderGreater(binaryExpression.Left.ToString(), ref leftNumeric, ref isRightStatementLeft);
            }

            if (binaryExpression.Right.Kind().ToString() == greater)
            {
                AnalysisBorderLess(binaryExpression.Right.ToString(), ref rightNumeric, ref isRightStatementRight);
                isRightStatementGreater = true;
            }
            else if (binaryExpression.Right.Kind().ToString() == less)
            {
                AnalysisBorderLess(binaryExpression.Right.ToString(), ref rightNumeric, ref isRightStatementRight);
            }


            if (isLeftStatementGreater == isRightStatementGreater)
            {
                if (isRightStatementLeft == isRightStatementRight)
                {
                    return true;
                }
                else
                {
                    if (leftNumeric >= rightNumeric)
                    {
                        if (isRightStatementLeft)
                        {
                            return true;
                        }
                        else 
                        { 
                            return false; 
                        }
                    }
                    else if (rightNumeric >= leftNumeric)
                    {
                        if (isRightStatementRight)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                }
            }
            else 
            {
                if (isRightStatementLeft != isRightStatementRight)
                {
                    return false;
                }
                else
                {
                   // if (leftNumeric >= rightNumeric)
                   // {
                        if (isRightStatementLeft)
                        {
                            if (isLeftStatementGreater)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else 
                        {
                            if (isLeftStatementGreater)
                            {
                                return false;
                            }
                            else
                            {
                                return true;
                            }
                        }
                   // }
                  //  else if (rightNumeric >= leftNumeric)
                 /* //  {
                        if (isRightStatementRight)
                        {
                            if (isRightStatementGreater)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            if (isRightStatementGreater)
                            {
                                return false;
                            }
                            else
                            {
                                return true;
                            }
                        }  */  
                    }
                }
            }
        }

        private static void AnalysisBorderGreater(string statement, ref double numeric, ref bool isRight)
        {
            try
            {
                numeric = double.Parse(statement.Substring(0, statement.IndexOf('>')).Replace(" ", ""), CultureInfo.InvariantCulture);
            }
            catch
            {
                numeric = double.Parse(statement.Substring(statement.IndexOf('>') + 1).Replace(" ", ""), CultureInfo.InvariantCulture);
                isRight = true;
            }
        }

        private static void AnalysisBorderLess(string statement, ref double numeric, ref bool isRight)
        {
            try
            {
                numeric = double.Parse(statement.Substring(0, statement.IndexOf('<')).Replace(" ", ""), CultureInfo.InvariantCulture);
            }
            catch
            {
                numeric = double.Parse(statement.Substring(statement.IndexOf('<') + 1).Replace(" ", ""), CultureInfo.InvariantCulture);
                isRight = true;
            }
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
