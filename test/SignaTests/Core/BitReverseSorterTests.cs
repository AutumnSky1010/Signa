using Signa.Core;

namespace SignaTests.Core;

public class BitReverseSorterTests
{
    [Theory]
    [InlineData(8, "0,4,2,6,1,5,3,7")]
    [InlineData(16, "0,8,4,12,2,10,6,14,1,9,5,13,3,11,7,15")]
    public void Sort(int n, string expectedCSV)
    {
        var data = Enumerable.Range(0, n).ToArray();
        BitReverseSorter.Sort(data);

        Assert.Equal(expectedCSV, string.Join(",", data));
    }
}