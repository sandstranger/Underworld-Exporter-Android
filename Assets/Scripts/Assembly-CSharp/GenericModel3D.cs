using System.Linq;
using UnityEngine;

public class GenericModel3D : Model3D
{
	public override int[] ModelTriangles(int meshNo)
	{
		switch (base.item_id)
		{
		case 336:
			return BenchTriangles().Reverse().ToArray();
		case 344:
			return TableTriangles().Reverse().ToArray();
		case 345:
			return BeamTriangles().Reverse().ToArray();
		case 348:
			return ChairTriangles(meshNo).Reverse().ToArray();
		case 350:
			return NightStandTriangles().Reverse().ToArray();
		case 361:
			return ShelfTriangles().Reverse().ToArray();
		default:
			return base.ModelTriangles(meshNo);
		}
	}

	public override Vector3[] ModelVertices()
	{
		switch (base.item_id)
		{
		case 336:
			return BenchVertices();
		case 344:
			return TableVertices();
		case 345:
			return BeamVertices();
		case 348:
			return ChairVertices();
		case 350:
			return NightStandVertices();
		case 361:
			return ShelfVertices();
		default:
			return base.ModelVertices();
		}
	}

	public override Color ModelColour(int meshNo)
	{
		return Color.grey;
	}

	public override Material ModelMaterials(int meshNo)
	{
		int num = ((!(UWEBase._RES == "UW2")) ? 30 : 34);
		switch (base.item_id)
		{
		case 336:
		case 344:
			return GameWorldController.instance.MaterialObj[num];
		case 348:
			if (meshNo == 0)
			{
				return GameWorldController.instance.MaterialObj[num];
			}
			if (UWEBase._RES == "UW2")
			{
				return GameWorldController.instance.MaterialObj[38];
			}
			return GameWorldController.instance.MaterialObj[num];
		case 350:
			return GameWorldController.instance.MaterialObj[num];
		case 361:
			return GameWorldController.instance.MaterialObj[38];
		default:
			return base.ModelMaterials(meshNo);
		}
	}

	public override float TextureScaling()
	{
		switch (base.item_id)
		{
		case 336:
		case 344:
		case 348:
		case 350:
		case 361:
			return 4f;
		default:
			return base.TextureScaling();
		}
	}

	public override int NoOfMeshes()
	{
		int num = base.item_id;
		if (num == 348)
		{
			return 2;
		}
		return base.NoOfMeshes();
	}

	private Vector3[] BenchVertices()
	{
		return new Vector3[32]
		{
			new Vector3(1f / 64f, 0.1875f, 37f / 128f),
			new Vector3(1f / 64f, 0.1875f, -37f / 128f),
			new Vector3(-0.1757813f, 0.1875f, -37f / 128f),
			new Vector3(-0.1757813f, 0.1875f, 37f / 128f),
			new Vector3(1f / 64f, 13f / 64f, 37f / 128f),
			new Vector3(1f / 64f, 13f / 64f, -37f / 128f),
			new Vector3(-0.1757813f, 13f / 64f, -37f / 128f),
			new Vector3(-0.1757813f, 13f / 64f, 37f / 128f),
			new Vector3(0f, 0.1875f, 31f / 128f),
			new Vector3(1f / 128f, 0f, 31f / 128f),
			new Vector3(1f / 128f, 0f, 27f / 128f),
			new Vector3(0f, 0.1523438f, 0.1757813f),
			new Vector3(0f, 0.1523438f, -0.1757813f),
			new Vector3(1f / 128f, 0f, -27f / 128f),
			new Vector3(1f / 128f, 0f, -31f / 128f),
			new Vector3(0f, 0.1875f, -31f / 128f),
			new Vector3(-0.1679688f, 0f, 31f / 128f),
			new Vector3(-0.1445313f, 0f, 31f / 128f),
			new Vector3(-0.125f, 0.1523438f, 31f / 128f),
			new Vector3(-0.03515625f, 0.1523438f, 31f / 128f),
			new Vector3(-1f / 64f, 0f, 31f / 128f),
			new Vector3(-0.1601563f, 0.1875f, 31f / 128f),
			new Vector3(-0.1679688f, 0f, -31f / 128f),
			new Vector3(-0.1445313f, 0f, -31f / 128f),
			new Vector3(-0.125f, 0.1523438f, -31f / 128f),
			new Vector3(-0.03515625f, 0.1523438f, -31f / 128f),
			new Vector3(-1f / 64f, 0f, -31f / 128f),
			new Vector3(-0.1601563f, 0.1875f, -31f / 128f),
			new Vector3(-0.1679688f, 0f, 27f / 128f),
			new Vector3(-0.1601563f, 0.1523438f, 0.1757813f),
			new Vector3(-0.1601563f, 0.1523438f, -0.1757813f),
			new Vector3(-0.1679688f, 0f, -27f / 128f)
		};
	}

