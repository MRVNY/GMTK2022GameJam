using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartTriggerScript : MonoBehaviour
{
     private void OnTriggerEnter(Collider other)
     {
         Debug.Log("start triggered");
         StartCoroutine(SceneManagerScript.Instance.LoadLevelWithDelay(0,0.7f));
     }
 
}
