using UnityEngine;
using System.Collections;

public class ActionAddMacrophage : MonoBehaviour, IPlayerAction {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	#region IPlayerAction implementation

	public void Use () {
		throw new System.NotImplementedException ();
	}

	#endregion
}
