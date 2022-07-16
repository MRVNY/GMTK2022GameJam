using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "LevelGrid", menuName = "Custom")]
public class LevelGrid : ScriptableObject
{

    public GridCell[,] levelGrid { get; private set; }
    private float cellSize;
    [SerializeField]
    private Tilemap lower; //currentColor
    private Tilemap upper; //goalColor

    public void InitLevelGrid()
    {
        var size = lower.size;
        levelGrid = new GridCell[size.x, size.y];
        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                var pos = new Vector3Int(i, j, 0);
                levelGrid[i, j] = new GridCell(pos, upper.GetColor(pos), lower.GetColor(pos));
            }
        }
    }
    public bool IsLevelDone()
    {
        foreach (GridCell cell in levelGrid)
        {
            if (!cell.IsGoalReached())
            {
                return false;    
            }
        }
        return true;
    }
}
