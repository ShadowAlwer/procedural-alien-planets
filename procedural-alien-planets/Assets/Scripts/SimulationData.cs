using UnityEngine;
using System;

[System.Serializable]
public class SimulationData {

    public bool saveCSV = false;

    public bool calculateLandMasses = false;

    public int simulatonRuns = 1;

    public int resolutionDelta = 0;

    public float baseRoughnessDelta = 0;

    public float roughnessDelta = 0;

    public float presistenceDelta = 0;

    public Vector3 centerDelta = Vector3.zero;

    public int seedDelta = 0;

    public bool addLayer = false;

    public Layer layer = new Layer(NoiseType.SIMPLEX, LayerSign.ADD);

    [Range(0, 1)]
    public  float seaLevelDelta = 0.05f;

    public void PrepareSimulation(Planet planet) {
        int lenght = planet.shapeSettings.noise.layers.Length+1;
        planet.resolution += resolutionDelta;
        planet.shapeSettings.procentSeaLevel += seaLevelDelta;
        planet.shapeSettings.noise.baseRoughness += baseRoughnessDelta;
        planet.shapeSettings.noise.roughness += roughnessDelta;
        planet.shapeSettings.noise.presistence += presistenceDelta;
        planet.shapeSettings.noise.center += centerDelta;
        planet.shapeSettings.noise.seed += seedDelta;
        if (addLayer) {
            Layer[] layers = new Layer[lenght];
            planet.shapeSettings.noise.layers.CopyTo(layers, 0);
            layers[lenght - 1]= layer;
            planet.shapeSettings.noise.layers =layers;
        }
        
    }

    public void RestoreSim(Planet planet) {
        int lenght = planet.shapeSettings.noise.layers.Length - simulatonRuns;
        planet.resolution -= resolutionDelta* simulatonRuns;
        planet.shapeSettings.procentSeaLevel -= seaLevelDelta * simulatonRuns;
        planet.shapeSettings.noise.baseRoughness -= baseRoughnessDelta * simulatonRuns;
        planet.shapeSettings.noise.roughness -= roughnessDelta * simulatonRuns;
        planet.shapeSettings.noise.presistence -= presistenceDelta * simulatonRuns;
        planet.shapeSettings.noise.center -= centerDelta * simulatonRuns;
        planet.shapeSettings.noise.seed -= seedDelta * simulatonRuns;
        if (addLayer) {
            Array.Resize(ref planet.shapeSettings.noise.layers, lenght);
        }
    }

}