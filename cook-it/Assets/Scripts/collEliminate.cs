using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collEliminate : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "Ingredient")
        {
            Destroy(this.gameObject);
        }
    }
}
