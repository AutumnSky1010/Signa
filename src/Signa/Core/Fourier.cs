using System.Numerics;

namespace Signa.Core;
public static class Fourier
{
    public static Complex[] FFT(int n, int[] signal)
    {
        if (!BitOperations.IsPow2(n))
        {
            throw new ArgumentException("");
        }

        if (signal.Length > n)
        {
            signal = signal.Take(n).ToArray();
        }
        else if (signal.Length < n)
        {
            var newArray = new int[n];
            int i;
            for (i = 0; i < signal.Length; i++)
            {
                newArray[i] = signal[i];
            }
            while (i < newArray.Length)
            {
                newArray[i] = 0;
            }
            signal = newArray;
        }

        var twiddleFactor = CalculateTwiddleFactor(n);
        BitReverseSorter.Sort(signal);
        var result = new Complex[n];
        for (int i = 0; i < result.Length; i++)
        {
            result[i] = new Complex(signal[i], 0);
        }
        FFTInternal(0, n, twiddleFactor, result);

        for (int i = 0; i < result.Length; i++)
        {
            result[i] *= 1.0d / n;
        }
        return result;
    }

    private static void FFTInternal(int startIndex, int n, Complex[] twiddleFactor, Complex[] result)
    {
        int halfCount = n / 2;
        if (halfCount != 1)
        {
            FFTInternal(startIndex, halfCount, twiddleFactor, result);
            FFTInternal(startIndex + halfCount, halfCount, twiddleFactor, result);
        }


        int index = startIndex;
        int twiddleIndex = 0;
        int twiddleIndexCommonDifference = result.Length / n;
        for (int i = 0; i < halfCount; i++)
        {
            int oddIndex = index + halfCount;

            var old = result[index];
            var oldOdd = result[oddIndex];
            result[index] = old + twiddleFactor[twiddleIndex] * oldOdd;
            result[oddIndex] = old + twiddleFactor[twiddleIndex + halfCount * twiddleIndexCommonDifference] * oldOdd;

            index++;
            twiddleIndex += twiddleIndexCommonDifference;
        }
    }

    private static Complex[] CalculateTwiddleFactor(int n)
    {
        var result = new Complex[n];
        var angleOfArc = 2 * Math.PI / n;
        for (var i = 0; i < n; i++)
        {
            result[i] = new Complex(Math.Cos(i * angleOfArc), -Math.Sin(i * angleOfArc));
        }

        return result;
    }
}
