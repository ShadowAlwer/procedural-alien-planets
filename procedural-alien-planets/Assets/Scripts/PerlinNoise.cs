
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ProceduralNoiseProject
{
    public class PerlinNoise : Noise
    {
        private float xof = 0;
        private float yof = 0;
        private float zof = 0;

        private float freq = 1;

        private int seed = 1;

        public PerlinNoise() { }

        public PerlinNoise(int _seed, float _freq)
        {
            UpdateSeed(_seed);
            Random.InitState(seed);
            xof = Random.value;
            yof = Random.value;
            zof = Random.value;
            freq = _freq;
        }

        public override void UpdateSeed(int _seed)
        {
            seed = _seed;
        }

        public override float Sample3D(float x, float y, float z)
        {
            Vector3 pointOnPlanet = new Vector3(x, y, z);
            pointOnPlanet.x += xof;
            pointOnPlanet.y += yof;
            pointOnPlanet.z += zof;

            float XY = _perlin3DFixed(pointOnPlanet.x, pointOnPlanet.y, freq);
            float XZ = _perlin3DFixed(pointOnPlanet.x, pointOnPlanet.z, freq);
            float YZ = _perlin3DFixed(pointOnPlanet.y, pointOnPlanet.z, freq);

            float YX = _perlin3DFixed(pointOnPlanet.y, pointOnPlanet.x, freq);
            float ZX = _perlin3DFixed(pointOnPlanet.z, pointOnPlanet.x, freq);
            float ZY = _perlin3DFixed(pointOnPlanet.z, pointOnPlanet.y, freq);

            float noise = (XY * XZ * YX * YZ * ZX * ZY);
            return noise;
        }


        private float _perlin3DFixed(float a, float b, float _freq)
        {
            return Mathf.Sin(_freq * Mathf.PI * Mathf.PerlinNoise(a, b));
        }

        public override float Sample1D(float x){
            return 1;
        }

        public override float Sample2D(float x, float y){
            return 1;
        }
    }
}
