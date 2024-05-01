using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Face : MonoBehaviour
{
    public Color color;
    void OnEnable()
    {
        color = GetComponent<MeshRenderer>().material.color;
    }

    public void AmIDownFace()
    {
        if (Mathf.Abs(transform.rotation.eulerAngles.z-180)<10
            && Mathf.Abs(transform.position.y) < 0.1f && Tiles.Instance != null)
        {
            Dice.downFaces.Add(this);
            Tiles.Instance.UpdateTile(this);
        }
    }

    private void OnTriggerStay(Collider col)
    {
        if (col.gameObject.CompareTag(gameObject.tag)) 
        {
            Dice.Instance.stick(col);
        }
    }
    
    
}
