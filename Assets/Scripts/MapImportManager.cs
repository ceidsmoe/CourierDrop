using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapImportManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		string filepath = "D:\\Users\\ceidsmoe\\Documents\\CourierDrop\\Assets\\MapMesh.obj";//"/sdcard/CourierDropMap.obj";

		//GameObject map = OBJLoader.LoadOBJFile(filepath);

		Mesh holderMesh = new Mesh();

		//Instantiate(map);
		/*ObjImporter mapImporter = new ObjImporter();
		holderMesh = mapImporter.ImportFile(filepath);

		MeshRenderer renderer = gameObject.GetComponent<MeshRenderer>();
		MeshCollider collider = gameObject.GetComponent<MeshCollider>();
        MeshFilter filter = gameObject.GetComponent<MeshFilter>();
        filter.mesh = holderMesh;
        collider.sharedMesh = holderMesh;*/
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
