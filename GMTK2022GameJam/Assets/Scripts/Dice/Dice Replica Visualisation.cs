using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceReplicaVisualisation : MonoBehaviour
{
    [SerializeField]
    private GameObject diceReplica;

    private Transform diceReplicaTr;
    private Dice _playerDice;

    // Start is called before the first frame update
    void Start()
    {
        _playerDice = Dice.Instance;
        diceReplicaTr = diceReplica.transform;

        MeshRenderer[] allFacesRenderers = _playerDice.GetComponentsInChildren<MeshRenderer>();
        for (int i = 0; i< diceReplicaTr.childCount; i++)
        {
            diceReplicaTr.GetChild(i).GetComponent<MeshRenderer>().material = _playerDice.transform.GetChild(i).GetComponent<MeshRenderer>().material;
        }

    }
}
