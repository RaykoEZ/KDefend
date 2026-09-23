using UnityEngine;

public class WriteImageToDesktop : MonoBehaviour
{
    [SerializeField] string m_fileNameNoExtension = default;
    [SerializeField] string m_folderName = default;
    [SerializeField] Texture2D m_photoSent = default;
    public void Write()
    {
        FileWriter writer = new FileWriter();
        // write to player desktop, do it silently if possible
        writer.SendPngToDesktop(filenameNoExtension: m_fileNameNoExtension, foldername: m_folderName, m_photoSent);
    }
}