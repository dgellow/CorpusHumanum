using UnityEngine;
using System.Collections;

public class ScenarioExemple : MonoBehaviour, IScenario {

	#region IScenario implementation

	public IEnumerator Play () {
		// Find organs in the game object hierarchy
		var heart = FindObjectOfType<OrganHeart> ().GetComponent<Organ> ();
		if (heart == null) {
			throw new MissingReferenceException ("Cannot find heart organ");
		}
		var brain = FindObjectOfType<OrganBrain> ().GetComponent<Organ> ();
		if (brain == null) {
			throw new MissingReferenceException ("Cannot find brain organ");
		}

		// Generate some enemies
		GameController.gameState.GenerateRandomEnemies (heart, 10);
		// Wait 
		yield return new WaitForSeconds (20); 

		// Generate specific enemies
		GameController.gameState.GenerateEnemies (brain, EnemyType.Octogon, 25);
		// Wait
		yield return new WaitForSeconds (35);

		// Another attack
		GameController.gameState.GenerateRandomEnemies (heart, 30);
		GameController.gameState.GenerateRandomEnemies (brain, 25);
	}

	#endregion
}
