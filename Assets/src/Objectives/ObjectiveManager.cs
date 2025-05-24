using System.Collections.Generic;
using UnityEngine;
using Curry.Events;
using UnityEngine.Events;

namespace Curry.Game
{
    public abstract class ObjectiveManager<T> : MonoBehaviour where T: IObjective
    {
        public enum ObjectiveState
        {
            Active,
            Complete,
            Fail
        }
        [SerializeField] protected UnityEvent<T> m_newObjective = default;
        [SerializeField] protected UnityEvent<T> m_objectiveComplete = default;
        [SerializeField] protected UnityEvent<T> m_objectiveFail = default;
        /// Preloaded objectives for test
        [SerializeField] protected List<T> m_TEST_initObjectives = default;
        protected List<GameObjective<T>> m_active = new List<GameObjective<T>>();
        protected List<GameObjective<T>> m_completed = new List<GameObjective<T>>();
        protected List<GameObjective<T>> m_failed = new List<GameObjective<T>>();

        public event OnObjectiveUpdate<T> OnNewObjective;
        public event OnObjectiveUpdate<T> ObjectiveCompleted;
        public event OnObjectiveUpdate<T> OnFailure;
        public IReadOnlyList<GameObjective<T>> GetObjectives(ObjectiveState state)
        {
            List<GameObjective<T>> ret = new List<GameObjective<T>>();
            List<GameObjective<T>> toGet;
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
                ret.Add(item);
            }
            return ret; 
        }
        public List<T> GetDetailsOf(ObjectiveState state)
        {
            var toGet = GetObjectives(state);
            var ret = new List<T>();
            foreach (var item in toGet)
            {
                ret.Add(item.Detail);
            }
            return ret;
        }
        protected void Start()
        {
#if UNITY_EDITOR
            Init(new List<T>(), m_TEST_initObjectives);
#endif
        }
        protected void OnDestroy()
        {
            Shutdown();
        }
        protected virtual void Shutdown()
        {
            foreach (GameObjective<T> objective in GetObjectives(ObjectiveState.Active))
            {
                ShutdownObjective(objective);
            }
        }
        public virtual void Init(
            List<T> completedObjectives,
            List<T> newObjectives) { }
        public GameObjective<T> GetByTitle(string title) 
        {
            return m_active.Find((x) => x.Detail.Title == title);
        }
        public void NewActiveObjective(GameObjective<T> objective)
        {
            PrepareObjective(objective, true);
            m_active.Add(objective);
            OnNewObjective?.Invoke(objective.Detail);
            m_newObjective?.Invoke(objective.Detail);
        }
        protected virtual void OnObjectiveComplete(GameObjective<T> completed) 
        {
            // Do some animation/notification for callbacks:
            ObjectiveCompleted?.Invoke(completed.Detail);
            m_objectiveComplete?.Invoke(completed.Detail);
            CleanupObjective(completed);
            m_completed.Add(completed);      
        }
        protected virtual void OnObjectiveFail(GameObjective<T> failed)
        {
            OnFailure?.Invoke(failed.Detail);
            m_objectiveFail?.Invoke(failed.Detail);
            CleanupObjective(failed);
            m_failed.Add(failed);
        }
        void CleanupObjective(GameObjective<T> completed)
        {
            completed.OnComplete -= OnObjectiveComplete;
            completed.OnFail -= OnObjectiveFail;
            completed?.Shutdown();
            m_active.Remove(completed);
        }
        protected void PrepareObjective(GameObjective<T> objective, bool isActive)
        {
            objective?.Init();
            if (isActive) 
            {
                objective.OnComplete += OnObjectiveComplete;
                objective.OnFail += OnObjectiveFail;
            }
        }
        protected void ShutdownObjective(GameObjective<T> objective)
        {
            objective?.Shutdown();
            objective.OnComplete -= OnObjectiveComplete;
            objective.OnFail -= OnObjectiveFail;
        }
    }
}