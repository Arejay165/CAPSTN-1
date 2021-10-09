using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public SFX[] sounds;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }


    }

    /*public void Start()
    {
        Debug.Log(sounds.Length);

        for(int i = 0; i <= sounds.Length;i++)
        {
            sounds[i].sound.Stop();
        }
    }*/

    public void playSound(int index)
    {
        sounds[index].sound.Play();  
    }

   
}
