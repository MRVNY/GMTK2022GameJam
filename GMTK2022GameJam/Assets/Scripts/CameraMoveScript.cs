using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraMoveScript : MonoBehaviour
{
    public Transform movingDice;
    public bool diceIsBlocked = false;
    
    public static CameraMoveScript Instance { get; private set; }
    // Start is called before the first frame update
    void Start()
    {
        if (Instance==null)
        {
            Instance = this;
        }
        else
        {
            throw new InvalidImplementationException("You should not try to instantiate a singleton twice !");
        }

    }
    void Update()
    {
        if (!diceIsBlocked)
        {
            Vector3 newPos = new()
            {
                x = movingDice.position.x,
                y = transform.position.y,
                z = movingDice.position.z,
            };
            transform.position = newPos;
        }
    }
}
