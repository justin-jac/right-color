using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static AudioClip jump, open, close, button;
    static AudioSource source;

    // Start is called before the first frame update
    void Start()
    {
        jump = Resources.Load<AudioClip> ("jump");
        open = Resources.Load<AudioClip> ("open");
        close = Resources.Load<AudioClip> ("close");
        button = Resources.Load<AudioClip> ("button");

        source = GetComponent<AudioSource> ();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void PlaySound (string clip)
    {
        switch (clip)
        {
            case "jump":
                source.PlayOneShot(jump); break;
            case "open":
                source.PlayOneShot(open); break;
            case "close":
                source.PlayOneShot(close); break;
            case "button":
                source.PlayOneShot(button); break;
        }
    }
}
