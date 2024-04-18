using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuScript : MonoBehaviour
{
    [SerializeField]
    private GameObject pauseMenuPanel;

    [SerializeField]
    private GameObject buttonsPanel;

    [SerializeField]
    private GameObject controlsPanel;

    [SerializeField]
    private GameObject audioPanel;

    public bool IsInPause => pauseMenuPanel.activeInHierarchy;

    private const int _pausePriorityValue = 0;
    void Start()
    {
        //reset panels
        controlsPanel.SetActive(false);
        audioPanel.SetActive(false);
        buttonsPanel.SetActive(true);
        pauseMenuPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(!IsVisible())
            {
                //show pause menu
                pauseMenuPanel.SetActive(true);
                buttonsPanel.SetActive(true);
                PauseManager.Instance.SetGameInPause(true, _pausePriorityValue);
            }
            else 
            {
                
                if(buttonsPanel.activeSelf)
                {
                    PauseManager.Instance.SetGameInPause(false, _pausePriorityValue);
                    pauseMenuPanel.SetActive(false);
                }
                else
                {
                    controlsPanel.SetActive(false);
                    audioPanel.SetActive(false);
                    buttonsPanel.SetActive(true);
                }
            }
        }
    }

    private bool IsVisible()
    {
        return pauseMenuPanel.activeSelf;
    }

    public void OnControlsButtonSelect()
    {
        print("OnControlsButtonSelect");

        buttonsPanel.SetActive(false);
        controlsPanel.SetActive(true);
    }
    public void OnAudioButtonSelect()
    {
        print("OnAudioButtonSelect");
        buttonsPanel.SetActive(false);
        audioPanel.SetActive(true);
    }
    public void OnGoToMenuButtonSelect()
    {
        print("OnGoToMenuButtonSelect");
        PauseManager.Instance.SetGameInPause(false, _pausePriorityValue);
        SceneManagerScript.Instance.LoadMainMenu();
    }

}
