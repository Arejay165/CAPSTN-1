using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public SFX[] sounds;
    public Musics[] BGM;

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

 

    public void playSound(int index)
    {
        sounds[index].sound.Play();  
    }

    public void playMusic(int index)
    {
       // BGM[index].musicFile.Play();
    }

    public void stopMusic(int index)
    {
        BGM[index].musicFile.Stop();
    }

}
