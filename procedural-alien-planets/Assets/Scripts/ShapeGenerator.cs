using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ProceduralNoiseProject;


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
        Noise noiseGen = getNoiseGen(settings.noise.type);

        Vector3 v = pointOnUnitSphere;
        float freq = settings.noise.baseRoughness;
        float amplitude = 1;
        float noise = 0;

        for (int i = 0; i < settings.noise.layers.Length; i++)
        {
            noiseGen = getNoiseGen(settings.noise.layers[i].type);
            Vector3 point=v*freq + settings.noise.center;

            noise+=calculateLayerValue(noiseGen, settings.noise.layers[i], point, amplitude);

            amplitude *= settings.noise.presistence;
            freq *= settings.noise.roughness;
        }

        if(!settings.isProcentSeaLevel && noise<settings.noise.seaLevel){
            noise = settings.noise.seaLevel;
            pointsInSea++;
            //noise = 0; //intresting results/ canions

            //realSeaLevel
            if(settings.realSeaLevel == 0){
                settings.realSeaLevel = (v * ((noise * settings.noise.power) + 1f)).magnitude;
            }
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

    private float calculateLayerValue(Noise noiseGen, Layer layer, Vector3 point, float amplitude){
            
            float value = noiseGen.Sample3D(point.x,point.y,point.z);

            if(layer.sign == LayerSign.ADD){
                value = (value+1)* amplitude; 
            }

            if(layer.sign == LayerSign.SUBTRACT){
                value = -(value+1)* amplitude; 
            }
            if(layer.sign == LayerSign.NEGATIVE){
                
                value = Mathf.Pow(value,4);   
                value = (value+1)*  amplitude; 
            }
             if(layer.sign == LayerSign.SUB_NEG){
                
                value = Mathf.Pow(value,4);   
                value = (1-(value+1))* amplitude; 
            }

            return value;
    }

    private Noise getNoiseGen(NoiseType type)
    {
        switch(type){
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
