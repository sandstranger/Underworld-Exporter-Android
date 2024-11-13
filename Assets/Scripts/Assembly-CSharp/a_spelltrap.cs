public class a_spelltrap : trap_base
{
	public override void ExecuteTrap(object_base src, int triggerX, int triggerY, int State)
	{
		string rES = UWEBase._RES;
		if (rES != null && rES == "UW2")
		{
			int spellIndex = GetSpellIndex();
			if (spellIndex == 212 || spellIndex == 213)
			{
				UWCharacter.Instance.PlayerMagic.CastEnchantment(base.gameObject, null, GetSpellIndex(), 1, 0);
				return;
			}
		}
		UWCharacter.Instance.PlayerMagic.CastEnchantment(base.gameObject, null, GetSpellIndex(), 2, 0);
	}

	public int GetSpellIndex()
	{
		return ((base.quality & 0xF) << 4) | (base.owner & 0xF);
	}
}
