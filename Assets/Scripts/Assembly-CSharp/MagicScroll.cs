using UnityEngine;

public class MagicScroll : enchantment_base
{
	public override bool use()
	{
		if (ConversationVM.InConversation)
		{
			return false;
		}
		if (UWEBase.CurrentObjectInHand == null)
		{
			UWCharacter.Instance.PlayerMagic.CastEnchantment(UWCharacter.Instance.gameObject, null, GetActualSpellIndex(), 1, 2);
			objInt().consumeObject();
			return true;
		}
		return ActivateByObject(UWEBase.CurrentObjectInHand);
	}

	public override string UseVerb()
	{
		return "cast";
	}

	public override bool ApplyAttack(short damage)
	{
		base.quality -= damage;
		if (base.quality <= 0)
		{
			ChangeType(213);
			base.gameObject.AddComponent<enchantment_base>();
			Object.Destroy(this);
		}
		return true;
	}
}
