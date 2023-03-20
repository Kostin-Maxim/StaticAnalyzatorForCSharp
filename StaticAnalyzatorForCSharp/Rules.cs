using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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


        private static readonly string ExceptionTypeName = typeof(Exception).FullName;
        internal static bool IsMissingThrowOperatorRule(SemanticModel model,
                               ObjectCreationExpressionSyntax node)
        {
            //if (!IsExceptionType(model, node))
            //    return false;

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

        internal static bool IsExceptionType(SemanticModel model,
                        SyntaxNode node)
        {
            ITypeSymbol nodeType = model.GetTypeInfo(node).Type;

            while (nodeType != null && !(Equals(nodeType.Name,
                                                ExceptionTypeName)))
                nodeType = nodeType.BaseType;

            return Equals(nodeType?.Name,
                          ExceptionTypeName);

        }
    }
}
