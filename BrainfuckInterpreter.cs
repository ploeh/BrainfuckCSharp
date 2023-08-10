namespace Ploeh.Katas.BrainfuckCSharp;

public sealed class BrainfuckInterpreter
{
    private readonly TextReader input;
    private readonly TextWriter output;

    public BrainfuckInterpreter(TextReader input, TextWriter output)
    {
        this.input = input;
        this.output = output;
    }

    public void Run(string program)
    {
        var imp = new InterpreterImp(program, input, output);
        imp.Run();
    }

    private sealed class InterpreterImp
    {
        private int instructionPointer;
        private int dataPointer;
        private readonly byte[] data;
        private readonly string program;
        private readonly TextReader input;
        private readonly TextWriter output;

        internal InterpreterImp(string program, TextReader input, TextWriter output)
        {
            data = new byte[30_000];
            this.program = program;
            this.input = input;
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
            WrapDataPointer();

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
                case ',':
                    data[dataPointer] = (byte)input.Read();
                    instructionPointer++;
                    break;
                case '[':
                    if (data[dataPointer] == 0)
                        MoveToMatchingClose();
                    else
                        instructionPointer++;
                    break;
                case ']':
                    if (data[dataPointer] != 0)
                        MoveToMatchingOpen();
                    else
                        instructionPointer++;
                    break;
                default:
                    instructionPointer++;
                    break;
            }
        }

        private void WrapDataPointer()
        {
            if (dataPointer == -1)
                dataPointer = data.Length - 1;
            if (dataPointer == data.Length)
                dataPointer = 0;
        }

        private void MoveToMatchingClose()
        {
            var nestingLevel = 1;
            while (0 < nestingLevel)
            {
                instructionPointer++;
                if (program[instructionPointer] == '[')
                    nestingLevel++;
                if (program[instructionPointer] == ']')
                    nestingLevel--;
            }
            instructionPointer++;
        }

        private void MoveToMatchingOpen()
        {
            var nestingLevel = 1;
            while (0 < nestingLevel)
            {
                instructionPointer--;
                if (program[instructionPointer] == ']')
                    nestingLevel++;
                if (program[instructionPointer] == '[')
                    nestingLevel--;
            }
            instructionPointer++;
        }
    }
}