using System.Linq;
using UnityEngine;

public class LargeBlackrockGem : Model3D
{
	private void Awake()
	{
		a_hack_trap_gemrotate.gem = this;
	}

	protected override void Start()
	{
		base.Start();
		for (int i = 0; i <= 7; i++)
		{
			if (i == Quest.instance.variables[6])
			{
				GetComponent<MeshRenderer>().materials[i].SetColor("_Color", Color.white);
			}
			else
			{
				GetComponent<MeshRenderer>().materials[i].SetColor("_Color", Color.blue);
			}
		}
	}

	public override bool use()
	{
		if (UWEBase.CurrentObjectInHand != null)
		{
			return ActivateByObject(UWEBase.CurrentObjectInHand);
		}
		return base.use();
	}

	public override bool ActivateByObject(ObjectInteraction ObjectUsed)
	{
		if (ObjectUsed != null && ObjectUsed.GetItemType() == 116)
		{
			if (ObjectUsed.owner == 1)
			{
				int num = ObjectUsed.item_id - 280;
				int num2 = 1 << num;
				Quest.instance.x_clocks[2]++;
				Quest.instance.QuestVariables[130] |= num2;
				UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, 338));
				UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, 338 + Quest.instance.x_clocks[2]));
				CameraShake.instance.ShakeEarthQuake((float)Quest.instance.x_clocks[2] * 0.2f);
				ObjectUsed.consumeObject();
			}
			else
			{
				UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, 347));
			}
			UWEBase.CurrentObjectInHand = null;
			return true;
		}
		return false;
	}

	public override Vector3[] ModelVertices()
	{
		return new Vector3[48]
		{
			new Vector3(-9f / 128f, 0.3554688f, 0f),
			new Vector3(-19f / 128f, 0.2773438f, -0.1679688f),
			new Vector3(-0.1289063f, 0f, -0.3164063f),
			new Vector3(-0.04296875f, 0.3554688f, -0.0625f),
			new Vector3(-0.3242188f, 0.08203125f, 0f),
			new Vector3(-7f / 32f, 0.2773438f, 0f),
			new Vector3(0.00390625f, 0.3554688f, -0.08984375f),
			new Vector3(0.00390625f, 0.08203125f, -21f / 64f),
			new Vector3(0.00390625f, 0.2773438f, -0.2382813f),
			new Vector3(-29f / 128f, 0.08203125f, -15f / 64f),
			new Vector3(-0.3085938f, 0f, -0.1445313f),
			new Vector3(-43f / 128f, 0f, -0.3476563f),
			new Vector3(15f / 64f, 0.08203125f, -15f / 64f),
			new Vector3(0.1367188f, 0f, -0.3164063f),
			new Vector3(11f / 32f, 0f, -0.3476563f),
			new Vector3(0.3164063f, 0f, -0.1445313f),
			new Vector3(0.3320313f, 0.08203125f, 0f),
			new Vector3(29f / 128f, 0.2773438f, 0f),
			new Vector3(5f / 32f, 0.2773438f, -0.1679688f),
			new Vector3(0.05078125f, 0.3554688f, -0.0625f),
			new Vector3(5f / 64f, 0.3554688f, 0f),
			new Vector3(0.00390625f, 0f, -47f / 64f),
			new Vector3(0.00390625f, 0.08203125f, 21f / 64f),
			new Vector3(0.1367188f, 0f, 0.3164063f),
			new Vector3(0.00390625f, 0f, 47f / 64f),
			new Vector3(-0.1289063f, 0f, 0.3164063f),
			new Vector3(11f / 32f, 0f, 0.3476563f),
			new Vector3(15f / 64f, 0.08203125f, 15f / 64f),
			new Vector3(-29f / 128f, 0.08203125f, 15f / 64f),
			new Vector3(-43f / 128f, 0f, 0.3476563f),
			new Vector3(0.3164063f, 0f, 0.1445313f),
			new Vector3(-0.3085938f, 0f, 0.1445313f),
			new Vector3(5f / 32f, 0.2773438f, 0.1679688f),
			new Vector3(-19f / 128f, 0.2773438f, 0.1679688f),
			new Vector3(0.00390625f, 0.2773438f, 0.2382813f),
			new Vector3(0.05078125f, 0.3554688f, 0.0625f),
			new Vector3(-0.04296875f, 0.3554688f, 0.0625f),
			new Vector3(0.00390625f, 0.3554688f, 0.08984375f),
			new Vector3(-41f / 64f, 0f, 0f),
			new Vector3(0.6367188f, 0f, 0f),
			new Vector3(-41f / 128f, 0f, -15f / 64f),
			new Vector3(7f / 64f, 0f, -13f / 32f),
			new Vector3(-0.4101563f, 0f, 13f / 128f),
			new Vector3(21f / 64f, 0f, 15f / 64f),
			new Vector3(-13f / 128f, 0f, 13f / 32f),
			new Vector3(-41f / 128f, 0f, 29f / 128f),
			new Vector3(21f / 64f, 0f, -29f / 128f),
			new Vector3(0.4179688f, 0f, 0.09765625f)
		};
	}

	public override int[] ModelTriangles(int meshNo)
	{
		switch (meshNo)
		{
		case 0:
			return new int[9] { 17, 27, 32, 17, 30, 27, 17, 16, 30 }.Reverse().ToArray();
		case 1:
			return new int[9] { 18, 16, 17, 18, 15, 16, 18, 12, 15 }.Reverse().ToArray();
		case 2:
			return new int[9] { 8, 12, 18, 8, 13, 12, 8, 7, 13 }.Reverse().ToArray();
		case 3:
			return new int[9] { 1, 7, 8, 1, 2, 7, 1, 9, 2 }.Reverse().ToArray();
		case 4:
			return new int[9] { 1, 10, 9, 1, 5, 10, 5, 4, 10 }.Reverse().ToArray();
		case 5:
			return new int[9] { 33, 4, 5, 33, 31, 4, 33, 28, 31 }.Reverse().ToArray();
		case 6:
			return new int[9] { 34, 28, 33, 34, 25, 28, 34, 22, 25 }.Reverse().ToArray();
		case 7:
			return new int[9] { 32, 22, 34, 32, 23, 22, 32, 27, 23 }.Reverse().ToArray();
		case 8:
			return new int[114]
			{
				24, 25, 22, 24, 22, 23, 26, 23, 27, 26,
				27, 30, 39, 30, 16, 39, 16, 15, 14, 15,
				12, 14, 12, 13, 13, 7, 21, 7, 2, 21,
				2, 9, 11, 9, 10, 11, 10, 4, 38, 4,
				31, 38, 31, 28, 29, 29, 28, 25, 0, 1,
				3, 0, 5, 1, 0, 33, 5, 0, 36, 33,
				33, 36, 37, 37, 34, 33, 37, 32, 34, 37,
				35, 32, 32, 35, 20, 32, 20, 17, 17, 20,
				18, 18, 19, 8, 8, 19, 6, 8, 6, 3,
				8, 3, 1, 18, 20, 19, 0, 3, 6, 0,
				6, 19, 0, 19, 20, 0, 20, 35, 0, 35,
				37, 0, 37, 36
			}.Reverse().ToArray();
		default:
			return base.ModelTriangles(meshNo);
		}
	}

	public override int NoOfMeshes()
	{
		return 9;
	}

	public override Color ModelColour(int meshNo)
	{
		if (meshNo == 8)
		{
			return Color.magenta;
		}
		return Color.blue;
	}
}
