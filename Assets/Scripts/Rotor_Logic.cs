using UnityEngine;
using System.Collections;

public class Rotor_Logic : MonoBehaviour {
    
    Rigidbody rig;

    public float force;
    public Transform visualModel; 

  
  
    public KeyCode positiveKey;
    public KeyCode negativeKey;


    float targetFloat;
    float curFloat;

	void Start () 
    {
        rig = GetComponent<Rigidbody>();
	}
	
	
	void FixedUpdate () 
    {
        SimulateAxis(); 

      
        visualModel.Rotate(0, curFloat * 10, 0);
       
        rig.AddForce(Vector3.up * force * curFloat);
            
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
