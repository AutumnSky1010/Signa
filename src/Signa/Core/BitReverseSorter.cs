namespace Signa.Core;
internal static class BitReverseSorter
{
    public static void Sort<T>(T[] signal)
    {
        int i = 0;
        int k;

        for (int j = 1; j < signal.Length - 1; j++)
        {
            k = signal.Length / 2;
            while (k <= i)
            {
                i -= k;
                k /= 2;
            }
            i += k;
            if (j < i)
            {
                (signal[i], signal[j]) = (signal[j], signal[i]);
            }
        }
    }
}