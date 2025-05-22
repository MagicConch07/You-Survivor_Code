using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoSingleton<SoundManager>
{
    private AudioSource _source;

    void Awake()
    {
        _source = GetComponent<AudioSource>();
    }
}
