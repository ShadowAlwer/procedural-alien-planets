using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NoiseSettings
{
    [Range(0,1)]
    public float power=1;
    [Range(0, 2)]
    public float roughness = 2;

    public float baseRoughness = 1;

    public float presistence = 0.5f;

    public int layers = 1;
    public Vector3 center=Vector3.zero;

    [Range(0, 10)]
    public float seaLevel = 1;

    public float freq = 1;
}
