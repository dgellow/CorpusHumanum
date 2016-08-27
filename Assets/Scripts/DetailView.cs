using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DetailView : MonoBehaviour {

	public Text organNameText;

	public void Render() {
		var organ = GameState.gameState.selectedOrgan;
		if (organ != null) {
			organNameText.text = organ.name;
		}
	}
}
