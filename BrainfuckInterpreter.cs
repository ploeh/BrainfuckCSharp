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
        var imp = new InterpreterImp(program, output);
        imp.Run();
    }

    private sealed class InterpreterImp
    {
        private int programPointer;
        private int dataPointer;
        private readonly byte[] data;
        private readonly string program;
        private readonly StringWriter output;

        internal InterpreterImp(string program, StringWriter output)
        {
            data = new byte[30_000];
            this.program = program;
            this.output = output;
        }

        internal void Run()
        {
            while (!IsDone)
                InterpretInstruction();
        }

        private bool IsDone => program.Length <= programPointer;

        private void InterpretInstruction()
        {
            var instruction = program[programPointer];
            switch (instruction)
            {
                case '>':
                    dataPointer++;
                    programPointer++;
                    break;
                case '+':
                    data[dataPointer]++;
                    programPointer++;
                    break;
                case '.':
                    output.Write((char)data[dataPointer]);
                    programPointer++;
                    break;
                default:
                    programPointer++;
                    break;
            }
        }
    }
}