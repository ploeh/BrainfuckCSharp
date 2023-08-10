namespace Ploeh.Katas.BrainfuckCSharp
{
    internal class BrainfuckInterpreter
    {
        private readonly StringWriter output;

        public BrainfuckInterpreter(StringWriter output)
        {
            this.output = output;
        }

        internal void Run(string program)
        {
            output.Write(' ');
        }
    }
}