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
    [Range(1, 8)]
    public int layers = 1;
    public Vector3 center=Vector3.zero;

    [Range(0, 10)]
    public int seaLevel = 1;
}
