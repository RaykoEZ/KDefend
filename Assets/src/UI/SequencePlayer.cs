using Curry.Events;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public delegate void OnSequenceFinish();
//Plays visual sequences in the scene
public class SequencePlayer : MonoBehaviour
{
    [Range(0f, 10f)]
    [SerializeField] protected float m_waitAfterSequenceFinishes = default;
    [SerializeField] protected PlayableDirector m_director = default;
    public event OnSequenceFinish OnFinish;
    // Start is called before the first frame update
    public virtual void PlaySequence() 
    {
        if (m_director.playableAsset == null) 
        {
            Debug.LogWarning("SequencePlayer: PlayableAsset is not set, cannot play sequence.");
            return;
        }
        StartCoroutine(PlaySequence_Internal(m_director.playableAsset));
    }
    public void PlaySequence(PlayableAsset toPlay)
    {
        if (toPlay == null) return;
        StartCoroutine(PlaySequence_Internal(toPlay));
    }
    protected void OnFinishCallback() 
    {
        OnFinish?.Invoke();
    }
    protected virtual IEnumerator PlaySequence_Internal(PlayableAsset toPlay) 
    {
        GameUtil.PlaySequence(m_director, toPlay);
        yield return new WaitForSeconds((float)m_director.playableAsset.duration);
        yield return new WaitForSeconds(m_waitAfterSequenceFinishes);
        OnFinish?.Invoke();
    }
}
