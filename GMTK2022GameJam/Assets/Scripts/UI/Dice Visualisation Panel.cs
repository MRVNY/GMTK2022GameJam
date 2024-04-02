using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceVisualisationPanel : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            Time.timeScale = 1.0f;
            gameObject.SetActive(false);
        }
    }
}
