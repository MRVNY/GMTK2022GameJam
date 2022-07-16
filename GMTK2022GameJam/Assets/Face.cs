using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Face : MonoBehaviour
{
    public Color color;
    // Start is called before the first frame update
    void OnEnable()
    {
        color = GetComponent<MeshRenderer>().material.color;
    }

    public void AmIDownFace()
    {
        if (Mathf.Abs(transform.rotation.eulerAngles.z-180)<10)
        {
            Dice.downFaces.Add(this);
            Tiles.UpdateTile(this);
        }
    }
}
