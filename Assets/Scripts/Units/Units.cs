using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
	void PlayLifespan (float duration);
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

abstract public class Ally : IHasLifespan {
	public UnitStatus status = UnitStatus.Good;
	public List<UnitTier> strongAgainst;
	public Organ organAttachedTo;
	public float lifespan;
	public float weakenedLifespan;

	public void updateStatus (UnitStatus previousState) {

		Debug.Log ("updating status from lifespan");
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
			this.updateStatus (status);
		}
	}
}

class Macrophage: Ally, ICanAttack, ICanBeAttacked {

	public Macrophage() {
		this.lifespan = GameController.gameState.macrophageLifespan;
		this.weakenedLifespan = GameController.gameState.macrophageWeakLifespan;
	}

	#region ICanAttack implementation

	public void Hurt () {
		var target = GameController.gameState.organsEnemies [organAttachedTo.id] [0];
		if (target != null && target.status != UnitStatus.Dead) {
			target.ReactToBeingHurt ();	
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

	public Neutrophil() {

		this.lifespan = GameController.gameState.neutrophilLifespan;
		this.weakenedLifespan = GameController.gameState.neutrophilWeakLifespan;
	}
	#	region ICanBehaveInCombat implementation
	public void CombatBehaviour () {
		var enemies = GameController.gameState.organsEnemies [organAttachedTo.id];
		var allies = GameController.gameState.organsAllies [organAttachedTo.id];
		foreach (var e in enemies) {
			e.ReactToBeingHurt ();
		}
		foreach (var a in allies) {
			var attackableAlly = a as ICanBeAttacked;
			if (attackableAlly != null) {
				attackableAlly.ReactToBeingHurt ();
			}
		}
		status = UnitStatus.Dead;
	}
	#endregion
}

public class Killer: Ally, ICanAttack {

	public Killer () {
		this.lifespan = GameController.gameState.killerLifespan;
		this.weakenedLifespan = GameController.gameState.killerWeakLifespan;
	}
	#region ICanAttack implementation
	public void Hurt () {
		var target = GameController.gameState.organsEnemies [organAttachedTo.id][0];
		while (target.status != UnitStatus.Dead) {
			target.ReactToBeingHurt ();
		}
	}
	#endregion
}

public class Helper: Ally, ICanBehaveInCombat {
	private int healDelay = 10;
	private bool canHeal = true;

	public Helper () {
		this.lifespan = GameController.gameState.helperLifespan;
		this.weakenedLifespan = GameController.gameState.helperWeakLifespan;
	}

	#region ICanBehaveInCombat implementation
	public void CombatBehaviour () {
		if (canHeal) {
			organAttachedTo.Heal (10);
			GameController.gameState.StartExternalCoroutine (PlayCooldown ());
		}
	}
	#endregion

	IEnumerator PlayCooldown() {
		canHeal = false;
		for (var i = healDelay; i >= 0; i--) {
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