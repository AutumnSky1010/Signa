using Signa.Core;
using System.Numerics;

namespace SignaTests.Core;
public class FourierTests
{
    [Fact]
    public void FFT()
    {
        int[] data = [0, 1, 0, -1, 0, 1, 0, -1];
        var fourier = new Fourier(data.Length);

        var actual = fourier.FFT(data);
        var expected = new Complex[]
        {
            new(0, 0),
            new(0, 0),
            new(0, -0.5),
            new(0, 0),
            new(0, 0),
            new(0, 0),
            new(0, 0.5),
            new(0, 0),
        };

        double tolerance = 0.001;
        AssertComplex(expected[0], actual[0], tolerance);
        AssertComplex(expected[1], actual[1], tolerance);
        AssertComplex(expected[2], actual[2], tolerance);
        AssertComplex(expected[3], actual[3], tolerance);
        AssertComplex(expected[4], actual[4], tolerance);
        AssertComplex(expected[5], actual[5], tolerance);
        AssertComplex(expected[6], actual[6], tolerance);
        AssertComplex(expected[7], actual[7], tolerance);
    }

    [Fact]
    public void IFFT()
    {
        var data = new Complex[]
        {
            new(0, 0),
            new(0, 0),
            new(0, -0.5),
            new(0, 0),
            new(0, 0),
            new(0, 0),
            new(0, 0.5),
            new(0, 0),
        };
        var fourier = new Fourier(data.Length);

        var actual = fourier.IFFT(data);
        var expected = new Complex[]
        {
            new(0, 0),
            new(1, 0),
            new(0, 0),
            new(-1, 0),
            new(0, 0),
            new(1, 0),
            new(0, 0),
            new(-1, 0),
        };

        double tolerance = 0.001;
        AssertComplex(expected[0], actual[0], tolerance);
        AssertComplex(expected[1], actual[1], tolerance);
        AssertComplex(expected[2], actual[2], tolerance);
        AssertComplex(expected[3], actual[3], tolerance);
        AssertComplex(expected[4], actual[4], tolerance);
        AssertComplex(expected[5], actual[5], tolerance);
        AssertComplex(expected[6], actual[6], tolerance);
        AssertComplex(expected[7], actual[7], tolerance);
    }

    private void AssertComplex(Complex expected, Complex actual, double tolerance)
    {
        Assert.Equal(expected.Real, actual.Real, tolerance);
        Assert.Equal(expected.Imaginary, actual.Imaginary, tolerance);
    }
}
