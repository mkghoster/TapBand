using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class ListUtils {
	
	public static T NextOf<T>(this IList<T> list, T item)
	{
        int nextIndex = (list.IndexOf(item) + 1);
        return list[nextIndex == list.Count ? 0 : nextIndex];
	}
}
