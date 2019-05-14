using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class TutorialNext : MonoBehaviour
{
    public void NextScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;

        if (sceneName == "TutorialScene-1")
        {
            SceneManager.LoadScene("TutorialScene-2");
        }
        else if (sceneName == "TutorialScene-2")
        {
            SceneManager.LoadScene("TutorialScene-3");
        }
        else if (sceneName == "TutorialScene-3")
        {
            SceneManager.LoadScene("TutorialScene-4");
        }
        else if (sceneName == "TutorialScene-4")
        {
            SceneManager.LoadScene("communativeProperty");
        }
        else if (sceneName == "communativeProperty")
        {
            SceneManager.LoadScene("TutorialScene-5");
        }
        else if (sceneName == "TutorialScene-5")
        {
            SceneManager.LoadScene("TutorialScene-6");
        }
        else if (sceneName == "TutorialScene-6")
        {
            SceneManager.LoadScene("TutorialScene-7");
        }
        else if (sceneName == "TutorialScene-7")
        {
            SceneManager.LoadScene("associativeProperty");
        }
        else if (sceneName == "associativeProperty")
        {
            SceneManager.LoadScene("TutorialScene-8");
        }
        else if (sceneName == "TutorialScene-8")
        {
            SceneManager.LoadScene("TutorialScene-9");
        }
        else if (sceneName == "TutorialScene-9")
        {
            SceneManager.LoadScene("TutorialScene-10");
        }
        else if (sceneName == "TutorialScene-10")
        {
            SceneManager.LoadScene("distributiveProperty");
        }
        else if (sceneName == "distributiveProperty")
        {
            SceneManager.LoadScene("TutorialScene-11");
        }
        else
        {
            SceneManager.LoadScene("menuScene");
        }
    }

}
