using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

	public Text timerText;
	private float startTime;

	// Use this for initialization
	void Start () {
		startTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		float currentTime = Time.time - startTime;

		string minutes = ((int) currentTime / 60).ToString();
		string seconds = (currentTime % 60).ToString("f2");

		timerText.text = minutes + ":" + seconds;
	}
}
