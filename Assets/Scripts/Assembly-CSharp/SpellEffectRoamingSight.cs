using UnityEngine;

public class SpellEffectRoamingSight : SpellEffect
{
	public int OldMana;

	public Vector3 OldPosition;

	public override void ApplyEffect()
	{
		UWCharacter.Instance.isRoaming = true;
		OldMana = UWCharacter.Instance.PlayerMagic.CurMana;
		UWCharacter.Instance.PlayerMagic.CurMana = 0;
		OldPosition = UWCharacter.Instance.transform.position;
		UWCharacter.Instance.transform.position = new Vector3(OldPosition.x, 5.5f, OldPosition.z);
		base.ApplyEffect();
	}

	public override void CancelEffect()
	{
		UWCharacter.Instance.isRoaming = false;
		UWCharacter.Instance.transform.position = OldPosition;
		UWCharacter.Instance.PlayerMagic.CurMana = OldMana;
		base.CancelEffect();
	}
}
