using UnityEngine;
using System.Collections;

public class Level3Scenario : MonoBehaviour, IScenario {

	public void Play() {
		Debug.Log ("Random enemies");
		StartCoroutine (PlayRandomEnemies ());

		Debug.Log ("Start scenarios");
		StartCoroutine (PlayScenario ());
	}

	IEnumerator PlayScenario () {
		var heart = FindObjectOfType<OrganHeart> ().GetComponent<Organ> ();
		var colon = FindObjectOfType<OrganColon> ().GetComponent<Organ> ();
		if (heart == null || colon == null) {
			throw new MissingReferenceException ("Cannot find organ");
		}

		Debug.Log ("first wave");
		GameController.gameState.GenerateEnemies (heart, UnitTier.Triangle, 3);
		GameController.gameState.GenerateEnemies (heart, UnitTier.None, 10);

		yield return new WaitForSeconds (20);

		Debug.Log ("second wave");
		GameController.gameState.GenerateEnemies (heart, UnitTier.Triangle, 10);

		yield return new WaitUntil (() => GameController.gameState.organsEnemies[heart.id].Count == 10); 

		Debug.Log ("colon wave launched");
		GameController.gameState.GenerateEnemies (colon, UnitTier.None, 10);

		yield return new WaitForSeconds (20);

		Debug.Log ("last wave");
		GameController.gameState.GenerateEnemies (heart, UnitTier.Triangle, 30);
		GameController.gameState.GenerateEnemies (colon, UnitTier.None, 30);
	}

	IEnumerator PlayRandomEnemies() {
		while (true) {
			var organ = FindObjectsOfType<Organ> ().GetRandomValue ();
			GameController.gameState.GenerateRandomEnemies (organ, Random.Range (1, 3));
			yield return new WaitForSeconds (Random.Range (1, 5));
		}
	}
}
