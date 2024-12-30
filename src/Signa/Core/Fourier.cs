using System.Numerics;

namespace Signa.Core;
public class Fourier
{
    private readonly Complex[] _twiddleFactorForFFT;

    private readonly Complex[] _twiddleFactorForIFFT;

    public Fourier(int n)
    {
        if (!BitOperations.IsPow2(n))
        {
            throw new ArgumentException("");
        }
        _twiddleFactorForFFT = CalculateTwiddleFactor(n);
        _twiddleFactorForIFFT = TwiddleFactorForIFFTFrom(_twiddleFactorForFFT);
        N = n;
    }

    public int N { get; }

    public Complex[] FFT<T>(T[] signal) where T : struct, INumber<T>
    {
        signal = ArrangeSize(N, signal);

        // ビットリバースソートし、結果用配列に格納する
        BitReverseSorter.Sort(signal);
        var result = new Complex[N];
        for (int i = 0; i < result.Length; i++)
        {
            result[i] = new Complex(double.CreateChecked(signal[i]), 0);
        }

        // N点FFTを実行
        ButterflyComputation(0, N, _twiddleFactorForFFT, result);

        // 1/Nを乗ずる
        for (int i = 0; i < result.Length; i++)
        {
            result[i] *= 1.0d / N;
        }
        return result;
    }

    public Complex[] IFFT(Complex[] spectrum)
    {
        spectrum = ArrangeSize(N, spectrum);
        // スペクトルをビットリバースソートする
        BitReverseSorter.Sort(spectrum);
        var result = new Complex[N];
        Array.Copy(spectrum, result, spectrum.Length);
        // IFFTを実行する
        ButterflyComputation(0, N, _twiddleFactorForIFFT, result);
        return result;
    }

    private static void ButterflyComputation(int startIndex, int n, Complex[] twiddleFactor, Complex[] result)
    {
        int halfCount = n / 2;
        // 2点まで分解してFFTする
        if (halfCount != 1)
        {
            ButterflyComputation(startIndex, halfCount, twiddleFactor, result);
            ButterflyComputation(startIndex + halfCount, halfCount, twiddleFactor, result);
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

    private static T[] ArrangeSize<T>(int n, T[] from) where T : struct
    {
        // 指定したNを超えている場合は信号を切り取る。
        if (from.Length > n)
        {
            return from.Take(n).ToArray();
        }
        // 指定したNより小さい信号が与えられた場合は0-paddingする
        else if (from.Length < n)
        {
            var newArray = new T[n];
            int i;
            for (i = 0; i < from.Length; i++)
            {
                newArray[i] = from[i];
            }
            return newArray;
        }
        // 上記以外は操作不要
        return from;
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

    private static Complex[] TwiddleFactorForIFFTFrom(Complex[] forFFT)
    {
        var forIFFT = new Complex[forFFT.Length];
        forIFFT[0] = forFFT[0];

        for (int i = 1; i < forFFT.Length; i++)
        {
            forIFFT[i] = forFFT[forFFT.Length - i];
        }

        return forIFFT;
    }
}
