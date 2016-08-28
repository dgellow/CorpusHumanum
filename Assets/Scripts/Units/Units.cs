using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum UnitStatus { Good, Weakened, Dead }

public struct Enemy {
	public UnitStatus status;
	public int damages;
	public EnemyType identification;
	public Enemy (int damages, EnemyType id) {
		this.damages = damages;
		identification = id;
		status = UnitStatus.Good;
	}
}

public struct Ally {
	public UnitStatus status;
	public List<EnemyType> strongAgainst;
	public int damages;
	public Ally(int damages, List<EnemyType> strengths) {
		status = UnitStatus.Good;
		strongAgainst = strengths;
		this.damages = damages;
	}
}