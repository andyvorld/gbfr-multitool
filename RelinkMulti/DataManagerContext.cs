using System.Collections.Concurrent;
using GBFRDataTools.Database.Generated;
using gbfrelink.utility.manager.Interfaces;
using Syroot.BinaryData;

namespace RelinkMulti;

public sealed class DataManagerContext(IDataManager dm) : IDisposable
{
    private IDataManager DataManager { get; init; } = dm;

    private ConcurrentDictionary<Type, Lazy<GameTable>> TableInstances { get; init; } = [];

    public T GetTable<T>() where T : GameTable, new()
    => (T)TableInstances.GetOrAdd(typeof(T), _ =>
            new Lazy<GameTable>(() =>
            {
                var tableName = $"system/table/{typeof(T).Name.ToSnakeCase()}.tbl";
                var file = DataManager.GetModdedOrAchiveFile(tableName);

                var db = Activator.CreateInstance<T>();
                db.Read(file);

                return db;
            })
        ).Value;

    public void Dispose()
    {
        foreach (var (Key, LazyTable) in TableInstances)
        {
            if (LazyTable.IsValueCreated)
            {
                var tableName = $"system/table/{Key.Name.ToSnakeCase()}.tbl";
                using var ms = new MemoryStream();
                using var bs = new BinaryStream(ms);
                LazyTable.Value.Write(bs);

                DataManager.AddOrUpdateExternalFile(tableName, ms.ToArray());
            }
        }
    }
}

public static class DMCExtensions
{
    extension(DataManagerContext dmc)
    {
        public void AddShopItem(string purchaseItem, string costItem, int costQty)
        {
            var tradeTable = dmc.GetTable<Trade>();
            var itmTable = dmc.GetTable<ItemTierMap>();
            var imlTable = dmc.GetTable<ItemMaterialList>();

            var rows = (from tradeRow in tradeTable.Rows
                        join itmRow in itmTable.Rows on tradeRow.ItemTierMapId equals itmRow.Key
                        join imlRow in imlTable.Rows on itmRow.MaterialId1 equals imlRow.Key
                        where tradeRow.SubKey == "BEF90A06" // Hidden 'Silver Centrum' entry, use as template
                        select (tradeRow, itmRow, imlRow)).First();

            string subKey = "CAFEBEEF";
            string itmKey = "CAFEBEEF";
            uint imlKey = 0xCAFEBEEF;

            var newTradeRow = rows.tradeRow with
            {
                MinQuestId = "00000000",
                MaxQuestId = "00000000",
                ItemPurchasable = purchaseItem,
                Key = 3,
                SubKey = subKey,
                SortOrder = 60000,

                ItemTierMapId = itmKey,
            };

            var newItmRow = rows.itmRow with
            {
                MaterialId1 = imlKey,
                Key = itmKey,
            };

            var newImlRow = rows.imlRow with
            {
                Key = imlKey,

                Item1 = costItem,
                Item2 = KnownHashes.EMPTY_HASH,
                Item3 = KnownHashes.EMPTY_HASH,
                Item4 = KnownHashes.EMPTY_HASH,
                Item5 = KnownHashes.EMPTY_HASH,
                Item6 = KnownHashes.EMPTY_HASH,
                Item7 = KnownHashes.EMPTY_HASH,
                Item8 = KnownHashes.EMPTY_HASH,
                Item10 = KnownHashes.EMPTY_HASH,
                Item11 = KnownHashes.EMPTY_HASH,
                Item12 = KnownHashes.EMPTY_HASH,

                ItemCount1 = costQty,
                ItemCount2 = 0,
                ItemCount3 = 0,
                ItemCount4 = 0,
                ItemCount5 = 0,
                ItemCount6 = 0,
                ItemCount7 = 0,
                ItemCount8 = 0,
                ItemCount10 = 0,
                ItemCount11 = 0,
                ItemCount12 = 0,
            };

            tradeTable.Rows.Add(newTradeRow);
            itmTable.Rows.Add(newItmRow);
            imlTable.Rows.Add(newImlRow);
        }
    }
}
