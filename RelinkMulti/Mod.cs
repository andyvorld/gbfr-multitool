using Reloaded.Hooks.ReloadedII.Interfaces;
using Reloaded.Mod.Interfaces;
using RelinkMulti.Template;
using RelinkMulti.Configuration;
using gbfrelink.utility.manager.Interfaces;
using GBFRDataTools.Database.Generated;

#if DEBUG
using System.Diagnostics;
#endif

namespace RelinkMulti;

/// <summary>
/// Your mod logic goes here.
/// </summary>
public class Mod : ModBase // <= Do not Remove.
{
    /// <summary>
    /// Provides access to the mod loader API.
    /// </summary>
    private readonly IModLoader _modLoader;

    /// <summary>
    /// Provides access to the Reloaded.Hooks API.
    /// </summary>
    /// <remarks>This is null if you remove dependency on Reloaded.SharedLib.Hooks in your mod.</remarks>
    private readonly IReloadedHooks? _hooks;

    /// <summary>
    /// Provides access to the Reloaded logger.
    /// </summary>
    private readonly ILogger _logger;

    /// <summary>
    /// Entry point into the mod, instance that created this class.
    /// </summary>
    private readonly IMod _owner;

    /// <summary>
    /// Provides access to this mod's configuration.
    /// </summary>
    private Config _configuration;

    /// <summary>
    /// The configuration of the currently executing mod.
    /// </summary>
    private readonly IModConfig _modConfig;

    public Mod(ModContext context)
    {
        _modLoader = context.ModLoader;
        _hooks = context.Hooks;
        _logger = context.Logger;
        _owner = context.Owner;
        _configuration = context.Configuration;
        _modConfig = context.ModConfig;

#if DEBUG
        // Attaches debugger in debug mode; ignored in release.
        Debugger.Launch();
#endif

        // For more information about this template, please see
        // https://reloaded-project.github.io/Reloaded-II/ModTemplate/

        // If you want to implement e.g. unload support in your mod,
        // and some other neat features, override the methods in ModBase.

        _logger.WriteLine($"[{_modConfig.ModId}] Initializing...");
        IDataManager dm;
        var ret = _modLoader.GetController<IDataManager>()!.TryGetTarget(out dm!);
        if (ret != false)
        {
            using var dmc = new DataManagerContext(dm);

            if (_configuration.ConstantTableConfig.Enabled)
            {
                foreach (var row in dmc.GetTable<Constant>().Rows)
                {
                    row.MaxLevelVoucherReward = _configuration.ConstantTableConfig.MaxLevelVoucherReward;
                    row.MaxLevelMSPReward = _configuration.ConstantTableConfig.MaxLevelMspReward;
                    row.MaxLevelRepeatXP = _configuration.ConstantTableConfig.MaxLevelRepeatXP;
                    // row.MaxTransmarvelStock = _configuration.ConstantTableConfig.MaxTransmarvelStock;
                    row.Unk21 = _configuration.ConstantTableConfig.MaxLevelTransmarvelReward;
                }
            }

            if (_configuration.RewardLotTableConfig.Enabled)
            {
                foreach (var row in dmc.GetTable<RewardLot>().Rows)
                {
                    if (row is { ItemId: not UtilityExtensions.EMPTY_HASH })
                    {
                        row.AmountGiven = (int)(row.AmountGiven * _configuration.RewardLotTableConfig.ItemDropMult);
                    }

                    if (row is { GemId: not UtilityExtensions.EMPTY_HASH })
                    {
                        row.GemCount = (int)(row.GemCount * _configuration.RewardLotTableConfig.SigilDropMult);
                    }
                }
            }

            if (_configuration.RewardLotTableConfig.Enabled && _configuration.EnableExperimental)
            {
                foreach (var row in dmc.GetTable<RewardSummon>().Rows)
                {
                    row.Unk4 = (int)(row.Unk4 * _configuration.RewardLotTableConfig.SummonDropMult); // Maybe - Min reward count
                    row.Unk5 = (int)(row.Unk5 * _configuration.RewardLotTableConfig.SummonDropMult); // Maybe - Max reward count
                }
            }

            if (_configuration.TradeConfig.Enabled)
            {
                foreach (var row in dmc.GetTable<Trade>().Rows)
                {
                    if (_configuration.TradeConfig.UnlimitedStock)
                    {
                        row.IsRefreshable = 0;
                        row.MaxStockForRefresh = 0;
                        row.AmountRefreshed = 0;
                        row.MaxStock = -1;
                    }

                    if (_configuration.TradeConfig.WarpathIsAwakeningPlus && WarpathPatch.PatchDict.TryGetValue(row.GemPurchasable, out var replacement))
                    {
                        row.GemPurchasable = replacement!;
                    }

                    if (_configuration.TradeConfig.NoQuestPrereq)
                    {
                        row.MinQuestId = "00000000";
                        row.MaxQuestId = "00000000";
                    }

                    if (_configuration.TradeConfig.NoShopRotation)
                    {
                        row.FeaturedWeight = 0;
                        row.IsRandomFeatured = 0;
                    }
                }
            }

            if (_configuration.TradeConfig.Enabled)
            {
                foreach (var row in dmc.GetTable<ItemMaterialList>().Rows)
                {
                    // Azurite shared to splendor key
                    if (row.Key == 900000)
                    {
                        row.ItemCount1 = _configuration.TradeConfig.AzuriteShardPerSplendor;
                    }

                    if (_configuration.TradeConfig.DaliaIsRafaleCoin && (row.Item1 == KnownHashes.SilverDaliaBadge || row.Item1 == KnownHashes.GoldDaliaBadge))
                    {
                        row.Item1 = KnownHashes.RafaleCoin;
                    }
                }
            }

            if (_configuration.EnemyConfig.Enabled)
            {
                foreach (var row in dmc.GetTable<EnemyExp>().Rows)
                {
                    row.ExpOnKill = (uint)(row.ExpOnKill * _configuration.EnemyConfig.ExpMult);
                    row.Unk6 = (uint)(row.Unk6 * _configuration.EnemyConfig.MspMult); // MSP on Kill
                }
            }

            if (_configuration.GemConfig.AllowAllSigilSynth)
            {
                foreach (var row in dmc.GetTable<Gem>().Rows)
                {
                    row.CanGemMix = 0; // Cant Gem Mix
                    row.CanSell = 0; // Cant Sell
                }
            }
        }

        // Apply changes to the game's data.i.
        dm.UpdateIndex();
        _logger.WriteLine($"[{_modConfig.ModId}] Initialized.");
    }

    #region Standard Overrides
    public override void ConfigurationUpdated(Config configuration)
    {
        // Apply settings from configuration.
        // ... your code here.
        _configuration = configuration;
        _logger.WriteLine($"[{_modConfig.ModId}] Config Updated: Applying");
    }
    #endregion

    #region For Exports, Serialization etc.
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public Mod() { }
#pragma warning restore CS8618
    #endregion
}
