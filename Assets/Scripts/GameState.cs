﻿using UnityEngine;
using System.Collections;

public class GameState : MonoBehaviour {
	public static GameState gameState;

	void Start () {
		if (gameState == null) {
			gameState = this;
			DontDestroyOnLoad (gameObject);
		} else if (gameState != this) {
			Destroy (gameState);
		}
	}

	public void Initialize() {
		Debug.Log ("Initialize game state");
	}

	public void LoadState() {
		Debug.Log ("Load state");
	}

	public void SaveState() {
		Debug.Log ("Save state");
	}

	public void LoadSettings() {
		Debug.Log ("Load settings");
	}

	public void SaveSettings() {
		Debug.Log ("Save settings");
	}
}