using System.Collections.Immutable;
using GBFRDataTools.Hashing;

namespace RelinkMulti;

public static class WarpathPatch
{
    public static readonly IDictionary<string, string> PatchDict;

    static IEnumerable<(string, string)> WarpathSigils()
    {
        for (int i = 114; i <= 132; i++)
        {
            yield return ($"GEEN_{i}_64", $"GEEN_{i}_90");
        }

        for (int i = 170; i <= 178; i++)
        {
            yield return ($"GEEN_{i}_64", $"GEEN_{i}_90");
        }
    }

    static WarpathPatch()
    {
        PatchDict = WarpathSigils().Select(x => (XXHash32Custom.HashAsString(x.Item1), XXHash32Custom.HashAsString(x.Item2)))
            .ToDictionary(x => x.Item1, x => x.Item2)
            .ToImmutableDictionary();
    }
}
