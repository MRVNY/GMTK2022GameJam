using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Tilemaps;

public class Tiles : MonoBehaviour
{
    public static Tiles Instance { get; private set; }
    public static Tilemap current;
    public static Tilemap goal;

    private List<Vector3> availablePlaces;

    private static int _wrongTiles = 0;
    // Start is called before the first frame update
    public void OnEnable()
    {
        if (Instance==null)
        {
            Instance = this;
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
                if (goal.HasTile(localPlace))
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
            if (previousColor!=Color.white && !ColorEquals( previousColor, downFace.color)) // has changed 
            {
                if (ColorEquals(previousColor, goalColor) && !ColorEquals(goalColor, downFace.color)) //was valid before and is not valid now
                {
                    //Debug.Log("Wrong color association");
                    // if(_wrongTiles != availablePlaces.Count) StartCoroutine(FailEffect(downFace));
                    _wrongTiles++;
                }
                else if (!ColorEquals(previousColor, goalColor) && ColorEquals(goalColor, downFace.color)) //wasnt valid before and is valid now
                {
                    //Debug.Log("Good color association");
                    if(_wrongTiles != availablePlaces.Count) StartCoroutine(SuccessEffect(downFace));
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

    IEnumerator SuccessEffect(Face face)
    {
        GameObject effect = Instantiate(Dice.Instance.StarsEffect, face.transform.position, Quaternion.Euler(90,0,0));
        ParticleSystem.MainModule PSMain = effect.GetComponent<ParticleSystem>().main;
        PSMain.startColor = new ParticleSystem.MinMaxGradient(face.color);
        
        yield return new WaitForSeconds(1); 
        Destroy(effect);
    }
    
    IEnumerator FailEffect(Face face)
    {
        GameObject effect = Instantiate(Dice.Instance.StarsEffect, face.transform.position, Quaternion.Euler(90,0,0));
        
        yield return new WaitForSeconds(1); 
        Destroy(effect);
    }


}
