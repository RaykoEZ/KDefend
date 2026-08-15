using System;
using UnityEngine;
using Curry.Game;

namespace Curry.Events
{
    public delegate void OnObjectiveUpdate<T>(T completed);
    public abstract class GameObjective<T> where T : IObjective
    {
        public event OnObjectiveUpdate<GameObjective<T>> OnComplete;
        public event OnObjectiveUpdate<GameObjective<T>> OnFail;
        public abstract T Detail { get; }
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
