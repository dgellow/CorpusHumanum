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

		GameController.gameState.GenerateAllies <Macrophage> (heart, 1);
		GameController.gameState.GenerateAllies <Neutrophil> (heart, 2);
		GameController.gameState.GenerateAllies <Helper> (heart, 3);
		GameController.gameState.GenerateAllies <Killer> (heart, 4);

		yield return new WaitForSeconds (5);

		GameController.gameState.GenerateAllies <Macrophage> (heart, 10);
		GameController.gameState.GenerateAllies <Neutrophil> (heart, 10);
		GameController.gameState.GenerateAllies <Helper> (heart, 10);
		GameController.gameState.GenerateAllies <Killer> (heart, 10);

		yield return new WaitForSeconds (10);

		GameController.gameState.GenerateAllies <Macrophage> (heart, 100);
		GameController.gameState.GenerateAllies <Neutrophil> (heart, 100);
		GameController.gameState.GenerateAllies <Helper> (heart, 100);
		GameController.gameState.GenerateAllies <Killer> (heart, 100);
		//GameController.gameState.GenerateEnemies (heart, UnitTier.Triangle, ennemiesFirstWaveBrain);

		yield break;
	}

	#endregion
}
