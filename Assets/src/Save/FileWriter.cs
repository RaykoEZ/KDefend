using UnityEngine;

public class FileWriter 
{
    public void WriteTextToDesktop(string filename, string folder, string content, bool encode = false) 
    {
        if (encode) 
        {
            FileUtil.Base64TextTo(FileUtil.s_desktopPath, folder, filename, content);
        } 
        else 
        {
            FileUtil.RawTextTo(FileUtil.s_desktopPath, folder, filename, new string[] { content });
        }
    }
    public void WriteToDesktop(string filename, string folder, string content) 
    {
        FileUtil.Base64TextTo(FileUtil.s_desktopPath, folder, filename, content);

    }
    public void SendPngToDesktop(string filename, string foldername, Texture2D texture) 
    {
        FileUtil.PngImageTo(FileUtil.s_desktopPath, foldername, filename, texture);
    }
}