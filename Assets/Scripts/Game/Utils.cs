using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class Utils {
	public static T GetRandomValue<T>(this IList<T> list)  {
		var index = Random.Range (0, list.Count);
		return list [index];
	}
}
