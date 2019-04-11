using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ingredientProperties : MonoBehaviour
{
    //this script stores the properties of ingredients and can be accessed from other objects
    // Start is called before the first frame update
    public string color;
    public int colorIndex;
    public string ingredientName; // do not call variable "name", conflicts with inherited variables
}
