using System.IO;
using UnityEngine;
public interface IFileValidator
{
    public bool Validate(FileInfo info, string content);
}
public abstract class BaseFileValidators : MonoBehaviour, IFileValidator 
{
    public abstract bool Validate(FileInfo info, string content);
}
