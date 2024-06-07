namespace Signa.Core;
internal static class BitReverseSorter
{
    public static void Sort<T>(T[] signal)
    {
        int halfCount = signal.Length / 2;
        int i = 0;
        for (int j = 0; j < halfCount; j += 2)
        {
            if (j < i)
            {
                (signal[i], signal[j]) = (signal[j], signal[i]);
                (signal[i + halfCount + 1], signal[j + halfCount + 1]) = (signal[j + halfCount + 1], signal[i + halfCount + 1]);
            }

            (signal[j + halfCount], signal[i + 1]) = (signal[i + 1], signal[j + halfCount]);

            for (int k = halfCount >> 1; k > (i ^= k); k >>= 1)
            {
            }
        }
    }
}