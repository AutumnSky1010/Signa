using Signa.Core;

namespace SignaTests.Core;

public class BitReverseSorterTests
{
    [Fact]
    public void Sort()
    {
        int[] data = [0, 1, 2, 3, 4, 5, 6, 7];
        int[] expected = [0, 4, 2, 6, 1, 5, 3, 7];

        BitReverseSorter.Sort(data);

        Assert.Equal(string.Join(",", expected), string.Join(",", data));
    }
}