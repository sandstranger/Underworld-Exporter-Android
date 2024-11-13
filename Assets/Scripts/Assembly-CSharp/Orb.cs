public class Orb : object_base
{
	protected override void Start()
	{
		base.Start();
		if (UWEBase._RES == "UW1" && GameWorldController.instance.LevelNo == 6 && !Quest.instance.isOrbDestroyed)
		{
			UWCharacter.Instance.PlayerMagic.CurMana = 0;
			UWCharacter.Instance.PlayerMagic.MaxMana = 0;
		}
	}

	public override bool LookAt()
	{
		if (base.link != 0)
		{
			ObjectInteraction objectIntAt = ObjectLoader.getObjectIntAt(base.link);
			if (objectIntAt != null)
			{
				int itemType = objectIntAt.GetItemType();
				if (itemType == 57)
				{
					return objectIntAt.GetComponent<trigger_base>().Activate(base.gameObject);
				}
			}
		}
		return base.LookAt();
	}

	public override bool use()
	{
		if (UWEBase.CurrentObjectInHand == null)
		{
			if (base.link != 0)
			{
				ObjectInteraction objectIntAt = ObjectLoader.getObjectIntAt(base.link);
				if (objectIntAt != null)
				{
					int itemType = objectIntAt.GetItemType();
					if (itemType == 56)
					{
						return objectIntAt.GetComponent<trigger_base>().Activate(base.gameObject);
					}
				}
			}
			return LookAt();
		}
		return ActivateByObject(UWEBase.CurrentObjectInHand);
	}

	public override bool ActivateByObject(ObjectInteraction ObjectUsed)
	{
		if (ObjectUsed != null)
		{
			int itemType = ObjectUsed.GetItemType();
			if (itemType == 110)
			{
				if (UWEBase._RES == "UW1" && GameWorldController.instance.LevelNo == 6)
				{
					UWEBase.CurrentObjectInHand = null;
					OrbRock.DestroyOrb(objInt());
					return true;
				}
				return base.ActivateByObject(ObjectUsed);
			}
		}
		return base.ActivateByObject(ObjectUsed);
	}
}
