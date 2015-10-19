using UnityEngine;
using System.Collections;
using System;

public class NumberUtil {
	public static bool FloatsEqual(float x, float y, float epsilon) {
		double absX = Math.Abs(x);
		double absY = Math.Abs(y);
		double diff = Math.Abs(x - y);
		
		if (x == y)
		{ // shortcut, handles infinities
			return true;
		} 
		else if (x == 0 || y == 0 || diff < Double.MinValue) 
		{
			// a or b is zero or both are extremely close to it
			// relative error is less meaningful here
			return diff < (epsilon * Double.MinValue);
		}
		else
		{ // use relative error
			return diff / (absX + absY) < epsilon;
		}
	}
}
