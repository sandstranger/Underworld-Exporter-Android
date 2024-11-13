public class StorageCrystal : object_base
{
	public string DisplayName;

	protected override void Start()
	{
		base.Start();
		DisplayName = BuildCrystalName(base.quality);
	}

	public string BuildCrystalName(int quality)
	{
		int[] array = new int[4] { 16, 6, 24, 6 };
		int num = quality * 2;
		num = -(num % 26);
		if (array[0] + num < 0)
		{
			array[0] = array[0] + num + 26;
		}
		else
		{
			array[0] += num;
		}
		num = quality;
		num = -(num % 10);
		if (array[1] + num < 0)
		{
			array[1] = array[1] + num + 10;
		}
		else
		{
			array[1] += num;
		}
		num = quality;
		num %= 10;
		if (array[3] + num > 10)
		{
			array[3] = array[3] + num - 10;
		}
		else
		{
			array[3] += num;
		}
		return ((char)(array[0] + 65)).ToString() + array[1] + ((char)(array[2] + 65)).ToString() + array[3];
	}

	public override bool use()
	{
		if (objInt().PickedUp && UWEBase.CurrentObjectInHand == null)
		{
			UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, 352) + DisplayName);
			return true;
		}
		return base.use();
	}

	public override string ContextMenuDesc(int item_id)
	{
		return StringController.instance.GetString(1, 352) + DisplayName;
	}
}
