using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class ShapeSettings : ScriptableObject
{
    public NoiseSettings noise;

    public int planetRadius=10;

    ShapeSettings(NoiseSettings noiseSettings) {
        noise = noiseSettings;
    }
    
}
