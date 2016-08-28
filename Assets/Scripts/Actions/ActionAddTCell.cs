using UnityEngine;
using System.Collections;

public enum TCellType {
	BCell,
	Helper,
	Killer
}

public class ActionAddTCell : MonoBehaviour, IPlayerAction {
	public TCellType type;

	#region IPlayerAction implementation

	public void Use () {
		throw new System.NotImplementedException ();
	}

	#endregion
}
