using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GlobalView : MonoBehaviour {

	public void Render() {
		var organs = GameController.gameState.organs;

		foreach(var o in organs) {
			if (o.isBeingScanned) {
				o.totalHealthMiniBar.gameObject.SetActive (true);
				var width = ((float)o.healthPoints / (float)o.maxHealthPoints) * o.totalHealthMiniBar.rect.width;
				o.healthMiniBar.sizeDelta = new Vector2 (width, 0);
				o.healthMiniBarImage.color = HealthPointsToColor (o);
			} else {
				o.totalHealthMiniBar.gameObject.SetActive (false);
			}
		}
	}

	string HealthPointsToText() {
		var organ = GameController.gameState.selectedOrgan;
		var hpPercentage = (float) organ.healthPoints / (float) organ.maxHealthPoints;
		if (hpPercentage < 0.2f) {
			return "Critical!";
		} else if (hpPercentage < 0.5f) {
			return "Not great";
		} else if (hpPercentage < 0.75f) {
			return "Good";
		} else {
			return "Healthy";
		}
	}

	Color HealthPointsToColor(Organ organ) {
		var hpPercentage = (float) organ.healthPoints / (float) organ.maxHealthPoints;
		if (hpPercentage < 0.2f) {
			// Red
			return new Color32 (255, 77, 77, 255);
		} else if (hpPercentage < 0.5f) {
			return new Color32 (255, 193, 7, 255);
		} else if (hpPercentage < 0.75f) {
			return new Color32 (205, 220, 57, 255);
		} else {
			// Green
			return new Color32 (139, 195, 74, 255);	
		}
	}

	Color HealthPointsToTextColor() {
		return Color.white;
	}
}
