using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainFace {

    ShapeGenerator shape;
    Mesh[] meshes;
    int resolution;
    Vector3 localUp;
    Vector3 axisA;
    Vector3 axisB;


    public TerrainFace(ShapeGenerator shape, MeshFilter[] filters, int resolution, Vector3 localUp)
    {
        this.shape=shape;
        this.resolution = resolution;
        this.localUp = localUp;

        meshes= new Mesh[4];
        for(int i=0; i<meshes.Length; i++){
            meshes[i]=filters[i].sharedMesh;
        }

        axisA = new Vector3(localUp.y, localUp.z, localUp.x);
        axisB = Vector3.Cross(localUp, axisA);
    }

    public void ConstructMesh()
    {
        Vector3[][] vertices= new Vector3[4][];
        for(int i=0;i<vertices.Length;i++){
            vertices[i]=new Vector3[(resolution/2+1) * (resolution/2+1)];
        }
        int[][] triangles= new int[4][];
        for(int i=0;i<vertices.Length;i++){
           triangles[i]=new int[((resolution/2)) * ((resolution/2)) * 6];
        }

        Debug.Log("Vertexy "+ vertices[0].Length);
        Debug.Log("Triangless "+ triangles[0].Length);
        //Vector3[] vertices = new Vector3[resolution * resolution];
        //int[] triangles = new int[(resolution - 1) * (resolution - 1) * 6];
        for(int k=0; k<meshes.Length; k++){
            int triIndex = 0;
            Vector2 meshSpace = getMeshSpace(k);
            for (int y = (int)meshSpace.y; y < meshSpace.y+resolution/2+1; y++)
            {
                for (int x = (int)meshSpace.x; x < (int)meshSpace.x+resolution/2+1; x++)
                {   
                    int ox=x-(int)meshSpace.x;
                    int oy=y-(int)meshSpace.y;

                    int i = ox + oy * (resolution/2+1);
                    Vector2 percent = new Vector2(x, y) / (resolution - 1);
                    Vector3 pointOnUnitCube = localUp + (percent.x-.5f)*2* axisA + (percent.y-.5f)*2* axisB;
                    Vector3 pointOnCube = (localUp*0.5f+ (percent.x-.5f)*2* axisA + (percent.y-.5f)*2* axisB)*(resolution-1)+localUp*(resolution-1)*0.5f;
                    //Vector3 pointOnUnitSphere = pointOnUnitCube *(shape.settings.planetRadius/pointOnUnitCube.magnitude);
                    Vector3 pointOnUnitSphere = pointOnUnitCube.normalized;
                    vertices[k][i] =shape.getPointElevation(pointOnUnitSphere);
                    //vertices[k][i]=pointOnUnitSphere;
                    if (ox != resolution/2 && oy != resolution/2)
                    {
                        triangles[k][triIndex] = i;
                        triangles[k][triIndex + 1] = i + (resolution/2+1) + 1;
                        triangles[k][triIndex + 2] = i + (resolution/2+1);

                        triangles[k][triIndex + 3] = i;
                        triangles[k][triIndex + 4] = i + 1;
                        triangles[k][triIndex + 5] = i + (resolution/2+1) + 1;
                        triIndex += 6;
                    }
                    
                    
                }
            }
            meshes[k].Clear();
            meshes[k].vertices = vertices[k];
            meshes[k].triangles = triangles[k];
            meshes[k].normals = vertices[k];
            meshes[k].RecalculateNormals();   
        }
    } 
    private Vector2 getMeshSpace(int k){
        switch(k){
            case 0: 
                return new Vector2Int(0,0);
            case 1: 
                return new Vector2Int(resolution/2,0);
            case 2: 
                return new Vector2Int(0,resolution/2);
            case 3: 
                return new Vector2Int(resolution/2,resolution/2);
            default: 
                return new Vector2Int(0,0);
                
        }
    }

}

    

    

