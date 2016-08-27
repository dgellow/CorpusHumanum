using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GlobalView : MonoBehaviour {

	public Text incomeReserveText;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI() {
		incomeReserveText.text = GameController.gameState.incomeReserve.ToString ();
	} 
}
