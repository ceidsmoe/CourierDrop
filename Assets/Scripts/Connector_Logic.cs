using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Connector_Logic : MonoBehaviour {

    public Renderer rendererToFindEdges;

    List<Vector3> directions = new List<Vector3>();
    List<Transform> Sockets = new List<Transform>(); 
    List<Transform> disabledSockets = new List<Transform>(); 
    Joint joint;


    [SerializeField]
    SelectDirections selectDirections;

    [System.Serializable]
    public class SelectDirections
    {
        public bool up;
        public bool down;
        public bool left;
        public bool right;
        public bool forward;
        public bool back;
    }


    public bool debugPos;
    public GameObject debugPrefab;

	void Start () 
    {
        Transform trans = rendererToFindEdges.transform; 
       
        if(GetComponent<Joint>())
            joint = GetComponent<Joint>();

       
	    if(selectDirections.up)
        {                
           
            Vector3 addDirection = trans.position + new Vector3(0, rendererToFindEdges.bounds.extents.y, 0);
            Vector3 finalPos = trans.position + addDirection;


            directions.Add(finalPos);
        }

        if (selectDirections.down)
        {
            Vector3 addDirection = -trans.up + new Vector3(0, -rendererToFindEdges.bounds.extents.y + 1, 0);
            Vector3 finalPos = trans.position + addDirection;

            directions.Add(finalPos);
        }

        if (selectDirections.right)
        {
            Vector3 addDirection = trans.right + new Vector3(rendererToFindEdges.bounds.extents.x - 1, 0, 0);
            Vector3 finalPos = trans.position + addDirection;

            directions.Add(finalPos);
        }

        if (selectDirections.left)
        {
            Vector3 addDirection = -trans.right + new Vector3(-rendererToFindEdges.bounds.extents.x + 1, 0, 0);
            Vector3 finalPos = trans.position + addDirection;

            directions.Add(finalPos);
        }

        if (selectDirections.forward)
        {
            Vector3 addDirection = trans.forward + new Vector3(0, 0, rendererToFindEdges.bounds.extents.z - 1);
            Vector3 finalPos = trans.position + addDirection;

            directions.Add(finalPos);
        }

        if (selectDirections.back)
        {
            Vector3 addDirection = -trans.forward + new Vector3(0, 0, -rendererToFindEdges.bounds.extents.z + 1);
            Vector3 finalPos = trans.position + addDirection;

            directions.Add(finalPos);
        }        

     
        for (int i = 0; i < directions.Count; i++)
        {
            GameObject obj;

            if (debugPos)
            {  
                obj = (GameObject)Instantiate(debugPrefab, directions[i], Quaternion.identity);
            }
            else
            {
                obj = new GameObject();
                obj.transform.position = directions[i];
            }

            obj.transform.parent = transform;
            Sockets.Add(obj.transform);
        }
	}
		
    public Transform ReturnClosestDirection(Vector3 pos)
    {
        Transform retVal = null;

        if (directions.Count > 0)
        {
            Transform closestTrans = Sockets[0];
            Vector3 closestPos = Sockets[0].position;
            float distance = Vector3.Distance(pos, closestPos);

            for (int i = 0; i < directions.Count; i++)
            {
                float tempDist = Vector3.Distance(pos, Sockets[i].position);

                if (tempDist < distance)
                {
                    closestPos = Sockets[i].position;
                    closestTrans = Sockets[i];
                    distance = tempDist;
                }
            }

            retVal = closestTrans;
        }

        return retVal;
        
    }
		
    public void DisableSocket(Transform socket)
    {
        if (socket)
        {
            socket.gameObject.SetActive(false);

            if (Sockets.Contains(socket))
                Sockets.Remove(socket);

            disabledSockets.Add(socket);
        }
    }

    public void AssignTargetToJoint(Transform target)
    {
            joint.connectedBody = target.GetComponent<Rigidbody>();
    }
}
