using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

    public static int level = 1;
    public string[] levelInstructions;

    public Sprite[] ingredientSprite;
    Dictionary<string, Sprite> ingredientLookup = new Dictionary<string, Sprite>();
    Tuple<string, string>[] combinedTypes = {
        Tuple.Create("sugar","butter"),
        Tuple.Create("flour", "butter"),
        Tuple.Create("flour", "sugar"),
        Tuple.Create("water", "flour"),
    };
    public Sprite[] comboSprite;
    public GameObject ingredient_image_only;

    public Text failTimesText;
    public Text overCollectedText;
    int failTimes = 0;
    int overCollectedTimes = 0;

    public Text stepText;
    int step = 1;

    public Text currentStates;
    public Text currentStepRecipe;

    public Image recipe_progress;
    public Sprite[] progress;
    int progress_index = 0;

    public Image progress_pause;
    public Sprite[] progress_pause_sprites;

    public GameObject leftWall;
    public GameObject rightWall;

    public Image[] ingredientTable;
    public Text[] needTable;
    public Text[] collectedTable;

    public AudioClip ingredient;
    public AudioClip wrongIng;
    public AudioClip stageWin;
    public AudioSource audio;

    Animator playerAnimator;

    // Start is called before the first frame update
    void Start()
    {
        // find the Player
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        player = players[0];
        playerAnimator = player.GetComponent<Animator>();

        // pause the game, press enter when ready to start
        failTimesText.text = "Press Enter to start the game!";
        Time.timeScale = 0;

        //generate the first ingredient
        Vector3 pos = new Vector3(0, 5, 0);
        //generate an ingredient and assign some properties to it
        createIngredient(pos);

        //hardcode the recipe
        updateRecipe(level);

        numSteps = recipe.Count;
        updateText(collectedIngredients,0);
        /*currentStepRecipe.text = "Need:\n";
        foreach (KeyValuePair<string, int> kvp in recipe[step - 1])
        {
            currentStepRecipe.text += (kvp.Key + ": " + (kvp.Value * ratio).ToString() + " , ");
        }*/

        AudioSource[] sources = GetComponents<AudioSource>();
        audio = sources[0];

        ingredient = sources[0].clip;
        wrongIng = sources[1].clip;
        stageWin = sources[2].clip;

        // generate ingredient lookup dictionary <name, Sprite>
        foreach (Sprite sp in ingredientSprite)
        {
            ingredientLookup[sp.name] = sp;
        }
        foreach (Sprite sp in comboSprite)
        {
            ingredientLookup[sp.name] = sp;
        }

        // set up pause images (in between each step)
        progress_pause.enabled = false;
        progress_pause.sprite = progress_pause_sprites[progress_index];

    }

    void updateRecipe(int level)
    {
        Debug.Log("update recipe");
        switch (level)
        {
            case 1:
                recipe[0] = new Dictionary<string, int> { { "flour", 4 }, { "sugar", 2 }, { "butter", 1 } };
                recipe[1] = new Dictionary<string, int> { { "oil", 3 }, { "heat", 2 } };
                recipe[2] = new Dictionary<string, int> { { "frosting", 1 }, { "sugar", 2 } };
                break;
            case 2:
                recipe[0] = new Dictionary<string, int> { { "flour", 6 }, { "sugar", 1 }, { "egg", 2 } };
                recipe[1] = new Dictionary<string, int> { { "heat", 3 }, { "butter", 4 } };
                recipe[2] = new Dictionary<string, int> { { "honey", 1 }, { "butter", 2 } };
                break;
            case 3:
                recipe[0] = new Dictionary<string, int> { { "flour", 3 }, { "sugar", 1 }, { "water", 2 } };
                recipe[1] = new Dictionary<string, int> { { "heat", 3 }, { "oil", 4 } };
                recipe[2] = new Dictionary<string, int> { { "honey", 3 }, { "sugar", 1 } };
                break;
        }
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
                //pos = new Vector3(i_x + 0.5f, 5, 0);
                if (UnityEngine.Random.Range(0, 100) < secondGenRand * 100)
                {
                    // random any ingredient
                    createIngredient(pos);
                }
                else if (UnityEngine.Random.Range(0, 100) < secondGenRand * 100 * 2)
                {
                    // random combo ingredient
                    Tuple<string, string> combo = combinedTypes[UnityEngine.Random.Range(0, combinedTypes.Length - 1)];
                    createCombinedIngredient(pos, combo.Item1, combo.Item2);
                    
                }
                else
                {
                    // random in-step ingredient
                    createIngredient(pos, true);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            //clean the collected time and update the failTimes;
            failTimes += 1;
            failTimesText.text = "Fails: " + failTimes.ToString() + " times";
            resetIngredients();
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (step > numSteps)
            {
                Debug.Log("Should jump to another scene");
                if (level < levelInstructions.Length)
                {
                    //load next level instruction scene
                    initialParameters(false);
                    SceneManager.LoadScene(levelInstructions[level-1]);
                }
                else
                {
                    initialParameters(true);
                    SceneManager.LoadScene("menuScene");
                }
                // return to scene 2 (menu screen)
                //UnityEngine.SceneManagement.SceneManager.LoadScene("menuScene");
            }
            //clean the collected time and update the steps;
            Time.timeScale = 1;
            // [when pressing enter to start game, replace with correct text]
            failTimesText.text = "Fails: " + failTimes.ToString() + " times";
            // make pause image invisible again
            progress_pause.enabled = false;
            progress_pause.sprite = progress_pause_sprites[progress_index];
        }


    }
    public void initialParameters(bool initialLevel)
    {
        //initialize parameters before entering a new level
        if (initialLevel)
        {
            level = 1;
        }
        else
        {
            level += 1;
        }
        updateRecipe(level);
        step = 1;
        ratio = 1;
        failTimes = 0;
        overCollectedTimes=0;
    }

    public void createIngredient(Vector3 pos, bool fromRecipe = false)
    {
        GameObject clone;
        clone = Instantiate(Ingredient, pos, Quaternion.identity);

        // set the ingredient type here
        int ingredientIndex;
        if (fromRecipe)
        {
            int wantedInd = UnityEngine.Random.Range(0,recipe[step - 1].Count);
            string wantedName = new List<string>(recipe[step - 1].Keys)[wantedInd];
            clone.GetComponent<SpriteRenderer>().sprite = Array.Find(ingredientSprite, x => x.name.ToString() == wantedName);
        }
        else
        {
            ingredientIndex = UnityEngine.Random.Range(0, ingredientSprite.Length);
            clone.GetComponent<SpriteRenderer>().sprite = ingredientSprite[ingredientIndex];
        }
        Debug.Log("create an ingredient:" + clone.GetComponent<SpriteRenderer>().sprite.name);
    }

    public void createCombinedIngredient(Vector3 pos, string ingredient1, string ingredient2)
    {
        // generates two Ingredient objects on top of each other
        // ingredient1 and ingredient2 are the names of the sprites (ingredients)
        GameObject clone1;
        clone1 = Instantiate(Ingredient, pos, Quaternion.identity);

        clone1.GetComponent<SpriteRenderer>().sprite = ingredientLookup[ingredient1];
        clone1.GetComponent<SpriteRenderer>().enabled = false; // make it invisible

        GameObject clone2;
        clone2 = Instantiate(Ingredient, pos, Quaternion.identity);

        clone2.GetComponent<SpriteRenderer>().sprite = ingredientLookup[ingredient2];
        clone2.GetComponent<SpriteRenderer>().enabled = false; // make it invisible

        // put new combined-sprite image on top
        GameObject topclone;
        topclone = Instantiate(ingredient_image_only, pos, Quaternion.identity);
        topclone.GetComponent<SpriteRenderer>().sprite = ingredientLookup[ingredient1+"-"+ingredient2];
        topclone.transform.SetParent(clone1.transform);
        Debug.Log("combined " + ingredientLookup[ingredient1 + "-" + ingredient2].name);
        Debug.Log("create a combined ingredient:" 
                    + clone1.GetComponent<SpriteRenderer>().sprite.name
                    + ", " + clone2.GetComponent<SpriteRenderer>().sprite.name);
    }
    public void addIngredient(string addIngredient)
    {
        /*This function will read one ingredient and add that to collected ingredients
         *addIngredient is the name of the newly catched ingredient
         */
        int compareCode;
        //the new ingredient will also be added to collected in this function
        compareCode = CompareAddedIngredient(collectedIngredients, recipe, ratio, step, addIngredient); 
        updateText(collectedIngredients,compareCode);
    }

    public void updateText(Dictionary<string, int> collectedIngredients, int compareCode)
    {
        //Debug.Log("1");
        bool enterNextStep = false;
        bool fail = false;
        bool overCollected = false;
        if (step > numSteps)
        {
            currentStates.text = "Congratulations! You have completed the cookies with a score of " + (100-badTasteIndex).ToString() + ".";
            Time.timeScale = 0;
            
            return;
            // end the game - go to ending screen(?)
        }
        switch (compareCode){
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
                    //fail = true;
                    overCollected = true;
                    overCollectedTimes += 1;
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
            audio.PlayOneShot(stageWin);
            failTimesText.text = "Press Enter to start the next step!";
            step += 1;
            resetIngredients();
            Time.timeScale = 0;

            // update recipe progress image
            //recipe_progress.sprite = progress[progress_index];
            progress_index += 1;
            progress_pause.enabled = true; // make visible
            progress_pause.sprite = progress_pause_sprites[progress_index];

        }
        if (fail)
        {
            audio.PlayOneShot(wrongIng);
            resetIngredients();
            playerAnimator.SetTrigger("wrongCatch");
            //failTimesText.text = "Fails: " + failTimes.ToString() + " times";
            
        }
        else
        {
            if (overCollected)
            {
                playerAnimator.SetTrigger("wrongCatch");
                audio.PlayOneShot(wrongIng);
                //overCollectedText.text = "Overcollected: " + overCollectedTimes.ToString() + " times";
            }
            else
            {
                audio.PlayOneShot(ingredient);
            }
        }
        

        stepText.text = "Step " + step.ToString();

        //// update collected ingredients
        //currentStates.text = "Collected: \n";
        //List<string> collected_keys_sorted = new List<string>(collectedIngredients_.Keys);
        //collected_keys_sorted.Sort();
        //foreach (string key in collected_keys_sorted)
        //{
        //    currentStates.text += (key + ": " + collectedIngredients_[key] + " , ");
        //}

        //// update recipe ingredients
        //currentStepRecipe.text = "Need: \n";
        //List<string> recipe_keys_sorted = new List<string>(recipe[step - 1].Keys);
        //recipe_keys_sorted.Sort();
        ////foreach (KeyValuePair<string, int> kvp in recipe[step-1])
        //foreach (string key in recipe_keys_sorted)
        //{
        //    currentStepRecipe.text += (key + ": " + (recipe[step - 1][key]*ratio).ToString() + " , ");
        //}
        updateIngredientTable(recipe, step, ratio);
    }

    private int CompareAddedIngredient(Dictionary<string, int> collectedIngredients_, Dictionary<int, Dictionary<string, int>> recipe, int ratio, int step, string addIngredient)
    {   /*This function takes the collected ingredients, recipe, ratio, current step and new added ingredient to compare 
        Assume collectedIngredient(without new added one) contains nothing outside of the recipe[step-1]
        return value: 0--the new collected ingredients are in the recipe and do not exceed the required quantity (recipe[step]*ratio)
                      1--the new collected ingredients contain something does not belong to the recipe at this step
                      2--the new colllected ingredients contain the right ingredients but some(but not all) of them  exceed the required quantity in this step
                      3--the new collected ingredients contain all the right ingredients and all of them equal or exceed the required quantity
         */
        if (collectedIngredients_.ContainsKey(addIngredient))
        {
            collectedIngredients_[addIngredient] += 1;
        }
        else
        {
            collectedIngredients_[addIngredient] = 1;
        }
        collectedIngredients = collectedIngredients_;
        Dictionary<string, int> currentRecipe = recipe[step-1];

        int numCollected = collectedIngredients_.Count; //just collect a new ingredient
        int numRequired = currentRecipe.Count;
        if (numCollected > numRequired)
        {
            return 1;
        }

        //bool exceed = false;
        if (numCollected <= numRequired)
        {
            //check whether new ingredient contains anything not in the recipe
            if (currentRecipe.ContainsKey(addIngredient))
            {
                //check whether something exceed the required quantity
                if (collectedIngredients_[addIngredient] > currentRecipe[addIngredient] * ratio)
                {
                    return 2; 
                }
            }
            else
            {
                return 1;
            }
        }

        bool fulfilled = true;
        if (numCollected == numRequired)
        {
            foreach (KeyValuePair<string, int> kvp in collectedIngredients_)
            {
                //check whether contains anything not in the recipe
                //check whether something exceed the required quantity
                if (kvp.Value < currentRecipe[kvp.Key] * ratio)
                {
                    fulfilled = false;
                }
            }
            if (fulfilled)
            {
                return 3;
            }
        }
        return -1;//error code
    }

    public void updateIngredientTable(Dictionary<int, Dictionary<string, int>> recipe,int step , int ratio)
    {
       
        Dictionary<string, int> currentRecipe = recipe[step - 1];
        List<string> recipe_keys_sorted = new List<string>(currentRecipe.Keys);
        recipe_keys_sorted.Sort();
        for (int i=0; i < recipe_keys_sorted.Count; i++)
        {
            string name = recipe_keys_sorted[i];
            ingredientTable[i].sprite = Array.Find(ingredientSprite, x => x.name.ToString() == name);
            needTable[i].text = (ratio*currentRecipe[name]).ToString();
            if (collectedIngredients.ContainsKey(name))
            {
                collectedTable[i].text = collectedIngredients[name].ToString();
                if (collectedIngredients[name] > currentRecipe[name]*ratio)
                {
                    collectedTable[i].color = Color.red;
                }
                else
                {
                    collectedTable[i].color = Color.black;
                }
            }
            else
            {
                collectedTable[i].text = "";
            }
        }
        int numIngredients = recipe_keys_sorted.Count;
        if (numIngredients < 3)
        {
            for(int i = numIngredients; i < 3; i++)
            {
                Color C = ingredientTable[i].color;
                C.a = 0;
                ingredientTable[i].color = C;
                needTable[i].text = "";
                collectedTable[i].text = "";
            }
        }
    }

    public void resetIngredients()
    {
        /*for (int i = 0; i < Data.numColors; i++)
        {
            numberColors[i] = 0;
        }
        FindObjectOfType<GameManager>().GetComponent<GameManager>().updateText(numberColors);*/
        collectedIngredients = new Dictionary<string, int>();
        //gm.GetComponent<GameManager>().updateText(collectedIngredients);
    }
}
