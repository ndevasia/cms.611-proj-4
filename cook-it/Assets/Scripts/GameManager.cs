﻿using System.Collections;
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

    Dictionary<int, Dictionary<string, int>> recipe = new Dictionary<int, Dictionary<string, int>>();
    int numSteps;
    int ratio = 1;
    int badTasteIndex = 0;

    public Sprite[] ingredientSprite;

    public Text failTimesText;
    int failTimes = 0;

    public Text stepText;
    int step = 1;

    public Text currentStates;
    public Text currentStepRecipe;

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
        
        //hardcode the recipe
        recipe[0] = new Dictionary<string, int> { { "flour", 4 }, { "sugar", 2 }, { "butter", 1 } };
        recipe[1] = new Dictionary<string, int> { { "oil", 3 }, { "heat", 2 } };
        recipe[2] = new Dictionary<string, int> { { "parsley", 1 }, { "sugar", 2 } };
        numSteps = recipe.Count;
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
            Time.timeScale = 1;
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
        bool enterNextStep = false;
        bool fail = false;
        if (step >= numSteps)
        {
            currentStates.text = "Congratulation! You have a " + badTasteIndex.ToString() + "-level bad taste meal!";
        }
        if (step == 1)
        {
            switch (CompareIngredient(collectedIngredients,recipe,ratio,step)){
                case 0: break;
                case 1: 
                    fail = true;
                    failTimes += 1;
                    break;
                case 2:
                    //update the ratio to desired value 
                    ratio += 1;
                    break;
                case 3:
                    enterNextStep = true;
                    break;
                default:
                    break;
            }
        }
        else
        {
            switch (CompareIngredient(collectedIngredients, recipe, ratio, step))
            {
                case 0: break;
                case 1:
                    fail = true;
                    failTimes += 1;
                    break;
                case 2:
                    fail = true;
                    failTimes += 1;
                    break;
                case 3:
                    enterNextStep = true;
                    break;
                default:
                    break;
            }
        }
        
        if (enterNextStep)
        {
            failTimesText.text = "Entering the next step, press Enter to continue!";
            step += 1;
            collectedIngredients = new Dictionary<string, int>();
            Time.timeScale = 0;
        }
        if (fail)
        {
            collectedIngredients = new Dictionary<string, int>();
            player.GetComponent<getIngredient>().resetIngredients();
            failTimesText.text = "Fails: " + failTimes.ToString() + " times";
        }
        stepText.text = "Step: " + step.ToString();

        currentStates.text = "Colledted:";
        foreach (KeyValuePair<string, int> kvp in collectedIngredients)
        {
            currentStates.text += (kvp.Key + ": " + kvp.Value.ToString() + " , ");
        }
        currentStepRecipe.text = "Need:";
        foreach (KeyValuePair<string, int> kvp in recipe[step-1])
        {
            currentStepRecipe.text += (kvp.Key + ": " + (kvp.Value*ratio).ToString() + " , ");
        }
    }

    private int CompareIngredient(Dictionary<string, int> collectedIngredients, Dictionary<int, Dictionary<string, int>> recipe, int ratio, int step)
    {   /*This function takes the collected ingredients, recipe, ratio and current step to compare 
        return value: 0--the collected ingredients are in the recipe and do not exceed the required quantity (recipe[step]*ratio)
                      1--the collected ingredients contain something does not belong to the recipe at this step
                      2--the colllected ingredients contain the right ingredients but some(but not all) of them  exceed the required quantity in this step
                      3--the collected ingredients contain all the right ingredients and all of them equal or exceed the required quantity
         */
        Dictionary<string, int> currentRecipe = recipe[step-1];
        int numCollected = collectedIngredients.Count;
        int numRequired = currentRecipe.Count;
        if (numCollected > numRequired)
        {
            return 1;
        }

        bool exceed = false;
        if (numCollected < numRequired)
        {
            foreach (KeyValuePair<string, int> kvp in collectedIngredients)
            {
                //check whether contains anything not in the recipe
                if (currentRecipe.ContainsKey(kvp.Key))
                {
                    //check whether something exceed the required quantity
                    if (kvp.Value > currentRecipe[kvp.Key] * ratio)
                    {
                        exceed = true;   
                    }
                }
                else
                {
                    return 1;
                }
            }
            if (exceed)
            {
                return 2;
            }
            else
            {
                return 0;
            }
        }

        bool fulfilled = true;
        if (numCollected == numRequired)
        {
            foreach (KeyValuePair<string, int> kvp in collectedIngredients)
            {
                //check whether contains anything not in the recipe
                if (currentRecipe.ContainsKey(kvp.Key))
                {
                    //check whether something exceed the required quantity
                    if (kvp.Value > currentRecipe[kvp.Key] * ratio)
                    {
                        exceed = true;
                    }
                    if (kvp.Value < currentRecipe[kvp.Key] * ratio)
                    {
                        fulfilled = false;
                    }
                }
                else return 1;  
            }
            if (fulfilled)
            {
                return 3;
            }
            else
            {
                if (exceed)
                {
                    return 2;
                }
                else return 0;
            }
        }
        return -1;//error code
    }
}
