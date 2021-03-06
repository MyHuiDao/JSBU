﻿using UnityEngine;
using UnityEngine.UI;
public class Ef_ : MonoBehaviour
{
    //public Texture[] textures;
    private Material material;
    private int index = 0;

    void Start()
    {
        //textures = Resources.LoadAll<Texture>("Water");
        material = GetComponent<Image>().material;
        InvokeRepeating("ChangeTexture", 0, 0.1f);
    }

    void ChangeTexture()
    {
        material.mainTexture = ResouseManager.Instance.WATERTEXTURES[index];//textures[index];
        index = (index + 1) % ResouseManager.Instance.WATERTEXTURES.Length;
    }
}
