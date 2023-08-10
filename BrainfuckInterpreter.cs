namespace Ploeh.Katas.BrainfuckCSharp;

public sealed class BrainfuckInterpreter
{
    private readonly StringWriter output;

    public BrainfuckInterpreter(StringWriter output)
    {
        this.output = output;
    }

    public void Run(string program)
    {
        output.Write(' ');
    }
}