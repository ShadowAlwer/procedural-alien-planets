using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainFace {

    ShapeGenerator shape;
    public Mesh[] meshes;
    int resolution;
    Vector3 localUp;
    Vector3 axisA;
    Vector3 axisB;

    int n2;

    int maxRes;
    
    public TerrainFace(ShapeGenerator shape, MeshFilter[] filters, int resolution, Vector3 localUp, int n2, int maxRes)
    {
        this.shape=shape;
        this.resolution = resolution;
        this.localUp = localUp;
        this.n2 = n2;
        this.maxRes = maxRes;

        meshes= new Mesh[n2];
        for(int i=0; i<n2; i++){
            meshes[i]=filters[i].sharedMesh;
        }

        axisA = new Vector3(localUp.y, localUp.z, localUp.x);
        axisB = Vector3.Cross(localUp, axisA);
    } 

    public void ConstructMesh_V2()
    {
        shape.settings.realSeaLevel = 0;
          Vector3[][] vertices= new Vector3[n2][];
          int[][] triangles= new int[n2][];

          int n=(int)Math.Sqrt(n2);
       
        for(int k=0; k<n2; k++){

            int triIndex = 0;
            // define boundries
            int start_x = (k%n)*maxRes-1;
            int start_y = (k/n)*maxRes-1;
            int end_x = start_x + maxRes;
            int end_y = start_y + maxRes;
            if(end_x>=resolution){
                end_x = resolution-1;
            }
            if(end_y>=resolution){
                end_y = resolution-1;
            }
            start_x = Math.Max(0,start_x);
            start_y = Math.Max(0,start_y);
            int lenght_x=end_x-start_x+1;
            int lenght_y=end_y-start_y+1;

            //define trinagles and vertices lenght;
            vertices[k]= new Vector3[lenght_x*lenght_y];
            triangles[k] = new int[lenght_x*lenght_y*6];


            for (int y = start_y ; y < start_y + lenght_y ; y++)
            {
                for (int x = start_x ; x < start_x + lenght_x; x++)
                {   
                    int ox=x-start_x;
                    int oy=y-start_y;
                    int i = ox + oy * lenght_x;

                    //------------------------------------------------------------------
                    
                    Vector2 percent = new Vector2(x, y) / (resolution - 1);
                    Vector3 pointOnUnitCube = localUp + (percent.x-.5f)*2* axisA + (percent.y-.5f)*2* axisB;
                    Vector3 pointOnCube = (localUp*0.5f+ (percent.x-.5f)*2* axisA + (percent.y-.5f)*2* axisB)*(resolution-1)+localUp*(resolution-1)*0.5f;
                    Vector3 pointOnUnitSphere = pointOnUnitCube *(shape.settings.planetRadius/pointOnUnitCube.magnitude);
                    //Vector3 pointOnUnitSphere = pointOnUnitCube.normalized;
                    vertices[k][i] =shape.getPointElevation(pointOnUnitSphere);
                    //vertices[k][i]=pointOnUnitSphere;
                    //----------------------------------------------------------------------------
                    if (ox < lenght_x-1 && oy < lenght_y-1)
                    {
                        triangles[k][triIndex] = i;
                        triangles[k][triIndex + 1] = i + lenght_x + 1;
                        triangles[k][triIndex + 2] = i + lenght_x;

                        triangles[k][triIndex + 3] = i;
                        triangles[k][triIndex + 4] = i + 1;
                        triangles[k][triIndex + 5] = i + lenght_x + 1;
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
        shape.isProcentSeaLevle = false;            
    }

    public void CalculateSeaLevel(){

        shape.isProcentSeaLevle = false;
          Vector3[][] vertices= new Vector3[n2][];
          int[][] triangles= new int[n2][];

          int n=(int)Math.Sqrt(n2);
       
        for(int k=0; k<n2; k++){

            // define boundries
            int start_x = (k%n)*maxRes-1;
            int start_y = (k/n)*maxRes-1;
            int end_x = start_x + maxRes;
            int end_y = start_y + maxRes;
            if(end_x>=resolution){
                end_x = resolution-1;
            }
            if(end_y>=resolution){
                end_y = resolution-1;
            }
            start_x = Math.Max(0,start_x);
            start_y = Math.Max(0,start_y);
            int lenght_x=end_x-start_x+1;
            int lenght_y=end_y-start_y+1;

            //define trinagles and vertices lenght;
            vertices[k]= new Vector3[lenght_x*lenght_y];
            triangles[k] = new int[lenght_x*lenght_y*6];


            for (int y = start_y ; y < start_y + lenght_y ; y++)
            {
                for (int x = start_x ; x < start_x + lenght_x; x++)
                {   
                    int ox=x-start_x;
                    int oy=y-start_y;
                    int i = ox + oy * lenght_x;
                    //------------------------------------------------------------------
                    
                    Vector2 percent = new Vector2(x, y) / (resolution - 1);
                    Vector3 pointOnUnitCube = localUp + (percent.x-.5f)*2* axisA + (percent.y-.5f)*2* axisB;
                    Vector3 pointOnCube = (localUp*0.5f+ (percent.x-.5f)*2* axisA + (percent.y-.5f)*2* axisB)*(resolution-1)+localUp*(resolution-1)*0.5f;
                    Vector3 pointOnUnitSphere = pointOnUnitCube *(shape.settings.planetRadius/pointOnUnitCube.magnitude);
                    //Vector3 pointOnUnitSphere = pointOnUnitCube.normalized;
                    vertices[k][i] =shape.getPointElevation(pointOnUnitSphere);
                    //vertices[k][i]=pointOnUnitSphere;
                    //----------------------------------------------------------------------------
                }
            }                 
        }
        shape.isProcentSeaLevle = true;
    }


    public float SeaMin(){
        return shape.sealLevelMinMax.min;
    }

    public float SeaMax(){
        return shape.sealLevelMinMax.max;
    }

}


/*
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
                    Vector3 pointOnUnitSphere = pointOnUnitCube *(shape.settings.planetRadius/pointOnUnitCube.magnitude);
                    //Vector3 pointOnUnitSphere = pointOnUnitCube.normalized;
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
*/
    

    

