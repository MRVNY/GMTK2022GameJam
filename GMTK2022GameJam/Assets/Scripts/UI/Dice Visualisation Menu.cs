using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceVisualisationMenu : MonoBehaviour
{

    [SerializeField]
    private GameObject panel;

    private const int _pausePriorityValue = 0;
    private bool _fadeInStarted;

    // Start is called before the first frame update
    void Start()
    {
        PauseManager.Instance.SetGameInPause(true, 1);
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
        {
            PauseManager.Instance.SetGameInPause(false, 1);
            panel.SetActive(false);
        }
    }
}
