using UnityEngine;

public class a_damage_trap : trap_base
{
	public override void ExecuteTrap(object_base src, int triggerX, int triggerY, int State)
	{
		if (base.owner == 0)
		{
			if (Random.Range(0, 11) >= 7)
			{
				UWCharacter.Instance.CurVIT = UWCharacter.Instance.CurVIT - base.quality;
			}
		}
		else if (!UWCharacter.Instance.isPoisonResistant() && UWCharacter.Instance.play_poison == 0)
		{
			UWCharacter.Instance.play_poison = (short)Random.Range(1, 6);
			if (UWCharacter.Instance.poison_timer == 0f)
			{
				UWCharacter.Instance.poison_timer = 30f;
			}
		}
	}
}
