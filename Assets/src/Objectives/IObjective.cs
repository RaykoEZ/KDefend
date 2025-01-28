using System;
using System.Collections.Generic;
using Curry.Game;

namespace Curry.Events
{
    public delegate void OnObjectiveUpdate(IObjective completed);
    public interface IObjective
    {
        string Title { get; }
        string Description { get; }
        event OnObjectiveUpdate OnComplete;
        event OnObjectiveUpdate OnFail;
        void Init();
        void Shutdown();
    }
}
