using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeGenerator 
{
    public ShapeSettings settings;

    public ShapeGenerator(ShapeSettings settings) {
        this.settings = settings;
    }
    

    public Vector3 getPointElevation(Vector3 pointOnUnitSphere) {

       return  applyPerlinNoise(pointOnUnitSphere);
    }


    public Vector3 applyPerlinNoise(Vector3 pointOnUnitSphere)
    {
        Vector3 v = pointOnUnitSphere;
        float freq = settings.noise.baseRoughness;
        float amplitude = 1;
        float noise = 0;

        for (int i = 0; i < settings.noise.layers; i++)
        {
            noise += (PerlinNoise3D.getPerlinNoise3D(v * freq + settings.noise.center, settings.noise.freq) + 1) * .5f * amplitude;
            amplitude *= settings.noise.presistence;
            freq *= settings.noise.roughness;
        }
        noise = Mathf.Max(0, noise - settings.noise.seaLevel);
        noise = noise * settings.noise.power;
        v = v * settings.planetRadius * (noise + 1);
        return v;
    }

}
