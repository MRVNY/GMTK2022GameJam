using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance { get; private set; }
    public bool IsGamePaused => _isGamePaused;

    private bool _isGamePaused;
    private int _pauseQueryPriority = int.MinValue;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("Two instances of singleton PauseManager.cs script were created. \nDestroying this instance");
            Destroy(this.gameObject);
        }
    }

    public void SetGameInPause(bool enablePause, int priorityOrder)
    {
        if(priorityOrder >= _pauseQueryPriority) 
        {
            _isGamePaused = enablePause;
            if(enablePause)
            {
                Time.timeScale = 0.0f;
                //store priority of the script pausing the game for later comparison
                _pauseQueryPriority = priorityOrder;
            }
            else
            {
                Time.timeScale = 1.0f;
                //clear _pauseQueryPriority value for later comparison
                _pauseQueryPriority = int.MinValue;
            }
        }
        else
        {
            print("SetPauseState call ignored because an other script pause the game with higher priority");
        }
    }
}
