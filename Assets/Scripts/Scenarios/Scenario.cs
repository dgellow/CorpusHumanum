using UnityEngine;
using System.Collections;

public enum Scenario { 
	Level1, 
	Level2, 
	Level3 
}

public interface IScenario {
	IEnumerator Play ();
}
