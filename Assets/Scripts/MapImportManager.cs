using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapImportManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		string filepath = "/sdcard/MapMesh.obj";
		Mesh holderMesh = new Mesh();
		ObjImporter mapImporter = new ObjImporter();
		holderMesh = mapImporter.ImportFile(filepath);

		MeshRenderer renderer = gameObject.GetComponent<MeshRenderer>();
		MeshCollider collider = gameObject.GetComponent<MeshCollider>();
        MeshFilter filter = gameObject.GetComponent<MeshFilter>();
        filter.mesh = holderMesh;
        collider.sharedMesh = holderMesh;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
