using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{
    [SerializeField]
    private GameObject moveInstructionsUI;
    [SerializeField]
    private GameObject resetInstructionsUI;
    [SerializeField]
    private GameObject diceAdviceUI;
    [SerializeField]
    private GameObject levelDoneUI;
    [SerializeField]
    private GameObject startUI;

    private int _firstLevelSceneIndex = 2;
    private int _levelSelectorSceneIndex = 1;
    public static SceneManagerScript Instance { get; private set; }

    public static bool IsGameOver { get; set; } = false;
    // Start is called before the first frame update
    void Start()
    {
        if (Instance==null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("Two instances of singletin SceneManagerScript.cs script were created. \nDestroying this instance");
            Destroy(this.gameObject);
        }

        LevelSpecificEvents();
    }

    // Update is called once per frame
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.N) && (IsGameOver))
        {
            LoadNextLevel();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            ReloadScene();
        }
    }

    public  void ReloadScene()
    {
        Scene scene = SceneManager.GetActiveScene(); 
        SceneManager.LoadScene(scene.name);
    }


    public void LoadLevelSelectorScene()
    {
        SceneManager.LoadScene(_levelSelectorSceneIndex);
    }
    public void LoadLevel(int levelId)
    {
        SceneManager.LoadScene(_firstLevelSceneIndex+levelId);
    }


    public void LoadNextLevel()
    {
        IsGameOver = false;

        if (SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCountInBuildSettings -1)
        {
            Global.ended = true;
            SceneManager.LoadScene(0);
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void LevelSpecificEvents()
    {
        Scene scene = SceneManager.GetActiveScene();
        switch (scene.name)
        {
            case "Level_1":
                moveInstructionsUI.SetActive(true);
                break;
            case "Level_2":
                diceAdviceUI.SetActive(true);
                break;
            case "Level_3":
                resetInstructionsUI.SetActive(true);
                break;
            default:
                break;
        }
    }
    
    public void OnWinEvent()
    {
        CleanUI();
        print("Success !!! The level is done my friend !");
        levelDoneUI.SetActive(true);
        IsGameOver = true;
        StartCoroutine((LoadNextLevelWithDelay(3f)));
    }

    private void CleanUI()
    {
        startUI.SetActive(false);
        resetInstructionsUI.SetActive(false);
        moveInstructionsUI.SetActive(false);
        diceAdviceUI.SetActive(false);
        levelDoneUI.SetActive(false);
    }
    public IEnumerator LoadLevelWithDelay(int levelId, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManagerScript.Instance.LoadLevel(levelId);
    }
    public IEnumerator LoadNextLevelWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManagerScript.Instance.LoadNextLevel();
    }

    public IEnumerator LoadMainMenuWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(0);
    }
    public IEnumerator LoadLevelSelectorSceneWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManagerScript.Instance.LoadLevelSelectorScene();
    }
}
