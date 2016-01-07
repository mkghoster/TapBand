using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class ListUtils {
	
	public static T NextOf<T>(this IList<T> list, T item)
	{
		return list[(list.IndexOf(item) + 1) == list.Count ? 0 : (list.IndexOf(item) + 1)];
	}
}
