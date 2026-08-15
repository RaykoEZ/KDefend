using UnityEngine;

public interface IDataSource<T> 
{ 
    public T Current { get; }
    public void Init(T save);
    public void UpdateSave(bool saveToFile = false);
}
public abstract class BaseSaveSource<T> : MonoBehaviour, IDataSource<T>
{
    protected T m_currentGameState;
    public T Current => m_currentGameState;
    public virtual void Init(T save)
    {
        m_currentGameState = save;
    }
    public abstract void UpdateSave(bool saveToFile = false);
}