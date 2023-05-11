using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

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

        internal static bool IsMissingThrowOperatorRule(SemanticModel model,
                               ObjectCreationExpressionSyntax node)
        {
            if (IsReferenceUsed(model, node.Parent))
                return false;

            return true;
        }

        internal static bool IsReferenceUsed(SemanticModel model,
                     SyntaxNode parentNode)
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

        /*internal static bool LogicEqualsFormat(InterpolationFormatClauseSyntax interpolationFormat)
        {
            string name = "";
            if (true)
                name = "a";
                name += "b";
            interpolationFormat.ChildNodes(brac)
            return false;
        }*/

        internal static bool IfStateEquals(BinaryExpressionSyntax ifStatement)
        {
            return SyntaxFactory.AreEquivalent(ifStatement.Left, ifStatement.Right);
        }
    }
}
