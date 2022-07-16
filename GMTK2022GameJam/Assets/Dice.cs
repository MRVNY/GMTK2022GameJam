using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class Dice : MonoBehaviour
{
    public GameObject up;
    public GameObject down;
    public GameObject left;
    public GameObject right;
    public GameObject front;
    public GameObject back;

    public GameObject N;
    public GameObject S;
    public GameObject E;
    public GameObject W;
    public GameObject center;

    private List<GameObject> diceFaces = new List<GameObject>();
    //private Rigidbody rb;
    private bool isRolling = false;
    private int step = 10;
    private float speed = 0.01f;
    private float wait = 0.2f;

    public static Face downFace = null;
    // Start is called before the first frame update
    void Start()
    {
        diceFaces.Add(up);
        diceFaces.Add(down);
        diceFaces.Add(left);
        diceFaces.Add(right);
        diceFaces.Add(front);
        diceFaces.Add(back);

        foreach (var face in diceFaces)
        {
            //face.GetComponent<Renderer>().material.SetColor("_Color", Color.green);
        }

        //rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // foreach (var face in diceFaces)
        // {
        //     face.GetComponent<Renderer>().material.SetColor("_Color", new Color(
        //         Random.Range(0f, 1f), 
        //         Random.Range(0f, 1f), 
        //         Random.Range(0f, 1f)
        //     ));
        // }
    }

    private void Update()
    {
        // var hori = Input.GetAxis("Horizontal");
        // var verti = Input.GetAxis("Vertical");
        //
        // if (hori != 0 || verti != 0)
        // {
        //     rb.AddForce(new Vector3(hori, 0, verti) * 10);
        //     isRolling = true;
        // }

        if (!isRolling)
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                StartCoroutine(moveUp());
                isRolling = true;
            }

            else if (Input.GetKey(KeyCode.DownArrow))
            {
                StartCoroutine(moveDown());
                isRolling = true;
            }

            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                StartCoroutine(moveLeft());
                isRolling = true;
            }

            else if (Input.GetKey(KeyCode.RightArrow))
            {
                StartCoroutine(moveRight());
                isRolling = true;
            }
        }
        //center.transform.position = transform.position;
    }

    IEnumerator moveUp()
    {
        for (int i = 0; i < (90 / step); i++)
        {
            transform.RotateAround(N.transform.position, Vector3.right, step);
            yield return new WaitForSeconds(speed);
        }
        center.transform.position = transform.position;

        yield return new WaitForSeconds(wait);
        isRolling = false;
        downFace = null;
    }
    
    IEnumerator moveDown()
    {
        for (int i = 0; i < (90 / step); i++)
        {
            transform.RotateAround(S.transform.position, Vector3.left, step);
            yield return new WaitForSeconds(speed);
        }
        center.transform.position = transform.position;
        yield return new WaitForSeconds(wait);
        isRolling = false;
        downFace = null;
    }
    
    IEnumerator moveLeft()
    {
        for (int i = 0; i < (90 / step); i++)
        {
            transform.RotateAround(W.transform.position, Vector3.forward, step);
            yield return new WaitForSeconds(speed);
        }
        center.transform.position = transform.position;
        yield return new WaitForSeconds(wait);
        isRolling = false;
        downFace = null;
    }
    
    IEnumerator moveRight()
    {
        for (int i = 0; i < (90 / step); i++)
        {
            transform.RotateAround(E.transform.position, Vector3.back, step);
            yield return new WaitForSeconds(speed);
        }
        center.transform.position = transform.position;
        yield return new WaitForSeconds(wait);
        isRolling = false;
        downFace = null;
    }
}
