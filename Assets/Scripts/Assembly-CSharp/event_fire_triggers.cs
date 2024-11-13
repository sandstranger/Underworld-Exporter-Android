public class event_fire_triggers : event_base
{
	public override void ExecuteEvent()
	{
		base.ExecuteEvent();
		int num = RawData[3];
		int num2 = RawData[4];
		ObjectLoaderInfo[] objInfo = UWClass.CurrentObjectList().objInfo;
		for (int i = 256; i <= objInfo.GetUpperBound(0); i++)
		{
			if (objInfo[i].ObjectTileX == num && objInfo[i].ObjectTileY == num2 && objInfo[i].instance != null && ObjectLoader.isTrigger(objInfo[i]))
			{
				objInfo[i].instance.GetComponent<trigger_base>().Activate(UWCharacter.Instance.gameObject);
			}
		}
	}

	public override string EventName()
	{
		return "Fire_Trigger";
	}

	public override string summary()
	{
		return base.summary() + "\n\t\tTileX=" + (int)RawData[3] + ",TileY=" + (int)RawData[4];
	}
}
