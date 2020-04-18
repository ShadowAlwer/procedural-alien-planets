﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class ShapeSettings : ScriptableObject
{
    public NoiseSettings noise;

    public int planetRadius = 10;

    public bool isProcentSeaLevel = false;

    [Range(0,1)]
    public float procentSeaLevel = 0;

    [HideInInspector]
    public float maxHeight;

    [HideInInspector]
    public float seaCoverage;


    ShapeSettings(NoiseSettings noiseSettings) {
        noise = noiseSettings;
    }
    
}
