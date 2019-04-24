using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class keepMusicActive : MonoBehaviour
{
    private AudioClip click;
    private AudioClip hover;
    private AudioSource audio;

    void start()
    {
        AudioSource[] sources = GetComponents<AudioSource>();
        audio = sources[0];

        click = sources[0].clip;
        hover = sources[1].clip;
    }

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

        AudioSource[] sources = GetComponents<AudioSource>();
        audio = sources[1];

        click = sources[1].clip;
        hover = sources[2].clip;
    }

    public void hoverSound()
    {
        audio.PlayOneShot(hover);
    }

    public void clickSound()
    {
        audio.PlayOneShot(click);
    }
}
