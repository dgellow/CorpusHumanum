using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ItemToResearch : MonoBehaviour {
	public int price;
	public Text researchPriceText;
	public Image researchPriceLogo;
	public Button researchButton;
	public Text researchText;
	public int researchDelay;
	public GameObject researchRootObject;
	public GameObject buyItemRootObject;

	[SerializeField]
	private bool researched = false;
	[SerializeField]
	private bool isResearching = false;
	private int countDown;

	void Start () {
		countDown = researchDelay;

		researchButton.onClick.AddListener (() => {
			researchPriceText.enabled = false;
			researchPriceLogo.enabled = false;
			StartCoroutine (PlayResearch ());
		});
	}

	void OnGUI () {
		researchButton.enabled = price <= GameController.gameState.incomeReserve;

		if (isResearching) {
			researchText.text = string.Format ("Researching... {0}s", countDown);
		} else {
			researchText.text = "Unlock for";
			researchPriceText.text = price.ToString ();
		}
	}

	IEnumerator PlayResearch () {
		isResearching = true;
		for (var i = researchDelay; i > 0; i--) {
			yield return new WaitForSeconds (1);
			countDown = i;
		}

		isResearching = false;
		researched = true;
		researchRootObject.SetActive (false);
		buyItemRootObject.SetActive (true);
	}
}
