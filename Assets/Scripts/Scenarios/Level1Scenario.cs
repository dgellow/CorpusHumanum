using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Level1Scenario : MonoBehaviour, IScenario {

	public int ennemiesFirstWaveBrain = 50;

	public void Play() {
		StartCoroutine (PlayRandomEnemies ());
		StartCoroutine (PlayScenario ());
	}

	IEnumerator PlayScenario () {
		Debug.Log ("Starting level 1");
		var heart = FindObjectOfType<OrganHeart> ().GetComponent<Organ> ();
		if (heart == null) {
			throw new MissingReferenceException ("Cannot find heart organ");
		}

		GameController.gameState.GenerateRandomEnemies (heart, 1);

		yield return new WaitForSeconds (5);

		GameController.gameState.GenerateRandomEnemies (heart, 10);

		yield return new WaitForSeconds (20);

		GameController.gameState.GenerateRandomEnemies (heart, 30);
	}

	IEnumerator PlayRandomEnemies() {
		while (true) {
			var organ = FindObjectsOfType<Organ> ().GetRandomValue ();
			GameController.gameState.GenerateRandomEnemies (organ, Random.Range (1, 3));
			yield return new WaitForSeconds (Random.Range (1, 5));
		}
	}
}
