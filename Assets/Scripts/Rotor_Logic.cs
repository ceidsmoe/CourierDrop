using UnityEngine;
using System.Collections;

public class Rotor_Logic : MonoBehaviour {
    
    Rigidbody rig;

    public float force; //The force we are going to apply

    public Transform visualModel; //The gameobject we are going to rotate to make the player think that the wheel/rotor is moving

    //Because we don't want to add a new axis for every button
    //We simply take to keys (that we can assign from the inspector)
    public KeyCode positiveKey;
    public KeyCode negativeKey;

    //We use this floats to simulate an axis for input
    float targetFloat;
    float curFloat;

	void Start () 
    {
        rig = GetComponent<Rigidbody>();
	}
	
	
	void FixedUpdate () 
    {
        SimulateAxis(); //Simulate the axis

        //rotate the visual model based on our custom axis
        visualModel.Rotate(0, curFloat * 10, 0);

        //add force to the rigidbody based on our custom axis
        rig.AddForce(Vector3.up * force * curFloat);
        //This of course will add force to the world up, so even if the rotor is facing down, the siege will go up
        //You can use this to add force on any other direction you want, maybe the transform.up
	}

    void SimulateAxis()
    {
        //If the player hits the positive key
        if (Input.GetKey(positiveKey))
        {
            //Then the target of our axis is 1
            targetFloat = 1;
        }
        else
        {
            //if he presses the negative key, without pressing the positive that is
            if (Input.GetKey(negativeKey))
            {
                //then the target is -1
                targetFloat = -1;
            }
            else
            {
                //If he doesn't press anything, then it's 0
                targetFloat = 0;
            }

        }

        //Interpolate the curfloat to the target float
        curFloat = Mathf.MoveTowards(curFloat, targetFloat, Time.deltaTime * 5);
    }
}
