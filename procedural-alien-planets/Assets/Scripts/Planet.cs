using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Planet : MonoBehaviour
{

    public int resolution = 10;

    public string csvName = "planet.csv";
    public ColorSettings colorSettings;
    public ShapeSettings shapeSettings;

    public ShapeGenerator shape;
    public ColorGenerator color;
    [HideInInspector]
    public float maxElevation;
    [HideInInspector]
    public float minElevation;

    [SerializeField, HideInInspector]
    MeshFilter[] meshFilters;
    TerrainFace[] terrainFaces;

    public SimulationData simData;

    float seaCalculationTime;

    float meshGenerationTime;

    float averageLandMasses;

    //Max res for a part of TerrainFace
    const int MAX_RES= 250;
    
    void Start(){
        ColorPlanet();
    }

    public void Generate()
    {

        int startRes = this.resolution;
        float startLevel = this.shapeSettings.procentSeaLevel;

        for(int i=0; i<simData.simulatonRuns; i++){

            seaCalculationTime = -1;
            meshGenerationTime = -1;

            if(shapeSettings.isProcentSeaLevel){
                Initialize();
                CalculateSealevel();
                shapeSettings.isProcentSeaLevel = false;
                GenerateMesh();
                ColorPlanet();
                shapeSettings.isProcentSeaLevel = true;
            }
            else{
            Initialize();
            GenerateMesh();
            ColorPlanet();
            }
            simData.PrepareSimulation(this);
            this.resolution += simData.resolutionDelta;
            this.shapeSettings.procentSeaLevel += simData.seaLevelDelta;
            Debug.Log("Sim Run #"+(i+1));
        }

        this.resolution = startRes;
        this.shapeSettings.procentSeaLevel = startLevel;
    }

    void Initialize()
    {

        int n = CalculateN();
        int n2=n*n;
        shape = new ShapeGenerator(shapeSettings);
        color = new ColorGenerator(colorSettings);
        

        if (meshFilters == null || meshFilters.Length == 0 || meshFilters.Length != n2*6)
        {
            for(int i=0; i<meshFilters.Length; i++){
                if(meshFilters[i])
                    DestroyImmediate(meshFilters[i].gameObject);
            }
            meshFilters=null;
            meshFilters = new MeshFilter[n2*6];
        }

        if(terrainFaces == null)
            terrainFaces = new TerrainFace[6];

        Vector3[] directions = { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };

        MeshFilter[][] tmp=new MeshFilter[6][];
        for(int i=0; i<tmp.Length; i++){
                tmp[i] = new MeshFilter[n2];
            }
        for (int i = 0; i < 6; i++)
        {
            for(int j=0; j<n2; j++){
                if (meshFilters[i*n2+j] == null)
                {   
                    GameObject meshObj = new GameObject("mesh");
                    meshObj.transform.parent = transform;

                    meshObj.AddComponent<MeshRenderer>().sharedMaterial = colorSettings.planetMaterial;
                    meshFilters[i*n2+j] = meshObj.AddComponent<MeshFilter>();
                    meshFilters[i*n2+j].sharedMesh = new Mesh();                   
                }
                tmp[i][j]=meshFilters[i*n2+j];
            }

            terrainFaces[i] = new TerrainFace(shape, tmp[i], resolution, directions[i], n2, MAX_RES);
        }
    }


    int CalculateN(){
        float res= resolution;
        int n =(int)Mathf.Ceil(res/(float)MAX_RES); 
        return n;

    }
    void GenerateMesh()
    {
        float startTime = Time.realtimeSinceStartup;
        
        foreach (TerrainFace face in terrainFaces)
        {
            face.ConstructMesh_V2();
        }
        float procent = (shape.pointsInSea * 100)/(resolution * resolution * 6);
        shapeSettings.seaCoverage = procent;
        maxElevation=shape.minmax.max;
        minElevation=shape.minmax.min;

        this.meshGenerationTime = Time.realtimeSinceStartup - startTime;
        
        if (simData.saveCSV) {
            TerrainCounter counter = new TerrainCounter(transform, shapeSettings.realSeaLevel, resolution);
            int i=0;
            float sum = 0;
            foreach(TerrainFace face in terrainFaces){
                sum += counter.countTerrain(face, i);
                i++;
            }
            this.shapeSettings.procentHeight = (this.maxElevation - this.minElevation)/this.minElevation;
            this.shapeSettings.averageLandMasses = sum/(i + 1);
            SaveDataToCSV();
        }
    }

    public void CalculateSealevel(){

        float startTime = Time.realtimeSinceStartup;

        MinMax level = new MinMax();
        foreach (TerrainFace face in terrainFaces)
        {
            face.CalculateSeaLevel();
            level.Update(face.SeaMin());
            level.Update(face.SeaMax());
        }

        float maxHeight = level.max;
        shapeSettings.maxHeight = maxHeight;
        
        float elevationDiff = level.max - level.min;
        shapeSettings.noise.seaLevel = level.min + elevationDiff * shapeSettings.procentSeaLevel;

        this.seaCalculationTime = Time.realtimeSinceStartup - startTime;
    }

    public void ColorPlanet()
    {
        if(shape == null){
            shape = new ShapeGenerator(shapeSettings);
            color = new ColorGenerator(colorSettings);
        }

        color.SetElevation(new MinMax(minElevation,maxElevation));
        color.SetColors();
    }


    void SaveDataToCSV(){
      PlanetDataCollector collector = new PlanetDataCollector(shapeSettings, resolution, seaCalculationTime, meshGenerationTime, csvName);
      collector.ToCSV();
    }

}