	private int[] BenchTriangles()
	{
		return new int[144]
		{
			4, 7, 6, 4, 6, 5, 6, 7, 2, 7,
			3, 2, 5, 2, 1, 5, 6, 2, 7, 0,
			3, 7, 4, 0, 4, 1, 0, 4, 5, 1,
			0, 1, 2, 0, 2, 3, 8, 18, 21, 8,
			19, 18, 8, 21, 19, 19, 21, 18, 15, 25,
			24, 15, 24, 27, 15, 24, 25, 15, 27, 24,
			26, 15, 25, 15, 26, 14, 14, 13, 15, 13,
			12, 15, 13, 26, 25, 13, 25, 12, 9, 8,
			10, 10, 8, 11, 9, 20, 19, 9, 19, 8,
			11, 8, 15, 12, 11, 15, 22, 23, 24, 22,
			24, 27, 31, 22, 27, 31, 27, 30, 31, 30,
			23, 23, 30, 24, 17, 21, 18, 17, 16, 21,
			16, 28, 29, 16, 29, 21, 30, 27, 21, 30,
			21, 29, 28, 17, 29, 17, 18, 29, 20, 10,
			19, 10, 11, 19
		};
	}

	private Vector3[] BeamVertices()
	{
		return new Vector3[32]
		{
			new Vector3(0f, 0f, 0f),
			new Vector3(0f, 0.0625f, 0f),
			new Vector3(0.0625f, 0.0625f, 0f),
			new Vector3(0.0625f, 0f, 0f),
			new Vector3(0f, 0f, 0.5f),
			new Vector3(0f, 0.0625f, 0.5f),
			new Vector3(0.0625f, 0.0625f, 0.5f),
			new Vector3(0.0625f, 0f, 0.5f),
			default(Vector3),
			default(Vector3),
			default(Vector3),
			default(Vector3),
			default(Vector3),
			default(Vector3),
			default(Vector3),
			default(Vector3),
			default(Vector3),
			default(Vector3),
			default(Vector3),
			default(Vector3),
			default(Vector3),
			default(Vector3),
			default(Vector3),
			default(Vector3),
			default(Vector3),
			default(Vector3),
			default(Vector3),
			default(Vector3),
			default(Vector3),
			default(Vector3),
			default(Vector3),
			default(Vector3)
		};
	}

	private int[] BeamTriangles()
	{
		return new int[36]
		{
			2, 1, 0, 3, 2, 0, 5, 6, 7, 4,
			5, 7, 1, 5, 4, 0, 1, 4, 2, 6,
			5, 1, 2, 5, 6, 2, 3, 7, 6, 3,
			7, 3, 0, 4, 7, 0
		};
	}

	private Vector3[] ShelfVertices()
	{
		return new Vector3[14]
		{
			new Vector3(-0.25f, 0.1289063f, -0.125f),
			new Vector3(-0.25f, 0.1601563f, -0.125f),
			new Vector3(0.25f, 0.1601563f, -0.125f),
			new Vector3(0.25f, 0.1289063f, -0.125f),
			new Vector3(-0.25f, 0.1289063f, 0.125f),
			new Vector3(-0.25f, 0.1601563f, 0.125f),
			new Vector3(0.25f, 0.1601563f, 0.125f),
			new Vector3(0.25f, 0.1289063f, 0.125f),
			new Vector3(21f / 128f, 0.1289063f, 0.125f),
			new Vector3(21f / 128f, 0f, 0.125f),
			new Vector3(21f / 128f, 0.1289063f, -0.00390625f),
			new Vector3(-21f / 128f, 0.1289063f, 0.125f),
			new Vector3(-21f / 128f, 0.1289063f, -0.00390625f),
			new Vector3(-21f / 128f, 0f, 0.125f)
		};
	}

