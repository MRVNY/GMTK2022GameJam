using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicSourceScript : MonoBehaviour
{
    public static MusicSourceScript Instance { get; private set; }
    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        if (Instance==null)
        {
            Instance = this;
        }
        else
        {
            return;
            //throw new InvalidImplementationException("You should not try to instantiate a singleton twice !");
        }
        audioSource = GetComponent<AudioSource>();
        //audioSource.Play();
        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
