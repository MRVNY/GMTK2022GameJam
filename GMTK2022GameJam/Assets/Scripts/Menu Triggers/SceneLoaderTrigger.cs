using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoaderTrigger : MonoBehaviour
{
    public string nameSceneToLoad;
    private void OnTriggerEnter(Collider other)
    {
        print(nameSceneToLoad + " trigger activated");
        StartCoroutine(SceneManagerScript.Instance.LoadSceneWithDelay(nameSceneToLoad,0.7f));
    }
}
