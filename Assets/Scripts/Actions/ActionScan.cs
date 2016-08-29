using UnityEngine;
using System.Collections;

public class ActionScan : MonoBehaviour, IPlayerAction {

	#region IPlayerAction implementation

	public void Use () {
		StartCoroutine (GameController.gameState.selectedOrgan.PlayScan());
	}

	#endregion
}
