using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceAI : Dice
{
    private GameObject[] moveSequence;

    private int currentDir;

    public GameObject startUI;
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        moveSequence = new GameObject[4]{E,S, W, N};
        currentDir = 0;

    }

    // Update is called once per frame
    void Update()
    {
        if (!isRolling)
        {
            
            startUI.SetActive(true);
            if (tilemap.HasTile(tilemap.WorldToCell(transform.position + 2 * (moveSequence[currentDir].transform.position-transform.position))))
            {
                StartCoroutine(move(moveSequence[currentDir]));
            }
            else
            {
                currentDir = (currentDir + 1) % 4;
            }
        }
    }
}