using System.Linq;
using UnityEngine;

public class Arrow : Model3D
{
	public override int[] ModelTriangles(int meshNo)
	{
		return new int[84]
		{
			2, 3, 4, 0, 2, 4, 1, 6, 5, 1,
			7, 6, 1, 8, 7, 1, 5, 8, 13, 9,
			0, 3, 11, 15, 8, 4, 3, 7, 8, 3,
			5, 0, 4, 8, 5, 4, 16, 12, 4, 4,
			12, 16, 2, 3, 4, 0, 2, 4, 1, 6,
			5, 1, 7, 6, 1, 8, 7, 1, 5, 8,
			13, 0, 9, 0, 5, 2, 2, 5, 6, 10,
			2, 14, 10, 14, 2, 2, 6, 3, 3, 6,
			7, 11, 3, 15
		}.Reverse().ToArray();
	}

	public override Vector3[] ModelVertices()
	{
		return new Vector3[17]
		{
			new Vector3(-1f / 128f, -1f / 128f, -0.1875f),
			new Vector3(0f, 0f, 7f / 32f),
			new Vector3(1f / 128f, -1f / 128f, -0.1875f),
			new Vector3(1f / 128f, 1f / 128f, -0.1875f),
			new Vector3(-1f / 128f, 1f / 128f, -0.1875f),
			new Vector3(-1f / 128f, -1f / 128f, 0.1875f),
			new Vector3(1f / 128f, -1f / 128f, 0.1875f),
			new Vector3(1f / 128f, 1f / 128f, 0.1875f),
			new Vector3(-1f / 128f, 1f / 128f, 0.1875f),
			new Vector3(-0.02734375f, -0.02734375f, -0.1875f),
			new Vector3(0.02734375f, -0.02734375f, -0.1875f),
			new Vector3(0.02734375f, 0.02734375f, -0.1875f),
			new Vector3(-0.02734375f, 0.02734375f, -0.1875f),
			new Vector3(-1f / 128f, -1f / 128f, -21f / 128f),
			new Vector3(1f / 128f, -1f / 128f, -21f / 128f),
			new Vector3(1f / 128f, 1f / 128f, -21f / 128f),
			new Vector3(-1f / 128f, 1f / 128f, -21f / 128f)
		};
	}
}
