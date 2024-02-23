using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

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

    [SerializeField] private TextMeshProUGUI frontNbMove;
    [SerializeField] private TextMeshProUGUI middleNbMove;
    [SerializeField] private TextMeshProUGUI backNbMove;
    [SerializeField] private string nbOfMovementsVirginText;
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
        DisplayNumberOfMovement();
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

    private void DisplayNumberOfMovement()
    {
        string textFiller = nbOfMovementsVirginText + " " + nbOfMovements;
        backNbMove.text = textFiller;
        frontNbMove.text = textFiller;
        middleNbMove.text = textFiller;
    }

    private void OnDestroy()
    {
        DiceEventSystem.TriggerDiceMove -= ListenToDiceMoving;
    }
}
