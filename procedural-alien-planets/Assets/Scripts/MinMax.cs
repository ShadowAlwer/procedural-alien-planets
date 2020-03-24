
public class MinMax {
  public float min {get; set;}
  public float max {get; set;}

    public MinMax(){
        min = float.MaxValue;
        max = float.MinValue;
    }

    public MinMax(float _min, float _max){
        min=_min;
        max=_max;
    }

    public  void Update(float value){
        if (value < min)
            min = value;
        
        if(value > max)
            max = value;
    }

}