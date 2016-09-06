using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Intro : MonoBehaviour {
	public string nextScene;

	void Update() {
		if (Input.anyKeyDown || Input.touchCount > 0) {
			var allEnded = true;
			foreach (var touch in Input.touches) {
				allEnded = allEnded && touch.phase == TouchPhase.Ended;
			}
			if (allEnded) {
				SceneManager.LoadScene (nextScene);
			}
		}
	}
}
