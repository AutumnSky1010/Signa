using System.Numerics;

namespace Signa.Core;
public class Fourier
{
    public static Complex[] FFT(int n, int[] signal)
    public Fourier(int n)
    {
        if (!BitOperations.IsPow2(N))
        {
            throw new ArgumentException("");
        }
        _twiddleFactorForFFT = CalculateTwiddleFactor(n);
        N = n;
    }

    public int N { get; }

    public Complex[] FFT(int[] signal)
        {
        // 指定したNを超えている場合は信号を切り取る。
        if (signal.Length > N)
        {
            signal = signal.Take(N).ToArray();
        }
        // 指定したNより小さい信号が与えられた場合は0-paddingする
        else if (signal.Length < N)
        {
            var newArray = new int[N];
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

        // ビットリバースソートし、結果用配列に格納する
        BitReverseSorter.Sort(signal);
        var result = new Complex[N];
        for (int i = 0; i < result.Length; i++)
        {
            result[i] = new Complex(signal[i], 0);
        }
        FFTInternal(0, n, twiddleFactor, result);

        // N点FFTを実行
        FFTInternal(0, N, _twiddleFactorForFFT, result);

        // 1/Nを乗ずる
        for (int i = 0; i < result.Length; i++)
        {
            result[i] *= 1.0d / N;
        }
        return result;
    }

    private static void FFTInternal(int startIndex, int n, Complex[] twiddleFactor, Complex[] result)
    {
        int halfCount = n / 2;
        // 2点まで分解してFFTする
        if (halfCount != 1)
        {
            FFTInternal(startIndex, halfCount, twiddleFactor, result);
            FFTInternal(startIndex + halfCount, halfCount, twiddleFactor, result);
        }

        // result用のindex
        int index = startIndex;
        // 回転因子用のindex
        int twiddleIndex = 0;
        // 1ループあたり回転因子のインデクスの増加量
        int twiddleIndexCommonDifference = result.Length / n;
        for (int i = 0; i < halfCount; i++)
        {
            // result[index]と対となるresultのインデクス
            int pairedIndex = index + halfCount;
            // twddle[twiddleIndex](回転因子)と対となる回転因子のインデクス
            int pairedTwiddleIndex = twiddleIndex + halfCount * twiddleIndexCommonDifference;

            // resultを書き換えるのであらかじめ記憶する
            var old = result[index];
            var oldPaired = result[pairedIndex];

            // バタフライ演算を行う
            result[index] = old + twiddleFactor[twiddleIndex] * oldPaired;
            result[pairedIndex] = old + twiddleFactor[pairedTwiddleIndex] * oldPaired;

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
