using System;
using System.Collections.Generic;
using System.Linq;
using Irony.Parsing;

namespace XamlStyler.Core.MarkupExtensions.Parser
{
    public class MarkupExtension : Value
    {
        public string TypeName { get; }
        public Argument[] Arguments { get; }

        public MarkupExtension(string typeName, params Argument[] arguments)
        {
            if (typeName == null) throw new ArgumentNullException(nameof(typeName));
            if (arguments == null) throw new ArgumentNullException(nameof(arguments));

            TypeName = typeName;
            Arguments = arguments;
        }

        private static string GetTypeName(ParseTreeNode node)
        {
            if (node.Term.Name != XamlMarkupExtensionGrammar.TypeNameTerm)
                return null;

            return node.Token.Text;
        }

        private static IEnumerable<Argument> GetArguments(IEnumerable<ParseTreeNode> nodes)
        {
            foreach (var node in nodes)
            {
                var argument =
                    PositionalArgument.Create(node)
                    ?? (Argument) NamedArgument.Create(node);

                if (argument != null)
                    yield return argument;
                else
                {
                    // Unwrap argument
                    foreach (var markupExtensionArgument in GetArguments(node.ChildNodes))
                        yield return markupExtensionArgument;
                }
            }
        }

        public new static MarkupExtension Create(ParseTreeNode node)
        {
            if (node.Term.Name != XamlMarkupExtensionGrammar.MarkupExtensionTerm)
                return null;

            return
                new MarkupExtension(GetTypeName(node.ChildNodes.First()),
                    GetArguments(node.ChildNodes.Skip(1)).ToArray());
        }
    }
}