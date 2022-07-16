using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Tilemaps;

public class Tiles : MonoBehaviour
{
    public static Tilemap current;
    public static Tilemap goal;
    public Dice dice;

    public static List<Vector3> availablePlaces;

    private static int validTiles = 0;
    // Start is called before the first frame update
    void Start()
    {
        var tileMaps = GetComponentsInChildren<Tilemap>();
        goal = tileMaps[0];
        current = tileMaps[1];
        Assert.IsTrue(goal.gameObject.name.Equals("Goal"));

        availablePlaces = new List<Vector3>();
 
        for (int n = current.cellBounds.xMin; n < current.cellBounds.xMax; n++)
        {
            for (int p = current.cellBounds.yMin; p < current.cellBounds.yMax; p++)
            {
                Vector3Int localPlace = (new Vector3Int(n, p, (int)current.transform.position.y));
                current.SetTileFlags(localPlace, TileFlags.None);
                Vector3 place = current.CellToWorld(localPlace);
                if (current.HasTile(localPlace))
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
            var tilePos = current.WorldToCell(Dice.downFace.transform.position);
            var previousColor = current.GetColor(tilePos);
            var goalColor = goal.GetColor(tilePos);

            if (Color.Equals(previousColor, Dice.downFace.color)) // nothing has changed
            {
                return;
            }

            current.SetColor(tilePos, Dice.downFace.color);

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
