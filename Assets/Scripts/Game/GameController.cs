using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {
	public static GameController gameState;
	public Organ[] organs;
	public Organ selectedOrgan;
	public float incomeRate = 1.2f;
	public int incomeAmount = 5;
	public int maxIncome = 1000;
	public int minIncome = 0;
	public bool generateIncome = true;
	public int incomeReserve = 0;
	public int scanDelay = 10;

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
	}

	public void StartGameLogic() {
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

	public bool GiveToIncomeReserve(int amount) {
		incomeReserve += amount;
		if (incomeReserve > maxIncome) {
			incomeReserve = maxIncome;
			return false;
		}
		return true;
	}

	public bool TakeFromIncomeReserve(int amount) {
		incomeReserve -= amount;
		if (incomeReserve < minIncome) {
			incomeReserve += amount;
			return false;
		}
		return true;
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

	public void GenerateRandomEnemies(Organ target, int number) {
		for (var i = 0; i < number; i++) {
			var type = EnemyType.SomethingElse; // <== FIXME Instead, randomize enemy type
			GenerateEnemies (target, type, 1);	
		}

		throw new System.NotImplementedException (); // <== FIXME Prove that you read comments, remove this!
	}

	public void GenerateEnemies(Organ target, EnemyType type, int number) {
		// Hint:
		// Use Instantiate to generate an object from a prefab
		//

		throw new System.NotImplementedException ();
	}
}
