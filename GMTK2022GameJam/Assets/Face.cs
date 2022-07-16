using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Face : MonoBehaviour
{
    private Dice dice;

    public Color color;
    // Start is called before the first frame update
    void Start()
    {
        dice = transform.parent.GetComponent<Dice>();
        color = GetComponent<MeshRenderer>().material.color;
    }

    // Update is called once per frame
    void Update()
    {
        // if(name=="Down")
        //     print(Mathf.Abs(transform.rotation.eulerAngles.z-180)<10);
        if (Dice.downFace==null && Mathf.Abs(transform.rotation.eulerAngles.z-180)<10)
        {
            Dice.downFace = this;
            Tiles.UpdateTile();
        }
    }
}
