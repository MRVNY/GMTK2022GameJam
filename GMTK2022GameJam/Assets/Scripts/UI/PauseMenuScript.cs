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


    // Start is called before the first frame update
    void Start()
    {
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
                pauseMenuPanel.SetActive(true);
                buttonsPanel.SetActive(true);
            }
            else 
            {
                if(buttonsPanel.activeSelf)
                {
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
        buttonsPanel.SetActive(false);
        controlsPanel.SetActive(true);
    }
    public void OnAudioButtonSelect()
    {
        buttonsPanel.SetActive(false);
        audioPanel.SetActive(true);
    }
    public void OnGoToMenuButtonSelect()
    {
        SceneManagerScript.Instance.LoadMainMenu();
    }
}
