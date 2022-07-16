using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Tiles : MonoBehaviour
{
    public static Tilemap tileMap;
    public Dice dice;

    public static List<Vector3> availablePlaces;
    // Start is called before the first frame update
    void Start()
    {
        tileMap = GetComponent<Tilemap>();
        
        availablePlaces = new List<Vector3>();
 
        for (int n = tileMap.cellBounds.xMin; n < tileMap.cellBounds.xMax; n++)
        {
            for (int p = tileMap.cellBounds.yMin; p < tileMap.cellBounds.yMax; p++)
            {
                Vector3Int localPlace = (new Vector3Int(n, p, (int)tileMap.transform.position.y));
                tileMap.SetTileFlags(localPlace, TileFlags.None);
                Vector3 place = tileMap.CellToWorld(localPlace);
                if (tileMap.HasTile(localPlace))
                {
                    //Tile at "place"
                    availablePlaces.Add(place);
                }
            }
        }
        
        
        print(availablePlaces.Count);
    }

    // Update is called once per frame
    public static void UpdateTile()
    {
        Vector3 offset = new Vector3(0.7f, 0, 0.7f);
        if (Dice.downFace != null)
        {
            foreach (var tile in availablePlaces)
            { 
                if(Vector3.Distance(tile + offset, Dice.downFace.transform.position) < 0.7f)
                {
                    // print(Vector3.Distance(tile, Dice.downFace.transform.position));
                    // print(tile);
                    tileMap.SetColor(tileMap.WorldToCell(tile), Dice.downFace.color);
                }
            }
        }
    }
}
