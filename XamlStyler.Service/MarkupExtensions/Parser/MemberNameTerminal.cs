namespace XamlStyler.Core.MarkupExtensions.Parser
{
    internal class MemberNameTerminal : MemberNameOrStringTerminal
    {
        public MemberNameTerminal(string name): base(name)
        {
        }

        protected override bool IsMemberName => true;
    }
}