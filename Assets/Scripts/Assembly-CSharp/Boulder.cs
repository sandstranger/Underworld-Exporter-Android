using System.Linq;
using UnityEngine;

public class Boulder : Model3D
{
	private const int LargeBoulder1 = 339;

	private const int LargeBoulder2 = 340;

	private const int MediumBoulder = 341;

	private const int SmallBoulder = 342;

	public override bool ActivateByObject(ObjectInteraction ObjectUsed)
	{
		if (ObjectUsed.item_id == 296)
		{
			switch (base.item_id)
			{
			case 339:
			case 340:
			{
				for (int j = 0; j < 2; j++)
				{
					ObjectLoaderInfo currObj2 = ObjectLoader.newObject(341, 0, 0, 0, 256);
					ObjectInteraction.CreateNewObject(UWEBase.CurrentTileMap(), currObj2, UWEBase.CurrentObjectList().objInfo, GameWorldController.instance.DynamicObjectMarker().gameObject, base.transform.position + new Vector3(Random.Range(-0.6f, 0.6f), 0f, Random.Range(-0.6f, 0.6f)));
					objInt().objectloaderinfo.InUseFlag = 0;
					Object.Destroy(base.gameObject);
				}
				break;
			}
			case 341:
			{
				for (int i = 0; i < 2; i++)
				{
					ObjectLoaderInfo currObj = ObjectLoader.newObject(342, 0, 0, 0, 256);
					ObjectInteraction.CreateNewObject(UWEBase.CurrentTileMap(), currObj, UWEBase.CurrentObjectList().objInfo, GameWorldController.instance.DynamicObjectMarker().gameObject, base.transform.position + new Vector3(Random.Range(-0.6f, 0.6f), 0f, Random.Range(-0.6f, 0.6f)));
					objInt().objectloaderinfo.InUseFlag = 0;
					Object.Destroy(base.gameObject);
				}
				break;
			}
			case 342:
			{
				ObjectLoaderInfo objectLoaderInfo = ObjectLoader.newObject(16, 0, 0, 1, 256);
				objectLoaderInfo.is_quant = 1;
				objectLoaderInfo.link = Random.Range(1, 3);
				ObjectInteraction.CreateNewObject(UWEBase.CurrentTileMap(), objectLoaderInfo, UWEBase.CurrentObjectList().objInfo, GameWorldController.instance.DynamicObjectMarker().gameObject, base.transform.position + new Vector3(Random.Range(-0.6f, 0.6f), 0f, Random.Range(-0.6f, 0.6f)));
				objInt().objectloaderinfo.InUseFlag = 0;
				Object.Destroy(base.gameObject);
				break;
			}
			}
			UWEBase.CurrentObjectInHand = null;
			return true;
		}
		return base.ActivateByObject(ObjectUsed);
	}

	public override string UseObjectOnVerb_World()
	{
		ObjectInteraction currentObjectInHand = UWEBase.CurrentObjectInHand;
		if (currentObjectInHand != null)
		{
			int num = currentObjectInHand.item_id;
			if (num == 296)
			{
				return "break with hammer";
			}
		}
		return base.UseObjectOnVerb_Inv();
	}

