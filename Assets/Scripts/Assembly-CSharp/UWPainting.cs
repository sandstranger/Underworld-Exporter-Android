using System.Linq;
using UnityEngine;

public class UWPainting : Model3D
{
	public override int NoOfMeshes()
	{
		return 2;
	}

	public override int[] ModelTriangles(int meshNo)
	{
		switch (meshNo)
		{
		case 0:
			return new int[6] { 4, 3, 2, 0, 4, 2 }.Reverse().ToArray();
		case 1:
			return new int[48]
			{
				5, 1, 0, 2, 5, 0, 6, 5, 2, 3,
				6, 2, 7, 6, 3, 4, 7, 3, 1, 7,
				4, 0, 1, 4, 6, 7, 8, 9, 6, 8,
				1, 5, 10, 11, 1, 10, 6, 9, 10, 5,
				6, 10, 7, 1, 11, 8, 7, 11
			}.Reverse().ToArray();
		default:
			return base.ModelTriangles(meshNo);
		}
	}

	public override Vector3[] ModelVertices()
	{
		return new Vector3[12]
		{
			new Vector3(-0.25f, 0.03515625f, 0.05078125f),
			new Vector3(-39f / 128f, 0f, 1f / 32f),
			new Vector3(-0.25f, 0.2851563f, 0.05078125f),
			new Vector3(0.25f, 0.2851563f, 0.05078125f),
			new Vector3(0.25f, 0.03515625f, 0.05078125f),
			new Vector3(-39f / 128f, 41f / 128f, 1f / 32f),
			new Vector3(39f / 128f, 41f / 128f, 1f / 32f),
			new Vector3(39f / 128f, 0f, 1f / 32f),
			new Vector3(39f / 128f, 0f, 0.05859375f),
			new Vector3(39f / 128f, 41f / 128f, 0.05859375f),
			new Vector3(-39f / 128f, 41f / 128f, 0.05859375f),
			new Vector3(-39f / 128f, 0f, 0.05859375f)
		};
	}

	public override Material ModelMaterials(int meshNo)
	{
		switch (meshNo)
		{
		case 0:
			return GameWorldController.instance.MaterialObj[42 + (base.flags & 7)];
		case 1:
			return GameWorldController.instance.MaterialObj[38];
		default:
			return base.ModelMaterials(meshNo);
		}
	}

	public override Vector2[] ModelUVs(Vector3[] verts)
	{
		Vector2[] array = base.ModelUVs(verts);
		array[0] = new Vector2(0f, 0f);
		array[2] = new Vector2(0f, 1f);
		array[3] = new Vector2(1f, 1f);
		array[4] = new Vector2(1f, 0f);
		return array;
	}

	public override Color ModelColour(int meshNo)
	{
		if (meshNo == 1)
		{
			return Color.yellow;
		}
		return base.ModelColour(meshNo);
	}
}
