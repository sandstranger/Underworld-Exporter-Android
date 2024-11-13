using UnityEngine;

public class GeneratorClasses : UWClass
{
	public int index;

	public int Style;

	public static bool RandomPercent(int percent)
	{
		return percent >= Random.Range(1, 101);
	}
}
