using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelSelectMenu : MonoBehaviour
{
    [SerializeField] private Grid grid;
    [SerializeField] private Transform menuTilePos;
    [SerializeField] private Tilemap current;
    [SerializeField] private Tilemap goal;
    [SerializeField] private Tile testDebugTile;
    private Vector3Int levelCenterTilePos;
    private int offsetFromCenterToFirst = 6;
    private void Start()
    {
        levelCenterTilePos = new Vector3Int(-5, 0, 0);
        Vector3Int tilePos = new Vector3Int(levelCenterTilePos.x + offsetFromCenterToFirst,0,0);
        current.SetTile(tilePos, null);
        goal.SetTile(tilePos, testDebugTile);
    }

}
