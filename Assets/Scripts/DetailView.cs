using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DetailView : MonoBehaviour {

	public Text organNameText;
	public Image organImage;
	public Text detectedTumorText;
	public Text detectedForeignBodyText;
	public Text detectedVirusText;

	public void Render() {
		var organ = GameController.gameState.selectedOrgan;
		if (organ != null) {
			organNameText.text = organ.name;
			detectedForeignBodyText.text = organ.nbDetectedForeignBody.ToString ();
			detectedTumorText.text = organ.nbDetectedTumor.ToString ();
			detectedVirusText.text = organ.nbDetectedVirus.ToString ();
			organImage.sprite = organ.spriteRenderer.sprite;
		}
	}
}
