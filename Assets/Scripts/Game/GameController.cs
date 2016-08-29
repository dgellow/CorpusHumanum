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
	public float lifespanDelay = 1f;
	public int incomeAmount = 5;
	public int maxIncome = 1000;
	public int minIncome = 0;
	public bool generateIncome = true;
	public int incomeReserve = 0;
	public int scanDelay = 10;
	public int collectDelay = 10;
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

				// 1. Macrophages eat random enemies
				var macrophages = allies.OfType<Macrophage> ();
				foreach (var m in macrophages) {
					m.Hurt ();
				}

				// 2. Killers kill random enemies with corresponding tier
				var killers = allies.OfType<Killer> ();
				foreach (var k in killers) {
					k.Hurt ();
				}

				// 3. Helpers heal random allies
				var helpers = allies.OfType<Helper> ();
				foreach (var h in helpers) {
					h.CombatBehaviour ();
				}

				// 4. Neutrophils damage everything and commit suicide
				var neutrophils = allies.OfType<Neutrophil> ();
				foreach (var n in neutrophils) {
					n.CombatBehaviour ();
				}

				// 5. Remove dead units
				foreach (var a in allies.Where (x => x.status == UnitStatus.Dead).ToList ()) {
					allies.Remove (a);
				}
				foreach (var e in enemies.Where (x => x.status == UnitStatus.Dead).ToList ()) {
					enemies.Remove (e);
				}

				// 6. Enemies attack the organ
				foreach (var e in enemies) {
					e.Hurt ();
				}
			}
			yield return new WaitForSeconds (combatDelay);
		}
	}

	IEnumerator LifespanUpdate() {
		while (true) {
			Debug.Log ("Update lifespan");



			yield return new WaitForSeconds (lifespanDelay);
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
			var tier = (UnitTier)Random.Range (1, System.Enum.GetValues (typeof(UnitTier)).Length);
			GenerateEnemies (target, tier, 1);	
		}
	}

	public void GenerateEnemies (Organ target, UnitTier tier, int number = 1) {
		
		var enemies = organsEnemies [target.id];
		for (var i = 0; i < number; i++) {
			var enemy = new Enemy (target, tier);
			enemy.damages = Random.Range (1, 4);
			enemies.Add (enemy);
		}
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

	public int CountEnemies (Organ organ, UnitTier tier) {
		return organsEnemies [organ.id].Where (x => x.tier == tier).Count ();
	}

	public int CountAllies (Organ organ) {
		return organsAllies [organ.id].Count;
	}

	public int CountAllies <T> (Organ organ) where T: Ally {
		return organsAllies [organ.id].Where (x => x is T).Count ();
	}

	public void StartExternalCoroutine(IEnumerator enumerator) {
		StartCoroutine (enumerator);
	}
}
