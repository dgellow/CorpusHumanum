using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ItemToBuy : MonoBehaviour {
	public int price;
	public Text priceText;
	public Button buyButton;

	private IPlayerAction action;

	void Start() {
		action = GetComponent<IPlayerAction> ();
		if (action == null) {
			throw new MissingComponentException ("Missing component implementing the IPlayerAction interface");
		}

		buyButton.onClick.AddListener (() => {
			if (GameController.gameState.TakeFromIncomeReserve (price)) {
				action.Use ();	
			}
		});
	}

	void OnGUI() {
		if (gameObject.activeInHierarchy) {
			priceText.text = price.ToString ();
			buyButton.enabled = price <= GameController.gameState.incomeReserve;
		}
	}
}
