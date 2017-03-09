using UnityEngine;
using System.Collections;

public class Wheel_Logic : MonoBehaviour {

    //See rotor logic script first for the uncommented parts

    WheelCollider wheelCollider;

    public Transform visualModel;

    public float force;

    public KeyCode positiveKey;
    public KeyCode negativeKey;

    float targetFloat;
    float curFloat;

    Transform cam; //Reference to our camera's transform

	void Start () 
    {
        wheelCollider = GetComponent<WheelCollider>();

        cam = Camera.main.transform;
	}
	
	void FixedUpdate () 
    {
        SimulateAxis();

        //Because the wheels could be of the opposite side
        //we want the float we are going to apply to their torque
        //to be relevant to where the camera is looking towards
        if(CompareCameraAngle())
        {
            //..so if it's the opposite of the camera, then simply convert it to a negative number
            curFloat = -curFloat;
        }

        visualModel.Rotate(0, curFloat * force, 0);

        //apply the motor torque to the wheel collider
        wheelCollider.motorTorque = force * curFloat;
	}

    //Returns if true if the transform is looking the opposite way from the camera 
    bool CompareCameraAngle()
    {
        bool retVal = false;

        //Get the two comparing vectors
        Vector3 camForward = cam.forward;
        camForward.y = 0; //and level them, so that we are comparing them on 2 axis

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
