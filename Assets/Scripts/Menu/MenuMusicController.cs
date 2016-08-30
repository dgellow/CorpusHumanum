using UnityEngine;
using System.Collections;

public class MenuMusicController : MonoBehaviour {

	public AudioSource source;
	public AudioClip start;
	public AudioClip loop;

	// Use this for initialization
	void Awake () {
		StartCoroutine (Play ());
	}

	IEnumerator Play() {
		source.clip = start;
		source.Play ();
		yield return new WaitForSecondsRealtime (start.length);
		source.clip = loop;
		source.loop = true;
		source.Play ();
	}
}
