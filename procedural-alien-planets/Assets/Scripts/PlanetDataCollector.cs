using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

public class PlanetDataCollector {

        public ShapeSettings shapeSettings;

        public  float resolution;

        static string CSV_LOCATION = "C:\\Users\\Alex\\Desktop\\planet.csv";

        static string[] HEADERS = {"Radius", "Resolution", "MaxHight", "SeaCoverage", "SeaLevel", "Layers"};


    public PlanetDataCollector(ShapeSettings _shapeSettings, float _resolution){
            shapeSettings= _shapeSettings;
            resolution= _resolution;
    }

    public void ToCSV(){
        Debug.Log("To CSV");

        var csv = new StringBuilder();
        if(!File.Exists(CSV_LOCATION)){
              WriteHeader();  
        }

        csv.Append(shapeSettings.planetRadius+",");
        csv.Append(resolution+",");
        csv.Append(shapeSettings.maxHeight+",");
        csv.Append(shapeSettings.seaCoverage+",");
        csv.Append(shapeSettings.noise.seaLevel+",");
        csv.Append(shapeSettings.noise.layers.Length);
        
        csv.Append("\n");
        File.AppendAllText(CSV_LOCATION, csv.ToString());

    }

    void WriteHeader(){
        var csvHeader = new StringBuilder();
        
        foreach(string header in HEADERS){
                csvHeader.Append(header+',');
        }
        csvHeader.Remove(csvHeader.Length-1,1);
        csvHeader.Append("\n");
        File.AppendAllText(CSV_LOCATION, csvHeader.ToString());

    }
    

}