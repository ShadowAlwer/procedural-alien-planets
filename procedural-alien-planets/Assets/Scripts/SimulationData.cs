using UnityEngine;

[System.Serializable]
public class SimulationData {

    public bool saveCSV = false;

    public int simulatonRuns = 1;

    public int resolutionDelta = 0;

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
        planet.shapeSettings.noise.baseRoughness += roughnessDelta;
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

}