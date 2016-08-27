using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Intro : MonoBehaviour {
	public string nextScene;

	void PointerClick() {
		SceneManager.LoadScene (nextScene);
	}
}
