using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEndPanelManager : MonoBehaviour
{
    [SerializeField] private GameObject panel;

    public void OpenPanel()
    {
        panel.SetActive(true);
        PauseControl.PauseGame(true);
    }

    public void EndGame()
    {
        MenuInput.Reset();
        PlayerGameInfo.Reset();
        SceneManager.LoadSceneAsync("MainMenu");
    }

    
}
