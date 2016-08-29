using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class GameController : MonoBehaviour {
	public static GameController gameState;
	public Organ[] organs;
	public List<Enemy>[] organsEnemies;
	public List<Ally>[] organsAllies;
	public Organ selectedOrgan;
	public float incomeRate = 1.2f;
	public float combatDelay = 2f;
	public int incomeAmount = 5;
	public int maxIncome = 1000;
	public int minIncome = 0;
	public bool generateIncome = true;
	public int incomeReserve = 0;
	public int scanDelay = 10;
	public float alliesScaleFactor = 0.3f;
	public float enemiesScaleFactor = 0.3f;
	public Scenario selectedScenario;

	void Awake () {
		if (gameState == null) {
			DontDestroyOnLoad (gameObject);
			gameState = this;

			organs = FindObjectsOfType<Organ> ();
			organsEnemies = new List<Enemy>[organs.Length];
			organsAllies = new List<Ally>[organs.Length];
			for (var i = 0; i < organs.Length; i++) {
				organs [i].id = i;
				organsAllies [i] = new List<Ally> ();
				organsEnemies [i] = new List<Enemy> ();
			}
		} else if (gameState != this) {
			Destroy (gameState);
		}
	}

	public void Initialize () {
		Debug.Log ("Initialize game state");
	}

	//can't inline that shit because…????because!
	IScenario scenarioForSelection (Scenario selection) {
		switch (selection) {
		case Scenario.Level1:
			return FindObjectOfType<Level1Scenario> ();
		case Scenario.Level2:
			return FindObjectOfType<Level2Scenario> ();
		case Scenario.Level3:
			return FindObjectOfType<Level3Scenario> ();
		default:
			return null;
		}
	}

	public void StartGameLogic () {
		Debug.Log ("starting game logic");

		//can't put this in Initialize since it's never called when we don't start from main menu
		IScenario currentScenario = scenarioForSelection (selectedScenario);
		if (currentScenario != null) {
			StartCoroutine (currentScenario.Play ());
		}

		StartCoroutine (CombatUpdate ());
		StartCoroutine (IncomeGenerator ());
	}

	public void LoadState () {
		Debug.Log ("Load state");
	}

	public void SaveState () {
		Debug.Log ("Save state");
	}

	public void LoadSettings () {
		Debug.Log ("Load settings");
	}

	public void SaveSettings () {
		Debug.Log ("Save settings");
	}

	public bool GiveToIncomeReserve (int amount) {
		incomeReserve += amount;
		if (incomeReserve > maxIncome) {
			incomeReserve = maxIncome;
			return false;
		}
		return true;
	}

	public bool TakeFromIncomeReserve (int amount) {
		incomeReserve -= amount;
		if (incomeReserve < minIncome) {
			incomeReserve += amount;
			return false;
		}
		return true;
	}

	IEnumerator CombatUpdate () {
		while (true) {
			Debug.Log ("updating combat");
			foreach (var o in organs) {
				var allies = organsAllies [o.id];
				var enemies = organsEnemies [o.id];

				if (allies != null && enemies != null) {
					var alliesForce = (allies.Count * alliesScaleFactor);

					var enemiesForce = 0f;
					foreach (var e in enemies) {
						enemiesForce += e.damages;
					}
					enemiesForce *= (enemies.Count * enemiesScaleFactor);

					var result = alliesForce - enemiesForce;
					if (result > 0) {
						//					compute damage for ally
					} else {
						o.healthPoints += (int)result;
					}
				}
			}
			yield return new WaitForSeconds (combatDelay);
		}
	}

	IEnumerator IncomeGenerator () {
		// Infinite loop
		while (true) {
			if (generateIncome) {
				Debug.Log ("Generate income");
				GiveToIncomeReserve (incomeAmount);
			}
			yield return new WaitForSeconds (incomeRate);
		}
	}

	public void GenerateRandomEnemies (Organ target, int number = 1) {
		for (var i = 0; i < number; i++) {
			var type = UnitTier.Triangle; // <== FIXME Instead, randomize enemy type
			GenerateEnemies (target, type, 1);	
		}

		throw new System.NotImplementedException (); // <== FIXME Prove that you read comments, remove this!
	}

	public void GenerateEnemies (Organ target, UnitTier type, int number = 1) {
		// Hint:
		// Use Instantiate to generate an object from a prefab
		//

		throw new System.NotImplementedException ();
	}
		
	public void GenerateAllies <T> (Organ target, int number = 1) where T: Ally, new() {
		GenerateAllies<T> (target, null, number);
	}

	public void GenerateAllies <T> (Organ target, List<UnitTier> strongAgainst , int number = 1) where T: Ally, new() {
		var allies = organsAllies [target.id];
		for (var i = 0; i < number; i++) {
			var ally = new T ();
			ally.strongAgainst = strongAgainst;
			ally.organAttachedTo = target;
			allies.Add (ally);
		}
	}

	public int CountEnemies (Organ organ) {
		return organsEnemies [organ.id].Count;
	}

	public int CountAllies (Organ organ) {
		return organsAllies [organ.id].Count;
	}

	public int CountAllies <T> (Organ organ) where T: Ally {
		var allies = organsAllies [organ.id];
		return allies.Where (x => x is T).Count ();
	}

	public void StartExternalCoroutine(IEnumerator enumerator) {
		StartCoroutine (enumerator);
	}
}
