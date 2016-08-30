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
	void ReactToBeingHurt (int amount);
}

public interface ICanBehaveInCombat {
	void CombatBehaviour ();
}

public interface IHasLifespan {
	void PlayLifespan (float duration);
}
	
public class Enemy: ICanAttack, ICanBeAttacked {
	public UnitStatus status = UnitStatus.Good;
	public int damages = 1;
	public int maxHP;
	private int currentHP;
	public UnitTier tier;
	public Organ target;

	public Enemy (Organ target, UnitTier tier) {
		this.tier = tier;
		this.target = target;
		maxHP = hpforUnitTier (tier);
		currentHP = maxHP;
	}

	private int hpforUnitTier(UnitTier tier) {
		switch (tier) {
		case UnitTier.None:
			return 10;
		case UnitTier.Triangle:
			return 50;
		case UnitTier.Square:
			return 80;
		case UnitTier.Octogon:
			return 150;
		case UnitTier.Circle:
			return 300;
		}
		return 0;
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
		currentHP -= amount;
		if ((float)currentHP / (float)maxHP < 0.25) { 
			status = UnitStatus.Weakened;
		}
		if (currentHP <= 0) {
			status = UnitStatus.Dead;
		}
	}

	#endregion
}

abstract public class Ally : IHasLifespan {
	public UnitStatus status = UnitStatus.Good;
	public List<UnitTier> strongAgainst;
	public Organ organAttachedTo;
	public float lifespan;
	public float weakenedLifespan;

	public void UpdateStatus (UnitStatus previousState) {
		switch (previousState) {
		case UnitStatus.Good:
			this.status = UnitStatus.Weakened;
			this.lifespan = this.weakenedLifespan;
			return;
		case UnitStatus.Weakened:
			this.status = UnitStatus.Dead;
			return;
		}
	}

	public void PlayLifespan (float duration) {
		lifespan -= duration;
		if (lifespan < 0) {
			this.UpdateStatus (status);
		}
	}
}

class Macrophage: Ally, ICanAttack, ICanBeAttacked {
	public const int maxHP = 11;
	public int currentHP = maxHP;

	public Macrophage() {
		this.lifespan = GameController.gameState.macrophageLifespan;
		this.weakenedLifespan = GameController.gameState.macrophageWeakLifespan;
	}

	#region ICanAttack implementation

	public void Hurt () {
		var enemies = GameController.gameState.organsEnemies [organAttachedTo.id];
		if (enemies.Count > 0) {
			var target = enemies [0];
			if (target != null && target.status != UnitStatus.Dead) {
				target.ReactToBeingHurt (1);	
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
		currentHP -= amount;
		if (currentHP < 3) { 
			status = UnitStatus.Weakened;
		}
		if (currentHP <= 0) {
			status = UnitStatus.Dead;
		}
	}
	#endregion
}

public class Neutrophil: Ally, ICanBehaveInCombat {

	public Neutrophil() {

		this.lifespan = GameController.gameState.neutrophilLifespan;
		this.weakenedLifespan = GameController.gameState.neutrophilWeakLifespan;
	}

	#region ICanBehaveInCombat implementation
	public void CombatBehaviour () {
		var nbTarget = GameController.gameState.neutrophilNbTarget;
		var enemies = GameController.gameState.organsEnemies [organAttachedTo.id].OfType<ICanBeAttacked> ().ToList ();
		var allies = GameController.gameState.organsAllies [organAttachedTo.id].OfType<ICanBeAttacked> ().ToList ();
		for (var i = 0; i < nbTarget; i++) {
			// Attack random enemy
			if (enemies.Count > 0) {
				var e = enemies.GetRandomValue ();
				e.ReactToBeingHurt (100);
			}

			// Attack random ally
			if (allies.Count > 0) {
				var a = allies.OfType<ICanBeAttacked> ().ToList ().GetRandomValue ();
				a.ReactToBeingHurt (10);
			}

			// Attack organ
			organAttachedTo.ReactToBeingHurt (10);
		}
		// Commit suicide
		status = UnitStatus.Dead;
	}
	#endregion
}

public class Killer: Ally, ICanAttack {

	public Killer () {
		this.lifespan = GameController.gameState.killerLifespan;
		this.weakenedLifespan = GameController.gameState.killerWeakLifespan;
		strongAgainst = new List<UnitTier> ();
	}

	#region ICanAttack implementation
	public void Hurt () {
		var enemies = GameController.gameState.organsEnemies [organAttachedTo.id];
		if (enemies.Count > 0) {
			var naturalEnemies = enemies.Where (x => strongAgainst.Contains (x.tier)).ToList ();
			var target = naturalEnemies.Count > 0 ? naturalEnemies.GetRandomValue () : enemies.GetRandomValue ();
			while (target.status != UnitStatus.Dead) {
				target.ReactToBeingHurt (10);
			}
		}
	}
	#endregion
}

public class Helper: Ally, ICanBehaveInCombat {
	private bool canHeal = true;

	public Helper () {
		this.lifespan = GameController.gameState.helperLifespan;
		this.weakenedLifespan = GameController.gameState.helperWeakLifespan;
	}

	#region ICanBehaveInCombat implementation
	public void CombatBehaviour () {
		if (canHeal) {
			var healableAllies = GameController.gameState.organsAllies[organAttachedTo.id].OfType<ICanBeAttacked> ().ToList ();
			for (var i = 0; i < GameController.gameState.helperNbTarget; i++) {
				var a = healableAllies.GetRandomValue () as Ally;
				if (a.status == UnitStatus.Weakened) {
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

	public BCell() {
		this.lifespan = GameController.gameState.bcellLifespan;
		this.weakenedLifespan = GameController.gameState.bcellWeakLifespan;
	}
	#region ICanBehaveInCombat implementation
	public void CombatBehaviour () {
		throw new System.NotImplementedException ();
	}
	#endregion
}