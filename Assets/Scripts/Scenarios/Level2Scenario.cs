using UnityEngine;
using System.Collections;

public class Level2Scenario : MonoBehaviour,  IScenario {

	public IEnumerator Play () {

		var heart = FindObjectOfType<OrganHeart> ().GetComponent<Organ> ();
		if (heart == null) {
			throw new MissingReferenceException ("Cannot find heart organ");
		}

		GameController.gameState.GenerateEnemies (heart, UnitTier.Triangle, 1);

		yield return new WaitForSeconds (5);

		GameController.gameState.GenerateEnemies (heart, UnitTier.Triangle, 10);

		yield return new WaitForSeconds (20);

		GameController.gameState.GenerateEnemies (heart, UnitTier.Triangle, 30);

		yield break;
	}
}
