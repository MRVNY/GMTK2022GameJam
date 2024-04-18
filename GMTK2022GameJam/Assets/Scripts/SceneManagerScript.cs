using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;
using System.Linq;

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

    private const int _firstLevelBuildIndex = 6;
    private const int _levelSelectorSceneIndex = 1;
    private const string maxCompletedLevelVarName = "maxCompletedLevel";
    public static SceneManagerScript Instance { get; private set; }

    public static bool IsGameOver { get; set; } = false;

    [SerializeField] private TextMeshProUGUI frontNewBestNbMove;
    [SerializeField] private TextMeshProUGUI middleNewBestNbMove;
    [SerializeField] private TextMeshProUGUI backNewBestNbMove;
    [SerializeField] private string newBestNbOfMovementsVirginText;
    [SerializeField] private TextMeshProUGUI frontBestNbMove;
    [SerializeField] private TextMeshProUGUI middleBestNbMove;
    [SerializeField] private TextMeshProUGUI backBestNbMove;
    [SerializeField] private string bestNbOfMovementsVirginText;
    [SerializeField] private TextMeshProUGUI frontNewNbMove;
    [SerializeField] private TextMeshProUGUI middleNewNbMove;
    [SerializeField] private TextMeshProUGUI backNewNbMove;
    [SerializeField] private string newNbOfMovementsVirginText;
    private int nbOfMovements;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("Two instances of singletin SceneManagerScript.cs script were created. \nDestroying this instance");
            Destroy(this.gameObject);
        }
    }
    void Start()
    {
        LevelSpecificEvents();

        nbOfMovements = 0;
        DiceEventSystem.TriggerDiceMove += ListenToDiceMoving;
    }

    // Update is called once per frame
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && (IsGameOver))
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
        SceneManager.LoadScene(_firstLevelBuildIndex + levelId);
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
        EndOfLevelScoreManagement();
        IsGameOver = true;

        int levelId = GetCurrentLevelId();
        UpdateMaxLevelCompletedId(levelId); 
        StartCoroutine((LoadNextLevelWithDelay(3f)));
    }


    //can be called in a level to know which is this level rank in the game progression ( first,second,tenth,etc... )
    public int GetCurrentLevelId()
    {
        print("this is level : " + (SceneManager.GetActiveScene().buildIndex - _firstLevelBuildIndex));
        return SceneManager.GetActiveScene().buildIndex - _firstLevelBuildIndex;
    }
    public int GetMaxLevelCompletedId()
    {
        return PlayerPrefs.GetInt(maxCompletedLevelVarName,-1);
    }
    public void UpdateMaxLevelCompletedId(int completedLevelId)
    {
        PlayerPrefs.SetInt(maxCompletedLevelVarName, completedLevelId);
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
        LoadMainMenu();
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public IEnumerator LoadSceneWithDelay(string sceneName, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneName);
    }
    public IEnumerator LoadLevelSelectorSceneWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManagerScript.Instance.LoadLevelSelectorScene();
    }

    private void ListenToDiceMoving()
    {
        nbOfMovements++;
    }


    private void EndOfLevelScoreManagement()
    {
        int previousBestScore = LoadScore();
        Debug.Log("Best : " + previousBestScore);
        Debug.Log("New : " + nbOfMovements);
        if (previousBestScore > nbOfMovements)
        {
            Debug.Log("New Save");
            SaveScore(nbOfMovements);
            DisplayNumberOfMovementNewBestScore();
        }
        else
        {
            Debug.Log("No Save");
            DisplayNumberOfMovementBestAndNewScore(previousBestScore);
        }
    }


    private void DisplayNumberOfMovementNewBestScore()
    {
        backNewBestNbMove.gameObject.SetActive(true);
        backNewBestNbMove.gameObject.SetActive(true);
        backNewBestNbMove.gameObject.SetActive(true);
        backBestNbMove.gameObject.SetActive(false);
        backBestNbMove.gameObject.SetActive(false);
        backBestNbMove.gameObject.SetActive(false);
        backNewNbMove.gameObject.SetActive(false);
        backNewNbMove.gameObject.SetActive(false);
        backNewNbMove.gameObject.SetActive(false);
        string textFiller = newBestNbOfMovementsVirginText + " " + nbOfMovements;
        backNewBestNbMove.text = textFiller;
        frontNewBestNbMove.text = textFiller;
        middleNewBestNbMove.text = textFiller;
    }

    private void DisplayNumberOfMovementBestAndNewScore(int bestNB)
    {
        backNewBestNbMove.gameObject.SetActive(false);
        backNewBestNbMove.gameObject.SetActive(false);
        backNewBestNbMove.gameObject.SetActive(false);
        backBestNbMove.gameObject.SetActive(true);
        backBestNbMove.gameObject.SetActive(true);
        backBestNbMove.gameObject.SetActive(true);
        backNewNbMove.gameObject.SetActive(true);
        backNewNbMove.gameObject.SetActive(true);
        backNewNbMove.gameObject.SetActive(true);
        string textFillerNew = newNbOfMovementsVirginText + " " + nbOfMovements;
        backNewNbMove.text = textFillerNew;
        frontNewNbMove.text = textFillerNew;
        middleNewNbMove.text = textFillerNew;
        string textFillerBest = bestNbOfMovementsVirginText + " " + bestNB;
        backBestNbMove.text = textFillerBest;
        frontBestNbMove.text = textFillerBest;
        middleBestNbMove.text = textFillerBest;
    }


    private void SaveScore(int score)
    {
        if(! Directory.Exists(Application.dataPath + "/Scores"))
        {
            Directory.CreateDirectory(Application.dataPath + "/Scores");
        }
        ScoreData newScore = new ScoreData();
        newScore.score = score;

        string jsonScore = JsonUtility.ToJson(newScore);
        File.WriteAllText(Application.dataPath + "/Scores/" + SceneManager.GetActiveScene().name + ".json", jsonScore);
    }


    /*
    public List<int> LoadAllBestScores(int levelIdMin, int levelIdMax)
    {
        List<int> scores = new List<int>();
        if (!Directory.Exists(Application.dataPath + "/Scores"))
        {
            return null;
        }
        for (int i = levelIdMin; i<=levelIdMax;i++)
        {
            if(!File.Exists(Application.dataPath + "/Scores/" + SceneManager.GetActiveScene().name + ".json"))
            {
                scores[i] = -1;
                return scores;
            }
            else
            {
                string scoreJson = File.ReadAllText(Application.dataPath + "/Scores/" + SceneManager.GetActiveScene().name + ".json");
                ScoreData bestScore = JsonUtility.FromJson<ScoreData>(scoreJson);
                scores[i] = bestScore.score;
            }
        }
        return scores;
    }*/

    public int LoadScore(int levelId)
    {
        string scenePath = SceneUtility.GetScenePathByBuildIndex(_firstLevelBuildIndex + levelId);
        if (scenePath == "")
        {
            Debug.LogError("Could not load data for level with levelId = " + levelId);
            return -1;
        }
        string sceneName = scenePath.Split("/")[^1];
        sceneName = sceneName.Split(".")[0];

        string scoreJson = File.ReadAllText(Application.dataPath + "/Scores/" + sceneName + ".json");
        ScoreData bestScore = JsonUtility.FromJson<ScoreData>(scoreJson);
        return bestScore.score;
    }

    private int LoadScore()
    {
        if (!Directory.Exists(Application.dataPath + "/Scores"))
        {
            Directory.CreateDirectory(Application.dataPath + "/Scores");
        }
        if (!File.Exists(Application.dataPath + "/Scores/" + SceneManager.GetActiveScene().name + ".json"))
        {
            return int.MaxValue;
        }
        string scoreJson = File.ReadAllText(Application.dataPath + "/Scores/" + SceneManager.GetActiveScene().name + ".json");
        ScoreData bestScore = JsonUtility.FromJson<ScoreData>(scoreJson);
        return bestScore.score;
    }

    private void OnDestroy()
    {
        DiceEventSystem.TriggerDiceMove -= ListenToDiceMoving;
    }
}
