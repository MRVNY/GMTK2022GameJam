using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsTriggerScript : MonoBehaviour
{
    public Map map;
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        print("ON TRIGGER ENTER");
        map.ShowCredit();
    }
}
