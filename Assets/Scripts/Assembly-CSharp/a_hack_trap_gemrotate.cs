using UnityEngine;

public class a_hack_trap_gemrotate : a_hack_trap
{
	public static LargeBlackrockGem gem;

	protected override void Start()
	{
		base.Start();
		if (gem == null)
		{
			gem = Object.FindObjectOfType<LargeBlackrockGem>();
		}
		UpdateGemFace();
	}

	public override void ExecuteTrap(object_base src, int triggerX, int triggerY, int State)
	{
		Quest.instance.variables[6]++;
		if (Quest.instance.variables[6] > a_hack_trap_teleport.NoOfWorlds)
		{
			Quest.instance.variables[6] = 0;
		}
		UpdateGemFace();
	}

	private void UpdateGemFace()
	{
		if (!(gem != null) || !(gem.GetComponent<MeshRenderer>() != null))
		{
			return;
		}
		for (int i = 0; i <= 7; i++)
		{
			if (i == Quest.instance.variables[6])
			{
				gem.GetComponent<MeshRenderer>().materials[i].SetColor("_Color", Color.white);
			}
			else
			{
				gem.GetComponent<MeshRenderer>().materials[i].SetColor("_Color", Color.blue);
			}
		}
	}

	public override bool WillFireRepeatedly()
	{
		return true;
	}
}
