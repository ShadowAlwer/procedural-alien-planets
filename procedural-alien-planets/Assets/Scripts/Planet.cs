using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{

    [Range(3, 509)]
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

    
    void Start(){
        ColorPlanet();
    }
    public void Generate()
    {
        Initialize();
        GenerateMesh();
        ColorPlanet();
    }

    void Initialize()
    {
        this.resolution = resolution%2 == 0 ? resolution-1 : resolution;
        shape = new ShapeGenerator(shapeSettings);
        color = new ColorGenerator(colorSettings);

        /*
        if (meshFilters == null || meshFilters.Length == 0)
        {
            meshFilters = new MeshFilter[6][];
            for(int i=0; i<meshFilters.Length; i++){
                meshFilters[i] = new MeshFilter[4];
            }
        }
        */

        if (meshFilters == null || meshFilters.Length == 0)
        {
            meshFilters = new MeshFilter[24];
        }

        if(terrainFaces == null)
            terrainFaces = new TerrainFace[6];

        Vector3[] directions = { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };

        MeshFilter[][] tmp=new MeshFilter[6][];
        for(int i=0; i<tmp.Length; i++){
                tmp[i] = new MeshFilter[4];
            }
        for (int i = 0; i < 6; i++)
        {
            for(int j=0; j<4; j++){
                if (meshFilters[i*4+j] == null)
                {
                    GameObject meshObj = new GameObject("mesh");
                    meshObj.transform.parent = transform;

                    meshObj.AddComponent<MeshRenderer>().sharedMaterial = colorSettings.planetMaterial;
                    meshFilters[i*4+j] = meshObj.AddComponent<MeshFilter>();
                    meshFilters[i*4+j].sharedMesh = new Mesh();
                    
                }
                tmp[i][j]=meshFilters[i*4+j];
            }

            terrainFaces[i] = new TerrainFace(shape, tmp[i], resolution, directions[i]);
        }
    }

    void GenerateMesh()
    {
        foreach (TerrainFace face in terrainFaces)
        {
            face.ConstructMesh();
        }

        maxElevation=shape.minmax.max;
        minElevation=shape.minmax.min;
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
