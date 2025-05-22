using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlphaAdjuster : MonoBehaviour
{
    public float alphaValue = 1.0f;
    
        private void Start()
        {
            AdjustAlpha();
        }
    
        private void AdjustAlpha()
        {
            Material material = GetComponent<Renderer>().material;
            material.color = new Color(material.color.r, material.color.g, material.color.b, alphaValue);
        }
}
