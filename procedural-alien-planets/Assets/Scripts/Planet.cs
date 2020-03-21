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
        this.resolution = resolution%2 == 0 ? resolution-1 : resolution;
        shape = new ShapeGenerator(shapeSettings);
        color = new ColorGenerator(colorSettings);

        if (meshFilters == null || meshFilters.Length == 0)
        {
            meshFilters = new MeshFilter[6][];
            for(int i=0; i<meshFilters.Length; i++){
                meshFilters[i] = new MeshFilter[4];
            }
        }

        if(terrainFaces == null)
            terrainFaces = new TerrainFace[6];

        Vector3[] directions = { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };

        for (int i = 0; i < 6; i++)
        {
            for(int j=0; j<4; j++){
                if (meshFilters[i][j] == null)
                {
                    GameObject meshObj = new GameObject("mesh");
                    meshObj.transform.parent = transform;

                    meshObj.AddComponent<MeshRenderer>().sharedMaterial = colorSettings.planetMaterial;
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
        color.SetElevation(shape.minmax);
    }

    void ColorPlanet()
    {
        foreach(MeshFilter[] filters in meshFilters){
            foreach(MeshFilter filter in filters){}
               // filter.GetComponent<MeshRenderer>().sharedMaterial.color = colorSettings.planetColor;

        }

        color.SetColors();

    }

}
