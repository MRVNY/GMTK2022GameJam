using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    private Face[] diceFaces;

    public bool isRolling = false;
    private int step = 10;
    private int blockStep = 3;
    private float speed = 0.01f;
    private float wait = 0.2f;
    private int offset = 3;
    private Dictionary<GameObject,Vector3> PointAxe;

    private Vector3 hori;
    private Vector3 verti;
    private GameObject targetDir;
    private Vector3 lastFallDir;
    public Vector3 cantRollBackDir = Vector3.zero;
    private Dictionary<GameObject,Vector3> blockCheck;
    
    public static List<Face> downFaces;
    public GameObject StarsEffect;
    public GameObject SmokeEffect;
    public static Dice Instance { get; private set; }

    private AudioSource audioSource;
    public AudioClip moveSound;
    
    BoxCollider[] allCubes;
    // Start is called before the first frame update

    protected Vector3 mouseStart;
    protected float mouseClick;


    protected void Awake()
    {
        Instance = this;

    }
    protected void Start()
    {   
        PointAxe = new Dictionary<GameObject, Vector3>();
        blockCheck = new Dictionary<GameObject, Vector3>();
        downFaces = new List<Face>();
        
        rotateData = new RotateData[4]
        {
            new RotateData{direction = "N", perspectiveOffset = new Vector3(offset,0,-offset), controlScheme = new GameObject[]{N,E,S,W}}, 
            new RotateData{direction = "E", perspectiveOffset = new Vector3(-offset,0,-offset), controlScheme = new GameObject[]{E,S,W,N}},
            new RotateData{direction = "S", perspectiveOffset = new Vector3(-offset,0,offset), controlScheme = new GameObject[]{S,W,N,E}},
            new RotateData{direction = "W", perspectiveOffset = new Vector3(offset,0,offset), controlScheme = new GameObject[]{W,N,E,S}}
        };
        
        currentRotation = rotateData[0];

        audioSource = GetComponentInParent<AudioSource>();
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
        
        readjust();
    }

    private void OnEnable()
    {
        if (Instance != null)
        {
            readjust();
            Instance = this;
        }
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
                if(cantRollBackDir != blockCheck[targetDir])
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
            
            if(targetDir!=null && cantRollBackDir != blockCheck[targetDir])
                StartCoroutine(
                    tilemap.HasTile(tilemap.WorldToCell(targetDir.transform.position + blockCheck[targetDir] * (height - 0.5f)))
                        ? move(targetDir)
                        : block(targetDir));
            
            // Separate
            if (Input.GetKeyDown("b") && allCubes.Length > 1 && Math.Abs(allCubes[0].transform.position.y - allCubes[1].transform.position.y) < 0.1f)
            {
                separate();
            }
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

        //Record the last direction where the dices fell flat 
        if (allCubes.Length > 1)
        {
            if(((targetDir==N || targetDir==S) && N.transform.position.z - S.transform.position.z > 1.5f)
               || ((targetDir==E || targetDir==W) && E.transform.position.x - W.transform.position.x > 1.5f))
                lastFallDir = blockCheck[targetDir];
        }
        // Reactivate detection of the other dice
        else if (!diceFaces[0].GetComponent<MeshCollider>().enabled)
        {
            foreach (var face in diceFaces)
            {
                face.GetComponent<MeshCollider>().enabled = true;
            }
            cantRollBackDir = Vector3.zero;
        }
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
        foreach (var face in diceFaces)
        {
            face.GetComponent<MeshCollider>().enabled = false;
        }
        col.transform.SetParent(transform);
        readjust();
    }
    
    private void separate()
    {
        //Decide which cube is the one to control
        Dice newCube, oldCube;
        int newCubeIndex = 0;
        if(lastFallDir.z!=0) newCubeIndex = Math.Sign(lastFallDir.z) == Math.Sign(allCubes[0].transform.position.z-allCubes[1].transform.position.z) ? 0 : 1;
        else if(lastFallDir.x!=0) newCubeIndex = Math.Sign(lastFallDir.x) == Math.Sign(allCubes[0].transform.position.x-allCubes[1].transform.position.x) ? 0 : 1;
        
        newCube = allCubes[newCubeIndex].gameObject.GetComponent<Dice>();
        oldCube = allCubes[1-newCubeIndex].gameObject.GetComponent<Dice>();
            
        //Move hierarchy
        newCube.transform.SetParent(transform.parent);
        oldCube.transform.SetParent(null);
        
        //Link references
        newCube.N = N;
        newCube.S = S;
        newCube.E = E;
        newCube.W = W;
        newCube.tilemap = tilemap;
        
        //disable oldCube
        newCube.enabled = true;
        oldCube.enabled = false;
        newCube.cantRollBackDir = -lastFallDir;
        
        if(newCube == this) readjust();
    }

    private void readjust()
    {
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