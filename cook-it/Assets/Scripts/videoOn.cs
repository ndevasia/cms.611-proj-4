using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class videoOn : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;
        if (sceneName == "associativeProperty")
        {
            GetComponent<VideoPlayer>().url = System.IO.Path.Combine(Application.streamingAssetsPath, "associative.mp4");
        }
        if (sceneName == "communativeProperty")
        {
            GetComponent<VideoPlayer>().url = System.IO.Path.Combine(Application.streamingAssetsPath, "communative.mp4");
        }
        if (sceneName == "distributiveProperty")
        {
            GetComponent<VideoPlayer>().url = System.IO.Path.Combine(Application.streamingAssetsPath, "distributive.mp4");
        }
        GetComponent<VideoPlayer>().Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
