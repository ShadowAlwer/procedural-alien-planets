using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PerlinNoise3D
{
     static float xof = Random.value;
     static float yof = Random.value;

     
        public static float Sample3D_OLD(Vector3 pointOnPlanet, float freq){

        pointOnPlanet.x += xof;
        pointOnPlanet.y += yof;

        float XY = _perlin3DFixed(pointOnPlanet.x,pointOnPlanet.y, freq);
        float XZ = _perlin3DFixed(pointOnPlanet.x,pointOnPlanet.z, freq);
        float YZ = _perlin3DFixed(pointOnPlanet.y,pointOnPlanet.z, freq);

        float YX = _perlin3DFixed(pointOnPlanet.y,pointOnPlanet.x, freq);
        float ZX = _perlin3DFixed(pointOnPlanet.z,pointOnPlanet.x, freq);
        float ZY = _perlin3DFixed(pointOnPlanet.z,pointOnPlanet.y, freq);

        float noise = (XY * XZ * YX * YZ * ZX * ZY);
        return noise;


    }


    static float _perlin3DFixed(float a, float b,float freq)
    {
        return Mathf.Sin(freq*Mathf.PI * Mathf.PerlinNoise(a, b));
    }

    
}

