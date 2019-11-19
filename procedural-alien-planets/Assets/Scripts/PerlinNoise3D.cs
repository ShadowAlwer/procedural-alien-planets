using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinNoise3D
{

    static float xof = Random.value;
    static float yof = Random.value;
    static public float getPerlinNoise3D(Vector3 pointOnPlanet, int freq){

        pointOnPlanet.x += xof;
        pointOnPlanet.y += yof;

        float XY = _perlin3DFixed(pointOnPlanet.x,pointOnPlanet.y);
        float XZ = _perlin3DFixed(pointOnPlanet.x,pointOnPlanet.z);
        float YZ = _perlin3DFixed(pointOnPlanet.y,pointOnPlanet.z);

        float YX = _perlin3DFixed(pointOnPlanet.y,pointOnPlanet.x);
        float ZX = _perlin3DFixed(pointOnPlanet.z,pointOnPlanet.x);
        float ZY = _perlin3DFixed(pointOnPlanet.z,pointOnPlanet.y);

        float noise = (XY * XZ * YX * YZ * ZX * ZY);
        return noise;


    }

    static float _perlin3DFixed(float a, float b)
    {
        return Mathf.Sin(Mathf.PI * Mathf.PerlinNoise(a, b));
    }
}
