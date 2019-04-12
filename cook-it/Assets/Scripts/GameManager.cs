using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject Ingredient;
    public float genTime = 1;     //ingredient generation time interval
    float currentTime = 0;
    //Color[] colors = Data.colors;
    //string[] colorNames = Data.colorNames;
    GameObject player;
    Dictionary<string, int> collectedIngredients = new Dictionary<string, int>();
    public Sprite[] ingredientSprite;

    public Text failTimesText;
    int failTimes = 0;

    public Text stepText;
    int step = 1;

    public Text currentStates;

    // Start is called before the first frame update
    void Start()
    {
        // find the Player
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        player = players[0];

        //generate the first ingredient
        Vector3 pos = new Vector3(0, 5, 0);
        //generate an ingredient and assign some properties to it
        createIngredient(pos);
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
            createIngredient(pos);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            //clean the collected time and update the failTimes;
            failTimes += 1;
            failTimesText.text = "Fails: " + failTimes.ToString() + " times";
            player.GetComponent<getIngredient>().resetIngredients();
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            //clean the collected time and update the steps;
            step += 1;
            stepText.text = "Step: " + step.ToString();
            player.GetComponent<getIngredient>().resetIngredients();
        }

    }

    public void createIngredient(Vector3 pos)
    {
        GameObject clone;
        clone = Instantiate(Ingredient, pos, Quaternion.identity);

        // set the ingredient type here
        int ingredientIndex = Random.Range(0, ingredientSprite.Length);
        clone.GetComponent<SpriteRenderer>().sprite = ingredientSprite[ingredientIndex];
        Debug.Log("create an ingredient:" + ingredientSprite[ingredientIndex]);

        /*int i = Random.Range(0, colors.Length);
        clone.GetComponent<ingredientProperties>().colorIndex = i;
        clone.GetComponent<SpriteRenderer>().material.color = colors[i];
        clone.GetComponent<ingredientProperties>().color = colorNames[i];*/
    }

    public void updateText(Dictionary<string, int> collectedIngredients)
    {
        //this function will update the text showing the collected ingredients
        currentStates.text = "Collected Ingredients: ";
        foreach (KeyValuePair<string, int> kvp in collectedIngredients)
        {
            currentStates.text += (kvp.Key + ": " + kvp.Value.ToString() + " , ");
        }
    }

    

    
}
