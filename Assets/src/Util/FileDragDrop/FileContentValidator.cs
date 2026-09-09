using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
// match all provided strings in file content
public class FileContentValidator : BaseFileValidators, IFileValidator
{
    [TextArea(1, 5)]
    public List<string> LookForStrings = default;
    public override bool Validate(FileInfo info, string content)
    {
        if (info == null) return false;
        bool ret = false;
        bool check = true;
        // true => all string patterns exist in file content
        foreach (var match in LookForStrings)
        {
            check &= content.Contains(match);
        }
        ret |= check;
        return ret;
    }
}