	private int[] ShelfTriangles()
	{
		return new int[48]
		{
			4, 7, 3, 0, 4, 3, 10, 9, 8, 9,
			10, 8, 13, 12, 11, 12, 13, 11, 2, 1,
			0, 3, 2, 0, 5, 6, 7, 4, 5, 7,
			5, 4, 0, 1, 5, 0, 6, 5, 1, 2,
			6, 1, 7, 6, 2, 3, 7, 2
		};
	}

	private int[] TableTriangles()
	{
		return new int[474]
		{
			9, 5, 6, 12, 9, 6, 15, 12, 6, 17,
			15, 6, 50, 17, 6, 53, 50, 6, 36, 35,
			34, 37, 36, 34, 29, 31, 30, 28, 29, 30,
			66, 64, 65, 67, 66, 65, 86, 84, 85, 87,
			86, 85, 58, 57, 56, 59, 58, 56, 57, 70,
			71, 56, 57, 71, 60, 58, 59, 61, 60, 59,
			76, 74, 75, 77, 76, 75, 78, 76, 77, 79,
			78, 77, 80, 78, 79, 81, 80, 79, 44, 42,
			43, 45, 44, 43, 42, 40, 41, 43, 42, 41,
			20, 2, 19, 21, 20, 19, 2, 23, 22, 19,
			2, 22, 46, 44, 45, 47, 46, 45, 23, 25,
			24, 22, 23, 24, 3, 1, 0, 4, 3, 0,
			6, 5, 4, 0, 6, 4, 7, 3, 4, 8,
			7, 4, 1, 55, 54, 0, 1, 54, 5, 9,
			8, 4, 5, 8, 53, 6, 0, 54, 53, 0,
			10, 7, 8, 11, 10, 8, 55, 52, 51, 54,
			55, 51, 9, 12, 11, 8, 9, 11, 50, 53,
			54, 51, 50, 54, 13, 10, 11, 14, 13, 11,
			52, 18, 16, 51, 52, 16, 14, 11, 12, 15,
			14, 12, 51, 16, 17, 50, 51, 17, 16, 14,
			15, 17, 16, 15, 18, 13, 14, 16, 18, 14,
			52, 55, 1, 18, 52, 1, 13, 18, 1, 10,
			13, 1, 7, 10, 1, 3, 7, 1, 35, 48,
			49, 34, 35, 49, 73, 86, 87, 72, 73, 87,
			31, 33, 32, 30, 31, 32, 33, 20, 21, 32,
			33, 21, 62, 60, 61, 63, 62, 61, 64, 62,
			63, 65, 64, 63, 68, 66, 67, 69, 68, 67,
			70, 68, 69, 71, 70, 69, 25, 27, 26, 24,
			25, 26, 27, 29, 28, 26, 27, 28, 58, 57,
			56, 59, 58, 56, 57, 70, 71, 56, 57, 71,
			60, 58, 59, 61, 60, 59, 76, 74, 75, 77,
			76, 75, 78, 76, 77, 79, 78, 77, 80, 78,
			79, 81, 80, 79, 44, 42, 43, 45, 44, 43,
			42, 40, 41, 43, 42, 41, 20, 2, 19, 21,
			20, 19, 2, 23, 22, 19, 2, 22, 46, 44,
			45, 47, 46, 45, 23, 25, 24, 22, 23, 24,
			3, 1, 0, 4, 3, 0, 6, 5, 4, 0,
			6, 4, 7, 3, 4, 8, 7, 4, 1, 55,
			54, 0, 1, 54, 5, 9, 8, 4, 5, 8,
			53, 6, 0, 54, 53, 0, 10, 7, 8, 11,
			10, 8, 55, 52, 51, 54, 55, 51, 9, 12,
			11, 8, 9, 11, 50, 53, 54, 51, 50, 54,
			13, 10, 11, 14, 13, 11, 52, 18, 16, 51,
			52, 16, 14, 11, 12, 15, 14, 12, 51, 16,
			17, 50, 51, 17, 16, 14, 15, 17, 16, 15,
			18, 13, 14, 16, 18, 14, 52, 55, 1, 18,
			52, 1, 13, 18, 1, 10, 13, 1, 7, 10,
			1, 3, 7, 1
		};
	}

