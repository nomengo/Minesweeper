using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXPlaying : MonoBehaviour
{
    public AudioSource buttonClicking;

    public void PlayClick()
    {
        buttonClicking.Play();
    }
}
