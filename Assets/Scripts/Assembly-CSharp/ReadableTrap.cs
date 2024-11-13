using UnityEngine;

public class ReadableTrap : object_base
{
	public override bool use()
	{
		if (UWEBase.CurrentObjectInHand == null)
		{
			UWHUD.instance.MessageScroll.Add("The book explodes in your face!");
			UWCharacter.Instance.ApplyDamage(Random.Range(1, 20));
			Quest.instance.QuestVariables[8] = 1;
			objInt().consumeObject();
			return true;
		}
		return ActivateByObject(UWEBase.CurrentObjectInHand);
	}
}