	private Vector3[] TableVertices()
	{
		return new Vector3[88]
		{
			new Vector3(-27f / 128f, 21f / 64f, -27f / 128f),
			new Vector3(-13f / 64f, 0.3398438f, -25f / 128f),
			new Vector3(5f / 32f, 0f, -5f / 32f),
			new Vector3(13f / 64f, 0.3398438f, -25f / 128f),
			new Vector3(27f / 128f, 21f / 64f, -27f / 128f),
			new Vector3(27f / 128f, 0.2851563f, -27f / 128f),
			new Vector3(-27f / 128f, 0.2851563f, -27f / 128f),
			new Vector3(17f / 64f, 0.3398438f, -17f / 128f),
			new Vector3(9f / 32f, 21f / 64f, -9f / 64f),
			new Vector3(9f / 32f, 0.2851563f, -9f / 64f),
			new Vector3(17f / 64f, 0.3398438f, 17f / 128f),
			new Vector3(9f / 32f, 21f / 64f, 9f / 64f),
			new Vector3(9f / 32f, 0.2851563f, 9f / 64f),
			new Vector3(13f / 64f, 0.3398438f, 25f / 128f),
			new Vector3(27f / 128f, 21f / 64f, 27f / 128f),
			new Vector3(27f / 128f, 0.2851563f, 27f / 128f),
			new Vector3(-27f / 128f, 21f / 64f, 27f / 128f),
			new Vector3(-27f / 128f, 0.2851563f, 27f / 128f),
			new Vector3(-13f / 64f, 0.3398438f, 25f / 128f),
			new Vector3(0.1601563f, 0.2851563f, -0.1601563f),
			new Vector3(19f / 128f, 0f, -5f / 32f),
			new Vector3(9f / 64f, 0.2851563f, -0.1601563f),
			new Vector3(0.1757813f, 0.2851563f, -5f / 32f),
			new Vector3(0.1679688f, 0f, -19f / 128f),
			new Vector3(0.1757813f, 0.2851563f, -0.125f),
			new Vector3(0.1679688f, 0f, -17f / 128f),
			new Vector3(0.1601563f, 0.2851563f, -0.1210938f),
			new Vector3(5f / 32f, 0f, -0.125f),
			new Vector3(9f / 64f, 0.2851563f, -0.1210938f),
			new Vector3(19f / 128f, 0f, -0.125f),
			new Vector3(0.125f, 0.2851563f, -0.125f),
			new Vector3(17f / 128f, 0f, -17f / 128f),
			new Vector3(0.125f, 0.2851563f, -5f / 32f),
			new Vector3(17f / 128f, 0f, -19f / 128f),
			new Vector3(9f / 64f, 0.2851563f, 0.1210938f),
			new Vector3(19f / 128f, 0f, 0.125f),
			new Vector3(17f / 128f, 0f, 17f / 128f),
			new Vector3(0.125f, 0.2851563f, 0.125f),
			new Vector3(17f / 128f, 0f, 19f / 128f),
			new Vector3(0.125f, 0.2851563f, 5f / 32f),
			new Vector3(19f / 128f, 0f, 5f / 32f),
			new Vector3(9f / 64f, 0.2851563f, 0.1601563f),
			new Vector3(5f / 32f, 0f, 5f / 32f),
			new Vector3(0.1601563f, 0.2851563f, 0.1601563f),
			new Vector3(0.1679688f, 0f, 19f / 128f),
			new Vector3(0.1757813f, 0.2851563f, 5f / 32f),
			new Vector3(0.1679688f, 0f, 17f / 128f),
			new Vector3(0.1757813f, 0.2851563f, 0.125f),
			new Vector3(5f / 32f, 0f, 0.125f),
			new Vector3(0.1601563f, 0.2851563f, 0.1210938f),
			new Vector3(-9f / 32f, 0.2851563f, 9f / 64f),
			new Vector3(-9f / 32f, 21f / 64f, 9f / 64f),
			new Vector3(-17f / 64f, 0.3398438f, 17f / 128f),
			new Vector3(-9f / 32f, 0.2851563f, -9f / 64f),
			new Vector3(-9f / 32f, 21f / 64f, -9f / 64f),
			new Vector3(-17f / 64f, 0.3398438f, -17f / 128f),
			new Vector3(-0.1601563f, 0.2851563f, -0.1601563f),
			new Vector3(-5f / 32f, 0f, -5f / 32f),
			new Vector3(-0.1679688f, 0f, -19f / 128f),
			new Vector3(-0.1757813f, 0.2851563f, -5f / 32f),
			new Vector3(-0.1679688f, 0f, -17f / 128f),
			new Vector3(-0.1757813f, 0.2851563f, -0.125f),
			new Vector3(-5f / 32f, 0f, -0.125f),
			new Vector3(-0.1601563f, 0.2851563f, -0.1210938f),
			new Vector3(-19f / 128f, 0f, -0.125f),
			new Vector3(-9f / 64f, 0.2851563f, -0.1210938f),
			new Vector3(-17f / 128f, 0f, -17f / 128f),
			new Vector3(-0.125f, 0.2851563f, -0.125f),
			new Vector3(-17f / 128f, 0f, -19f / 128f),
			new Vector3(-0.125f, 0.2851563f, -5f / 32f),
			new Vector3(-19f / 128f, 0f, -5f / 32f),
			new Vector3(-9f / 64f, 0.2851563f, -0.1601563f),
			new Vector3(-0.1601563f, 0.2851563f, 0.1210938f),
			new Vector3(-5f / 32f, 0f, 0.125f),
			new Vector3(-0.1679688f, 0f, 17f / 128f),
			new Vector3(-0.1757813f, 0.2851563f, 0.125f),
			new Vector3(-0.1679688f, 0f, 19f / 128f),
			new Vector3(-0.1757813f, 0.2851563f, 5f / 32f),
			new Vector3(-5f / 32f, 0f, 5f / 32f),
			new Vector3(-0.1601563f, 0.2851563f, 0.1601563f),
			new Vector3(-19f / 128f, 0f, 5f / 32f),
			new Vector3(-9f / 64f, 0.2851563f, 0.1601563f),
			new Vector3(-17f / 128f, 0f, 19f / 128f),
			new Vector3(-0.125f, 0.2851563f, 5f / 32f),
			new Vector3(-17f / 128f, 0f, 17f / 128f),
			new Vector3(-0.125f, 0.2851563f, 0.125f),
			new Vector3(-19f / 128f, 0f, 0.125f),
			new Vector3(-9f / 64f, 0.2851563f, 0.1210938f)
		};
	}

