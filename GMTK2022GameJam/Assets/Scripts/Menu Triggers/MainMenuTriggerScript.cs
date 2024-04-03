using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenuTriggerScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        print("main menu triggered");
        StartCoroutine(SceneManagerScript.Instance.LoadMainMenuWithDelay(0.7f));
    }
}
