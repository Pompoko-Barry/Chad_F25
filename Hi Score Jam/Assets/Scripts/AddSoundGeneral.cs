using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddSoundGeneral : MonoBehaviour
{

    public AudioSource sfxSource;
    public AudioClip backgroundSound;
    // Start is called before the first frame update
    void Start()
    {
        sfxSource.clip = (backgroundSound);
        sfxSource.Play();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
