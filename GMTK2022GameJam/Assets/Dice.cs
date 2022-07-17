using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class Dice : MonoBehaviour
{
    public GameObject N;
    public GameObject S;
    public GameObject E;
    public GameObject W;
    
    public Tilemap tilemap;
    protected Face[] diceFaces;

    public bool isRolling = false;
    protected int step = 10;
    protected int blockStep = 3;
    protected float speed = 0.01f;
    protected float wait = 0.2f;
    protected Dictionary<GameObject,Vector3> PointAxe = new Dictionary<GameObject, Vector3>();

    protected Vector3 hori;
    protected Vector3 verti;
    
    public static List<Face> downFaces = new List<Face>();
    
    BoxCollider[] allCubes;
    // Start is called before the first frame update
    protected void Start()
    {
        diceFaces = GetComponentsInChildren<Face>();
        
        PointAxe.Add(N,Vector3.right);
        PointAxe.Add(S,Vector3.left);
        PointAxe.Add(E,Vector3.back);
        PointAxe.Add(W,Vector3.forward);
            
        hori = new Vector3(GetComponent<MeshFilter>().mesh.bounds.size.x, 0, 0);
        verti = new Vector3(0, 0, GetComponent<MeshFilter>().mesh.bounds.size.z);
        
        allCubes = transform.parent.GetComponentsInChildren<BoxCollider>();
        recenter();
        findDownFaces();
    }

    private void Update()
    {
        if (!isRolling)
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                if(tilemap.HasTile(tilemap.WorldToCell(transform.position + verti)))
                    StartCoroutine(move(N));
                else
                    StartCoroutine(block(N));
            }
            
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                if (tilemap.HasTile(tilemap.WorldToCell(transform.position - verti)))
                    StartCoroutine(move(S));
                else
                    StartCoroutine(block(S));
            }
            
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                if (tilemap.HasTile(tilemap.WorldToCell(transform.position - hori)))
                    StartCoroutine(move(W));
                else
                    StartCoroutine(block(W));
            }
            
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                if (tilemap.HasTile(tilemap.WorldToCell(transform.position + hori)))
                    StartCoroutine(move(E));
                else
                    StartCoroutine(block(E));
            }
        }
    }

    protected IEnumerator move(GameObject point)
    {
        isRolling = true;
        for (int i = 0; i < (90 / step); i++)
        {
            transform.RotateAround(point.transform.position, PointAxe[point], step);
            yield return new WaitForSeconds(speed);
        }
        
        yield return new WaitForSeconds(wait);
        recenter();
        findDownFaces();
        isRolling = false;
    }
    
    protected IEnumerator block(GameObject point)
    {
        isRolling = true;
        for(int i=0; i<blockStep; i++)
        {
            transform.RotateAround(point.transform.position, PointAxe[point], step);
            yield return new WaitForSeconds(speed);
        }
        
        for(int i=0; i<blockStep; i++)
        {
            transform.RotateAround(point.transform.position, PointAxe[point], -step);
            yield return new WaitForSeconds(speed);
        }

        yield return new WaitForSeconds(wait);
        isRolling = false;
    }

    protected void recenter()
    {
        var center = Vector3.zero;

        float newN = Single.NegativeInfinity;
        float newS = Single.PositiveInfinity;
        float newE = Single.NegativeInfinity;
        float newW = Single.PositiveInfinity;
        
        foreach (var cube in allCubes)
        {
            var position = cube.transform.position;
            
            center += position;
            newN = Mathf.Max(newN, position.z);
            newS = Mathf.Min(newS, position.z);
            newE = Mathf.Max(newE, position.x);
            newW = Mathf.Min(newW, position.x);
        }
        
        center /= allCubes.Length;
        
        N.transform.position = new Vector3(center.x, 0, newN) + verti/2;
        S.transform.position = new Vector3(center.x, 0, newS) - verti/2;
        E.transform.position = new Vector3(newE, 0, center.z) + hori/2;
        W.transform.position = new Vector3(newW, 0, center.z) - hori/2;
    }

    protected void findDownFaces()
    {
        downFaces.Clear();
        foreach (var face in diceFaces)
        {
            face.AmIDownFace();
        }
    }
}
