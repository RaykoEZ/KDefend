using System;
using UnityEngine;
using Curry.Game;

namespace Curry.Events
{
    [Serializable]
    public class GameObjective : MonoBehaviour, IObjective
    {
        [SerializeField] protected string m_title = default;
        [SerializeField] protected string m_description = default;
        public event OnObjectiveUpdate OnComplete;
        public event OnObjectiveUpdate OnFail;
        public virtual string Title => m_title;
        public virtual string Description => m_description;
        public virtual void Init() { }
        public virtual void Shutdown() { }
        public virtual void Complete() 
        {
            OnComplete?.Invoke(this);
        }
        public virtual void Fail()
        {
            OnFail?.Invoke(this);
        }
    }
}
