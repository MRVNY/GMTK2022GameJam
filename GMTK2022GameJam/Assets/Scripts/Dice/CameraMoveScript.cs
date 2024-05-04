using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraMoveScript : MonoBehaviour
{
    private Camera cam;

    [SerializeField] private CinemachineVirtualCamera Cam_N;
    [SerializeField] private CinemachineVirtualCamera Cam_E;
    [SerializeField] private CinemachineVirtualCamera Cam_S;
    [SerializeField] private CinemachineVirtualCamera Cam_W;
    
    private CinemachineFramingTransposer[] transposers;
    
    private CinemachineVirtualCamera[] vcams;
    private CinemachineVirtualCamera currentCam;
        
    public bool diceIsBlocked = false;
    public bool isOrthographic = true;

    private Vector3 dicePos, posOrtho, posPersp, targetPos;
    private Vector3 velocity = Vector3.zero;
    
    public static CameraMoveScript Instance { get; private set; }
    // Start is called before the first frame update
    void Start()
    {
        if (Instance==null)
        {
            Instance = this;
        }
        
        cam = GetComponent<Camera>();
        targetPos = transform.position;
        
        vcams = new CinemachineVirtualCamera[4]{Cam_N, Cam_W, Cam_S, Cam_E};
        transposers = new CinemachineFramingTransposer[4];
        foreach (CinemachineVirtualCamera vcam in vcams)
        {
            transposers[Array.IndexOf(vcams, vcam)] = vcam.GetCinemachineComponent<CinemachineFramingTransposer>();
        }
        currentCam = Cam_N;
    }
    void Update()
    {
        if(Input.GetKeyDown("p"))
        {
            foreach (var transposer in transposers)
            {
                if (transposer.m_ScreenX == 0.5f)
                {
                    transposer.m_ScreenX = 0.3f;
                    transposer.m_ScreenY = 0.25f;
                }
                else
                {
                    transposer.m_ScreenX = 0.5f;
                    transposer.m_ScreenY = 0.5f;
                }
            }
            
        }
        
        if(Input.GetKeyDown("a"))// && !transitioning)
        {
            Dice.Instance.RotateCamera(1);
            
            currentCam = vcams[(Array.IndexOf(vcams, currentCam) + 1) % 4];
            foreach (var vcam in vcams)
                vcam.Priority = 0;
            currentCam.Priority = 10;
        }
        else if(Input.GetKeyDown("d"))// && !transitioning)
        {
            Dice.Instance.RotateCamera(-1);
            
            currentCam = vcams[(Array.IndexOf(vcams, currentCam) + 4 - 1) % 4];
            foreach (var vcam in vcams)
                vcam.Priority = 0;
            currentCam.Priority = 10;
        }
    }
    
    public void NewFollow(Transform target)
    {
        foreach (CinemachineVirtualCamera vcam in vcams)
        {
            vcam.Follow = target;
        }
    }
}
