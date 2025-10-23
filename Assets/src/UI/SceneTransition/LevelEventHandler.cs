using Curry.Events;
using System;
using UnityEditor;
using UnityEngine;
// Persistent event listener to handle scene changes
[RequireComponent(typeof(LevelLoader))]
public class LevelEventHandler : MonoBehaviour 
{
    [SerializeField] CurryGameEventListener m_newGame = default;
    [SerializeField] CurryGameEventListener m_continueGame = default;
    [SerializeField] CurryGameEventListener m_returnToTitle = default;
    LevelLoader Loader => GetComponent<LevelLoader>();
    private void Start()
    {
        m_newGame?.Init();
        m_continueGame?.Init();
        m_returnToTitle?.Init();
    }
    public void ContinueGame() 
    {
        // Load save and start game
        GoToGameScene();
    }
    public void NewGame() 
    {
        // Start game from the start
        GoToGameScene();
    }
    public void ReturnToTitle()
    {
        Loader?.LoadScene(1);
    }
    public void QuitGame()
    {
        if (Application.isEditor)
        {
            EditorApplication.ExitPlaymode();
        }
        else
        {
            Application.Quit();
        }
    }
    void GoToGameScene() 
    {
        Loader?.LoadScene(2);
    }
}
