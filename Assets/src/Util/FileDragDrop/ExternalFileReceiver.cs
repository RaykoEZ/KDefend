using B83.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
public class ExternalFileDropInfo
{
    public string content;
    public FileInfo fileInfo;
    public Vector2 pos;
}
[Serializable]
// item in a list to check whenever a file is dragged in
public class ExternalFileEvent 
{
    [SerializeField] DefaultFileValidor m_fileValidator = default;
    [SerializeField] UnityEvent<ExternalFileDropInfo> m_triggerOnDraggedIn = default;
    public DefaultFileValidor FileValidator => m_fileValidator;
    public UnityEvent<ExternalFileDropInfo> TriggerOnDraggedIn => m_triggerOnDraggedIn;
}
// Listens to files dragged into game window, trigger events
public delegate void ExternalFileDropped(ExternalFileDropInfo dropInfo);
public class ExternalFileReceiver : MonoBehaviour
{
    [SerializeField] bool m_activateOnEnable = default;
    [SerializeField] List<ExternalFileEvent> m_fileEvents = default;
    public event ExternalFileDropped FileDropped;
    private void OnEnable()
    {
        if (m_activateOnEnable) 
        {
            Activate();
        }
    }
    private void OnDisable()
    {
        Deactivate();
    }
    public void Activate() 
    {
        UnityDragAndDropHook.InstallHook();
        UnityDragAndDropHook.OnDroppedFiles += OnFiles;
    }
    public void Deactivate() 
    {
        UnityDragAndDropHook.UninstallHook();
        UnityDragAndDropHook.OnDroppedFiles -= OnFiles;
    }
    void OnFiles(List<string> aFiles, Vector2 aPos)
    {
        string file = "";
        FileInfo fi = null;
        foreach (var f in aFiles)
        {
            fi = new FileInfo(f);
            string ext = fi.Extension.ToLower();
            // detect file extensions to respond to
            if (string.IsNullOrEmpty(ext))
            {
                file = f;
                break;
            }
        }
        // go through event list to trigger valid events
        ProcessEvents(fi, file, aPos);
    }
    void ProcessEvents(FileInfo fileInfo, string content, Vector2 aPos) 
    {
        if (fileInfo == null) return;
        // If the user dropped a supported file, create a DropInfo and pass to other listeners
        var info = new ExternalFileDropInfo
        {
            content = content,
            fileInfo = fileInfo,
            pos = aPos
        };
        foreach (var e in m_fileEvents)
        {
            // check if file dropped is what we want
            if (e.FileValidator.Validate(fileInfo, content))
            {
                e.TriggerOnDraggedIn?.Invoke(info);
            }
        }
        FileDropped?.Invoke(info);
    }
}