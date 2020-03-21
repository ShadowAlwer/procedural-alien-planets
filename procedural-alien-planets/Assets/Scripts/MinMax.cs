
public class MinMax {
  public float min {get; private set;}
  public float max {get; private set;}

    public MinMax(){
        min = float.MaxValue;
        max = float.MinValue;
    }

    public  void Update(float value){
        if (value < min)
            min = value;
        
        if(value > max)
            max = value;
    }

}