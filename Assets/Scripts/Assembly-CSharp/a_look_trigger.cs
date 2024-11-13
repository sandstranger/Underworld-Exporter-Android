using UnityEngine;

public class a_look_trigger : trigger_base
{
	public override bool Activate(GameObject src)
	{
		if (base.zpos > 0 && UWCharacter.Instance.PlayerSkills.GetSkill(12) + Random.Range(0, 16) <= base.zpos)
		{
			Debug.Log("unable to activate look trigger due to skills");
			return false;
		}
		return base.Activate(src);
	}
}
