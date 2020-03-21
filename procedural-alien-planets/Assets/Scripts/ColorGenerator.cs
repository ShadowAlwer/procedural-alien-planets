using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorGenerator 
{
   ColorSettings settings;
   Texture2D planetTexture;
   public int resolution=50;

   public ColorGenerator(ColorSettings _settings){
       settings= _settings;
       planetTexture = new Texture2D(resolution,1);
   }

   public void SetElevation(MinMax minmax){
       settings.planetMaterial.SetVector("_elevationMinMax", new Vector4(minmax.min, minmax.max));
   }

   public void SetColors(){
       Color[] colors= new Color[resolution];
       for(int i=0; i<resolution; i++){
           colors[i] = settings.planetColor.Evaluate(i/(resolution-1f));
       }
       planetTexture.SetPixels(colors);
       planetTexture.Apply();
       settings.planetMaterial.SetTexture("_planetTexture", planetTexture);

   }
}
