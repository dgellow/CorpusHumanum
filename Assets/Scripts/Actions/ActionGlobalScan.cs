using UnityEngine;
using System.Collections;

public class ActionGlobalScan : MonoBehaviour, IPlayerAction {

	#region IPlayerAction implementation

	public void Use () {
		foreach (var o in GameController.gameState.organs) {
			o.Scan ();	
		}
	}

	#endregion
}
