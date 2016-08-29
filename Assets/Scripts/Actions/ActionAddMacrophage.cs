using UnityEngine;
using System.Collections;

public class ActionAddMacrophage : MonoBehaviour, IPlayerAction {

	#region IPlayerAction implementation

	public void Use () {
		GameController.gameState.GenerateAllies<Macrophage> (GameController.gameState.selectedOrgan);
	}

	#endregion
}
