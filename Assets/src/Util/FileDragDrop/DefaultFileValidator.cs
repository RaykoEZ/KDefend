using System;
using System.IO;

[Serializable]
// just match filename
public class DefaultFileValidator : BaseFileValidators, IFileValidator
{
    public string AcceptedFilenames = default;
    public string FileExtension = default;
    public override bool Validate(FileInfo info, string content)
    {
        if (info == null) return false;
        bool ret =
            string.IsNullOrEmpty(AcceptedFilenames) || (info.Name == AcceptedFilenames) &&
            info.Extension == FileExtension;
        return ret;
    }
}