using Signa;

namespace SignaTests;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        Assert.True(new Class1().IsSignaProjectStarted());
    }
}