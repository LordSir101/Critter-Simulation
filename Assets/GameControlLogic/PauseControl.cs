using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseControl : MonoBehaviour
{
    public static bool gameIsPaused = false;

    // void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.Escape))
    //     {
    //         gameIsPaused = !gameIsPaused;
    //         PauseGame();
    //     }
    // }

    public static void PauseGame(bool pause)
    {
        if(pause)
        {
            Time.timeScale = 0f;
        }
        else 
        {
            Time.timeScale = 1;
        }
    }
}
