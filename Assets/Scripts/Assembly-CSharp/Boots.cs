public class Boots : Armour
{
	protected override void Start()
	{
		base.Start();
		if (base.item_id == 47 && UWEBase._RES == "UW1")
		{
			base.link = 0;
		}
	}

	public override int GetActualSpellIndex()
	{
		if (base.item_id == 47 && UWEBase._RES == "UW1")
		{
			return 278;
		}
		return base.GetActualSpellIndex();
	}
}
