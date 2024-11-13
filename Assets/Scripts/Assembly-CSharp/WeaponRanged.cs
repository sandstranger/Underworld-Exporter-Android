public class WeaponRanged : Weapon
{
	public int AmmoType()
	{
		return GameWorldController.instance.objDat.rangedStats[base.item_id - 24].ammo;
	}

	public int Damage()
	{
		return GameWorldController.instance.objDat.rangedStats[base.item_id - 24].damage;
	}
}
