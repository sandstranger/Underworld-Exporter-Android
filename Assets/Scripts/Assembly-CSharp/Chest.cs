using System.Linq;
using UnityEngine;

public class Chest : Barrel
{
	protected override int NoOfMeshes()
	{
		return 2;
	}

	protected override int[] ModelTriangles(int meshNo)
	{
		switch (meshNo)
		{
		case 0:
			return new int[126]
			{
				18, 56, 21, 18, 13, 56, 13, 54, 56, 13,
				53, 54, 53, 50, 54, 53, 49, 50, 49, 46,
				50, 49, 47, 46, 47, 48, 45, 47, 45, 46,
				48, 52, 51, 48, 51, 45, 51, 12, 55, 51,
				52, 12, 55, 15, 26, 55, 12, 15, 20, 5,
				6, 5, 20, 38, 5, 37, 30, 5, 38, 37,
				30, 37, 39, 30, 39, 29, 29, 39, 40, 29,
				40, 28, 28, 40, 41, 28, 41, 2, 2, 41,
				43, 2, 43, 42, 42, 43, 44, 42, 44, 1,
				1, 44, 25, 1, 25, 0, 0, 25, 15, 25,
				26, 15, 21, 6, 18, 21, 20, 6, 19, 56,
				38, 22, 56, 19, 65, 67, 66, 65, 64, 67,
				44, 55, 24, 55, 23, 24
			}.Reverse().ToArray();
		case 1:
			return new int[105]
			{
				25, 24, 23, 25, 23, 26, 21, 22, 19, 21,
				19, 20, 15, 12, 13, 15, 13, 18, 6, 5,
				0, 5, 1, 0, 37, 38, 65, 37, 65, 66,
				67, 56, 54, 67, 64, 56, 37, 67, 54, 67,
				37, 66, 39, 37, 54, 39, 54, 50, 50, 46,
				40, 50, 40, 39, 46, 45, 41, 46, 41, 40,
				45, 51, 43, 45, 43, 41, 51, 44, 43, 51,
				55, 44, 1, 5, 30, 1, 30, 29, 1, 29,
				28, 1, 5, 28, 1, 28, 2, 1, 2, 42,
				12, 53, 13, 12, 49, 53, 12, 47, 49, 12,
				48, 47, 12, 57, 48
			}.Reverse().ToArray();
		default:
			return new int[3];
		}
	}

