﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum Scenario { Level1, Level2, Level3 }

public class GameController : MonoBehaviour {
	public static GameController gameState;
	public Organ[] organs;
	public Organ selectedOrgan;
	public float incomeRate = 1.2f;
	public float combatDelay = 2f;
	public int incomeAmount = 5;
	public int maxIncome = 1000;
	public int minIncome = 0;
	public bool generateIncome = true;
	public int incomeReserve = 0;
	public int scanDelay = 10;
	public float attackScale = 0.3f;
	public float defenceScale = 0.3f;
	public Scenario selectedScenario;

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
	//can't inline that shit because…????because!	
	private IScenario scenarioForSelection(Scenario selection ) {
		switch (selection) {
		case Scenario.Level1:
			return FindObjectOfType<Level1Scenario> ();
		case Scenario.Level2:
			return FindObjectOfType<Level2Scenario> ();
		case Scenario.Level3:
			return FindObjectOfType<Level3Scenario> ();
		}
		//"not all path return a value, c# pls
		return null;
	}

	public void StartGameLogic() {
		Debug.Log ("starting game logic");
		//can't put this in Initialize since it's never called when we don't start from main menu

		IScenario current = scenarioForSelection (selectedScenario);
		StartCoroutine (current.Play ());
		StartCoroutine (CombatUpdate ());
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

	IEnumerator CombatUpdate() {
		while (true) {
			Debug.Log ("updating combat");
			foreach (var o in organs) {
				var attackForce = 0f;
				if (o.allies != null) {
					foreach (var a in o.allies) {
						attackForce = attackForce + a.damages;
					}
					attackForce = o.allies.Count * attackScale * attackForce;
				}

				var defenceForce = 0f;
				var enemyCount = 0;
				if (o.enemies != null) {
					foreach (KeyValuePair<Enemy, int> entry in o.enemies) {
						defenceForce = entry.Key.damages * entry.Value;
						enemyCount += entry.Value;
					}
					defenceForce = enemyCount * defenceScale * defenceForce;
				}

				var result = attackForce - defenceForce;
				if (result > 0) {
					//					compute damage for ally
				} else {
					o.healthPoints += (int) result;
				}
			}
			yield return new WaitForSeconds (combatDelay);
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
