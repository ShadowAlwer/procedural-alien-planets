using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetRotationController : MonoBehaviour
{

    public float rotationSpeed=2;
    public float orbitSpeed = 0.1f;
    // Start is called before the first frame update
    public bool orbit = true;
    public Transform point;
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if(orbit){
            this.transform.RotateAround(point.position, Vector3.up, orbitSpeed);
        }
        this.transform.Rotate(Vector3.up * rotationSpeed* Time.deltaTime, Space.Self);
    }
}
