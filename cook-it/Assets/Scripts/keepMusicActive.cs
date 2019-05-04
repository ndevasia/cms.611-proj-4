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
            Destroy(this.gameObject);

        DontDestroyOnLoad(this.gameObject);
    }
}
