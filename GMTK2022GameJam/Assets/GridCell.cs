using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell
{
    public Color ObjColor{ get; private set; }
    public Color currColor { get; private set; }

    public GridCell(Color objColor, Color currColor)
    {
        ObjColor = objColor;
        this.currColor = currColor;
    }



}
