using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectTrigger : MonoBehaviour
{
    [SerializeField]
    private int levelId;

    [SerializeField]
    private FontModels fontsModelsSo;

    [SerializeField]
    private Material numbersMat;

    [SerializeField]
    private float figuresSeparationOffset = 5;

    [SerializeField]
    private Vector3 offset;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("level select triggered");
        StartCoroutine(SceneManagerScript.Instance.LoadLevelWithDelay(levelId,2f));
    }

    public void Generate3dNumbers()
    {
        if( levelId >= 9)
        {
            InstantiateFigureAtPos((levelId + 1) / 10, new Vector3(-figuresSeparationOffset, 0, 0));
            InstantiateFigureAtPos((levelId + 1) % 10, new Vector3(figuresSeparationOffset, 0, 0));
        }
        else
        {
            InstantiateFigureAtPos(levelId + 1, Vector3.zero);
        }
    }

    public GameObject InstantiateFigureAtPos(int figureValue, Vector3 localPos)
    {
        GameObject go = new GameObject("" + figureValue);
        var meshFilter = go.AddComponent<MeshFilter>();
        meshFilter.mesh = fontsModelsSo.meshes[figureValue];
        var meshRenderer = go.AddComponent<MeshRenderer>();
        meshRenderer.material = numbersMat;

        go.transform.parent = transform.GetChild(0).transform;
        go.transform.Rotate(Vector3.right, 90f);
        go.transform.localScale = Vector3.one;
        go.transform.localPosition = localPos + offset;

        return go;
    }
}
