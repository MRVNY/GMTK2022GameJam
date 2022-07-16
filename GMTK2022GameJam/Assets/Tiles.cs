using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Tilemaps;

public class Tiles : MonoBehaviour
{
    public static Tilemap lower;
    public static Tilemap upper;
    public Dice dice;

    public static List<Vector3> availablePlaces;

    private static int validTiles = 0;
    // Start is called before the first frame update
    void Start()
    {
        var tileMaps = GetComponentsInChildren<Tilemap>();
        upper = tileMaps[0];
        lower = tileMaps[1];
        Assert.IsTrue(upper.gameObject.name.Equals("Upper"));

        availablePlaces = new List<Vector3>();
 
        for (int n = lower.cellBounds.xMin; n < lower.cellBounds.xMax; n++)
        {
            for (int p = lower.cellBounds.yMin; p < lower.cellBounds.yMax; p++)
            {
                Vector3Int localPlace = (new Vector3Int(n, p, (int)lower.transform.position.y));
                lower.SetTileFlags(localPlace, TileFlags.None);
                Vector3 place = lower.CellToWorld(localPlace);
                if (lower.HasTile(localPlace))
                {
                    //Tile at "place"
                    availablePlaces.Add(place);
                }
            }
        }
        
        
        print(availablePlaces.Count);
    }


    public static void UpdateTile()
    {
        Vector3 offset = new Vector3(0.7f, 0, 0.7f);
        if (Dice.downFace != null)
        {
            var tilePos = lower.WorldToCell(Dice.downFace.transform.position);
            var previousColor = lower.GetColor(tilePos);
            var goalColor = upper.GetColor(tilePos);

            if (Color.Equals(previousColor, Dice.downFace.color)) // nothing has changed
            {
                return;
            }

            lower.SetColor(tilePos, Dice.downFace.color);

            if (Color.Equals(Dice.downFace.color, goalColor)) // changed to good Color
            {
                validTiles++;

                if(validTiles == availablePlaces.Count)
                {
                    Debug.Log("Success !!! The level is done my friend !");
                }
            }
            else //the Color has changed from goal Color to a wrong Color
            {
                validTiles--;
            }
        }

    }

}
