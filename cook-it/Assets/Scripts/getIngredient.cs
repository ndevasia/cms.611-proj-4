using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class getIngredient : MonoBehaviour
{
    int[] numberColors = new int[Data.numColors];
    private void Start()
    {
        for(int i = 0; i < Data.numColors; i++)
        {
            numberColors[i] = 0;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ingredient")
        {
            //get the color of the ingredient and change the player color to it
            Color colcolor = collision.gameObject.GetComponent<Renderer>().material.color;
            this.gameObject.GetComponent<SpriteRenderer>().material.color = colcolor;
            //update the collected ingredients and change the text
            numberColors[collision.gameObject.GetComponent<ingredientProperties>().colorIndex] += 1;
            FindObjectOfType<GameManager>().GetComponent<GameManager>().updateText(numberColors);
        }
        else if (collision.gameObject.tag == "Wall")
        {
            //reset the speed of the player if collide with the wall
            this.gameObject.GetComponent<PlayerMovement>().resetSpeed();
        }
    }

    public void resetIngredients()
    {
        for (int i = 0; i < Data.numColors; i++)
        {
            numberColors[i] = 0;
        }
        FindObjectOfType<GameManager>().GetComponent<GameManager>().updateText(numberColors);
    }
}
