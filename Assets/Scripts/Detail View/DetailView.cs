using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DetailView : MonoBehaviour {

	public Text organNameText;
	public Image backgroundOrgan;
	public RectTransform fullHealthBar;
	public RectTransform healthBar;
	public Text healthBarText;

	private Image healthBarImage;

	public Text enemyCount;
	public Text allyCount;

	public void Render() {
		var organ = GameController.gameState.selectedOrgan;
		if (organ != null) {
			organNameText.text = organ.name;
			backgroundOrgan.sprite = organ.image.sprite;
		}
			
		healthBarImage = healthBar.GetComponent<Image> ();
		if (organ.isBeingScanned) {
			healthBar.gameObject.SetActive (true);
			var width = ((float) organ.healthPoints / (float) organ.maxHealthPoints) * fullHealthBar.rect.width;
			healthBar.sizeDelta = new Vector2 (width, 0);
			healthBarText.text = string.Format ("{0} ({1}s)", HealthPointsToText (), organ.countDownScan);
			healthBarText.color = HealthPointsToTextColor ();
			healthBarImage.color = HealthPointsToColor ();
		} else {
			healthBar.gameObject.SetActive (false);
			healthBarText.text = "???";
			healthBarText.color = Color.black;
			healthBarImage.color = Color.grey;
		}
			
		if (organ.isBeingCollected) {
			var countMacrophage = GameController.gameState.CountAllies<Macrophage> (organ);
			var countNeutrophil = GameController.gameState.CountAllies<Neutrophil> (organ);
			var countKiller = GameController.gameState.CountAllies <Killer> (organ);
			var countHelper = GameController.gameState.CountAllies <Helper> (organ);
			allyCount.text = string.Format (
				@"# Allies
- Macrophages: {0}
- Neutrophil: {1}
- Killer: {2}
- Helper: {3}
", countMacrophage, countNeutrophil, countKiller, countHelper);

			var countTotal = GameController.gameState.CountEnemies (organ);
			var countTriangle = GameController.gameState.CountEnemies (organ, UnitTier.Triangle);
			var countCircle = GameController.gameState.CountEnemies (organ, UnitTier.Circle);
			var countSquare = GameController.gameState.CountEnemies (organ, UnitTier.Square);
			var countOctogon = GameController.gameState.CountEnemies (organ, UnitTier.Octogon);
			enemyCount.text = string.Format (
				@"# Enemies
- Total: {0}
- Triangle: {1}
- Circle: {2}
- Square: {3}
- Octogon: {4}
", countTotal, countTriangle, countCircle, countSquare, countOctogon);
		} else {
			allyCount.text = "";
			enemyCount.text = "";
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

	Color HealthPointsToColor() {
		var organ = GameController.gameState.selectedOrgan;
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
