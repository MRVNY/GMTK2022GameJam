using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell
{
    public Vector3Int pos { get; private set; }
    public Color goalColor{ get; private set; }
    public Color currColor { get; set; }

    public GridCell(Vector3Int pos, Color goalColor, Color currColor)
    {
        this.pos = pos;
        this.goalColor = goalColor;
        this.currColor = currColor;
    }

    public bool IsGoalReached()
    {
        return goalColor.Equals(currColor);
    }



}
