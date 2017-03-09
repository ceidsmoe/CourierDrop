using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Manager : MonoBehaviour {

	bool playMode;

	//public FreeCameraLook cameraRig;

	List<Transform> PlayerParts = new List<Transform> ();

	[SerializeField]
	Transform startingCube;

	[SerializeField]
	GameObject partToPlacePrefab;
	GameObject partToPlace;

	Transform socketToPlace;

	Vector3 placedPosition;


	// Use this for initialization
	void Start () {
		PlayerParts.Add (startingCube);
	}
	
	// Update is called once per frame
	void Update () {
		if (!playMode) {
			InstantiatePart ();
		} 
		else {
			if (partToPlace) {
				Destroy (partToPlace);
				partToPlace = null;
			}
		}
	}


	void InstantiatePart() {
		if (!partToPlace) {
			if (partToPlacePrefab) {
				partToPlace = Instantiate (partToPlacePrefab, -Vector3.up * 2000, Quaternion.identity) as GameObject;
			}
		} 
		else {
			Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
			RaycastHit hit;

			if (Physics.Raycast (ray, out hit, Mathf.Infinity)) {
				CheckHit (hit);
			}

			partToPlace.transform.position = placedPosition;

			if (Input.GetTouch (0).phase == TouchPhase.Ended) {
				SiegePart_base placeBase = partToPlace.GetComponent<SiegePart_base> ();

				PlayerParts.Add (partToPlace.transform);
				partToPlace.GetComponentInChildren<Collider> ().enabled = true;

				placeBase.DisableSocket (socketToPlace);

				placeBase.AssignTargetToJoint (socketToPlace.parent);
				partToPlace.transform.position = placedPosition;
			}
		}
	}



	void CheckHit(RaycastHit hit) {
		if (hit.transform.GetComponent<SiegePart_base> ()) {
			SiegePart_base partBase = hit.transform.GetComponent<SiegePart_base> ();

			socketToPlace = partBase.ReturnClosestDirection (hit.point);

			if (socketToPlace) {
				placedPosition - socketToPlace.position;
			}

			partToPlace.transform.LookAt (partBase.rendererToFindEdges.bounds.center);
		} 
		else {
			placedPosition = new Vector3 (0, -2000, 0);
		}
	}



	public void PassNewPrefabToInstantiate(GameObject prefab) {
		if (partToPlace) {
			if (PlayerParts.Contains (partToPlace.transform)) {
				PlayerParts.Remove (partToPlace.transform);
			}
			Destroy (partToPlace);
		}

		partToPlacePrefab = prefab;
	}



	public void EnablePlayMode() {
		for (int i = 0; i < PlayerParts.Count; i++) {
			if (PlayerParts [i] != null) {
				PlayerParts [i].GetComponent<Rigidbody> ().isKinematic = false;
			}
		}

		playMode = true;
	}
}
