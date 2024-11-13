using System.Linq;
using UnityEngine;

public class Bed : Model3D
{
	public override bool use()
	{
		if (UWEBase.CurrentObjectInHand == null)
		{
			UWCharacter.Instance.Sleep();
			return true;
		}
		return ActivateByObject(UWEBase.CurrentObjectInHand);
	}

	public override string UseVerb()
	{
		return "sleep";
	}

	public override int NoOfMeshes()
	{
		return 4;
	}

	public override int[] ModelTriangles(int MeshNo)
	{
		switch (MeshNo)
		{
		case 0:
			return new int[225]
			{
				62, 56, 55, 62, 55, 63, 56, 45, 46, 56,
				46, 55, 55, 47, 54, 55, 46, 47, 55, 54,
				63, 45, 56, 62, 45, 62, 25, 45, 61, 46,
				47, 48, 53, 47, 53, 54, 48, 41, 52, 48,
				52, 53, 52, 42, 51, 52, 41, 42, 42, 43,
				50, 42, 50, 51, 43, 49, 50, 43, 44, 49,
				50, 49, 66, 49, 57, 66, 50, 66, 51, 49,
				44, 57, 57, 44, 58, 46, 61, 47, 44, 43,
				42, 44, 42, 58, 24, 23, 25, 62, 61, 24,
				62, 24, 25, 25, 23, 26, 14, 12, 15, 14,
				13, 12, 42, 13, 58, 13, 14, 58, 54, 53,
				52, 54, 52, 51, 54, 51, 65, 54, 65, 64,
				48, 77, 42, 48, 42, 41, 48, 47, 77, 77,
				47, 75, 26, 27, 76, 26, 4, 27, 4, 26,
				60, 4, 60, 3, 4, 5, 27, 4, 3, 5,
				5, 3, 0, 59, 12, 8, 8, 12, 7, 30,
				12, 78, 7, 12, 30, 7, 30, 6, 6, 30,
				78, 7, 6, 9, 8, 7, 9, 5, 11, 10,
				5, 10, 6, 5, 76, 27, 5, 6, 78, 5,
				78, 76, 26, 76, 25, 25, 76, 75, 47, 61,
				75, 42, 77, 58, 78, 12, 77, 77, 12, 13,
				44, 13, 58, 59, 9, 10, 59, 10, 15, 60,
				23, 0, 23, 11, 0
			}.Reverse().ToArray();
		case 1:
			return new int[30]
			{
				76, 78, 16, 76, 16, 20, 16, 32, 74, 16,
				78, 32, 76, 20, 71, 71, 31, 76, 20, 16,
				71, 16, 74, 71, 31, 71, 74, 31, 74, 32
			}.Reverse().ToArray();
		case 2:
			return new int[6] { 75, 76, 78, 75, 78, 77 }.Reverse().ToArray();
		case 3:
			return new int[30]
			{
				34, 33, 36, 34, 36, 37, 33, 80, 31, 33,
				34, 80, 36, 79, 37, 79, 36, 32, 36, 33,
				32, 33, 31, 32, 80, 34, 37, 80, 37, 79
			}.Reverse().ToArray();
		default:
			return base.ModelTriangles(MeshNo);
		}
	}

	public override Color ModelColour(int meshNo)
	{
		switch (meshNo)
		{
		case 0:
			return Color.red;
		case 1:
			return Color.green;
		case 2:
			return Color.white;
		case 3:
			return Color.black;
		default:
			return base.ModelColour(meshNo);
		}
	}

