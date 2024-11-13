public class SpellProp_Tym : SpellProp
{
	public override void onHitPlayer()
	{
		UWCharacter.Instance.ParalyzeTimer = 15f;
		UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, 355));
	}
}
