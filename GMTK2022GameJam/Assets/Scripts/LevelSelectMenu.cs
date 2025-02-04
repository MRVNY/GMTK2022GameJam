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

    [SerializeField]
    private int firstTriggerLevelId = 0;

    private int _maxLevelIdAvailable;
    private Vector3Int levelCenterTilePos;
    private int offsetFromCenterToFirst = 3;

    public static LevelSelectMenu Instance;
    public int MaxLevelIdAvailable => _maxLevelIdAvailable;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("Two instances of singletin LevelSelectMenu.cs script were created. \nDestroying this instance");
            Destroy(this.gameObject);
        }
    }
    private void Start()
    {
        levelCenterTilePos = Vector3Int.RoundToInt(current.cellBounds.center);

        //current.SetTile(levelCenterTilePos, testDebugTile);

        print(levelCenterTilePos);

        _maxLevelIdAvailable = SceneManagerScript.Instance.GetMaxLevelCompletedId() +1;
        print("_maxLevelIdAvailable " + _maxLevelIdAvailable);
        BoundsInt bounds = current.cellBounds;
        for (int x = current.cellBounds.min.x; x< current.cellBounds.max.x; x++)
        {
            for (int y= current.cellBounds.min.y; y< current.cellBounds.max.y; y++)
            {
                if(current.GetTile(new Vector3Int(x, y, 0)) != null)
                {
                    int levelLockValue = _maxLevelIdAvailable < firstTriggerLevelId ? -1 : _maxLevelIdAvailable - firstTriggerLevelId;
                    if (y >= offsetFromCenterToFirst + levelCenterTilePos.y + levelLockValue)
                    {
                        current.SetTile(new Vector3Int(x, y, 0), null);
                        goal.SetTile(new Vector3Int(x, y, 0), testDebugTile);
                    }
                }

            }
        }
        

    }

}
