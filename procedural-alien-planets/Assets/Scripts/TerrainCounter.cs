using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class TerrainCounter 
{
    Transform planetCenter;

    float planetSeaLevel;

    int planetResolution;

    Vector2Int searchPoint;

    Vector2Int currentPoint;

    List<TerrainPoint> listToCheck;

    TerrainPoint[][] terrainPoints;

    int counter = 0;

    bool debug = true;

    static string FILE_LOCATION = "C:\\Users\\Alex\\Desktop\\Badania\\";

    int fileSufix = 0;
    static readonly  Vector2Int[] TRANSFORMS = {
        new  Vector2Int(-1, -1), new  Vector2Int(-1, 0), new  Vector2Int(-1, 1),
        new  Vector2Int(0, -1),  /*current position ***/ new  Vector2Int(0, 1),
        new  Vector2Int(1, -1),  new  Vector2Int(1, 0),  new  Vector2Int(1, 1)
    };
    public TerrainCounter(Transform center, float seaLevel, int resolution, int sufix = 0){
        planetCenter = center;
        planetSeaLevel = seaLevel;
        planetResolution = resolution;
        listToCheck = new List<TerrainPoint>();
        fileSufix = sufix;
    }
    public int countTerrain(TerrainFace face, int suffix = 0){

            if(face.meshes.Length == 0){
                Debug.Log("No mesh for analize!");
                return -1;
            }
            if(face.meshes.Length > 1){
                Debug.Log("Can count only for resolutions smaller than 251 vertices!");
                return -1;
            }

            fileSufix = suffix;
            counter = 0;
            listToCheck.Clear();
            Mesh mesh = face.meshes[0];

            InitializeTerrainPoints(mesh);           
            Count();

            if(debug){
                SaveFace();
            }

            Debug.Log("Terrain masses on face: " + counter);
            return counter;
    }

    private void Count()
    {
         for(int i = 0; i< terrainPoints.Length; i++){
            for(int j = 0; j< terrainPoints.Length; j++){
                TerrainPoint point = terrainPoints[i][j];

                while(listToCheck.Count > 0){
                    // check points from list
                    CheckNeighbors(terrainPoints[listToCheck[0].facePosition.x][listToCheck[0].facePosition.y]);
                    listToCheck.Remove(listToCheck[0]);
                }

                if(point.kind == 0){
                    counter ++;
                    point.kind = counter;
                    listToCheck.Add(point);
                }        
            }
         }
    }

    private void CheckNeighbors(TerrainPoint terrainPoint)
    {
        for(int i = 0; i< TRANSFORMS.Length; i++){
            Vector2Int neighborPosition = terrainPoint.facePosition + TRANSFORMS[i];

            if(neighborPosition.x >= 0 && neighborPosition.x < planetResolution && 
            neighborPosition.y >= 0 && neighborPosition.y < planetResolution){
                TerrainPoint neighbor = terrainPoints[neighborPosition.x][neighborPosition.y];
                    if(neighbor.kind != -1 && neighbor.kind == 0){
                        neighbor.kind = terrainPoint.kind;
                        listToCheck.Add(neighbor);
                    }
            }
        }
    }

    private void InitializeTerrainPoints(Mesh mesh)
    {
        terrainPoints = new TerrainPoint[planetResolution][];
        for(int i = 0; i< terrainPoints.Length; i++){
            terrainPoints[i] = new TerrainPoint[planetResolution];
            for(int j = 0; j < terrainPoints.Length; j++){
                terrainPoints[i][j] = new TerrainPoint();
            }
        }
        
        for(int i = 0; i < terrainPoints.Length; i++){
            for(int j = 0; j < terrainPoints.Length; j++){
                //World Position
                terrainPoints[i][j].worldPosition = mesh.vertices[i*planetResolution + j];
                //Face Position
                terrainPoints[i][j].facePosition = new Vector2Int(i,j);
                //Terrain Kind
                float distance = Vector3.Distance(terrainPoints[i][j].worldPosition, Vector3.zero);
                if(distance > planetSeaLevel+0.005f*planetSeaLevel){
                    terrainPoints[i][j].kind = 0;
                } else {
                    terrainPoints[i][j].kind = -1;
                }
            }        
        }

    }


    void SaveFace(){
        var csv = new StringBuilder();

         for(int i = 0; i < terrainPoints.Length; i++){
            for(int j = 0; j < terrainPoints.Length; j++){
                if(terrainPoints[i][j].kind == -1){
                    csv.Append("___");
                }else {
                    csv.Append(terrainPoints[i][j].kind.ToString("D3"));
                }
            }
            csv.Append("\n");
         }
        csv.Append("\n");
        File.WriteAllText(FILE_LOCATION+"terrainPoints"+fileSufix+".txt", csv.ToString());
    }
}