	private int[] ChairTriangles(int MeshNo)
	{
		switch (MeshNo)
		{
		case 0:
			return new int[102]
			{
				1, 0, 2, 0, 3, 2, 1, 24, 23, 1,
				23, 0, 5, 7, 6, 5, 4, 7, 6, 7,
				32, 33, 6, 32, 20, 22, 21, 20, 20, 25,
				20, 25, 29, 20, 14, 22, 20, 29, 14, 26,
				28, 27, 26, 19, 28, 26, 30, 19, 30, 31,
				19, 3, 7, 4, 3, 0, 7, 23, 22, 0,
				0, 22, 14, 7, 31, 32, 7, 19, 31, 28,
				19, 29, 29, 19, 14, 14, 13, 10, 14, 15,
				13, 13, 13, 13, 19, 8, 18, 8, 11, 18,
				14, 19, 15, 15, 19, 18, 18, 11, 13, 18,
				13, 15
			};
		case 1:
			return new int[12]
			{
				0, 10, 8, 0, 8, 7, 10, 13, 11, 10,
				11, 8
			};
		default:
			return base.ModelTriangles(MeshNo);
		}
	}

	private Vector3[] ChairVertices()
	{
		return new Vector3[36]
		{
			new Vector3(5f / 32f, 19f / 64f, 0.1367188f),
			new Vector3(0.1679688f, 0f, 0.1367188f),
			new Vector3(0.1679688f, 0f, 0.1054688f),
			new Vector3(5f / 32f, 31f / 128f, 9f / 128f),
			new Vector3(5f / 32f, 31f / 128f, -9f / 128f),
			new Vector3(0.1679688f, 0f, -0.1054688f),
			new Vector3(0.1679688f, 0f, -0.1367188f),
			new Vector3(5f / 32f, 19f / 64f, -0.1367188f),
			new Vector3(-0.04296875f, 19f / 64f, -0.1367188f),
			new Vector3(-9f / 128f, 19f / 64f, 0f),
			new Vector3(-0.04296875f, 19f / 64f, 0.1367188f),
			new Vector3(-15f / 128f, 43f / 64f, -0.1367188f),
			new Vector3(-0.1445313f, 43f / 64f, 0f),
			new Vector3(-15f / 128f, 43f / 64f, 0.1367188f),
			new Vector3(-3f / 32f, 19f / 64f, 0.1367188f),
			new Vector3(-0.1445313f, 43f / 64f, 0.1367188f),
			new Vector3(-0.1679688f, 43f / 64f, 0f),
			new Vector3(-15f / 128f, 19f / 64f, 0f),
			new Vector3(-0.1445313f, 43f / 64f, -0.1367188f),
			new Vector3(-3f / 32f, 19f / 64f, -0.1367188f),
			new Vector3(-5f / 32f, 0f, 0.1367188f),
			new Vector3(-0.1054688f, 0f, 0.1367188f),
			new Vector3(-0.04296875f, 31f / 128f, 0.1367188f),
			new Vector3(13f / 128f, 31f / 128f, 0.1367188f),
			new Vector3(17f / 128f, 0f, 0.1367188f),
			new Vector3(-5f / 32f, 0f, 0.1054688f),
			new Vector3(-5f / 32f, 0f, -0.1367188f),
			new Vector3(-5f / 32f, 0f, -0.1054688f),
			new Vector3(-0.1054688f, 15f / 64f, -9f / 128f),
			new Vector3(-0.1054688f, 15f / 64f, 9f / 128f),
			new Vector3(-0.1054688f, 0f, -0.1367188f),
			new Vector3(-0.04296875f, 31f / 128f, -0.1367188f),
			new Vector3(13f / 128f, 31f / 128f, -0.1367188f),
			new Vector3(17f / 128f, 0f, -0.1367188f),
			new Vector3(-0.1054688f, 31f / 128f, 9f / 128f),
			new Vector3(-0.1054688f, 31f / 128f, -9f / 128f)
		};
	}

