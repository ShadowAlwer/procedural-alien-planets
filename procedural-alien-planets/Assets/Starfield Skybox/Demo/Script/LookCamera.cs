using UnityEngine;
using System.Collections;

public class LookCamera : MonoBehaviour 
{
    public float speed = 10.0f;

    public float mouseSensitivityX = 5.0f;
	public float mouseSensitivityY = 5.0f;
    
	float rotY = 0.0f;
    
	void Start()
	{
		if (GetComponent<Rigidbody>())
			GetComponent<Rigidbody>().freezeRotation = true;
	}

	void Update()
	{	
        // rotation        
        if (Input.GetMouseButton(1)) 
        {
            float rotX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * mouseSensitivityX;
            rotY += Input.GetAxis("Mouse Y") * mouseSensitivityY;
            rotY = Mathf.Clamp(rotY, -89.5f, 89.5f);
            transform.localEulerAngles = new Vector3(-rotY, rotX, 0.0f);
        }
		
		if (Input.GetKey(KeyCode.Q))
		{
			 transform.Translate(new Vector3(0,speed * Time.deltaTime,0));
		}

		if (Input.GetKey(KeyCode.E))
		{
			transform.Translate(new Vector3(0,-speed * Time.deltaTime,0));
		}
		if (Input.GetKey(KeyCode.W))
		{
			transform.Translate(new Vector3(0,0,speed * Time.deltaTime));
		}
		if (Input.GetKey(KeyCode.S))
		{
			transform.Translate(new Vector3(0,0,-speed * Time.deltaTime));
		}
		if (Input.GetKey(KeyCode.A))
		{
			transform.Translate(new Vector3(-speed * Time.deltaTime,0,0));
		}
		if (Input.GetKey(KeyCode.D))
		{
			transform.Translate(new Vector3(speed * Time.deltaTime,0,0));
		}

	}
}
