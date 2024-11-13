using UnityEngine;

public class SpellEffectStealth : SpellEffect
{
	public int StealthLevel;

	public override void ApplyEffect()
	{
		base.ApplyEffect();
		UWCharacter.Instance.StealthLevel = Mathf.Max(StealthLevel, UWCharacter.Instance.StealthLevel);
	}

	public override void CancelEffect()
	{
		base.CancelEffect();
		UWCharacter.Instance.StealthLevel = 0;
	}

	private void Update()
	{
		if (Active)
		{
			UWCharacter.Instance.StealthLevel = Mathf.Max(StealthLevel, UWCharacter.Instance.StealthLevel);
		}
	}
}
