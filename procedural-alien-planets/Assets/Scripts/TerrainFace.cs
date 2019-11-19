using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainFace {

    Mesh mesh;
    int resolution;
    Vector3 localUp;
    Vector3 axisA;
    Vector3 axisB;
    NoiseSettings setting;


    public TerrainFace(Mesh mesh, int resolution, Vector3 localUp, NoiseSettings settings)
    {
        this.mesh = mesh;
        this.resolution = resolution;
        this.localUp = localUp;
        this.setting = settings;

        axisA = new Vector3(localUp.y, localUp.z, localUp.x);
        axisB = Vector3.Cross(localUp, axisA);
    }

    public void ConstructMesh(int planetRadius)
    {
        Vector3[] vertices = new Vector3[resolution * resolution];
        int[] triangles = new int[(resolution - 1) * (resolution - 1) * 6];
        int triIndex = 0;

        for (int y = 0; y < resolution; y++)
        {
            for (int x = 0; x < resolution; x++)
            {
                int i = x + y * resolution;
                Vector2 percent = new Vector2(x, y) / (resolution - 1);
                Vector3 pointOnUnitCube = localUp + (percent.x - .5f) * 2 * axisA + (percent.y - .5f) * 2 * axisB;
                Vector3 pointOnUnitSphere = pointOnUnitCube.normalized;
                vertices[i] =applyPerlinNoise(planetRadius, pointOnUnitSphere);
              
                if (x != resolution - 1 && y != resolution - 1)
                {
                    triangles[triIndex] = i;
                    triangles[triIndex + 1] = i + resolution + 1;
                    triangles[triIndex + 2] = i + resolution;

                    triangles[triIndex + 3] = i;
                    triangles[triIndex + 4] = i + 1;
                    triangles[triIndex + 5] = i + resolution + 1;
                    triIndex += 6;
                }
            }
        }
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.normals = vertices;
        mesh.RecalculateNormals();
        
    }

    public Vector3 applyPerlinNoise(int planetRadius, Vector3 v){

        float freq = setting.baseRoughness;
        float amplitude = 1;
        float noise = 0;

        for (int i = 0; i < setting.layers; i++) {
            noise += (PerlinNoise3D.getPerlinNoise3D(v * freq + setting.center, 0) + 1) * .5f * amplitude;
            amplitude *= setting.presistence;
            freq *= setting.roughness;          
        }

        noise = Mathf.Max(0, noise - setting.seaLevel);
        noise = noise * setting.power;
        v = v * planetRadius * (noise + 1);


        
        return v;
    }

    
}
