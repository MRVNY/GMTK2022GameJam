using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectTrigger : MonoBehaviour
{
    [SerializeField]
    private int levelId;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("level select triggered");
        StartCoroutine(SceneManagerScript.Instance.LoadLevelWithDelay(levelId,2f));
    }
}
