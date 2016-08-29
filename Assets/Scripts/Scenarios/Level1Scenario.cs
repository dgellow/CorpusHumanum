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

		GameController.gameState.GenerateAllies <Macrophage> (heart, 4);
		GameController.gameState.GenerateEnemies (heart, UnitTier.Triangle, ennemiesFirstWaveBrain);

		yield break;
	}

	#endregion
}