	public override Vector3[] ModelVertices()
	{
		return new Vector3[81]
		{
			new Vector3(-0.2226563f, 0f, -49f / 128f),
			new Vector3(15f / 64f, 21f / 128f, -0.3632813f),
			new Vector3(-0.125f, 0.3164063f, 0.2617188f),
			new Vector3(-0.25f, 0f, -49f / 128f),
			new Vector3(-0.25f, 39f / 128f, -49f / 128f),
			new Vector3(-0.2148438f, 0.2226563f, -49f / 128f),
			new Vector3(0.2148438f, 0.2226563f, -49f / 128f),
			new Vector3(0.25f, 39f / 128f, -49f / 128f),
			new Vector3(0.25f, 0f, -49f / 128f),
			new Vector3(0.2226563f, 0f, -49f / 128f),
			new Vector3(7f / 32f, 15f / 128f, -49f / 128f),
			new Vector3(-7f / 32f, 15f / 128f, -49f / 128f),
			new Vector3(0.25f, 0.2226563f, -0.3476563f),
			new Vector3(0.25f, 0.2226563f, 11f / 32f),
			new Vector3(0.25f, 15f / 128f, 11f / 32f),
			new Vector3(0.25f, 15f / 128f, -45f / 128f),
			new Vector3(0.2148438f, 0.2695313f, -0.3476563f),
			new Vector3(0.2148438f, 0.2695313f, 21f / 64f),
			new Vector3(15f / 64f, 21f / 128f, 11f / 32f),
			new Vector3(-15f / 64f, 21f / 128f, -0.3632813f),
			new Vector3(-0.2148438f, 0.2695313f, -0.3476563f),
			new Vector3(-0.2148438f, 0.2695313f, 21f / 64f),
			new Vector3(-15f / 64f, 21f / 128f, 11f / 32f),
			new Vector3(-0.25f, 15f / 128f, -45f / 128f),
			new Vector3(-0.25f, 15f / 128f, 11f / 32f),
			new Vector3(-0.25f, 0.2226563f, 11f / 32f),
			new Vector3(-0.25f, 0.2226563f, -0.3476563f),
			new Vector3(-0.2148438f, 39f / 128f, -0.3476563f),
			new Vector3(-0.2148438f, 39f / 128f, 0.1992188f),
			new Vector3(0.2148438f, 39f / 128f, 0.1992188f),
			new Vector3(0.2148438f, 39f / 128f, -0.3476563f),
			new Vector3(-15f / 64f, 21f / 128f, 7f / 32f),
			new Vector3(15f / 64f, 21f / 128f, 7f / 32f),
			new Vector3(-0.1601563f, 0.2695313f, 15f / 64f),
			new Vector3(-0.1601563f, 0.2695313f, 41f / 128f),
			new Vector3(-0.1367188f, 39f / 128f, 0.3085938f),
			new Vector3(0.1601563f, 0.2695313f, 15f / 64f),
			new Vector3(0.1601563f, 0.2695313f, 41f / 128f),
			new Vector3(0.1367188f, 39f / 128f, 0.3085938f),
			new Vector3(0.125f, 0.3164063f, 0.2617188f),
			new Vector3(0f, 0.2226563f, 11f / 32f),
			new Vector3(9f / 64f, 69f / 128f, 11f / 32f),
			new Vector3(7f / 32f, 0.375f, 11f / 32f),
			new Vector3(0.2226563f, 0.4804688f, 11f / 32f),
			new Vector3(0.25f, 0.4804688f, 11f / 32f),
			new Vector3(-0.25f, 0.4804688f, 11f / 32f),
			new Vector3(-0.2226563f, 0.4804688f, 11f / 32f),
			new Vector3(-7f / 32f, 0.375f, 11f / 32f),
			new Vector3(-9f / 64f, 69f / 128f, 11f / 32f),
			new Vector3(0.25f, 63f / 128f, 49f / 128f),
			new Vector3(0.2226563f, 63f / 128f, 49f / 128f),
			new Vector3(7f / 32f, 0.3867188f, 49f / 128f),
			new Vector3(9f / 64f, 0.5507813f, 49f / 128f),
			new Vector3(-9f / 64f, 0.5507813f, 49f / 128f),
			new Vector3(-7f / 32f, 0.3867188f, 49f / 128f),
			new Vector3(-0.2226563f, 63f / 128f, 49f / 128f),
			new Vector3(-0.25f, 63f / 128f, 49f / 128f),
			new Vector3(0.25f, 0f, 49f / 128f),
			new Vector3(0.25f, 0f, 0.3554688f),
			new Vector3(0.25f, 0f, -0.3554688f),
			new Vector3(-0.25f, 0f, -0.3554688f),
			new Vector3(-0.25f, 0f, 0.3554688f),
			new Vector3(-0.25f, 0f, 49f / 128f),
			new Vector3(-0.2226563f, 0f, 49f / 128f),
			new Vector3(-7f / 32f, 15f / 128f, 49f / 128f),
			new Vector3(7f / 32f, 15f / 128f, 49f / 128f),
			new Vector3(0.2226563f, 0f, 49f / 128f),
			new Vector3(-0.25f, 15f / 128f, 0.3554688f),
			new Vector3(-0.2226563f, 15f / 128f, 49f / 128f),
			new Vector3(0.25f, 15f / 128f, 0.3554688f),
			new Vector3(0.2226563f, 15f / 128f, 49f / 128f),
			new Vector3(-7f / 32f, 0.2695313f, 13f / 64f),
			new Vector3(-7f / 32f, 0.2695313f, -45f / 128f),
			new Vector3(7f / 32f, 0.2695313f, -45f / 128f),
			new Vector3(7f / 32f, 0.2695313f, 13f / 64f),
			new Vector3(-0.2226563f, 0.2226563f, 11f / 32f),
			new Vector3(-0.2226563f, 0.2226563f, -0.3476563f),
			new Vector3(0.2226563f, 0.2226563f, 11f / 32f),
			new Vector3(0.2226563f, 0.2226563f, -0.3476563f),
			new Vector3(0.2226563f, 0.2226563f, 43f / 128f),
			new Vector3(-0.2226563f, 0.2226563f, 43f / 128f)
		};
	}
}
