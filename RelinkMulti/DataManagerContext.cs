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
