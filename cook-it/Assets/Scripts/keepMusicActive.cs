using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class keepMusicActive : MonoBehaviour
{
    private void Awake()
    {
        // Keep music active no matter which scene we are in
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Music");
        if (objs.Length > 1)
        {
            Destroy(objs[0].gameObject);
            //Debug.Log("cleared obj: " + objs.Length);
            //Debug.Log(objs[0].gameObject);
            //Debug.Log(objs[1].gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }
}
