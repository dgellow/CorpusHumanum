using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActionAddHealer : MonoBehaviour, IPlayerAction {
	#region IPlayerAction implementation

	public void Use () {
		GameController.gameState.GenerateAllies<Helper> (GameController.gameState.selectedOrgan);
	}

	#endregion
}
