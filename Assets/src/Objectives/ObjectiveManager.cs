using System.Collections.Generic;
using Curry.UI;
using UnityEngine;
using Curry.Events;
using UnityEngine.Events;

namespace Curry.Game
{
    public class ObjectiveManager : MonoBehaviour
    {
        public enum ObjectiveState
        {
            Active,
            Complete,
            Fail
        }
        [SerializeField] protected UnityEvent<IObjective> m_newObjective = default;
        [SerializeField] protected UnityEvent<IObjective> m_objectiveComplete = default;
        [SerializeField] protected UnityEvent<IObjective> m_objectiveFail = default;
        /// Preloaded objectives
        [SerializeField] protected List<GameObjective> m_initObjectives = default;
        protected List<IObjective> m_active = new List<IObjective>();
        protected List<IObjective> m_completed = new List<IObjective>();
        protected List<IObjective> m_failed = new List<IObjective>();
        public event OnObjectiveUpdate OnNewObjective;
        public event OnObjectiveUpdate ObjectiveCompleted;
        public event OnObjectiveUpdate OnFailure;
        public IReadOnlyList<GameObjective> GetObjectives(ObjectiveState state)
        {
            List<GameObjective> ret = new List<GameObjective>();
            List<IObjective> toGet;
            switch (state)
            {
                case ObjectiveState.Active:
                    toGet = m_active;
                    break;
                case ObjectiveState.Complete:
                    toGet = m_completed;
                    break;
                case ObjectiveState.Fail:
                    toGet = m_failed;
                    break;
                default:
                    toGet = m_completed;
                    break;
            }
            foreach (var item in toGet)
            {
                ret.Add(item as GameObjective);
            }
            return ret; 
        }
        protected void Start()
        {
            foreach (IObjective objective in m_initObjectives)
            {
                NewActiveObjective(objective);
            }
        }
        protected void OnDestroy()
        {
            Shutdown();
        }
        protected virtual void Init(  
            List<GameObjective> active, 
            List<GameObjective> completed,
            List<GameObjective> failed) 
        {
            foreach (IObjective objective in active)
            {
                NewActiveObjective(objective);
            }
            foreach (IObjective complete in completed)
            {
                m_completed.Add(complete);
            }
            foreach (IObjective fail in failed)
            {
                m_failed.Add(fail);
            }
        }
        protected virtual void Shutdown()
        {
            foreach (IObjective objective in GetObjectives(ObjectiveState.Active))
            {
                ShutdownObjective(objective);
            }
        }
        public bool TryGetByTitle(string title, out IObjective result) 
        {
            bool ret = !string.IsNullOrEmpty(title);
            result = null;
            if (ret) 
            {
                result = m_active.Find((x) => x.Title == title);
            }
            return ret && result != null;
        }
        public void NewActiveObjective(IObjective objective)
        {
            PrepareObjective(objective, true);
            m_active.Add(objective);
            OnNewObjective?.Invoke(objective);
            m_newObjective?.Invoke(objective);
        }
        protected virtual void OnObjectiveComplete(IObjective completed) 
        {
            if (m_active.Contains(completed))
            {
                // Do some animation/notification for callbacks:
                ObjectiveCompleted?.Invoke(completed);
                m_objectiveComplete?.Invoke(completed);
                CleanupObjective(completed);
                m_completed.Add(completed);
            }
        }
        protected virtual void OnObjectiveFail(IObjective failed)
        {
            if (m_active.Contains(failed))
            {
                OnFailure?.Invoke(failed);
                m_objectiveFail?.Invoke(failed);
                CleanupObjective(failed);
                m_failed.Add(failed);
            }
        }
        void CleanupObjective(IObjective completed)
        {
            completed.OnComplete -= OnObjectiveComplete;
            completed.OnFail -= OnObjectiveFail;
            completed?.Shutdown();
            m_active.Remove(completed);
        }
        protected void PrepareObjective(IObjective objective, bool isActive)
        {
            objective?.Init();
            if (isActive) 
            {
                objective.OnComplete += OnObjectiveComplete;
                objective.OnFail += OnObjectiveFail;
            }
        }
        protected void ShutdownObjective(IObjective objective)
        {
            objective?.Shutdown();
            objective.OnComplete -= OnObjectiveComplete;
            objective.OnFail -= OnObjectiveFail;
        }
    }
}
