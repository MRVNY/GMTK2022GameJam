using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapScript : MonoBehaviour
{
    public Tilemap tm;
    public Grid gr;
    public Vector3Int pos;
    public Color color;
    // Start is called before the first frame update
    void Start()
    {
        tm.SetColor(pos,color);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("gr.cellSize "+gr.cellSize);
        Debug.Log(tm.size);
    }
}
