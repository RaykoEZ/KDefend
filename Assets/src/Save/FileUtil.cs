using System;
using System.IO;
using System.Text;
using UnityEngine;
internal static class FileUtil 
{
    internal static string s_operatorFolder = "For R_OX016";
    internal static string s_gamestateSavePath = Application.persistentDataPath;
    internal static string s_desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
    // Uses UTF8 to encode string
    internal static void Base64TextTo(string path, string folderName, string fileName, string content) 
    {
        if (string.IsNullOrWhiteSpace(fileName)) { return; }
        bool hasFolder = TryMakeFolder(path, folderName);
        byte[] encoded = Encoding.UTF8.GetBytes(content);
        string result = Convert.ToBase64String(encoded);
        string resultPath = hasFolder? $"{path}/{folderName}/{fileName}" : $"{path}/{fileName}";
        File.WriteAllText(resultPath, result);
    }
    // Writes New raw text fil to desktop
    internal static void RawTextTo(string path, string folderName, string fileName, string[] content)
    {
        if (string.IsNullOrWhiteSpace(fileName)) { return; }

        bool hasFolder = TryMakeFolder(path, folderName);
        string resultPath = hasFolder ? $"{path}/{folderName}/{fileName}" : $"{path}/{fileName}";
        using (StreamWriter sw = new StreamWriter(resultPath, append: false)) 
        {
            foreach(var line in content)
            {
                sw.WriteLine(line);
            }
        }
    }
    internal static void ClearTextFile(string path) 
    {
        File.WriteAllText(path, string.Empty);
    }
    internal static void AppendToTextFile(string path, string[] content) 
    {
        using (StreamWriter sw = new StreamWriter(path, append: true)) 
        {
            foreach (var line in content)
            {
                sw.WriteLine(line);
            }
        }
    }
    internal static void PngImageTo(string path, string folderName, string fileName, Texture2D image)
    {
        if (string.IsNullOrWhiteSpace(fileName)) { return; }
        bool hasFolder = TryMakeFolder(path, folderName);
        string resultPath = hasFolder? $"{path}/{folderName}" : $"{path}";
        byte[] bytes = image.EncodeToPNG();
        File.WriteAllBytes(resultPath + "/" + fileName + ".png", bytes);
    }
    internal static bool TryMakeFolder(string path, string folderName) 
    {
        if (string.IsNullOrWhiteSpace(folderName)) 
        {
            return false;
        }
        string resultPath = $"{path}/{folderName}";
        if (!Directory.Exists(resultPath))
        {
            Directory.CreateDirectory(resultPath);
            return true;
        }
        return true;
    }
}

