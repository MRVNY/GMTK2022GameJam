using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoveScript : MonoBehaviour
{
    public Transform movingDice;

    void Update()
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
