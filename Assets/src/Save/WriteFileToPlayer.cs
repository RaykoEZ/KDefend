using UnityEngine;
// Write a file to player's desktop
public class WriteFileToPlayer : MonoBehaviour 
{
    [SerializeField] string m_folderName = default;
    [SerializeField] FileWriteDetail m_fileWriteDetail = default;
    public void Write()
    {
        FileWriter writer = new FileWriter();
        // write to player desktop, do it silently if possible
        writer.WriteToDesktop(m_fileWriteDetail.Filename, m_folderName, m_fileWriteDetail.RawContent);
    }
}
