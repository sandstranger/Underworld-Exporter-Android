using System.Linq;
using UnityEngine;

public class MoonGate : Model3D
{
	public override Vector3[] ModelVertices()
	{
		return new Vector3[8]
		{
			new Vector3(-0.375f, 0f, 0f),
			new Vector3(0.375f, 0f, 0f),
			new Vector3(0.375f, 1.25f, 0f),
			new Vector3(-0.375f, 1.25f, 0f),
			new Vector3(-0.375f, 0f, 0.025f),
			new Vector3(0.375f, 0f, 0.025f),
			new Vector3(0.375f, 1.25f, 0.025f),
			new Vector3(-0.375f, 1.25f, 0.025f)
		};
	}

	public override int[] ModelTriangles(int meshNo)
	{
		return new int[12]
		{
			1, 2, 3, 1, 3, 0, 4, 7, 6, 4,
			6, 5
		}.Reverse().ToArray();
	}

	public override Color ModelColour(int meshNo)
	{
		return GameWorldController.instance.palLoader.Palettes[0].ColorAtPixel((byte)(base.link - 512), false);
	}

	public override Vector2[] ModelUVs(Vector3[] verts)
	{
		return new Vector2[8]
		{
			new Vector2(0f, 0f),
			new Vector2(0f, 1f),
			new Vector2(1f, 1f),
			new Vector2(1f, 0f),
			new Vector2(0f, 0f),
			new Vector2(0f, 1f),
			new Vector2(1f, 1f),
			new Vector2(1f, 0f)
		};
	}

	public override bool isSolidModel()
	{
		return false;
	}
}
