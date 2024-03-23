using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class Dice : MonoBehaviour
{
    private Vector3 center;
    public GameObject N,S,E,W;
    private float height;
    private RotateData[] rotateData;
    public RotateData currentRotation;
    private int UP=0, RIGHT=1, DOWN=2, LEFT=3;
    
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
    protected GameObject targetDir;
    protected Dictionary<GameObject,Vector3> blockCheck = new Dictionary<GameObject, Vector3>();
    
    public static List<Face> downFaces = new List<Face>();
    public GameObject StarsEffect;
    public GameObject SmokeEffect;
    public static Dice Instance { get; private set; }

    private AudioSource audioSource;
    public AudioClip moveSound;
    
    BoxCollider[] allCubes;
    // Start is called before the first frame update

    protected Vector3 mouseStart;
    protected float mouseClick;
    protected void Start()
    {
        
        if (Instance==null)
        {
            Instance = this;
        }
        
        rotateData = new RotateData[4]
        {
            new RotateData{direction = "N", perspectiveOffset = new Vector3(2,0,-2), controlScheme = new GameObject[]{N,E,S,W}}, 
            new RotateData{direction = "E", perspectiveOffset = new Vector3(-2,0,-2), controlScheme = new GameObject[]{E,S,W,N}},
            new RotateData{direction = "S", perspectiveOffset = new Vector3(-2,0,2), controlScheme = new GameObject[]{S,W,N,E}},
            new RotateData{direction = "W", perspectiveOffset = new Vector3(2,0,2), controlScheme = new GameObject[]{W,N,E,S}}
        };
        
        currentRotation = rotateData[0];

        audioSource = GetComponent<AudioSource>();
        diceFaces = GetComponentsInChildren<Face>();
        
        PointAxe.Add(N,Vector3.right);
        PointAxe.Add(E,Vector3.back);
        PointAxe.Add(S,Vector3.left);
        PointAxe.Add(W,Vector3.forward);
            
        hori = new Vector3(GetComponent<MeshFilter>().mesh.bounds.size.x, 0, 0);
        verti = new Vector3(0, 0, GetComponent<MeshFilter>().mesh.bounds.size.z);

        blockCheck.Add(N, verti);
        blockCheck.Add(S, -verti);
        blockCheck.Add(E, hori);
        blockCheck.Add(W, -hori);
        
        allCubes = transform.parent.GetComponentsInChildren<BoxCollider>();
        recenter();
        findDownFaces();
    }
    
    private void Update()
    {
        //Mouse controls
        if (Input.GetMouseButtonDown(0) && !InPause())
        {
            if(Time.time - mouseClick < 0.2f) SceneManagerScript.Instance.ReloadScene();
            mouseStart = Input.mousePosition;
            mouseClick = Time.time;
        }

        if (!isRolling && Input.GetMouseButtonUp(0) && !InPause())
        {
            Vector3 dir = Input.mousePosition - mouseStart;
            if(dir.magnitude > 50)
            {
                if(Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
                {
                    if(dir.x < 0) targetDir = currentRotation.controlScheme[LEFT];
                    else targetDir = currentRotation.controlScheme[RIGHT];
                }
                else
                {
                    if(dir.y < 0) targetDir = currentRotation.controlScheme[DOWN];
                    else targetDir = currentRotation.controlScheme[UP];
                }
                StartCoroutine(
                    tilemap.HasTile(tilemap.WorldToCell(targetDir.transform.position + blockCheck[targetDir] * (height - 0.5f)))
                        ? move(targetDir)
                        : block(targetDir));
            }
            dir = Vector3.zero;
        }
        
        //Keyboard controls
        if (!isRolling)
        {
            if (Input.GetKey(KeyCode.UpArrow)) targetDir = currentRotation.controlScheme[UP];
            else if (Input.GetKey(KeyCode.RightArrow)) targetDir = currentRotation.controlScheme[RIGHT];
            else if (Input.GetKey(KeyCode.DownArrow)) targetDir = currentRotation.controlScheme[DOWN];
            else if (Input.GetKey(KeyCode.LeftArrow)) targetDir = currentRotation.controlScheme[LEFT];
            else targetDir = null;
            
            if(targetDir!=null)
                StartCoroutine(
                    tilemap.HasTile(tilemap.WorldToCell(targetDir.transform.position + blockCheck[targetDir] * (height - 0.5f)))
                        ? move(targetDir)
                        : block(targetDir));
        }
    }

    protected IEnumerator move(GameObject point)
    {
        //Debug.Log("Bouge");
        DiceEventSystem.DiceMoved();
        audioSource.clip = moveSound;
        audioSource.Play();
        isRolling = true;
        for (int i = 0; i < (90 / step); i++)
        {
            transform.RotateAround(point.transform.position, PointAxe[point], step);
            yield return new WaitForSeconds(speed);
        }
        
        recenter();
        findDownFaces();
        
        yield return new WaitForSeconds(wait);
        
        isRolling = false;
        
    }

    private bool InPause()
    {
        return !PauseMenuScript.Instance || PauseMenuScript.Instance.IsInPause;
    }
    protected IEnumerator block(GameObject point)
    {
        if(CameraMoveScript.Instance!=null) 
            CameraMoveScript.Instance.diceIsBlocked = true;
        
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
        if(CameraMoveScript.Instance!=null) 
            CameraMoveScript.Instance.diceIsBlocked = false;
    }

    public void recenter()
    {
        center = Vector3.zero;

        float newN = Single.NegativeInfinity;
        float newS = Single.PositiveInfinity;
        float newE = Single.NegativeInfinity;
        float newW = Single.PositiveInfinity;
        float UP = Single.NegativeInfinity;
        float DOWN = Single.PositiveInfinity;
        

        foreach (var cube in allCubes)
        {
            var position = cube.transform.position;
            
            center += position;
            newN = Mathf.Max(newN, position.z);
            newS = Mathf.Min(newS, position.z);
            newE = Mathf.Max(newE, position.x);
            newW = Mathf.Min(newW, position.x);
            UP = Mathf.Max(UP, position.y);
            DOWN = Mathf.Min(DOWN, position.y);
        }
        
        center /= allCubes.Length;
        
        N.transform.position = new Vector3(center.x, 0, newN) + verti/2;
        S.transform.position = new Vector3(center.x, 0, newS) - verti/2;
        E.transform.position = new Vector3(newE, 0, center.z) + hori/2;
        W.transform.position = new Vector3(newW, 0, center.z) - hori/2;
        height = ((UP - DOWN) / hori.x) + 1;
    }

    public void findDownFaces()
    {
        downFaces.Clear();
        foreach (var face in diceFaces)
        {
            face.AmIDownFace();
        }
    }

    public void stick(Collider col)
    {
        col.transform.SetParent(transform);
        allCubes = transform.parent.GetComponentsInChildren<BoxCollider>();
        diceFaces = GetComponentsInChildren<Face>();
        recenter();
        findDownFaces();
    }
    
    public void RotateCamera(int direction)
    {
        currentRotation = rotateData[(Array.IndexOf(rotateData, currentRotation)+direction+4)%4];
    }
}

public struct RotateData
{
    public string direction;
    public Vector3 perspectiveOffset;
    public GameObject[] controlScheme;
}