	private int[] NightStandTriangles()
	{
		return new int[198]
		{
			0, 2, 3, 4, 0, 3, 35, 39, 38, 34,
			35, 38, 39, 37, 36, 38, 39, 36, 28, 26,
			27, 29, 28, 27, 26, 25, 24, 27, 26, 24,
			19, 23, 22, 18, 19, 22, 23, 21, 20, 22,
			23, 20, 12, 10, 11, 13, 12, 11, 10, 9,
			1, 11, 10, 1, 3, 2, 0, 4, 3, 0,
			4, 0, 5, 6, 4, 5, 3, 4, 6, 7,
			3, 6, 2, 3, 7, 8, 2, 7, 0, 2,
			8, 5, 0, 8, 32, 33, 36, 37, 32, 36,
			14, 15, 12, 13, 14, 12, 30, 24, 25, 31,
			30, 25, 16, 17, 20, 21, 16, 20, 18, 17,
			16, 19, 18, 16, 30, 31, 28, 29, 30, 28,
			35, 39, 38, 34, 35, 38, 39, 37, 36, 38,
			39, 36, 28, 26, 27, 29, 28, 27, 26, 25,
			24, 27, 26, 24, 19, 23, 22, 18, 19, 22,
			23, 21, 20, 22, 23, 20, 12, 10, 11, 13,
			12, 11, 10, 9, 1, 11, 10, 1, 3, 2,
			0, 4, 3, 0, 4, 0, 5, 6, 4, 5,
			3, 4, 6, 7, 3, 6, 2, 3, 7, 8,
			2, 7, 0, 2, 8, 5, 0, 8
		};
	}

