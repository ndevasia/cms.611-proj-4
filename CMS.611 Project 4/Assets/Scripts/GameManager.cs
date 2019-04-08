using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject Ingredient;
    public float genTime = 1;     //ingredient generation time interval
    float currentTime = 0;
    Color[] colors = new Color[6];
    string[] colorNames = new string[6];
    // Start is called before the first frame update
    void Start()
    {
        //initialize all possible colors
        colors[0] = Color.red;
        colorNames[0] = "Red";
        colors[1] = Color.green;
        colorNames[1] = "Green";
        colors[2] = Color.blue;
        colorNames[2] = "Blue";
        colors[3] = Color.yellow;
        colorNames[3] = "Yellow";
        colors[4] = Color.cyan;
        colorNames[4] = "Cyan";
        colors[5] = Color.magenta;
        colorNames[5] = "Magenta";
        //generate the first ingredient
        Vector3 pos = new Vector3(0, 5, 0);
        //generate an ingredient and assign some properties to it
        GameObject clone;
        clone = Instantiate(Ingredient, pos, Quaternion.identity);
        int i = Random.Range(0, colors.Length);
        clone.GetComponent<ingredientProperties>().colorIndex = i;
        clone.GetComponent<Renderer>().material.color = colors[i];
        clone.GetComponent<ingredientProperties>().color = colorNames[i];


    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;    //keep track of the current time
        if (currentTime > genTime)
        {
            //generate a new ingredient and assign properties
            currentTime += -genTime;
            float i_x = Random.Range(-4, 5);
            Vector3 pos = new Vector3(i_x, 5, 0);
            GameObject clone;
            clone = Instantiate(Ingredient, pos, Quaternion.identity);
            int i = Random.Range(0, colors.Length);
            clone.GetComponent<ingredientProperties>().colorIndex = i;
            clone.GetComponent<Renderer>().material.color = colors[i];
            clone.GetComponent<ingredientProperties>().color = colorNames[i];
        }
    }

    public Text currentStates;
    public void updateText(int[] numberColors)
    {
        //this function will update the text showing the collected ingredients
        currentStates.text = "";
        for(int i =0;i<6; i++)
        {
            currentStates.text += (colorNames[i] + ": " + numberColors[i].ToString()+" , ");
        }  
    }
}
