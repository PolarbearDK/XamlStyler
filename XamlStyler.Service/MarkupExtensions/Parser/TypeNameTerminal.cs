using Irony.Parsing;

namespace XamlStyler.Core.MarkupExtensions.Parser
{
    internal class TypeNameTerminal : Terminal
    {
        public TypeNameTerminal(string name) : base(name)
        {
        }

        public override Token TryMatch(ParsingContext context, ISourceStream source)
        {
            while (!source.EOF())
            {
                switch (source.PreviewChar)
                {
                    case '\n':
                    case '\r':
                    case ' ':
                    case '}':
                        if (source.PreviewPosition > source.Position)
                            return source.CreateToken(this.OutputTerminal);
                        return context.CreateErrorToken(Name + " was expected");
                }
                source.PreviewPosition++;
            }

            return context.CreateErrorToken("Unbalanced " + Name);
        }
    }
}