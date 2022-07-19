using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapScript : MonoBehaviour
{
    public Tilemap tm;
    public Grid gr;
    public Vector3Int pos;
    // Start is called before the first frame update
    void Awake()
    {
        tm.SetTileFlags(pos, TileFlags.None);
        tm.SetColor(pos,Color.black);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