	public override int[] ModelTriangles(int meshNo)
	{
		switch (base.item_id)
		{
		case 339:
		case 340:
			return new int[258]
			{
				24, 25, 2, 33, 5, 4, 26, 33, 4, 4,
				5, 3, 2, 4, 3, 3, 18, 6, 7, 9,
				19, 19, 11, 10, 21, 12, 23, 11, 19, 9,
				27, 26, 14, 28, 27, 14, 29, 28, 14, 30,
				29, 14, 31, 30, 14, 28, 7, 27, 9, 7,
				28, 28, 29, 9, 30, 22, 21, 31, 22, 30,
				25, 24, 22, 22, 31, 25, 26, 25, 14, 4,
				25, 26, 5, 34, 27, 15, 5, 27, 7, 15,
				27, 1, 2, 0, 34, 5, 33, 6, 8, 17,
				0, 6, 17, 18, 5, 15, 2, 3, 0, 0,
				3, 6, 3, 5, 18, 34, 5, 33, 24, 25,
				2, 33, 5, 4, 26, 33, 4, 4, 5, 3,
				2, 4, 3, 3, 18, 6, 7, 9, 19, 19,
				11, 10, 21, 12, 23, 11, 19, 9, 27, 26,
				14, 28, 27, 14, 29, 28, 14, 30, 29, 14,
				31, 30, 14, 28, 7, 27, 9, 7, 28, 28,
				29, 9, 30, 22, 21, 31, 22, 30, 25, 24,
				22, 22, 31, 25, 26, 25, 14, 4, 25, 26,
				5, 34, 27, 15, 5, 27, 7, 15, 27, 34,
				33, 26, 34, 26, 27, 14, 25, 31, 25, 4,
				2, 2, 1, 24, 1, 0, 17, 1, 17, 8,
				15, 7, 18, 18, 7, 19, 18, 19, 10, 18,
				10, 6, 8, 6, 10, 1, 8, 24, 8, 10,
				24, 24, 10, 11, 12, 21, 22, 12, 22, 24,
				24, 23, 12, 24, 11, 23, 23, 11, 21, 21,
				11, 30, 11, 9, 30, 30, 9, 29
			}.Reverse().ToArray();
		case 341:
			return new int[249]
			{
				1, 22, 2, 13, 5, 4, 23, 13, 4, 4,
				5, 3, 4, 3, 29, 2, 4, 29, 3, 5,
				16, 6, 8, 15, 15, 10, 9, 18, 11, 20,
				16, 6, 15, 7, 15, 9, 10, 15, 8, 24,
				23, 12, 25, 24, 12, 26, 25, 12, 27, 26,
				12, 28, 27, 12, 25, 6, 24, 8, 6, 25,
				25, 26, 8, 27, 19, 18, 28, 19, 27, 22,
				1, 19, 19, 28, 22, 23, 22, 12, 4, 22,
				23, 5, 30, 24, 6, 5, 24, 15, 7, 14,
				0, 15, 14, 30, 5, 13, 5, 6, 16, 29,
				3, 0, 3, 16, 15, 0, 3, 15, 1, 22,
				2, 13, 5, 4, 23, 13, 4, 4, 5, 3,
				4, 3, 29, 2, 4, 29, 3, 5, 16, 6,
				8, 15, 15, 10, 9, 18, 11, 20, 16, 6,
				15, 7, 15, 9, 10, 15, 8, 24, 23, 12,
				25, 24, 12, 26, 25, 12, 27, 26, 12, 28,
				27, 12, 25, 6, 24, 8, 6, 25, 25, 26,
				8, 27, 19, 18, 28, 19, 27, 22, 1, 19,
				19, 28, 22, 23, 22, 12, 4, 22, 23, 5,
				30, 24, 6, 5, 24, 13, 23, 24, 30, 13,
				24, 8, 26, 27, 8, 27, 10, 12, 22, 28,
				10, 27, 18, 4, 2, 22, 2, 29, 0, 18,
				19, 11, 9, 10, 18, 9, 18, 20, 7, 9,
				20, 0, 14, 2, 2, 14, 7, 2, 7, 20,
				2, 20, 11, 2, 11, 1, 1, 11, 19
			}.Reverse().ToArray();
		default:
			return new int[177]
			{
				6, 9, 8, 11, 12, 8, 1, 2, 0, 2,
				4, 0, 9, 12, 11, 0, 8, 1, 1, 3,
				2, 15, 14, 7, 16, 15, 7, 17, 16, 7,
				18, 17, 7, 19, 18, 7, 16, 4, 15, 5,
				4, 16, 16, 17, 5, 18, 10, 9, 19, 10,
				18, 13, 1, 10, 10, 19, 13, 7, 13, 19,
				14, 13, 7, 3, 13, 14, 14, 2, 3, 4,
				2, 15, 12, 1, 8, 12, 10, 1, 9, 10,
				12, 8, 9, 11, 6, 9, 8, 11, 12, 8,
				1, 2, 0, 2, 4, 0, 9, 12, 11, 0,
				8, 1, 1, 13, 3, 15, 14, 7, 16, 15,
				7, 17, 16, 7, 18, 17, 7, 19, 18, 7,
				16, 4, 15, 5, 4, 16, 16, 17, 5, 18,
				10, 9, 19, 10, 18, 13, 1, 10, 10, 19,
				13, 7, 13, 19, 14, 13, 7, 3, 13, 14,
				14, 2, 3, 4, 2, 15, 2, 14, 15, 9,
				6, 18, 6, 5, 17, 6, 17, 18, 5, 0,
				4, 8, 0, 5, 5, 6, 8
			}.Reverse().ToArray();
		}
	}

