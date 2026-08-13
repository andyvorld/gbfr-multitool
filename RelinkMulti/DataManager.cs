using gbfrelink.utility.manager.Interfaces;

namespace RelinkMulti;

internal static class IDataManagerExtension
{
    extension (IDataManager dm)
    {
        public byte[] GetModdedOrAchiveFile(string fileName) 
            => (byte[])dm.GetType().GetMethod("GetArchiveFile")!.Invoke(dm!, [fileName])!;
    }
}
