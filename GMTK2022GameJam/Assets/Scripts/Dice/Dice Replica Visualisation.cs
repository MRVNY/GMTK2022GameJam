using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceReplicaVisualisation : MonoBehaviour
{
    [SerializeField]
    private GameObject diceReplica;



    private Dice _diceClone;

    private Transform diceReplicaTr;
    private Dice _playerDice;
    private List<Color> _diceColors;

    // Start is called before the first frame update
    void Start()
    {
        _playerDice = Dice.Instance;

        
        diceReplicaTr = diceReplica.transform;
        _startRotation = diceReplicaTr.rotation.eulerAngles;
        _diceColors = new List<Color>();
        MeshRenderer[] allFacesRenderers = _playerDice.GetComponentsInChildren<MeshRenderer>();
        for (int i = 0; i< diceReplicaTr.childCount; i++)
        {
            diceReplicaTr.GetChild(i).GetComponent<MeshRenderer>().material = _playerDice.transform.GetChild(i).GetComponent<MeshRenderer>().material;
        }


        /*
        diceClone = Instantiate(_playerDice,transform);
        diceClone.name = "Dice_Replica";
        
        diceClone.gameObject.layer = gameObject.layer;
        foreach (Transform child in diceClone.transform)
        {
            child.gameObject.layer = gameObject.layer;
        }
        */

    }

    [SerializeField]
    private float sensitivity = 1;

    private int _mouseLeftClickId = 0;
    private Vector3 _mouseReference;
    private Vector3 _mouseOffset;
    private Vector3 _rotation;
    private Vector3 _startRotation;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(_mouseLeftClickId))
        {
            print("GetMouseButtonDown");
            _mouseReference = Input.mousePosition;
        }
        if (Input.GetMouseButton(_mouseLeftClickId))
        {
            print("GetMouseButton");
            // offset
            _mouseOffset = (Input.mousePosition - _mouseReference);

            // apply rotation
            _rotation.y = -(_mouseOffset.x + _mouseOffset.y) * sensitivity;

            // rotate
            //diceReplicaTr.Rotate(_rotation);
            float XaxisRotation = _mouseOffset.x * sensitivity;
            float YaxisRotation = _mouseOffset.y * sensitivity;

            diceReplicaTr.Rotate(Vector3.down, XaxisRotation);
            diceReplicaTr.Rotate(Vector3.right, YaxisRotation);

            // store mouse
            _mouseReference = Input.mousePosition;
        }
    }
}
