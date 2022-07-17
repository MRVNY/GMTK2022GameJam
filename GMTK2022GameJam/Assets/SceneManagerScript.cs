using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{
    public static SceneManagerScript Instance { get; private set; }

    public static bool IsGameOver { get; set; } = false;
    // Start is called before the first frame update
    void Start()
    {
        if (Instance==null)
        {
            Instance = this;
        }
        else
        {
            throw new InvalidImplementationException("You should not try to instantiate a singleton twice !");
        }
    }

    // Update is called once per frame
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.N) && (IsGameOver))
        {
            LoadNextlevel();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            ReloadScene();
        }
    }

    public  void ReloadScene()
    {
        Scene scene = SceneManager.GetActiveScene(); 
        SceneManager.LoadScene(scene.name);
    }

    public void LoadNextlevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
