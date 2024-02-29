using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoaderTrigger : MonoBehaviour
{
    public string nameSceneToLoad;
    public float delay = 2.0f;
    private void OnTriggerEnter(Collider other)
    {
        print(nameSceneToLoad + " trigger activated");
        StartCoroutine(SceneManagerScript.Instance.LoadSceneWithDelay(nameSceneToLoad, delay));
    }
}
