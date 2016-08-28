using UnityEngine;
using System.Collections;

public class ActionScan : MonoBehaviour, IPlayerAction {

	#region IPlayerAction implementation

	public void Use () {
		Debug.Log ("SCAN!");	
		StartCoroutine (GameController.gameState.selectedOrgan.PlayScan());
	}

	#endregion
}
