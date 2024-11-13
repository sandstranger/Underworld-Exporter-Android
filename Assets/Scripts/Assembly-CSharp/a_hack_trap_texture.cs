using UnityEngine;

public class a_hack_trap_texture : a_hack_trap
{
	public override void ExecuteTrap(object_base src, int triggerX, int triggerY, int State)
	{
		if (base.link == 0)
		{
			return;
		}
		GameObject gameObjectAt = ObjectLoader.getGameObjectAt(base.link);
		if (gameObjectAt != null)
		{
			TMAP component = gameObjectAt.GetComponent<TMAP>();
			if (component != null)
			{
				component.owner = base.owner;
				component.TextureIndex = UWEBase.CurrentTileMap().texture_map[base.owner];
				TMAP.CreateTMAP(gameObjectAt, component.TextureIndex);
			}
			else
			{
				Debug.Log("no tmap found on " + gameObjectAt.name);
			}
		}
	}
}
