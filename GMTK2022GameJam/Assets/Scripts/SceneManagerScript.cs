using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;

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

    // Start is called before the first frame update
    void Start()
    {
        if (Instance==null)
        {
            Instance = this;
        }
        else
        {
        }

        LevelSpecificEvents();

        nbOfMovements = 0;
        DiceEventSystem.TriggerDiceMove += ListenToDiceMoving;
    }

    // Update is called once per frame
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.N) && (IsGameOver))
        {
            LoadNextlevel();
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

    public void LoadNextlevel()
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
    public IEnumerator LoadNextLevelWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManagerScript.Instance.LoadNextlevel();
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
