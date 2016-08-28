using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DetailView : MonoBehaviour {

	public Text organNameText;
	public Image backgroundOrgan;
	public RectTransform healthBar;
	public Text healthBarText;

	private Image healthBarImage;

	public void Render() {
		var organ = GameController.gameState.selectedOrgan;
		if (organ != null) {
			organNameText.text = organ.name;
			backgroundOrgan.sprite = organ.image.sprite;
		}
			
		healthBarImage = healthBar.GetComponent<Image> ();
		if (organ.isBeingScanned) {
			healthBar.gameObject.SetActive (true);
			healthBarText.text = string.Format ("{0} ({1}s)", HealthPointsToText (), organ.countDownScan);
			healthBarText.color = HealthPointsToTextColor ();
			healthBarImage.color = HealthPointsToColor ();
		} else {
			healthBar.gameObject.SetActive (false);
			healthBarText.text = "???";
			healthBarText.color = Color.black;
			healthBarImage.color = Color.grey;
		}
	}

	string HealthPointsToText() {
		var hp = GameController.gameState.selectedOrgan.healthPoints;
		if (hp < 20) {
			return "Critical!";
		} else if (hp < 50) {
			return "Not great";
		} else if (hp < 75) {
			return "Good";
		} else {
			return "Healthy";
		}
	}

	Color HealthPointsToColor() {
		var hp = GameController.gameState.selectedOrgan.healthPoints;
		if (hp < 20) {
			return new Color32 (255, 77, 77, 255);
		} else if (hp < 50) {
			return new Color32 (255, 193, 7, 255);
		} else if (hp < 75) {
			return new Color32 (205, 220, 57, 255);
		} else {
			return new Color32 (139, 195, 74, 255);	
		}
	}

	Color HealthPointsToTextColor() {
		return Color.white;
	}
}
