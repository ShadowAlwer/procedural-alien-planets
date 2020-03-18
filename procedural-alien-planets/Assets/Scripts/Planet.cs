using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{

    [Range(2, 512)]
    public int resolution = 10;

    public ColorSettings colorSettings;
    public ShapeSettings shapeSettings;

    public ShapeGenerator shape;

    [SerializeField, HideInInspector]
    MeshFilter[][] meshFilters;
    TerrainFace[] terrainFaces;
    


    public void Generate()
    {
        Initialize();
        GenerateMesh();
        ColorPlanet();
    }

    void Initialize()
    {
        shape =new ShapeGenerator(shapeSettings);

        if (meshFilters == null || meshFilters.Length == 0)
        {
            meshFilters = new MeshFilter[6][];
            for(int i=0; i<meshFilters.Length; i++){
                meshFilters[i] = new MeshFilter[4];
            }
        }
        terrainFaces = new TerrainFace[6];

        Vector3[] directions = { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };

        for (int i = 0; i < 6; i++)
        {
            for(int j=0; j<4; j++){
                if (meshFilters[i][j] == null)
                {
                    GameObject meshObj = new GameObject("mesh");
                    meshObj.transform.parent = transform;

                    meshObj.AddComponent<MeshRenderer>().sharedMaterial = new Material(Shader.Find("Standard"));
                    meshFilters[i][j] = meshObj.AddComponent<MeshFilter>();
                    meshFilters[i][j].sharedMesh = new Mesh();
                }
            }

            terrainFaces[i] = new TerrainFace(shape, meshFilters[i], resolution, directions[i]);
        }
    }

    void GenerateMesh()
    {
        foreach (TerrainFace face in terrainFaces)
        {
            face.ConstructMesh();
        }
    }

    void ColorPlanet()
    {
        foreach(MeshFilter[] filters in meshFilters){
            foreach(MeshFilter filter in filters)
                filter.GetComponent<MeshRenderer>().sharedMaterial.color = colorSettings.planetColor;

        }

    }

}
