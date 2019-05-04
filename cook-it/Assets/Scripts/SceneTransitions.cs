using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitions : MonoBehaviour
{
    // Start is called before the first frame update
    public string game;
    public string instructions;
    public string tutorial;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void toGame()
    {
        SceneManager.LoadScene(game);
    }

    public void toInstructions()
    {
        SceneManager.LoadScene(instructions);
    }

    public void toTutorial()
    {
        SceneManager.LoadScene(tutorial);
    }

    public void loadNext()
    {
        Debug.Log(SceneManager.GetActiveScene().buildIndex);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }


}
