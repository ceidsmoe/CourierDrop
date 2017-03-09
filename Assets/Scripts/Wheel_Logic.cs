using UnityEngine;
using System.Collections;

public class Wheel_Logic : MonoBehaviour {


    WheelCollider wheelCollider;

    public Transform visualModel;

    public float force;

    public KeyCode positiveKey;
    public KeyCode negativeKey;

    float targetFloat;
    float curFloat;

    Transform cam;

	void Start () 
    {
        wheelCollider = GetComponent<WheelCollider>();

        cam = Camera.main.transform;
	}
	
	void FixedUpdate () 
    {
        SimulateAxis();

        if(CompareCameraAngle())
        {
            curFloat = -curFloat;
        }

        visualModel.Rotate(0, curFloat * force, 0);

        wheelCollider.motorTorque = force * curFloat;
	}
		
    bool CompareCameraAngle()
    {
        bool retVal = false;

        Vector3 camForward = cam.forward;
        camForward.y = 0;

        Vector3 transForward = transform.forward;
        transForward.y = 0;

        float angle = Vector3.Angle(camForward, transForward);

        if(angle > 45)
        {
            retVal = true;
        }

        return retVal;
    }

    void SimulateAxis()
    {
        if (Input.GetKey(positiveKey))
        {
            targetFloat = 1;
        }
        else
        {
            if (Input.GetKey(negativeKey))
            {
                targetFloat = -1;
            }
            else
            {
                targetFloat = 0;
            }

        }

        curFloat = Mathf.MoveTowards(curFloat, targetFloat, Time.deltaTime * 5);
    }
}
