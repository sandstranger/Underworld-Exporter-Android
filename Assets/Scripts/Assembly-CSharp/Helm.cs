public class Helm : Armour
{
	public override int GetActualSpellIndex()
	{
		return base.link - 512;
	}
}
