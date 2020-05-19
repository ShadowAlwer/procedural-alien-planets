using UnityEngine;

[System.Serializable]
public class SimulationData {

    public int simulatonRuns = 1;

    public int simulationDelta = 0;

    [Range(0, 1)]
    public  float seaLevelDelta = 0.05f;

}