	protected override Vector3[] ModelVertices()
	{
		return new Vector3[68]
		{
			new Vector3(-0.0625f, 0f, -0.1835938f),
			new Vector3(-5f / 64f, 5f / 32f, -25f / 128f),
			new Vector3(-1f / 32f, 0.2070313f, -11f / 64f),
			new Vector3(0.04296875f, 0.1835938f, -0.1875f),
			new Vector3(7f / 128f, 11f / 64f, 0.1875f),
			new Vector3(5f / 64f, 5f / 32f, -25f / 128f),
			new Vector3(9f / 128f, 0f, -0.1835938f),
			new Vector3(-0.0625f, 9f / 64f, -0.1914063f),
			new Vector3(-7f / 128f, 0.01171875f, -0.1835938f),
			new Vector3(0.0625f, 0.01171875f, -0.1835938f),
			new Vector3(0.0625f, 9f / 64f, -0.1914063f),
			new Vector3(-0.0625f, 9f / 64f, 0.1914063f),
			new Vector3(-5f / 64f, 5f / 32f, 25f / 128f),
			new Vector3(5f / 64f, 5f / 32f, 25f / 128f),
			new Vector3(0.0625f, 9f / 64f, 0.1914063f),
			new Vector3(-0.0625f, 0f, 0.1835938f),
			new Vector3(-7f / 128f, 0.01171875f, 0.1835938f),
			new Vector3(0.0625f, 0.01171875f, 0.1835938f),
			new Vector3(9f / 128f, 0f, 0.1835938f),
			new Vector3(5f / 64f, 19f / 128f, -23f / 128f),
			new Vector3(9f / 128f, 1f / 64f, -0.1679688f),
			new Vector3(9f / 128f, 1f / 64f, 0.1679688f),
			new Vector3(5f / 64f, 19f / 128f, 23f / 128f),
			new Vector3(-5f / 64f, 19f / 128f, 23f / 128f),
			new Vector3(-5f / 64f, 19f / 128f, -23f / 128f),
			new Vector3(-0.0625f, 1f / 64f, -0.1679688f),
			new Vector3(-0.0625f, 1f / 64f, 0.1679688f),
			new Vector3(-0.05859375f, 0.1875f, -0.1835938f),
			new Vector3(0f, 7f / 32f, -0.1679688f),
			new Vector3(0.03515625f, 0.2070313f, -11f / 64f),
			new Vector3(0.05859375f, 0.1875f, -0.1835938f),
			new Vector3(0.0625f, 5f / 32f, -25f / 128f),
			new Vector3(1f / 32f, 0.1914063f, -23f / 128f),
			new Vector3(0f, 13f / 64f, -11f / 64f),
			new Vector3(-1f / 32f, 0.1914063f, -23f / 128f),
			new Vector3(-0.05078125f, 23f / 128f, -0.1875f),
			new Vector3(-0.0625f, 5f / 32f, -25f / 128f),
			new Vector3(0.05859375f, 0.1875f, -21f / 128f),
			new Vector3(5f / 64f, 5f / 32f, -23f / 128f),
			new Vector3(0.03515625f, 0.2070313f, -5f / 32f),
			new Vector3(0f, 7f / 32f, -0.1523438f),
			new Vector3(-1f / 32f, 0.2070313f, -5f / 32f),
			new Vector3(-7f / 128f, 0.1875f, -0.1835938f),
			new Vector3(-7f / 128f, 0.1875f, -21f / 128f),
			new Vector3(-5f / 64f, 5f / 32f, -23f / 128f),
			new Vector3(-1f / 32f, 0.2070313f, 5f / 32f),
			new Vector3(0f, 7f / 32f, 0.1523438f),
			new Vector3(0f, 7f / 32f, 0.1679688f),
			new Vector3(-1f / 32f, 0.2070313f, 11f / 64f),
			new Vector3(0.03515625f, 0.2070313f, 11f / 64f),
			new Vector3(0.03515625f, 0.2070313f, 5f / 32f),
			new Vector3(-7f / 128f, 0.1875f, 21f / 128f),
			new Vector3(-7f / 128f, 0.1875f, 0.1835938f),
			new Vector3(0.05859375f, 0.1875f, 0.1835938f),
			new Vector3(0.05859375f, 0.1875f, 21f / 128f),
			new Vector3(-5f / 64f, 5f / 32f, 23f / 128f),
			new Vector3(5f / 64f, 5f / 32f, 23f / 128f),
			new Vector3(-0.05859375f, 0.1875f, 0.1835938f),
			new Vector3(0.0625f, 5f / 32f, 25f / 128f),
			new Vector3(1f / 32f, 0.1914063f, 23f / 128f),
			new Vector3(0f, 13f / 64f, 11f / 64f),
			new Vector3(-1f / 32f, 0.1914063f, 23f / 128f),
			new Vector3(-0.05078125f, 23f / 128f, 0.1875f),
			new Vector3(-0.0625f, 5f / 32f, 25f / 128f),
			new Vector3(5f / 64f, 5f / 32f, 1f / 64f),
			new Vector3(5f / 64f, 5f / 32f, -1f / 64f),
			new Vector3(0.06640625f, 11f / 64f, -1f / 128f),
			new Vector3(0.06640625f, 11f / 64f, 1f / 128f)
		};
	}

	public override Color ModelColour(int meshNo)
	{
		if (meshNo == 1)
		{
			return Color.green;
		}
		return Color.red;
	}
}
