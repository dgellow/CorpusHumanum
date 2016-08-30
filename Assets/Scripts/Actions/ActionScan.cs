using UnityEngine;
using System.Collections;

public class ActionScan : MonoBehaviour, IPlayerAction {

	#region IPlayerAction implementation

	public void Use () {
		GameController.gameState.selectedOrgan.Scan();
	}

	#endregion
}
