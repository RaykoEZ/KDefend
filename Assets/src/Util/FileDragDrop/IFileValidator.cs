using System;
using System.IO;
public interface IFileValidator
{
    public bool Validate(FileInfo info, string content);
}
[Serializable]
public class DefaultFileValidor : IFileValidator
{
    public string AcceptedFilenames = default;
    public virtual bool Validate(FileInfo info, string content)
    {
        if (info == null) return false;
        bool ret = AcceptedFilenames == info.Name;
        return ret;
    }
}
