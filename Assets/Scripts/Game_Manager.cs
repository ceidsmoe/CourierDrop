using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Game_Manager : MonoBehaviour {

    bool playMode; //If it's play mode, then it's not build mode :P

    public FreeCameraLook cameraRig; //Our camera's rig

    public GameObject myCar; //The parent gameobject containing all the parts

    public GameObject m_builderObjectsContainer;

    public GameObject m_areaDescriptionLoaderPanel;

    public GameObject m_meshBuildPanel;

    //A list that hold every part we have attached to our Siege equipment(?)
    List<Transform> PlayerParts = new List<Transform>();

    [SerializeField]
    Transform startingCube; //The starter cube, as I said in the video, 
    //you can simply drag and drop it in the list instead of this shenannigans, I blame coffee for this

    [SerializeField]
    GameObject partToPlacePrefab; //The prefab we want to instantiate
    GameObject partToPlace; //The actual instantiated game object

    Transform socketToPlace; //The socket we are going to place it to
    
    Vector3 placePos; //Where we are going to place it

    void Start()
    {
        PlayerParts.Add(startingCube); //see above, I still blame coffee for this
    }

    void Update()
    {
        if (!playMode)
        {
            //This is basically the build mode
            InstantiatePart();
        }
        else
        { //If we are at play mode
            //and we had a part we where suppose to place
            if (partToPlace)
            {
                //destroy it
                Destroy(partToPlace);
                partToPlace = null;
            } 
        }

        //We enable/disable the camera rig with the middle mouse button, **only for the video purposes
        if (Input.GetMouseButtonDown(2))
        {
            cameraRig.enabled = true;
        }

        if (Input.GetMouseButtonUp(2))
        {
            cameraRig.enabled = false;
        }
    }

    void InstantiatePart()
    {
        if (!partToPlace) //If we don't have a part to place
        {
            if (partToPlacePrefab) //but we have a prefab for it
            {
                //instantiate the part on a position way out of sight from the camera
                partToPlace = Instantiate(partToPlacePrefab, -Vector3.up * 2000, Quaternion.identity, myCar.transform) as GameObject;
            }
            //if we don't have a prefab, then the player is either sleeping or haven't decided what to place
        }
        else
        {
            //If we have a part to place

            //Make a raycast from the camera to mouse position
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                //Check what we hit
                CheckHit(hit);
            }

            //Update the part's position
            partToPlace.transform.position = placePos;

            //And if we click
            if(Input.GetMouseButtonUp(0) && socketToPlace != null)
            {
                //...store the Siege part base of the "part to place"
                SiegePart_base placeBase = partToPlace.GetComponent<SiegePart_base>();

                //add it to the list
                PlayerParts.Add(partToPlace.transform);
                //enable it's collider
                partToPlace.GetComponentInChildren<Collider>().enabled = true; 

                //assign a target to the joint
                placeBase.AssignTargetToJoint(socketToPlace.parent);

                //disable the socket
                placeBase.DisableSocket(socketToPlace);

                //update the position once more
                partToPlace.transform.position = placePos;
            
                partToPlace = null;
            }
        }

    }

    void CheckHit(RaycastHit hit)
    {
        //If we hit a gameobject that has the SiegePart_base script then we hit a part of our siege engine
        if(hit.transform.GetComponent<SiegePart_base>())
        {
            //Store it
            SiegePart_base partBase = hit.transform.GetComponent<SiegePart_base>();
                      
            //Find the closest socket to our hit.point
            socketToPlace = partBase.ReturnClosestDirection(hit.point);

            //if we have a socket, then update the place pos
            if (socketToPlace) //The socket might return null if the part doesn't have any sockets, this avoids errors from that
            {
                placePos = socketToPlace.position;

                //We want to look at the center of the other parts mesh
                //Since the transform.position gives the pivot of a gameobject,
                //it might not always be the correct position we want our part to look
                //thus we simply find the center of that other parts mesh

                Vector3 dir = partBase.rendererToFindEdges.bounds.center - socketToPlace.position;

                dir.Normalize();

                if (dir == Vector3.zero)
                    dir = -socketToPlace.forward;

                Quaternion rot = Quaternion.LookRotation(dir);
                partToPlace.transform.rotation = rot;
                //partToPlace.transform.LookAt(partBase.rendererToFindEdges.bounds.center);  
            }        
        }
        else
        {
            //If we din't hit an object, hide the part then
            placePos = new Vector3(0, -2000, 0);
        }
    }

    //Drop this script into a UI button's event system and assign the prefab from there
    public void PassNewPrefabToInstantiate (GameObject prefab)
    {
        if (partToPlace) //If we already had a part
        {
            //remove it from the list
            if (PlayerParts.Contains(partToPlace.transform))
                PlayerParts.Remove(partToPlace.transform);
          
            //and anihilate it
            Destroy(partToPlace);
        }

        //Pass the new prefab as the part to place prefab
        partToPlacePrefab = prefab;
    }

    public void EnablePlayMode()
    {
        //Enable the play mode,
        //basically find every rigid body in our list and make it to not be kinematic
        for(int i = 0; i < PlayerParts.Count; i++)
        {
            if(PlayerParts[i] != null)
                PlayerParts[i].GetComponent<Rigidbody>().isKinematic = false;
   
        }

        playMode = true;

        //Another way you can do this, is instead of the isKinematic true/false
        //Would be by using the Time.scale and changing it from 0 to 1 etc.
        //Of course for functions that are using the Time.delta time (camera scripts etc.)
        //you should do either another timer or simply avoid using the deltaTime.

        m_builderObjectsContainer.gameObject.SetActive(false);

        // Disable unused components in tango application.
        /*m_tangoApplication.m_areaDescriptionLearningMode = false;
        m_tangoApplication.m_enableDepth = false;*/

        // Set UI panel to the mesh interaction panel.
        m_areaDescriptionLoaderPanel.SetActive(true);
        m_meshBuildPanel.SetActive(true);
    }
}
