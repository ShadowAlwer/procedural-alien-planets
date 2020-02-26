using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetRotationController : MonoBehaviour
{

    public float rotationSpeed=2;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        //this.transform.RotateAround(transform.position, Vector3.up, rotationSpeed);
        this.transform.Rotate(Vector3.up * rotationSpeed* Time.deltaTime);
    }
}
