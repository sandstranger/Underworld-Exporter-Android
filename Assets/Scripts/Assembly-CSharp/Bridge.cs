using UnityEngine;

public class Bridge : map_object
{
	protected override void Start()
	{
		if (base.invis == 0)
		{
			TileMapRenderer.RenderBridge(GameWorldController.instance.SceneryModel, UWEBase.CurrentTileMap(), UWEBase.CurrentObjectList(), objInt().objectloaderinfo.index);
		}
		base.Start();
		BoxCollider component = GetComponent<BoxCollider>();
		if (component != null)
		{
			component.center = new Vector3(0.08f, 0.006f, 0.08f);
			component.size = new Vector3(1.2f, 0.18f, 1.2f);
		}
		if (base.ObjectTileX <= 63 && base.ObjectTileY <= 63)
		{
			int num = (base.enchantment << 3) | (base.flags & 0x3F);
			if (num < 2)
			{
				UWEBase.CurrentTileMap().Tiles[base.ObjectTileX, base.ObjectTileY].hasBridge = true;
			}
		}
	}

	public override bool LookAt()
	{
		if (base.invis == 0)
		{
			if (((base.enchantment << 3) | base.flags) < 2)
			{
				return base.LookAt();
			}
			int num = (base.enchantment << 3) | (base.flags & 0x3F);
			UWHUD.instance.MessageScroll.Add(StringController.instance.TextureDescription(510 - (num - 210)));
			return true;
		}
		return true;
	}

	public override bool use()
	{
		if (base.flags >= 2)
		{
			GameObject gameObjectAt = ObjectLoader.getGameObjectAt(base.link);
			if (gameObjectAt != null && gameObjectAt.GetComponent<trigger_base>() != null)
			{
				return gameObjectAt.GetComponent<trigger_base>().Activate(base.gameObject);
			}
		}
		return false;
	}

	public override string ContextMenuUsedDesc()
	{
		return "";
	}
}
