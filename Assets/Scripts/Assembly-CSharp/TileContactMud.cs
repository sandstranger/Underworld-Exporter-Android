using UnityEngine;

public class TileContactMud : TileContactWater
{
	protected override void TileContactEvent(ObjectInteraction obj, Vector3 position)
	{
		int item_id = obj.item_id;
		if (item_id == 230 && obj.link == 579)
		{
			BasiliskOilOnMud(obj);
		}
		base.TileContactEvent(obj, position);
	}

	private void BasiliskOilOnMud(ObjectInteraction obj)
	{
		if (Quest.instance.x_clocks[3] < 2)
		{
			Quest.instance.x_clocks[3] = 2;
			UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, 332));
		}
	}
}
