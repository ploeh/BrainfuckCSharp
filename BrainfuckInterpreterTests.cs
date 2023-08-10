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
}
