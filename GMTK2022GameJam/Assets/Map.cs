using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public Camera mainCamera;

    public DiceAI ai;

    public Dice player;

    public GameObject Welcome;

    public GameObject Credit;

    public GameObject Thank;

    public GameObject canvas;

    public GameObject triger;
    // Start is called before the first frame update
    void Start()
    {
        ai.isRolling = true;
        player.isRolling = true;
        StartCoroutine(Flip());

        if (Global.ended)
        {
            Welcome.SetActive(false);
            Credit.SetActive(false);
            Thank.SetActive(true);
        }
        else
        {
            Welcome.SetActive(true);
            Credit.SetActive(false);
            Thank.SetActive(false);
        }
    }

    public void ShowCredit()
    {
        Welcome.SetActive(false);
        Credit.SetActive(true);
        Thank.SetActive(false);
        triger.SetActive(false);
        StartCoroutine(DoubleFlip());
    }

    IEnumerator Flip()
    {
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < 180; i++)
        {
            transform.RotateAround(transform.position, Vector3.left, 1);
            yield return new WaitForSeconds(0.01f);
        }
        yield return new WaitForSeconds(1f);
        player.recenter();
        player.findDownFaces();
        ai.isRolling = false;
        player.isRolling = false;
    }
    
    IEnumerator DoubleFlip()
    {
        ai.gameObject.SetActive(false);
        player.isRolling = true;
        canvas.SetActive(false);
        
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < 180; i++)
        {
            transform.RotateAround(transform.position, Vector3.left, 1);
            yield return new WaitForSeconds(0.01f);
        }
        yield return new WaitForSeconds(2f);
        for (int i = 0; i < 180; i++)
        {
            transform.RotateAround(transform.position, Vector3.left, 1);
            yield return new WaitForSeconds(0.01f);
        }
        yield return new WaitForSeconds(1f);
        player.recenter();
        player.findDownFaces();
        ai.isRolling = false;
        player.isRolling = false;
        
        Welcome.SetActive(true);
        Credit.SetActive(false);
        Thank.SetActive(false);
    }
}
