using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class getIngredient : MonoBehaviour
{
    //int[] numberColors = new int[Data.numColors];
    GameManager gm;
    //Dictionary<string, int> collectedIngredients = new Dictionary<string, int>();

    private void Start()
    {
        gm = FindObjectOfType<GameManager>();

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ingredient")
        {
            string ingredientName = collision.gameObject.GetComponent<ingredientProperties>().ingredientName;
            Debug.Log("collected: " + ingredientName);
            //if (collectedIngredients.ContainsKey(ingredientName))
            //{
            //    collectedIngredients[ingredientName] += 1;
            //}
            //else
            //{
            //    collectedIngredients[ingredientName] = 1;
            //}
            gm.GetComponent<GameManager>().addIngredient(ingredientName);
            
        }
        else if (collision.gameObject.tag == "Wall")
        {
            //reset the speed of the player if collide with the wall
            this.gameObject.GetComponent<PlayerMovement>().resetSpeed();
        }
    }

    


}
