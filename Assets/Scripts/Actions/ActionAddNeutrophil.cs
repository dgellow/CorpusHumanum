using UnityEngine;
using System.Collections;

public class ActionAddNeutrophil : MonoBehaviour, IPlayerAction {

	#region IPlayerAction implementation

	public void Use () {
		GameController.gameState.GenerateAllies<Neutrophil> (GameController.gameState.selectedOrgan);
	}

	#endregion
}
