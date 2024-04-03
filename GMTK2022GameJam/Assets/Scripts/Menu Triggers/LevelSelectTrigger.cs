using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    [SerializeField]
    private RectTransform uiBestScore;

    [SerializeField]
    private List<TMP_Text> scoreTexts;

    [SerializeField]
    private bool uiOnLeftSide;


    private void Start()
    {
        if (levelId > SceneManagerScript.Instance.GetMaxLevelCompletedId() + 1)
        {
            return;
        }
        uiBestScore.gameObject.SetActive(true);
        
        bool hasPlayerDoneLevel = levelId <= SceneManagerScript.Instance.GetMaxLevelCompletedId();
        print("hasPlayerDoneLevel " + hasPlayerDoneLevel + ",  levelId " + levelId + ", LevelSelectMenu.Instance.MaxLevelIdAvailable " + LevelSelectMenu.Instance.MaxLevelIdAvailable);
        if (hasPlayerDoneLevel)
        {
            int bestScore = SceneManagerScript.Instance.LoadScore(levelId);
            foreach (var txt in scoreTexts)
            {

                txt.text = "Done in " + bestScore + " moves";
            }
        }
        else
        {
            foreach (var txt in scoreTexts)
            {

                txt.text = "";
            }
        }
        
        if (levelId % 2 == 1)
        {
            Vector3 pos = uiBestScore.anchoredPosition;
            pos.x *= -1;
            uiBestScore.anchoredPosition = pos;
            print(gameObject.name);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("level select triggered");
        StartCoroutine(SceneManagerScript.Instance.LoadLevelWithDelay(levelId,0.7f));
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
