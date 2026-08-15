using System;
using UnityEngine;

[CreateAssetMenu(fileName = "file_", menuName = "IO/File to write in gameplay", order = 0)]
public class FileWriteDetail : ScriptableObject
{
    /// <summary>
    /// m_filename - name of file (with extension)
    /// </summary>
    [SerializeField] string m_filename = default;
    [TextArea(5, 50)]
    [SerializeField] string m_rawContent = default;
    public string Filename { get => m_filename; }
    public string RawContent { get => m_rawContent; }
}