	public override Vector3[] ModelVertices()
	{
		switch (base.item_id)
		{
		case 339:
		case 340:
			return new Vector3[36]
			{
				new Vector3(-0.06640625f, 39f / 128f, -0.08203125f),
				new Vector3(-9f / 64f, 41f / 128f, 0f),
				new Vector3(-0.2070313f, 0.2773438f, 1f / 64f),
				new Vector3(-0.1054688f, 21f / 64f, -9f / 64f),
				new Vector3(-0.2617188f, 0.2070313f, -1f / 64f),
				new Vector3(-19f / 128f, 0.2539063f, -11f / 64f),
				new Vector3(1f / 64f, 23f / 64f, -0.1132813f),
				new Vector3(0.07421875f, 0.2382813f, -0.2773438f),
				new Vector3(-1f / 64f, 0.4101563f, -3f / 128f),
				new Vector3(0.2070313f, 19f / 128f, -19f / 128f),
				new Vector3(0.125f, 0.3710938f, 1f / 32f),
				new Vector3(0.2460938f, 0.2617188f, 0.125f),
				new Vector3(3f / 128f, 0.2460938f, 21f / 64f),
				new Vector3(-0.125f, 43f / 128f, 0.2070313f),
				new Vector3(-0.09765625f, 0.05859375f, 19f / 128f),
				new Vector3(0.02734375f, 31f / 128f, -0.2539063f),
				new Vector3(0.1445313f, 9f / 32f, -0.1210938f),
				new Vector3(-0.0625f, 0.3125f, -5f / 64f),
				new Vector3(1f / 32f, 39f / 128f, -0.1875f),
				new Vector3(5f / 32f, 0.2773438f, -0.1132813f),
				new Vector3(0.06640625f, 0.2695313f, 0.125f),
				new Vector3(0.2460938f, 23f / 128f, 0.2460938f),
				new Vector3(0.125f, 23f / 128f, 0.3710938f),
				new Vector3(0f, 39f / 128f, 37f / 128f),
				new Vector3(-17f / 128f, 43f / 128f, 41f / 128f),
				new Vector3(-0.2460938f, 9f / 64f, 0.125f),
				new Vector3(-0.1132813f, 0.05859375f, 0.06640625f),
				new Vector3(-0.05078125f, 0.05859375f, -1f / 64f),
				new Vector3(0.05859375f, 0.05859375f, -3f / 128f),
				new Vector3(9f / 64f, 0.05859375f, 0.05078125f),
				new Vector3(0.125f, 0.05859375f, 21f / 128f),
				new Vector3(0.04296875f, 0.05859375f, 0.2226563f),
				new Vector3(0.04296875f, 0.3007813f, -23f / 128f),
				new Vector3(-9f / 64f, 0.2070313f, -0.1132813f),
				new Vector3(-0.125f, 13f / 64f, -17f / 128f),
				new Vector3(0.06640625f, 0.2539063f, -0.2539063f)
			};
		case 341:
			return new Vector3[31]
			{
				new Vector3(-0.04296875f, 13f / 64f, -7f / 128f),
				new Vector3(-11f / 128f, 0.2226563f, 0.2148438f),
				new Vector3(-0.1367188f, 0.1875f, 0.01171875f),
				new Vector3(-9f / 128f, 7f / 32f, -3f / 32f),
				new Vector3(-0.1757813f, 0.1367188f, -0.01171875f),
				new Vector3(-0.09765625f, 0.1679688f, -0.1132813f),
				new Vector3(0.05078125f, 0.1601563f, -0.1875f),
				new Vector3(-0.01171875f, 35f / 128f, -1f / 64f),
				new Vector3(0.1367188f, 0.09765625f, -0.09765625f),
				new Vector3(0.08203125f, 0.2460938f, 3f / 128f),
				new Vector3(21f / 128f, 0.1757813f, 0.08203125f),
				new Vector3(1f / 64f, 21f / 128f, 7f / 32f),
				new Vector3(-0.06640625f, 5f / 128f, 0.09765625f),
				new Vector3(-3f / 32f, 0.1445313f, -11f / 128f),
				new Vector3(-3f / 128f, 0.25f, -0.02734375f),
				new Vector3(0.1054688f, 0.1875f, -5f / 64f),
				new Vector3(3f / 128f, 13f / 64f, -0.125f),
				new Vector3(0.04296875f, 23f / 128f, 0.08203125f),
				new Vector3(21f / 128f, 0.1210938f, 21f / 128f),
				new Vector3(0.08203125f, 0.1210938f, 0.2460938f),
				new Vector3(0f, 13f / 64f, 0.1914063f),
				new Vector3(-0.05078125f, 0.2226563f, 9f / 64f),
				new Vector3(-21f / 128f, 3f / 32f, 0.08203125f),
				new Vector3(-5f / 64f, 5f / 128f, 0.04296875f),
				new Vector3(-1f / 32f, 5f / 128f, -0.01171875f),
				new Vector3(5f / 128f, 5f / 128f, -1f / 64f),
				new Vector3(3f / 32f, 5f / 128f, 1f / 32f),
				new Vector3(0.08203125f, 5f / 128f, 7f / 64f),
				new Vector3(0.02734375f, 5f / 128f, 19f / 128f),
				new Vector3(-0.09765625f, 0.2070313f, -3f / 64f),
				new Vector3(-0.08203125f, 0.1367188f, -0.08984375f)
			};
		default:
			return new Vector3[20]
			{
				new Vector3(1f / 128f, 5f / 64f, -3f / 64f),
				new Vector3(-1f / 32f, 11f / 128f, 0.08203125f),
				new Vector3(-0.03515625f, 0.0625f, -0.04296875f),
				new Vector3(-0.06640625f, 0.05078125f, -0.00390625f),
				new Vector3(1f / 64f, 0.05859375f, -9f / 128f),
				new Vector3(0.05078125f, 0.03515625f, -0.03515625f),
				new Vector3(0.0625f, 0.06640625f, 1f / 32f),
				new Vector3(-3f / 128f, 0.01171875f, 0.03515625f),
				new Vector3(-1f / 64f, 11f / 128f, 7f / 128f),
				new Vector3(0.0625f, 3f / 64f, 0.0625f),
				new Vector3(1f / 32f, 3f / 64f, 3f / 32f),
				new Vector3(0f, 5f / 64f, 0.07421875f),
				new Vector3(0.00390625f, 0.0625f, 0.08203125f),
				new Vector3(-0.0625f, 0.03515625f, 1f / 32f),
				new Vector3(-0.02734375f, 0.01171875f, 1f / 64f),
				new Vector3(-0.01171875f, 0.01171875f, -0.00390625f),
				new Vector3(0.01171875f, 0.01171875f, -0.00390625f),
				new Vector3(0.03515625f, 0.01171875f, 0.01171875f),
				new Vector3(1f / 32f, 0.01171875f, 5f / 128f),
				new Vector3(1f / 128f, 0.01171875f, 7f / 128f)
			};
		}
	}

	public override Color ModelColour(int meshNo)
	{
		return Color.gray;
	}
}
