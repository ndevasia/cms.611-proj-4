using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class getIngredient : MonoBehaviour
{
    //int[] numberColors = new int[Data.numColors];
    GameManager gm;
    Dictionary<string, int> collectedIngredients = new Dictionary<string, int>();

    public AudioClip ingredient;
    public AudioClip wrongIng;
    public AudioSource audio;

    private void Start()
    {
        gm = FindObjectOfType<GameManager>();

        /*for(int i = 0; i < Data.numColors; i++)
        {
            numberColors[i] = 0;
        }*/

        AudioSource[] sources = GetComponents<AudioSource>();
        audio = sources[0];

        ingredient = sources[0].clip;
        wrongIng = sources[1].clip;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ingredient")
        {
            audio.PlayOneShot(ingredient);
            //get the color of the ingredient and change the player color to it
            //Color colcolor = collision.gameObject.GetComponent<Renderer>().material.color;
            //this.gameObject.GetComponent<SpriteRenderer>().material.color = colcolor;
            //update the collected ingredients and change the text
            //numberColors[collision.gameObject.GetComponent<ingredientProperties>().colorIndex] += 1;
            //FindObjectOfType<GameManager>().GetComponent<GameManager>().updateText(numberColors);
            string ingredientName = collision.gameObject.GetComponent<ingredientProperties>().ingredientName;
            Debug.Log("collected: " + ingredientName);
            if (collectedIngredients.ContainsKey(ingredientName))
            {
                collectedIngredients[ingredientName] += 1;
            }
            else
            {
                collectedIngredients[ingredientName] = 1;
            }
            gm.GetComponent<GameManager>().updateText(collectedIngredients);
            
        }
        else if (collision.gameObject.tag == "Wall")
        {
            //reset the speed of the player if collide with the wall
            this.gameObject.GetComponent<PlayerMovement>().resetSpeed();
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
        gm.GetComponent<GameManager>().updateText(collectedIngredients);
    }


}