	private Vector3[] NightStandVertices()
	{
		return new Vector3[40]
		{
			new Vector3(15f / 128f, 0.1835938f, -15f / 128f),
			new Vector3(-3f / 32f, -0.00390625f, -0.07421875f),
			new Vector3(-15f / 128f, 0.1835938f, -15f / 128f),
			new Vector3(-15f / 128f, 0.1835938f, 0.1679688f),
			new Vector3(15f / 128f, 0.1835938f, 0.1679688f),
			new Vector3(15f / 128f, 0.1679688f, -15f / 128f),
			new Vector3(15f / 128f, 0.1679688f, 0.1679688f),
			new Vector3(-15f / 128f, 0.1679688f, 0.1679688f),
			new Vector3(-15f / 128f, 0.1679688f, -15f / 128f),
			new Vector3(-3f / 32f, 0.1679688f, -0.0625f),
			new Vector3(-3f / 32f, 0.1679688f, -11f / 128f),
			new Vector3(-3f / 32f, -0.00390625f, -11f / 128f),
			new Vector3(-9f / 128f, 0.1679688f, -11f / 128f),
			new Vector3(-0.08203125f, -0.00390625f, -11f / 128f),
			new Vector3(-0.08203125f, -0.00390625f, -0.07421875f),
			new Vector3(-9f / 128f, 0.1679688f, -0.0625f),
			new Vector3(9f / 128f, 0.1679688f, -0.0625f),
			new Vector3(0.08203125f, -0.00390625f, -0.07421875f),
			new Vector3(3f / 32f, -0.00390625f, -0.07421875f),
			new Vector3(3f / 32f, 0.1679688f, -0.0625f),
			new Vector3(0.08203125f, -0.00390625f, -11f / 128f),
			new Vector3(9f / 128f, 0.1679688f, -11f / 128f),
			new Vector3(3f / 32f, -0.00390625f, -11f / 128f),
			new Vector3(3f / 32f, 0.1679688f, -11f / 128f),
			new Vector3(3f / 32f, -0.00390625f, 0.125f),
			new Vector3(3f / 32f, 0.1679688f, 0.1132813f),
			new Vector3(3f / 32f, 0.1679688f, 0.1367188f),
			new Vector3(3f / 32f, -0.00390625f, 0.1367188f),
			new Vector3(9f / 128f, 0.1679688f, 0.1367188f),
			new Vector3(0.08203125f, -0.00390625f, 0.1367188f),
			new Vector3(0.08203125f, -0.00390625f, 0.125f),
			new Vector3(9f / 128f, 0.1679688f, 0.1132813f),
			new Vector3(-9f / 128f, 0.1679688f, 0.1132813f),
			new Vector3(-0.08203125f, -0.00390625f, 0.125f),
			new Vector3(-3f / 32f, -0.00390625f, 0.125f),
			new Vector3(-3f / 32f, 0.1679688f, 0.1132813f),
			new Vector3(-0.08203125f, -0.00390625f, 0.1367188f),
			new Vector3(-9f / 128f, 0.1679688f, 0.1367188f),
			new Vector3(-3f / 32f, -0.00390625f, 0.1367188f),
			new Vector3(-3f / 32f, 0.1679688f, 0.1367188f)
		};
	}

	private int[] BedTriangles(int MeshNo)
	{
		if (MeshNo == 0)
		{
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
			};
		}
		return base.ModelTriangles(MeshNo);
	}

	private Vector3[] BedVertices()
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
