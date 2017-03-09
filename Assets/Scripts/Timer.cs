﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

	public Text timerText;
	private float startTime;
	private bool finished = false;

	// Use this for initialization
	void Start () {
		startTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		if (finished) {
			waitThenRefresh();
            return;
		}

		float currentTime = Time.time - startTime;

		string minutes = ((int) currentTime / 60).ToString();
		string seconds = (currentTime % 60).ToString("f2");

		timerText.text = minutes + ":" + seconds;
	}

	void Finished ()
    {
        timerText.color = Color.yellow;
        finished = true;
    }

    IEnumerator waitThenRefresh()
    {

    	yield return new WaitForSeconds (2f);

    	Application.LoadLevel(Application.loadedLevel);
    }
}