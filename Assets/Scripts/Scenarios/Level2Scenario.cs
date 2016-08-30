using UnityEngine;
using System.Collections;

public class Level2Scenario : MonoBehaviour,  IScenario {

	public void Play () {
		StartCoroutine (PlayRandomEnemies ());
		StartCoroutine (PlayScenario ());
	}

	IEnumerator PlayScenario () {

		var heart = FindObjectOfType<OrganHeart> ().GetComponent<Organ> ();
		if (heart == null) {
			throw new MissingReferenceException ("Cannot find heart organ");
		}

		GameController.gameState.GenerateEnemies (heart, UnitTier.Triangle, 1);

		yield return new WaitForSeconds (5);

		GameController.gameState.GenerateEnemies (heart, UnitTier.Triangle, 10);

		yield return new WaitForSeconds (20);

		GameController.gameState.GenerateEnemies (heart, UnitTier.Triangle, 30);
	}

	IEnumerator PlayRandomEnemies() {
		while (true) {
			var organ = FindObjectsOfType<Organ> ().GetRandomValue ();
			GameController.gameState.GenerateRandomEnemies (organ, Random.Range (1, 3));
			yield return new WaitForSeconds (Random.Range (1, 5));
		}
	}
}
