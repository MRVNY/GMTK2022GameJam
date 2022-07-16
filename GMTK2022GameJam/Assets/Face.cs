using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Face : MonoBehaviour
{
    private Dice dice;
    // Start is called before the first frame update
    void Start()
    {
        dice = transform.parent.GetComponent<Dice>();
    }

    // Update is called once per frame
    void Update()
    {
        // if(name=="Down")
        //     print(Mathf.Abs(transform.rotation.eulerAngles.z-180)<10);
        if (dice.downFace==null && Mathf.Abs(transform.rotation.eulerAngles.z-180)<10)
        {
            dice.downFace = gameObject;
            print(name);
        }
    }
}
