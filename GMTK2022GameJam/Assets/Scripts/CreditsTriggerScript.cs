using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsTriggerScript : MonoBehaviour
{
    public Map map;

    private bool detect = true;
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (detect)
        {
            print("collision");
            detect = false;
            map.ShowCredit();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.GetComponent<Dice>()?.isActiveAndEnabled== true)
        {
            detect = true;
            print("HERE");
        }
    }
}
