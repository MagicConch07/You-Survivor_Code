using System;
using System.Collections;
using System.Collections.Generic;
using ObjectPooling;
using UnityEngine;

public class MuzzleFlame : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] _particles;

    public void PlayParticle()
    {
        foreach (ParticleSystem par in _particles)
        {
            par.time = 0;
            par.Play();
        }
    }
}
