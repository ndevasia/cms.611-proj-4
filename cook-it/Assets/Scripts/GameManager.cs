using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject Ingredient;
    public float genTime = 1;     //ingredient generation time interval
    public float secondGenProb = 0.5f; //probalbility to generate two ingredient at the same time
    public float secondGenRand = 0.2f; //probalbility to generate a random second ingredient instead of from the recipe
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

    public Image recipe_progress;
    public Sprite[] progress;
    int progress_index = 0;

    public GameObject leftWall;
    public GameObject rightWall;

    public AudioClip ingredient;
    public AudioClip wrongIng;
    public AudioSource audio;

    // Start is called before the first frame update
    void Start()
    {
        // find the Player
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        player = players[0];

        // pause the game, press enter when ready to start
        failTimesText.text = "Press Enter to start the game!";
        Time.timeScale = 0;

        //generate the first ingredient
        Vector3 pos = new Vector3(0, 5, 0);
        //generate an ingredient and assign some properties to it
        createIngredient(pos);
        
        //hardcode the recipe
        recipe[0] = new Dictionary<string, int> { { "flour", 4 }, { "sugar", 2 }, { "butter", 1 } };
        recipe[1] = new Dictionary<string, int> { { "oil", 3 }, { "heat", 2 } };
        recipe[2] = new Dictionary<string, int> { { "parsley", 1 }, { "sugar", 2 } };
        numSteps = recipe.Count;
        updateText(collectedIngredients);
        /*currentStepRecipe.text = "Need:\n";
        foreach (KeyValuePair<string, int> kvp in recipe[step - 1])
        {
            currentStepRecipe.text += (kvp.Key + ": " + (kvp.Value * ratio).ToString() + " , ");
        }*/

        AudioSource[] sources = GetComponents<AudioSource>();
        audio = sources[0];

        ingredient = sources[0].clip;
        wrongIng = sources[1].clip;
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;    //keep track of the current time
        if (currentTime > genTime)
        {
            //generate a new ingredient and assign properties
            currentTime += -genTime;
            float i_x = UnityEngine.Random.Range(leftWall.transform.position[0]+1, rightWall.transform.position[0]-1);
            Vector3 pos = new Vector3(i_x, 5, 0);
            createIngredient(pos);

            //  chance to spawn 2 ingredients
            if (UnityEngine.Random.Range(0, 100) < secondGenProb*100)
            {
                float new_i_x = UnityEngine.Random.Range(leftWall.transform.position[0] + 1, rightWall.transform.position[0] - 1);
                // make sure ingredients are minimum 5 distance away (too close = too hard to catch)
                while (Mathf.Abs(new_i_x - i_x) < 5)
                {
                    new_i_x = UnityEngine.Random.Range(leftWall.transform.position[0] + 1, rightWall.transform.position[0] - 1);
                }
                pos = new Vector3(new_i_x, 5, 0);
                if (UnityEngine.Random.Range(0, 100) < secondGenRand * 100)
                {
                    createIngredient(pos);
                }
                else
                {
                    createIngredient(pos, true);
                }
            }
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
            if (step > numSteps)
            {
                // return to scene 2 (menu screen)
                UnityEngine.SceneManagement.SceneManager.LoadScene(2);
            }
            //clean the collected time and update the steps;
            Time.timeScale = 1;
            // [when pressing enter to start game, replace with correct text]
            failTimesText.text = "Fails: " + failTimes.ToString() + " times";
        }


    }

    public void createIngredient(Vector3 pos, bool fromRecipe = false)
    {
        GameObject clone;
        clone = Instantiate(Ingredient, pos, Quaternion.identity);

        // set the ingredient type here
        int ingredientIndex;
        if (fromRecipe)
        {
            int wantedInd = UnityEngine.Random.Range(0,recipe[step - 1].Count - 1);
            string wantedName = new List<string>(recipe[step - 1].Keys)[wantedInd];
            clone.GetComponent<SpriteRenderer>().sprite = Array.Find(ingredientSprite, x => x.name.ToString() == wantedName);
        }
        else
        {
            ingredientIndex = UnityEngine.Random.Range(0, ingredientSprite.Length);
            clone.GetComponent<SpriteRenderer>().sprite = ingredientSprite[ingredientIndex];
        }
        Debug.Log("create an ingredient:" + clone.name.ToString());
    }

    public void updateText(Dictionary<string, int> collectedIngredients)
    {
        bool enterNextStep = false;
        bool fail = false;
        if (step > numSteps)
        {
            currentStates.text = "Congratulations! You have a " + badTasteIndex.ToString() + "-level bad taste meal!";
            Time.timeScale = 0;
            return;
            // end the game - go to ending screen(?)
        }
        switch (CompareIngredient(collectedIngredients,recipe,ratio,step)){
            case 0: break;
            case 1: 
                fail = true;
                failTimes += 1;
                ratio = 1;
                break;
            case 2:
                if (step == 1)
                {
                    //update the ratio to desired value 
                    ratio += 1; 
                }
                else
                {
                    fail = true;
                    failTimes += 1;
                }
                  
                break;
            case 3:
                enterNextStep = true;
                break;
            default:
                break;
        }

                     
        
        if (enterNextStep)
        {
            failTimesText.text = "Press Enter to start the next step!";
            step += 1;
            collectedIngredients = new Dictionary<string, int>();
            player.GetComponent<getIngredient>().resetIngredients();
            Time.timeScale = 0;

            // update recipe progress image
            recipe_progress.sprite = progress[progress_index];
            progress_index += 1;

        }
        if (fail)
        {
            audio.PlayOneShot(wrongIng);
            collectedIngredients = new Dictionary<string, int>();
            player.GetComponent<getIngredient>().resetIngredients();
            failTimesText.text = "Fails: " + failTimes.ToString() + " times";
        }
        else
        {
            audio.PlayOneShot(ingredient);
        }

        stepText.text = "Step " + step.ToString();

        // update collected ingredients
        currentStates.text = "Collected: \n";
        List<string> collected_keys_sorted = new List<string>(collectedIngredients.Keys);
        collected_keys_sorted.Sort();
        foreach (string key in collected_keys_sorted)
        {
            currentStates.text += (key + ": " + collectedIngredients[key] + " , ");
        }

        // update recipe ingredients
        currentStepRecipe.text = "Need: \n";
        List<string> recipe_keys_sorted = new List<string>(recipe[step - 1].Keys);
        recipe_keys_sorted.Sort();
        //foreach (KeyValuePair<string, int> kvp in recipe[step-1])
        foreach (string key in recipe_keys_sorted)
        {
            currentStepRecipe.text += (key + ": " + (recipe[step - 1][key]*ratio).ToString() + " , ");
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
