using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data : MonoBehaviour
{
    //this file can should be accessable to every other scripts to store and change data
    public static int numColors = 7;
    public static Color[] colors = new Color[numColors];
    public static string[] colorNames = new string[numColors];
    private void Start()
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
        colors[4] = new Color(255, 127, 80);
        colorNames[4] = "Orange";
        colors[5] = new Color(255, 0, 255);
        colorNames[5] = "Purple";
        colors[6] = Color.white;
        colorNames[6] = "White";

    }
    

}
