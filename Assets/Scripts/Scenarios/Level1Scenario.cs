using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Level1Scenario : MonoBehaviour, IScenario {

	public int ennemiesFirstWaveBrain = 50;


	#region IScenario implementation

	public IEnumerator Play () {
		Debug.Log ("Starting level 1");
		var heart = FindObjectOfType<OrganHeart> ().GetComponent<Organ> ();
		if (heart == null) {
			throw new MissingReferenceException ("Cannot find heart organ");
		}

		GameController.gameState.GenerateEnemies (heart, UnitTier.None, 1);

		yield return new WaitForSeconds (5);

		GameController.gameState.GenerateEnemies (heart, UnitTier.None, 10);

		yield return new WaitForSeconds (20);

		GameController.gameState.GenerateEnemies (heart, UnitTier.None, 30);

		yield break;
	}

	#endregion
}
