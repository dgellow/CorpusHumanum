using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Level1Scenario : MonoBehaviour, IScenario {

	public IEnumerator Play () {
		Debug.Log ("starting level 1");
		var heart = FindObjectOfType<OrganHeart> ().GetComponent<Organ> ();
		if (heart == null) {
			throw new MissingReferenceException ("Cannot find heart organ");
		}

		heart.allies.Add (new Ally (1, new List<EnemyType>()));
		heart.allies.Add (new Ally (1, new List<EnemyType>()));
		heart.allies.Add (new Ally (1, new List<EnemyType>()));
		heart.allies.Add (new Ally (1, new List<EnemyType>()));

		yield break;
	}
}
