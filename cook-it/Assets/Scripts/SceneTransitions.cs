using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitions : MonoBehaviour
{
    // Start is called before the first frame update
    public string game;
    public string instructions;

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


}
