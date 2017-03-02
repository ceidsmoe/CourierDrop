using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapImportManager : MonoBehaviour {

	public GameObject emptyPrefabWithMeshRenderer;
	public GameObject spawnedPrefab;

	void Awake() {
		//gameObject.AddComponent<ObjImporter>();
	}

	// Use this for initialization
	void Start () {
		/*string filepath = "D:\\Users\\ceidsmoe\\Documents\\CourierDrop\\Assets\\MapMesh.obj";//"/sdcard/CourierDropMap.obj";

		ObjImporter imp = new ObjImporter();

		MeshFilter filter = gameObject.GetComponent<MeshFilter>();

		filter.mesh = imp.ImportFile(filepath);
		filter.mesh.RecalculateBounds();
		filter.mesh.RecalculateNormals();

		//GameObject map = OBJLoader.LoadOBJFile(filepath);

		/*Mesh importedMesh = GetComponent<ObjImporter>().ImportFile(filepath);
        spawnedPrefab = Instantiate(emptyPrefabWithMeshRenderer,transform.position,transform.rotation);
        spawnedPrefab.GetComponent<MeshFilter>().mesh=importedMesh;

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
