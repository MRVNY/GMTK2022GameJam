using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoveScript : MonoBehaviour
{
    private Camera cam;
        
    public bool diceIsBlocked = false;
    public bool isOrthographic = true;

    private Vector3 dicePos, posOrtho, posPersp, targetPos;
    private Quaternion targetRot;
    private Vector3 velocity = Vector3.zero;
    private bool transitioning = false;
    
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
        targetRot = transform.rotation;
    }
    void Update()
    {
        if (!diceIsBlocked)
        {
            dicePos = Dice.Instance.transform.position;
            
            posOrtho = new Vector3(dicePos.x, transform.position.y, dicePos.z);
            posPersp = posOrtho + Dice.Instance.currentRotation.perspectiveOffset;
            // posPersp = posOrtho + new Vector3(2, 0, -2);
            
            if(Input.GetKeyDown("p") && !transitioning)
            {
                isOrthographic = !isOrthographic;
            }

            else
            {
                if (isOrthographic) targetPos = posOrtho;
                else targetPos = posPersp;
            }
            
            transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, 0.2f);
        }
        
        if(Input.GetKeyDown("a") && !transitioning)
        {
            //rotate on y axis
            targetRot = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + 90, 0);
            Dice.Instance.RotateCamera(1);
            transitioning = true;
        }
        else if(Input.GetKeyDown("d") && !transitioning)
        {
            //rotate on y axis
            targetRot = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y - 90, 0);
            Dice.Instance.RotateCamera(-1);
            transitioning = true;
        }

        if (transitioning)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 0.01f);
            if (Quaternion.Angle(transform.rotation, targetRot) == 0)
            {
                transitioning = false;
            }
        }
    }
}
