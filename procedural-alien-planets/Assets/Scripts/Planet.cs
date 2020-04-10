using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Planet : MonoBehaviour
{

    public int resolution = 10;


    public ColorSettings colorSettings;
    public ShapeSettings shapeSettings;

    public ShapeGenerator shape;
    public ColorGenerator color;

    public float maxElevation;

    public float minElevation;

    [SerializeField, HideInInspector]
    MeshFilter[] meshFilters;
    TerrainFace[] terrainFaces;

    const int MAX_RES= 250;
    
    void Start(){
        ColorPlanet();
    }

    public void Generate()
    {
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
    }

    void Initialize()
    {

        int n = CalculateN();
        int n2=n*n;
        Debug.Log("n "+ n+ ", n2 "+n2);
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
                    Debug.Log("NULL");
                    GameObject meshObj = new GameObject("mesh");
                    meshObj.transform.parent = transform;

                    meshObj.AddComponent<MeshRenderer>().sharedMaterial = colorSettings.planetMaterial;
                    meshFilters[i*n2+j] = meshObj.AddComponent<MeshFilter>();
                    meshFilters[i*n2+j].sharedMesh = new Mesh();
                    Debug.Log("Meshes "+(i*n2+j));
                    
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
        foreach (TerrainFace face in terrainFaces)
        {
            face.ConstructMesh_V2();
        }
        float procent = (shape.pointsInSea * 100)/(resolution * resolution * 6);
        Debug.Log("Ocean % Caverage:"+procent);
        maxElevation=shape.minmax.max;
        minElevation=shape.minmax.min;
    }

    public void CalculateSealevel(){
        MinMax level = new MinMax();
        foreach (TerrainFace face in terrainFaces)
        {
            face.CalculateSeaLevel();
            level.Update(face.SeaMin());
            level.Update(face.SeaMax());
        }
        float elevationDiff = level.max - level.min;
        Debug.Log("Elevation diff: "+elevationDiff);
        Debug.Log("Level min: " + level.min + "Level max: " + level.max);
        shapeSettings.noise.seaLevel = level.min + elevationDiff* shapeSettings.procentSeaLevel;
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

}
