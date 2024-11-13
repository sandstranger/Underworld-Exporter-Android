public class Coin : object_base
{
	protected override void Start()
	{
		base.Start();
	}

	public override void MergeEvent()
	{
		base.MergeEvent();
	}

	public override void Split()
	{
		base.Split();
	}

	public override int AliasItemId()
	{
		if (base.item_id == 160)
		{
			return 161;
		}
		if (base.item_id == 161)
		{
			return 160;
		}
		return base.AliasItemId();
	}

	private void ChangeCoinType()
	{
		switch (base.item_id)
		{
		case 161:
			if (objInt().GetQty() > 1)
			{
				ChangeType(160);
			}
			break;
		case 160:
			if (objInt().GetQty() == 1)
			{
				ChangeType(161);
			}
			break;
		}
	}
}
