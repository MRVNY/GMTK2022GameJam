using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public Camera mainCamera;

    public DiceAI ai;

    public Dice player;
    // Start is called before the first frame update
    void Start()
    {
        ai.isRolling = true;
        player.isRolling = true;
        StartCoroutine(Flip());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator Flip()
    {
        yield return new WaitForSeconds(1f);
        Camera.main.orthographic = false;
        for (int i = 0; i < 180; i++)
        {
            transform.RotateAround(transform.position, Vector3.left, 1);
            yield return new WaitForSeconds(0.01f);
        }
        yield return new WaitForSeconds(1f);
        Camera.main.orthographic = true;
        ai.isRolling = false;
        player.isRolling = false;
    }
}
