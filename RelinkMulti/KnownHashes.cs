using System.Reflection.Metadata;
using GBFRDataTools.Hashing;

namespace RelinkMulti;

public static partial class KnownHashes
{
    public const string EMPTY_HASH = "887AE0B0";

    public static readonly string AzuriteSplendor = XXHash32Custom.HashAsString("ITEM_14_0000");
    public static readonly string AzuriteShard = XXHash32Custom.HashAsString("ITEM_14_0001");

    public static readonly string RafaleCoin = XXHash32Custom.HashAsString("ITEM_15_0000");
    public static readonly string KnickknackVoucher = XXHash32Custom.HashAsString("ITEM_21_0000");

    public static readonly string SilverDaliaBadge = XXHash32Custom.HashAsString("ITEM_14_0031");
    public static readonly string GoldDaliaBadge = XXHash32Custom.HashAsString("ITEM_14_0032");
}
