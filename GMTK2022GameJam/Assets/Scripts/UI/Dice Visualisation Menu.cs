using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceVisualisationMenu : MonoBehaviour
{

    [SerializeField]
    private GameObject panel;

    [SerializeField]
    private GameObject newDicePanel;


    [SerializeField]
    private bool isNewDicePanelVisible;
    
    private const int _pausePriorityValue = 0;
    private bool _fadeInStarted;

    // Start is called before the first frame update
    void Start()
    {
        PauseManager.Instance.SetGameInPause(true, 1);

        newDicePanel.SetActive(isNewDicePanelVisible);
    }

    // Update is called once per frame
    void Update()
    {
        //Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return)
        
        if (Input.anyKeyDown && !Input.GetMouseButtonDown(0) && !Input.GetMouseButtonDown(1) && !Input.GetMouseButtonDown(2))
        {
            PauseManager.Instance.SetGameInPause(false, 1);
            panel.SetActive(false);
        }
    }
}
