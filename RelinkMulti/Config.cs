using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using RelinkMulti.Template.Configuration;

namespace RelinkMulti.Configuration;

[DisplayName("Constant Table")]
public class ConstantTableConfig
{
    [Description("Enable constant table mods")]
    [DefaultValue(false)]
    [Display(Order = 0)]
    public bool Enabled { get; set; } = false;

    [DisplayName("Max Level voucher reward")]
    [Description("Number of voucher reward given at max level")]
    [DefaultValue(3)]
    [Display(Order = 1)]
    public int MaxLevelVoucherReward { get; set; } = 3;

    [DisplayName("Max Level MSP reward")]
    [Description("Number of MSP reward given at max level")]
    [DefaultValue(100)]
    [Display(Order = 2)]
    public int MaxLevelMspReward { get; set; } = 100;

    [DisplayName("Max Level repeat EXP")]
    [Description("EXP required to gain rewards at max level")]
    [DefaultValue(400000)]
    [Display(Order = 3)]
    public int MaxLevelRepeatXP { get; set; } = 400000;

    // [DisplayName("Max transmarvel voucher stock")]
    // [Description("Max number of transmarvel voucher that can be held")]
    // [DefaultValue(999)]
    // [Display(Order = 4)]
    // public uint MaxTransmarvelStock { get; set; } = 999;

    [DisplayName("Max Level transmarvel voucher")]
    [Description("Number of transmarvel voucher reward given at max level")]
    [DefaultValue(1)]
    [Display(Order = 5)]
    public int MaxLevelTransmarvelReward { get; set; } = 1;
}

[DisplayName("Quest Rewards")]
public class RewardLotTableConfig
{
    [Description("Enable quest reward mods")]
    [DefaultValue(false)]
    [Display(Order = 0)]
    public bool Enabled { get; set; } = false;

    [DisplayName("Item drop multiplier")]
    [Description("Item drop multiplier")]
    [DefaultValue(1)]
    [Display(Order = 1)]
    public double ItemDropMult { get; set; } = 1;

    [DisplayName("Sigil drop multiplier")]
    [Description("Sigil drop multiplier")]
    [DefaultValue(1)]
    [Display(Order = 2)]
    public double SigilDropMult { get; set; } = 1;

    [DisplayName("Wrightstone drop multiplier")]
    [Description("Wrightstone drop multiplier")]
    [DefaultValue(1)]
    [Display(Order = 3)]
    public double WrightstoneDropMult { get; set; } = 1;

    [DisplayName("⚠️ Summon drop multiplier")]
    [Description("[Experimental] Summon drop multiplier")]
    [DefaultValue(1)]
    [Display(Order = 4)]
    public double SummonDropMult { get; set; } = 1;

    [DisplayName("EXP multiplier")]
    [Description("EXP on completion multiplier")]
    [DefaultValue(1)]
    [Display(Order = 5)]
    public double ExpMult { get; set; } = 1;

    [DisplayName("Rupies multiplier")]
    [Description("Rupies on completion multiplier")]
    [DefaultValue(1)]
    [Display(Order = 6)]
    public double GoldMult { get; set; } = 1;

    [DisplayName("MSP multiplier")]
    [Description("MSP on completion multiplier")]
    [DefaultValue(1)]
    [Display(Order = 7)]
    public double MspMult { get; set; } = 1;
}

[DisplayName("Knickknack Shack")]
public class TradeConfig
{
    [Description("Enable Knickknack Shack shop mods")]
    [DefaultValue(false)]
    [Display(Order = 0)]
    public bool Enabled { get; set; } = false;

    [DisplayName("Replace warpath with Awakening+")]
    [Description("Replace warpath with Awakening+ in Seiro's Knickknack Shack")]
    [DefaultValue(false)]
    public bool WarpathIsAwakeningPlus { get; set; } = false;

    [DisplayName("Unlimited stock")]
    [Description("Remove stock cap on all items in shop")]
    [DefaultValue(false)]
    public bool UnlimitedStock { get; set; } = false;

    [DisplayName("Azurite shards per splendor")]
    [Description("Change trade ratio of azurite shards per azurite splendor")]
    [DefaultValue(30)]
    public int AzuriteShardPerSplendor { get; set; } = 30;

