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
    
    public Tilemap tilemap;

    private List<GameObject> diceFaces = new List<GameObject>();
    //private Rigidbody rb;
    private bool isRolling = false;
    private int step = 10;
    private float speed = 0.01f;
    private float wait = 0.2f;

    private Vector3 hori;
    private Vector3 verti;
    

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
        hori = new Vector3(GetComponent<MeshFilter>().mesh.bounds.size.x, 0, 0);
        verti = new Vector3(0, 0, GetComponent<MeshFilter>().mesh.bounds.size.z);
        
        recenter();
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
            if (Input.GetKey(KeyCode.UpArrow) 
                && tilemap.HasTile(tilemap.WorldToCell(transform.position + verti)))
            {
                StartCoroutine(moveUp());
                isRolling = true;
            }

            else if (Input.GetKey(KeyCode.DownArrow)
                     && tilemap.HasTile(tilemap.WorldToCell(transform.position - verti)))
            {
                StartCoroutine(moveDown());
                isRolling = true;
            }

            else if (Input.GetKey(KeyCode.LeftArrow)
                     && tilemap.HasTile(tilemap.WorldToCell(transform.position - hori)))
            {
                StartCoroutine(moveLeft());
                isRolling = true;
            }

            else if (Input.GetKey(KeyCode.RightArrow)
                     && tilemap.HasTile(tilemap.WorldToCell(transform.position + hori)))
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
        // center.transform.position = transform.position;
        yield return new WaitForSeconds(wait);
        recenter();
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
        // center.transform.position = transform.position;
        yield return new WaitForSeconds(wait);
        recenter();
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
        // center.transform.position = transform.position;
        yield return new WaitForSeconds(wait);
        recenter();
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
        // center.transform.position = transform.position;
        yield return new WaitForSeconds(wait);
        recenter();
        isRolling = false;
        downFace = null;
    }

    void recenter()
    {
        var allCubes = transform.parent.GetComponentsInChildren<MeshRenderer>();
        var center = Vector3.zero;
        foreach (var cube in allCubes)
        {
            center += cube.transform.position;
        }
        center /= allCubes.Length;

        float newN = Single.NegativeInfinity;
        float newS = Single.PositiveInfinity;
        float newE = Single.NegativeInfinity;
        float newW = Single.PositiveInfinity;
        
        foreach (var cube in allCubes)
        {
            var position = cube.transform.position;
            newN = Mathf.Max(newN, position.z);
            newS = Mathf.Min(newS, position.z);
            newE = Mathf.Max(newE, position.x);
            newW = Mathf.Min(newW, position.x);
        }
        
        N.transform.position = new Vector3(center.x, 0, newN);
        S.transform.position = new Vector3(center.x, 0, newS);
        E.transform.position = new Vector3(newE, 0, center.z);
        W.transform.position = new Vector3(newW, 0, center.z);
    }
}
