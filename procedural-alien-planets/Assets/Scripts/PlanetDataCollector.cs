using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

public class PlanetDataCollector {

        public ShapeSettings shapeSettings;

        public  float resolution;

        public float seaCalculationTime;

        public float meshCalculationTime;

        static string CSV_LOCATION = "C:\\Users\\Alex\\Desktop\\Badania\\";

        string csvName;

        static string[] HEADERS = {"Radius", "Resolution", "MaxHight", "SeaCoverage", "% Sea Level", 
                                    "SeaLevel", "Layers", "Frequency", "Base Roughness",   
                                    "Roughness", "Presistence", "Mesh Calc Time","Sea Calc Time"};


    public PlanetDataCollector(ShapeSettings _shapeSettings, float _resolution, float _seaCalculationTime, 
                                float _meshCalculationTime, string _csvName){
            shapeSettings= _shapeSettings;
            resolution= _resolution;
            seaCalculationTime = _seaCalculationTime;
            meshCalculationTime = _meshCalculationTime;
            csvName = _csvName;

    }

    public void ToCSV(){

        var csv = new StringBuilder();
        if(!File.Exists(CSV_LOCATION+csvName)){
              WriteHeader();  
        }

        csv.Append(shapeSettings.planetRadius+",");
        csv.Append(resolution+",");
        csv.Append(shapeSettings.maxHeight+",");
        csv.Append(shapeSettings.seaCoverage+",");
        csv.Append(shapeSettings.procentSeaLevel+",");
        csv.Append(shapeSettings.noise.seaLevel+",");
        csv.Append(shapeSettings.noise.layers.Length+",");
        csv.Append(shapeSettings.noise.freq+",");
        csv.Append(shapeSettings.noise.baseRoughness+",");
        csv.Append(shapeSettings.noise.roughness+",");
        csv.Append(shapeSettings.noise.presistence+",");
        csv.Append(meshCalculationTime+",");
        csv.Append(seaCalculationTime);
   
        csv.Append("\n");
        File.AppendAllText(CSV_LOCATION+csvName, csv.ToString());

    }

    void WriteHeader(){
        var csvHeader = new StringBuilder();
        
        foreach(string header in HEADERS){
                csvHeader.Append(header+',');
        }
        csvHeader.Remove(csvHeader.Length-1,1);
        csvHeader.Append("\n");
        File.AppendAllText(CSV_LOCATION+csvName, csvHeader.ToString());

    }
    

}