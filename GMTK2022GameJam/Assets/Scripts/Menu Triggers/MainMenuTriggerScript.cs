using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuTriggerScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        print("main menu triggered");
        StartCoroutine(SceneManagerScript.Instance.LoadMainMenuWithDelay(2f));
    }
}
