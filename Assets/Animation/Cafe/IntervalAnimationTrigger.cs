using UnityEngine;
using System.Collections.Generic;
using System.Collections;
[RequireComponent (typeof(Animator))]
// plays a list of animation every now and then, each animation option is sampled uniformly
public class IntervalAnimationTrigger : MonoBehaviour
{
    [Range(0f, 999f)]
    [SerializeField] float m_medianIntervalInSeconds = default;
    [Range(0f, 998.9f)]
    [SerializeField] float m_intervalRange = default;
    [SerializeField] List<AnimationClip> m_randomAnimsToPlay = default;
    float MinInterval => m_medianIntervalInSeconds - m_intervalRange;
    float MaxInterval => m_medianIntervalInSeconds + m_intervalRange;
    public void OnEnable() 
    {
        StartCoroutine(Play());
    }
    public void OnDisable() 
    { 
        StopAllCoroutines();
    }
    IEnumerator Play() 
    {
        if (m_randomAnimsToPlay.Count == 0) yield break;
        int toPlay = 0;
        while (gameObject.activeSelf) 
        {
            yield return new WaitForSeconds (Random.Range(MinInterval, MaxInterval));
            toPlay = Random.Range(0, m_randomAnimsToPlay.Count);
            GetComponent<Animator>().Play(m_randomAnimsToPlay[toPlay].name);
        }
    }
}
