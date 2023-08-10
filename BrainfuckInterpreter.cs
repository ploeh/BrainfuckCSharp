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
        private int instructionPointer;
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

        private bool IsDone => program.Length <= instructionPointer;

        private void InterpretInstruction()
        {
            var instruction = program[instructionPointer];
            switch (instruction)
            {
                case '>':
                    dataPointer++;
                    instructionPointer++;
                    break;
                case '<':
                    dataPointer--;
                    instructionPointer++;
                    break;
                case '+':
                    data[dataPointer]++;
                    instructionPointer++;
                    break;
                case '-':
                    data[dataPointer]--;
                    instructionPointer++;
                    break;
                case '.':
                    output.Write((char)data[dataPointer]);
                    instructionPointer++;
                    break;
                case '[':
                    if (data[dataPointer] == 0)
                        instructionPointer = program.IndexOf(']', instructionPointer);
                    else
                        instructionPointer++;
                    break;
                case ']':
                    if (data[dataPointer] != 0)
                        instructionPointer = program.LastIndexOf('[', instructionPointer);
                    else
                        instructionPointer++;
                    break;
                default:
                    instructionPointer++;
                    break;
            }
        }
    }
}