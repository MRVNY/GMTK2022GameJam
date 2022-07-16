using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class LevelGrid : MonoBehaviour
{

    public GridCell[,] levelGrid { get; private set; }
    private float cellSize;
    [SerializeField]
    private Tilemap tm;
    // Start is called before the first frame update
    void Start()
    {
        var size = tm.size;
        levelGrid = new GridCell[size.x,size.y]; 
        for(int i = 0; i < size.x; i++)
        {
            for(int j = 0; j < size.y; j++)
            {
                levelGrid[i, j] = new GridCell(Color.white, tm.GetColor(new Vector3Int(i,j,0)));
                Debug.Log(new Vector2(i,j)+" = "+levelGrid[i, j].currColor);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
