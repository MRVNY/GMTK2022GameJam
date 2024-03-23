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

    public static PauseMenuScript Instance { get; private set; }

    public bool IsInPause => pauseMenuPanel.activeInHierarchy;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("Two instances of singletin PauseMenuScript.cs script were created. \nDestroying this instance");
            Destroy(this.gameObject);
        }
    }
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
                Time.timeScale = 0.0f;
            }
            else 
            {
                
                if(buttonsPanel.activeSelf)
                {
                    Time.timeScale = 1.0f;
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
        Time.timeScale = 1.0f;
        SceneManagerScript.Instance.LoadMainMenu();
    }
}
