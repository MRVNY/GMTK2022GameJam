using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuLevelSelectionTriggerScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("menu level selector triggered");
        StartCoroutine(SceneManagerScript.Instance.LoadLevelSelectorSceneWithDelay(2f));
    }

}
