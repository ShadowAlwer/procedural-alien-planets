using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using  static NoiseType;

using ProceduralNoiseProject;
using System;

public class ShapeGenerator 
{
    public ShapeSettings settings;

    public PerlinNoise perlin;

    public WorleyNoise worley;

    public SimplexNoise simplex;

    public MinMax minmax;

    public MinMax sealLevelMinMax;

    public bool isProcentSeaLevle;

    public int pointsInSea;

    public ShapeGenerator(ShapeSettings settings) {
        this.minmax = new MinMax();
        this.sealLevelMinMax = new MinMax();
        this.settings = settings;
        pointsInSea = 0;
        perlin = new PerlinNoise(settings.noise.seed,settings.noise.freq);
        worley = new WorleyNoise(settings.noise.seed, settings.noise.freq, settings.noise.jitterWorley);
        simplex = new SimplexNoise( settings.noise.seed,settings.noise.freq);
    }
    

    public Vector3 getPointElevation(Vector3 pointOnUnitSphere)
    {
        Noise noiseGen = getNoiseGen();

        Vector3 v = pointOnUnitSphere;
        float freq = settings.noise.baseRoughness;
        float amplitude = 1;
        float noise = 0;

        for (int i = 0; i < settings.noise.layers; i++)
        {
            Vector3 point=v*freq + settings.noise.center;
            noise += (noiseGen.Sample3D(point.x,point.y,point.z)+1)* .6f * amplitude;
            amplitude *= settings.noise.presistence;
            freq *= settings.noise.roughness;
        }

        if(!settings.isProcentSeaLevel && noise<settings.noise.seaLevel){
            noise = settings.noise.seaLevel;
            pointsInSea++;
            //noise = 0; //intresting results/ canions
        }

        if(settings.isProcentSeaLevel){
            sealLevelMinMax.Update(noise);
        }

        //noise = Mathf.Max(0, noise - settings.noise.seaLevel);
        noise = noise * settings.noise.power;
        v = v * (noise + 1f);
        if(!settings.isProcentSeaLevel){
            minmax.Update(v.magnitude);
        }
        return v;
    }

    private Noise getNoiseGen()
    {
        switch(settings.noise.type){
            case NoiseType.PERLIN:
                return perlin;
            case NoiseType.WORLEY:
                return worley;
            case NoiseType.SIMPLEX:
                return simplex;
            default:
                return simplex;
        }
    }
}
