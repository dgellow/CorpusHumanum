using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public enum UnitStatus { Good, Weakened, Dead }

public enum UnitTier {
	None,
	Triangle,
	Circle,
	Square,
	Octogon
}

public interface ICanAttack {
	void Hurt ();
}

public interface ICanBeAttacked {
	void ReactToBeingHurt ();
	void ReactToBeingHurt (int amount);
}

public interface ICanBehaveInCombat {
	void CombatBehaviour ();
}

public interface IHasLifespan {
	IEnumerator PlayLifespan ();
}
	
public class Enemy: ICanAttack, ICanBeAttacked {
	public UnitStatus status = UnitStatus.Good;
	public int damages = 1;
	public UnitTier tier;
	public Organ target;

	public Enemy (Organ target, UnitTier tier) {
		this.tier = tier;
		this.target = target;
	}
		
	#region ICanAttack implementation

	public void Hurt () {
		target.ReactToBeingHurt (damages);
	}

	#endregion

	#region ICanBeAttacked implementation

	public void ReactToBeingHurt () {
		if (status == UnitStatus.Weakened) {
			status = UnitStatus.Dead;
		} else if (status == UnitStatus.Good) {
			status = UnitStatus.Weakened;
		}
	}

	public void ReactToBeingHurt (int amount) {
		ReactToBeingHurt ();
	}

	#endregion
}

abstract public class Ally {
	public UnitStatus status = UnitStatus.Good;
	public List<UnitTier> strongAgainst;
	public Organ organAttachedTo;
}

class Macrophage: Ally, ICanAttack, ICanBeAttacked {

	#region ICanAttack implementation

	public void Hurt () {
		var enemies = GameController.gameState.organsEnemies [organAttachedTo.id];
		if (enemies.Count > 0) {
			var target = enemies [0];
			if (target != null && target.status != UnitStatus.Dead) {
				target.ReactToBeingHurt ();	
			}
		}
	}

	#endregion

	#region ICanBeAttacked implementation

	public void ReactToBeingHurt() {
		if (status == UnitStatus.Weakened) {
			status = UnitStatus.Dead;
		} else if (status == UnitStatus.Good) {
			status = UnitStatus.Weakened;
		}
	}

	public void ReactToBeingHurt (int amount) {
		ReactToBeingHurt ();
	}

	#endregion
}

public class Neutrophil: Ally, ICanBehaveInCombat {

	#region ICanBehaveInCombat implementation
	public void CombatBehaviour () {
		var nbTarget = GameController.gameState.neutrophilNbTarget;
		var enemies = GameController.gameState.organsEnemies [organAttachedTo.id];
		var allies = GameController.gameState.organsAllies [organAttachedTo.id];
		for (var i = 0; i < nbTarget; i++) {
			// Attack random enemy
			if (enemies.Count > 0) {
				var e = enemies.GetRandomValue ();
				e.ReactToBeingHurt ();
			}
			// Attack random ally
			if (allies.Count > 0) {
				var a = allies.OfType<ICanBeAttacked> ().ToList ().GetRandomValue ();
				a.ReactToBeingHurt ();
			}
		}
		// Commit suicide
		status = UnitStatus.Dead;
	}
	#endregion
}

public class Killer: Ally, ICanAttack {

	public Killer() {
		strongAgainst = new List<UnitTier> ();
	}

	#region ICanAttack implementation
	public void Hurt () {
		var enemies = GameController.gameState.organsEnemies [organAttachedTo.id];
		if (enemies.Count > 0) {
			var naturalEnemies = enemies.Where (x => strongAgainst.Contains (x.tier)).ToList ();
			var target = naturalEnemies.Count > 0 ? naturalEnemies.GetRandomValue () : enemies.GetRandomValue ();
			while (target.status != UnitStatus.Dead) {
				target.ReactToBeingHurt ();
			}
		}
	}
	#endregion
}

public class Helper: Ally, ICanBehaveInCombat {
	private bool canHeal = true;

	#region ICanBehaveInCombat implementation
	public void CombatBehaviour () {
		if (canHeal) {
			var healableAllies = GameController.gameState.organsAllies[organAttachedTo.id].OfType<ICanBeAttacked> ().ToList ();
			for (var i = 0; i < GameController.gameState.helperNbTarget; i++) {
				var a = healableAllies.GetRandomValue () as Ally;
				if (a.status == UnitStatus.Dead) {
					a.status = UnitStatus.Weakened;
				} else if (a.status == UnitStatus.Weakened) {
					a.status = UnitStatus.Good;
				}
			}
			organAttachedTo.Heal (GameController.gameState.helperHealAmount);
			GameController.gameState.StartExternalCoroutine (PlayCooldown ());
		}
	}
	#endregion

	IEnumerator PlayCooldown() {
		canHeal = false;
		for (var i = GameController.gameState.helperHealDelay; i >= 0; i--) {
			yield return new WaitForSeconds (1);
		}
		canHeal = true;
	}
}

public class BCell: Ally, ICanBehaveInCombat {

	#region ICanBehaveInCombat implementation
	public void CombatBehaviour () {
		throw new System.NotImplementedException ();
	}
	#endregion
}