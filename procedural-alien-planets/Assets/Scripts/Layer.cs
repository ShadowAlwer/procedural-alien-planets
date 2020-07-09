

[System.Serializable]
public class Layer {
    public NoiseType type;

    public LayerSign sign;  

    public Layer(NoiseType _type, LayerSign _sign){
         type=_type;
         sign=_sign;
    }

}