using Xunit;

namespace Ploeh.Katas.BrainfuckCSharp;

public sealed class BrainfuckInterpreterTests
{
    [Theory]
    [InlineData("++++++++++++++++++++++++++++++++.", " ")] // 32 increments; ASCII 32 is space
    [InlineData("+++++++++++++++++++++++++++++++++.", "!")] // 33 increments; ASCII 32 is !
    [InlineData("+>++++++++++++++++++++++++++++++++.", " ")] // 32 increments after >; ASCII 32 is space
    public void Run(string program, string expected)
    {
        using var output = new StringWriter();
        var sut = new BrainfuckInterpreter(output);

        sut.Run(program);
        var actual = output.ToString();

        Assert.Equal(expected, actual);
    }
}
