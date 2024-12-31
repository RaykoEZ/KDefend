using UnityEngine;
using UnityEngine.Events;

namespace Curry.Explore
{
    [RequireComponent(typeof(Animator))]
    public class HideableUI : MonoBehaviour 
    {
        [SerializeField] UnityEvent m_onShowEvent = default;
        [SerializeField] UnityEvent m_onHideEvent = default;
        protected Animator GetAnim => GetComponent<Animator>();
        public virtual void Show() 
        {
            GetAnim?.SetBool("Show", true);
            m_onShowEvent?.Invoke();
        }
        public virtual void Hide() 
        {
            GetAnim?.SetBool("Show", false);
            m_onHideEvent?.Invoke();
        }
        public virtual void Toggle() 
        {
            bool current = GetAnim.GetBool("Show");
            GetAnim?.SetBool("Show", !current);
            if (current) 
            {
                m_onHideEvent?.Invoke();
            }
            else 
            {
                m_onShowEvent?.Invoke();
            }
        }
    }
}