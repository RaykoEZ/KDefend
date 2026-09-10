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
    [SerializeField] List<BaseFileValidators> m_fileValidators = default;
    [SerializeField] UnityEvent<ExternalFileDropInfo> m_triggerOnDraggedIn = default;
    public List<BaseFileValidators> FileValidators => m_fileValidators;
    public UnityEvent<ExternalFileDropInfo> TriggerOnDraggedIn => m_triggerOnDraggedIn;
}
// Listens to files dragged into game window, trigger events
public delegate void ExternalFileDropped(ExternalFileDropInfo dropInfo);
public class ExternalFileReceiver : MonoBehaviour
{
    [SerializeField] bool m_activateOnEnable = default;
    [SerializeField] List<ExternalFileEvent> m_fileEvents = default;
    [SerializeField] UnityEvent m_onFileInvalid = default;
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
    void OnFiles(List<string> draggInFiles, Vector2 aPos)
    {
        string file = "";
        FileInfo fi = null;
        foreach (var currentFile in draggInFiles)
        {
            fi = new FileInfo(currentFile);
            string ext = fi.Extension.ToLower();
            // detect file extensions to respond to
            if (!string.IsNullOrEmpty(ext))
            {
                file = currentFile;
                break;
            }
        }
        if (!string.IsNullOrEmpty(file)) 
        {
            var contentBytes = File.ReadAllBytes(file);
            string content = System.Text.Encoding.Default.GetString(contentBytes);
            // go through event list to trigger valid events
            ProcessEvents(fi, content, aPos);
        }
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
        int n = 0;
        foreach (var e in m_fileEvents)
        {
            // check if file dropped is what we want
            if (ValidateAll(fileInfo, content, e.FileValidators))
            {
                e.TriggerOnDraggedIn?.Invoke(info);
                ++n;
            }
        }        
        if (n == 0) 
        {
            m_onFileInvalid?.Invoke();
        }
        FileDropped?.Invoke(info);
    }
    bool ValidateAll(FileInfo fileInfo, string content, List<BaseFileValidators> validators) 
    {
        bool ret = true;
        foreach (var valid in validators) 
        {
            ret &= valid.Validate(fileInfo, content);
        }
        return ret;
    }
}