namespace NI2S.Node.Core.ComponentModel
{
    /// <summary>
    /// Reader/Write locker type
    /// </summary>
    public enum ReaderWriteLockType
    {
        Read,
        Write,
        UpgradeableRead
    }
}