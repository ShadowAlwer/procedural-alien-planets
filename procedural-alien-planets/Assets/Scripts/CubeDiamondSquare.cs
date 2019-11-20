using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeDiamondSquare 
{
    public int m=2;

    public float[][][] cube;

    public CubeDiamondSquare(int m) {
        this.m = m;
        cube = new float[(2^m)+1][][];
        for (int i = 0; i < (2 ^ m) + 1; i++) {
            cube[i] = new float[(2 ^ m) + 1][];
            for (int j = 0; j < (2 ^ m) + 1; j++)
            {
                cube[i][j] = new float[(2 ^ m) + 1];
                for (int k = 0; k < (2 ^ m) + 1; k++) {
                    cube[i][j][k] = -1f;
                }
            }
        }
    }

    public void CalulateCube(Vector3Int zero, int mi) {

        Vector3Int max = new Vector3Int(zero.x+(2^mi), zero.y + (2 ^ mi), zero.z + (2 ^ mi));
        Vector3Int half = new Vector3Int(max.x / 2, max.y / 2, max.z / 2);

        if (mi == m) {
            CalculateCorners(zero, half, max);
        }

        CubeStep(zero, half, max);
        DiamondStep(zero, half, max);
        SquareStep(zero, half, max);

        if (mi > 1) {
            //TODO: set new zeros and call CalculateCube() again

        }

    }

    

    private void CalculateCorners(Vector3Int zero, Vector3Int half, Vector3Int max)
    {
        cube[zero.x][zero.y][zero.z] = UnityEngine.Random.value;
        cube[zero.x][zero.y][max.z] = UnityEngine.Random.value;
        cube[zero.x][max.y][zero.z] = UnityEngine.Random.value;
        cube[max.x][zero.y][zero.z] = UnityEngine.Random.value;

        cube[max.x][max.y][max.z] = UnityEngine.Random.value;
        cube[max.x][max.y][zero.z] = UnityEngine.Random.value;
        cube[max.x][zero.y][max.z] = UnityEngine.Random.value;
        cube[zero.x][max.y][max.z] = UnityEngine.Random.value;

    }

    private void CubeStep(Vector3Int zero, Vector3Int half, Vector3Int max)
    {
        cube[half.x][half.y][half.z] = (cube[zero.x][zero.y][zero.z] +
                                        cube[zero.x][zero.y][max.z] +
                                        cube[zero.x][max.y][zero.z] +
                                        cube[max.x][zero.y][zero.z] +
                                        cube[max.x][max.y][max.z] +
                                        cube[max.x][max.y][zero.z] +
                                        cube[max.x][zero.y][max.z] +
                                        cube[zero.x][max.y][max.z]) / 6 + (UnityEngine.Random.value - 0.5f) / 5f;
    }

    private void DiamondStep(Vector3Int zero, Vector3Int half, Vector3Int max)
    {
        cube[half.x][half.y][zero.z] = (cube[half.x][half.y][half.z] +
                                      cube[zero.x][half.y][zero.z] +
                                      cube[max.x][half.y][zero.z] +
                                      cube[half.x][zero.y][zero.z] +
                                      cube[half.x][max.y][zero.z]) / 5 + (UnityEngine.Random.value - 0.5f);

        cube[half.x][half.y][max.z]= (cube[half.x][half.y][half.z] +
                                      cube[zero.x][half.y][max.z]+
                                      cube[max.x][half.y][max.z]+
                                      cube[half.x][zero.y][max.z]+
                                      cube[half.x][max.y][max.z]) /5 + (UnityEngine.Random.value - 0.5f);

        cube[half.x][zero.y][half.z]=(cube[half.x][half.y][half.z]+
                                      cube[zero.x][zero.y][half.z]+
                                      cube[max.x][zero.y][half.z]+
                                      cube[half.x][zero.y][zero.z]+
                                      cube[half.x][zero.y][max.z]) / 5 + (UnityEngine.Random.value - 0.5f);

        cube[half.x][max.y][half.z] = (cube[half.x][half.y][half.z] +
                                      cube[zero.x][max.y][half.z] +
                                      cube[max.x][max.y][half.z] +
                                      cube[half.x][max.y][zero.z] +
                                      cube[half.x][max.y][max.z]) / 5 + (UnityEngine.Random.value - 0.5f);

        cube[zero.x][half.y][half.z] =(cube[half.x][half.y][half.z] +
                                       cube[zero.x][zero.y][half.z]+
                                       cube[zero.x][max.y][half.z] +
                                       cube[zero.x][half.y][zero.z] +
                                       cube[zero.x][half.y][max.z]) / 5 + (UnityEngine.Random.value - 0.5f);

        cube[max.x][half.y][half.z] = (cube[half.x][half.y][half.z] +
                                       cube[max.x][zero.y][half.z] +
                                       cube[max.x][max.y][half.z] +
                                       cube[max.x][half.y][zero.z] +
                                       cube[max.x][half.y][max.z]) / 5 + (UnityEngine.Random.value - 0.5f);
    }

    private void SquareStep(Vector3Int zero, Vector3Int half, Vector3Int max)
    {
       //TODO: write square step, good luck!
    }

}
