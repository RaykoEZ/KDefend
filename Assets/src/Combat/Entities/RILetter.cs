using UnityEngine;
// Create clue to hidden boss
public class RILetter : Collectible
{
    [SerializeField] string m_folderName = default;
    [SerializeField] FileWriteDetail m_fileWriteDetail = default;
    [SerializeField] Texture2D m_photoSent = default;
    public override void UseItem()
    {
        base.UseItem();
        FileWriter writer = new FileWriter();
        // write to player desktop, do it silently if possible
        writer.WriteToDesktop(m_fileWriteDetail.Filename, m_folderName, m_fileWriteDetail.RawContent);
        writer.SendPngToDesktop(filename: "fromRI", foldername: m_folderName, m_photoSent);
    }
}
