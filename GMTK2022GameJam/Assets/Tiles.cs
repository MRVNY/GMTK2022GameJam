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

    private static int wrongTiles = 0;
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

        UpdateTile();
        wrongTiles=availablePlaces.Count;
        print(availablePlaces.Count);
    }


    public static void UpdateTile()
    {
        Vector3 offset = new Vector3(0.7f, 0, 0.7f);
        if (Dice.downFace != null)
        {
            //gerer le cas où on passe d'une non couleur a une mauvaise couleur
            var tilePos = current.WorldToCell(Dice.downFace.transform.position);
            var previousColor = current.GetColor(tilePos);
            var goalColor = goal.GetColor(tilePos);

            if (ColorEquals(previousColor, Dice.downFace.color)) // nothing has changed
            {
                return;
            }

            current.SetColor(tilePos, Dice.downFace.color);
            if (!ColorEquals(previousColor, Dice.downFace.color)) // has changed 
            {
                if (ColorEquals(previousColor, goalColor) && !ColorEquals(goalColor, Dice.downFace.color)) //was valid before and is not valid now
                {
                    //Debug.Log("Wrong color association");
                    wrongTiles++;
                }
                else if (!ColorEquals(previousColor, goalColor) && ColorEquals(goalColor, Dice.downFace.color)) //wasnt valid before and is valid now
                {
                    //Debug.Log("Good color association");
                    wrongTiles--;

                    if (wrongTiles == 0)
                    {
                        OnWinEvent();
                    }
                }

            }
            //Debug.Log("wrongTiles = " + wrongTiles);
        }

    }

    public static bool ColorEquals(Color a, Color b)
    {
        var eps = 0.1f;
        /*Debug.Log(a.ToString());
        Debug.Log(b.ToString());
        Debug.Log(a.r + " , " + b.r + " , " + a.g + " , " + b.g + " , " + a.b + " , " + b.b + " , " + a.a + " , " + b.a);*/
        return (Mathf.Abs(a.r - b.r) + Mathf.Abs(a.g - b.g) + Mathf.Abs(a.b - b.b) + Mathf.Abs(a.a - b.a)) < eps;
    }

    private static void OnWinEvent()
    {
        Debug.Log("Success !!! The level is done my friend !");
        new WaitForSeconds(5f);
        SceneManagerScript.LoadNextlevel();
    }
}
