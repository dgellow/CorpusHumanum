using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActionAddKiller : MonoBehaviour, IPlayerAction {
	public List<UnitTier> strongAgainst;

	#region IPlayerAction implementation

	public void Use () {
		GameController.gameState.GenerateAllies<Killer> (GameController.gameState.selectedOrgan, strongAgainst);
	}

	#endregion
}
