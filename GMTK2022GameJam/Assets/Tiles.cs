using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Tilemaps;

public class Tiles : MonoBehaviour
{
    public static Tiles Instance { get; private set; }
    public static Tilemap current;
    public static Tilemap goal;
    public Dice dice;

    public static List<Vector3> availablePlaces;

    public GameObject onWinUI;
    private static int _wrongTiles = 0;
    // Start is called before the first frame update
    public void OnEnable()
    {
        if (Instance==null)
        {
            Instance = this;
        }
        else
        {
            throw new InvalidImplementationException("You should not try to instantiate a singleton twice !");
        }
        var tileMaps = GetComponentsInChildren<Tilemap>();
        goal = tileMaps[0];
        Assert.IsTrue(goal.gameObject.name.Equals("Goal"));
        current = tileMaps[1];
        Assert.IsTrue(current.gameObject.name.Equals("Current"));


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
        
        _wrongTiles=availablePlaces.Count;
        //print(availablePlaces.Count);
    }


    public void UpdateTile(Face downFace)
    {
        
        Vector3 offset = new Vector3(0.7f, 0, 0.7f);
        if (downFace != null)
        {
            //gerer le cas o� on passe d'une non couleur a une mauvaise couleur
            var tilePos = current.WorldToCell(downFace.transform.position);
            var previousColor = current.GetColor(tilePos);
            var goalColor = goal.GetColor(tilePos);

            if (ColorEquals(previousColor, downFace.color)) // nothing has changed
            {
                return;
            }

            current.SetColor(tilePos, downFace.color);
            if (!ColorEquals(previousColor, downFace.color)) // has changed 
            {
                if (ColorEquals(previousColor, goalColor) && !ColorEquals(goalColor, downFace.color)) //was valid before and is not valid now
                {
                    //Debug.Log("Wrong color association");
                    _wrongTiles++;
                }
                else if (!ColorEquals(previousColor, goalColor) && ColorEquals(goalColor, downFace.color)) //wasnt valid before and is valid now
                {
                    //Debug.Log("Good color association");
                    _wrongTiles--;

                    if (_wrongTiles == 0)
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

    private void OnWinEvent()
    {
        print("Success !!! The level is done my friend !");
        onWinUI.SetActive(true);
        SceneManagerScript.IsGameOver = true;
        StartCoroutine((LoadNextLevel()));
    }

    IEnumerator LoadNextLevel()
    {
        yield return new WaitForSeconds(10f);
        SceneManagerScript.Instance.LoadNextlevel();
    }
}