    [DisplayName("Dalia Badge is Rafale Coin")]
    [Description("Replace Silver/Gold Dalia Badge costs with Rafale coins")]
    [DefaultValue(false)]
    public bool DaliaIsRafaleCoin { get; set; } = false;

    [DisplayName("⚖️ Remove quest pre-req on shop")]
    [Description("[Unbalanced] This is not balanced at all, some debug shop entries get revealed with this option")]
    [DefaultValue(false)]
    public bool NoQuestPrereq { get; set; } = false;

    [DisplayName("No shop rotation")]
    [Description("Reveals all rotating shop options permanently")]
    [DefaultValue(false)]
    public bool NoShopRotation { get; set; } = false;

    [DisplayName("Sell voucher for Rafale Coin")]
    [Description("Sell X vouchers for 1 rafale Coin, 0 = disabled")]
    [DefaultValue(0)]
    public int SellVoucherForRafaleCoin { get; set; } = 0;

    [DisplayName("⚖️ Add special sigils to treasure trade")]
    [Description("[Unbalanced] Add special sigils to treasure trade, crab related sigils will change in level once purchased")]
    [DefaultValue(false)]
    public bool AddSpecialSigils { get; set; } = false;
}

[DisplayName("Enemy")]
public class EnemyConfig
{
    [Description("Enable enemy mods")]
    [DefaultValue(false)]
    [Display(Order = 0)]
    public bool Enabled { get; set; } = false;

    [DisplayName("EXP Multiplier")]
    [Description("Change the EXP on kill by a multiplier")]
    [DefaultValue(1)]
    public double ExpMult { get; set; } = 1;

    [DisplayName("MSP Multiplier")]
    [Description("Change the MSP on kill by a multiplier")]
    [DefaultValue(1)]
    public double MspMult { get; set; } = 1;

    [DisplayName("Rupies Multiplier")]
    [Description("Change the rupies on kill by a multiplier")]
    [DefaultValue(1)]
    public double GoldMult { get; set; } = 1;
}

[DisplayName("Sigil")]
public class GemConfig
{
    [DisplayName("⚖️ All all sigils in Synth")]
    [Description("[Unbalanced] Allow all sigils to be used in synthesis. Illegal sigils can be created.")]
    [DefaultValue(false)]
    public bool AllowAllSigilSynth { get; set; } = false;
}

[DisplayName("Weapon Crafting")]
public class WeaponCraftingConfig
{
    public enum TerminusCostEnum
    {
        Default,
        [Display(Name = "One Ultimate Memory")]
        OneMemory,
        [Display(Name = "Knickknack Vouchers")]
        Vouchers
    }

    [DisplayName("Terminus craft cost")]
    [Description("Change terminus weapon craft cost")]
    [DefaultValue(TerminusCostEnum.Default)]
    public TerminusCostEnum TerminusCost { get; set; } = default;
}

public class Config : Configurable<Config>
{
    /*
        User Properties:
            - Please put all of your configurable properties here.

        By default, configuration saves as "Config.json" in mod user config folder.    
        Need more config files/classes? See Configuration.cs

        Available Attributes:
        - Category
        - DisplayName
        - Description
        - DefaultValue

        // Technically Supported but not Useful
        - Browsable
        - Localizable

        The `DefaultValue` attribute is used as part of the `Reset` button in Reloaded-Launcher.
    */

    [DisplayName("⚠️ Enable experimental settings")]
    [Description("Enable experimental settings")]
    [DefaultValue(false)]
    [Display(Order = 0)]
    public bool EnableExperimental { get; set; } = false;

    public ConstantTableConfig ConstantTableConfig { get; set; } = new();

    public RewardLotTableConfig RewardLotTableConfig { get; set; } = new();

    public TradeConfig TradeConfig { get; set; } = new();

    public EnemyConfig EnemyConfig { get; set; } = new();

    public GemConfig GemConfig { get; set; } = new();

    public WeaponCraftingConfig WeaponCraftingConfig { get; set; } = new();
}

/// <summary>
/// Allows you to override certain aspects of the configuration creation process (e.g. create multiple configurations).
/// Override elements in <see cref="ConfiguratorMixinBase"/> for finer control.
/// </summary>
public class ConfiguratorMixin : ConfiguratorMixinBase
{
    // 
}
