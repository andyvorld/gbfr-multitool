using GBFRDataTools.Database.Generated;
using gbfrelink.utility.manager.Interfaces;
using Syroot.BinaryData;

namespace RelinkMulti;

internal static class IDataManagerExtension
{
    extension(IDataManager dm)
    {
        public byte[] GetModdedOrAchiveFile(string fileName)
            => (byte[])dm.GetType().GetMethod("GetArchiveFile")!.Invoke(dm!, [fileName])!;

        public bool UpdateTable<T>(Action<T> act, bool enabled = true) where T : GameTable, new()
        {
            var tableName = $"system/table/{typeof(T).Name.ToSnakeCase()}.tbl";
            if (enabled)
            {
                var file = dm.GetModdedOrAchiveFile(tableName);
                var db = Activator.CreateInstance<T>();
                db.Read(file);

                act.Invoke(db);

                using var ms = new MemoryStream();
                using var bs = new BinaryStream(ms);
                db.Write(bs);

                // Add/Update game file
                dm.AddOrUpdateExternalFile(tableName, ms.ToArray());

                return true;
            }

            return false;
        }
    }
}
