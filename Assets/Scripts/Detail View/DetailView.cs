using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DetailView : MonoBehaviour {

	public Text organNameText;
	public Image backgroundOrgan;

	public void Render() {
		var organ = GameController.gameState.selectedOrgan;
		if (organ != null) {
			organNameText.text = organ.name;
			backgroundOrgan.sprite = organ.image.sprite;
		}
	}
}
