using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
	public static GameController gameState;
	public Organ[] organs;
	public Organ selectedOrgan;
	public int incomeRate = 10;
	public int incomeAmount = 5;
	public int maxIncome = 1000;
	public int minIncome = 0;
	public bool generateIncome = true;
	public int incomeReserve = 0;

	void Awake() {
		if (gameState == null) {
			DontDestroyOnLoad (gameObject);
			gameState = this;

			organs = FindObjectsOfType<Organ> ();
		} else if (gameState != this) {
			Destroy (gameState);
		}
	}

	public void Initialize() {
		Debug.Log ("Initialize game state");
		StartCoroutine (IncomeGenerator ());
	}

	public void LoadState() {
		Debug.Log ("Load state");
	}

	public void SaveState() {
		Debug.Log ("Save state");
	}

	public void LoadSettings() {
		Debug.Log ("Load settings");
	}

	public void SaveSettings() {
		Debug.Log ("Save settings");
	}

	public void GiveToIncomeReserve(int amount) {
		incomeReserve += amount;
		if (incomeReserve > maxIncome) {
			incomeReserve = maxIncome;
		}
	}

	public void TakeFromIncomeReserve(int amount) {
		incomeReserve -= amount;
		if (incomeReserve < minIncome) {
			incomeReserve = minIncome;
		}
	}

	IEnumerator IncomeGenerator() {
		// Infinite loop
		while (true) {
			if (generateIncome) {
				Debug.Log ("Generate income");
				GiveToIncomeReserve (incomeAmount);
			}
			yield return new WaitForSeconds (incomeRate);
		}
	}
}
