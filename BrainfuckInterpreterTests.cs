using Microsoft.VisualStudio.TestPlatform.TestHost;
using Xunit;

namespace Ploeh.Katas.BrainfuckCSharp;

public sealed class BrainfuckInterpreterTests
{
    [Theory]
    [InlineData("++++++++++++++++++++++++++++++++.", " ")] // 32 increments; ASCII 32 is space
    [InlineData("+++++++++++++++++++++++++++++++++.", "!")] // 33 increments; ASCII 32 is !
    [InlineData("+>++++++++++++++++++++++++++++++++.", " ")] // 32 increments after >; ASCII 32 is space
    [InlineData("+++++++++++++++++++++++++++++++++-.", " ")] // 33 increments and 1 decrement; ASCII 32 is space
    [InlineData(">+<++++++++++++++++++++++++++++++++.", " ")] // 32 increments after movement; ASCII 32 is space
    public void Run(string program, string expected)
    {
        using var output = new StringWriter();
        var sut = new BrainfuckInterpreter(output);

        sut.Run(program);
        var actual = output.ToString();

        Assert.Equal(expected, actual);
    }

    // Copied from https://en.wikipedia.org/wiki/Brainfuck
    const string addTwoProgram = @"
++       Cell c0 = 2
> +++++  Cell c1 = 5

[        Start your loops with your cell pointer on the loop counter (c1 in our case)
< +      Add 1 to c0
> -      Subtract 1 from c1
]        End your loops with the cell pointer on the loop counter

At this point our program has added 5 to 2 leaving 7 in c0 and 0 in c1
but we cannot output this value to the terminal since it is not ASCII encoded

To display the ASCII character ""7"" we must add 48 to the value 7
We use a loop to compute 48 = 6 * 8

++++ ++++  c1 = 8 and this will be our loop counter again
[
< +++ +++  Add 6 to c0
> -        Subtract 1 from c1
]
< .        Print out c0 which has the value 55 which translates to ""7""!";

    [Fact]
    public void AddTwoValues()
    {
        using var output = new StringWriter();
        var sut = new BrainfuckInterpreter(output);

        sut.Run(addTwoProgram);
        var actual = output.ToString();

        Assert.Equal("7", actual);
    }

    // Copied from https://en.wikipedia.org/wiki/Brainfuck
    const string helloProgram = @"
[ This program prints ""Hello World!"" and a newline to the screen, its
  length is 106 active command characters. [It is not the shortest.]

  This loop is an ""initial comment loop"", a simple way of adding a comment
  to a BF program such that you don't have to worry about any command
  characters. Any ""."", "","", ""+"", ""-"", ""<"" and "">"" characters are simply
  ignored, the ""["" and ""]"" characters just have to be balanced. This
  loop and the commands it contains are ignored because the current cell
  defaults to a value of 0; the 0 value causes this loop to be skipped.
]
++++++++               Set Cell #0 to 8
[
    >++++               Add 4 to Cell #1; this will always set Cell #1 to 4
    [                   as the cell will be cleared by the loop
        >++             Add 2 to Cell #2
        >+++            Add 3 to Cell #3
        >+++            Add 3 to Cell #4
        >+              Add 1 to Cell #5
        <<<<-           Decrement the loop counter in Cell #1
    ]                   Loop until Cell #1 is zero; number of iterations is 4
    >+                  Add 1 to Cell #2
    >+                  Add 1 to Cell #3
    >-                  Subtract 1 from Cell #4
    >>+                 Add 1 to Cell #6
    [<]                 Move back to the first zero cell you find; this will
                        be Cell #1 which was cleared by the previous loop
    <-                  Decrement the loop Counter in Cell #0
]                       Loop until Cell #0 is zero; number of iterations is 8

The result of this is:
Cell no :   0   1   2   3   4   5   6
Contents:   0   0  72 104  88  32   8
Pointer :   ^

>>.                     Cell #2 has value 72 which is 'H'
>---.                   Subtract 3 from Cell #3 to get 101 which is 'e'
+++++++..+++.           Likewise for 'llo' from Cell #3
>>.                     Cell #5 is 32 for the space
<-.                     Subtract 1 from Cell #4 for 87 to give a 'W'
<.                      Cell #3 was set to 'o' from the end of 'Hello'
+++.------.--------.    Cell #3 for 'rl' and 'd'
>>+.                    Add 1 to Cell #5 gives us an exclamation point
>++.                    And finally a newline from Cell #6";

    // All three program variations are from https://en.wikipedia.org/wiki/Brainfuck
    [Theory]
    [InlineData(helloProgram)]
    [InlineData("++++++++[>++++[>++>+++>+++>+<<<<-]>+>+>->>+[<]<-]>>.>---.+++++++..+++.>>.<-.<.+++.------.--------.>>+.>++.")]
    [InlineData("+[-->-[>>+>-----<<]<--<---]>-.>>>+.>>..+++[.>]<<<<.+++.------.<<-.>>>>+.")]
    public void Hello(string program)
    {
        using var output = new StringWriter();
        var sut = new BrainfuckInterpreter(output);

        sut.Run(program);
        var actual = output.ToString();

        // The three programs have slightly different outputs. One includes a
        // comma, while others have a trailing newline. These assertions
        // capture the essentials.
        Assert.StartsWith("Hello", actual);
        Assert.EndsWith("World!", actual.TrimEnd());
        Assert.InRange(actual.Length, 12, 13);
    }
}
