using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Curry.Util;
public class LevelLoader : MonoBehaviour
{
    [SerializeField] CoroutineManager m_coroutine = default;
    [SerializeField] LoadingScreenSequencer m_loadScreen = default;
    bool m_inProgress = false;
    // Loads a single scene and sets it active 
    public void LoadScene(int sceneIndex, bool additive = false, bool setActive = true) 
    {
        if (sceneIndex < 0 || m_inProgress) 
        { 
            return; 
        }
        m_inProgress = true;
        m_coroutine.ScheduleCoroutine(LoadLevel_Internal(sceneIndex, additive, setActive));
        m_coroutine.StartScheduledCoroutines();
    }
    IEnumerator LoadLevel_Internal(int sceneIndex, bool additive = false, bool setActive = true) 
    {
        // play transition animaton
        yield return m_loadScreen.FadeOut();
        AsyncOperation op;
        // start scene loading
        op = SceneManager.LoadSceneAsync(
            sceneIndex,
            additive ? LoadSceneMode.Additive : LoadSceneMode.Single);
        op.allowSceneActivation = false;
        // transition to new scene when loading and animation are done
        if (setActive)
        {
            yield return StartCoroutine(SetLoadedScene(op));
        }
    }
    IEnumerator SetLoadedScene(AsyncOperation op) 
    {
        yield return m_loadScreen.FadeIn();
        // transition to new scene when loading and animation are done
        yield return new WaitUntil(() => op.progress >= 0.9f);
        yield return new WaitForSeconds(0.5f);
        op.allowSceneActivation = true;
        m_inProgress = false;
    }
}