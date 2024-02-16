using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Tilemaps;

public class Tiles : MonoBehaviour
{
    public static Tiles Instance { get; private set; }
    private Tilemap _current;
    private Tilemap _goal;

    private List<Vector3> availablePlaces;

    private static int _wrongTiles = 0;
    public void OnEnable()
    {
        if (Instance==null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("Two instances of singletin Tiles.cs script were created. \nDestroying this instance");
            Destroy(this.gameObject);
        }
        var tileMaps = GetComponentsInChildren<Tilemap>();
        _goal = tileMaps[0];
        Assert.IsTrue(_goal.gameObject.name.Equals("Goal"));
        _current = tileMaps[1];
        Assert.IsTrue(_current.gameObject.name.Equals("Current"));


        availablePlaces = new List<Vector3>();
 
        for (int n = _current.cellBounds.xMin; n < _current.cellBounds.xMax; n++)
        {
            for (int p = _current.cellBounds.yMin; p < _current.cellBounds.yMax; p++)
            {
                Vector3Int localPlace = (new Vector3Int(n, p, (int)_current.transform.position.y));
                _current.SetTileFlags(localPlace, TileFlags.None);
                Vector3 place = _current.CellToWorld(localPlace);
                if (_goal.HasTile(localPlace))
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
            var tilePos = _current.WorldToCell(downFace.transform.position);
            var previousColor = _current.GetColor(tilePos);
            var _goalColor = _goal.GetColor(tilePos);

            if (ColorEquals(previousColor, downFace.color)) // nothing has changed
            {
                return;
            }

            _current.SetColor(tilePos, downFace.color);
            if (previousColor!=Color.white && !ColorEquals( previousColor, downFace.color)) // has changed 
            {
                if (ColorEquals(previousColor, _goalColor) && !ColorEquals(_goalColor, downFace.color)) //was valid before and is not valid now
                {
                    //Debug.Log("Wrong color association");
                    _wrongTiles++;
                }
                else if (!ColorEquals(previousColor, _goalColor) && ColorEquals(_goalColor, downFace.color)) //wasnt valid before and is valid now
                {
                    //Debug.Log("Good color association");
                    _wrongTiles--;

                    if (_wrongTiles == 0)
                    {
                        SceneManagerScript.Instance.OnWinEvent();
                    }
                }
            }
            //Debug.Log("wrongTiles = " + _wrongTiles);
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
    


}
