using UnityEngine;
using System.Collections;

public class ActionCollect : MonoBehaviour, IPlayerAction {

	#region IPlayerAction implementation

	public void Use () {
		StartCoroutine (GameController.gameState.selectedOrgan.PlayCollect());
	}

	#endregion
}
