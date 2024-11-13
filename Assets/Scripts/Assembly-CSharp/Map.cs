using UnityEngine;

public class Map : object_base
{
	public override bool use()
	{
		if (UWEBase.CurrentObjectInHand == null)
		{
			return OpenMap(GameWorldController.instance.LevelNo);
		}
		return ActivateByObject(UWEBase.CurrentObjectInHand);
	}

	public bool OpenMap(int levelno)
	{
		Time.timeScale = 0f;
		WindowDetect.InMap = true;
		UWHUD.instance.RefreshPanels(4);
		UWCharacter.Instance.playerMotor.jumping.enabled = false;
		MapInteraction.UpdateMap(levelno);
		if (UWEBase._RES != "UW2" && MusicController.instance != null)
		{
			MusicController.instance.InMap = true;
		}
		UWHUD.instance.MessageScroll.Clear();
		UWHUD.instance.ContextMenu.text = "";
		return true;
	}

	public override bool LookAt()
	{
		base.isquant = 0;
		if (objInt().PickedUp)
		{
			UWHUD.instance.MessageScroll.Add(StringController.instance.GetFormattedObjectNameUW(objInt()) + "\n" + StringController.instance.GetString(1, 151));
		}
		else
		{
			UWHUD.instance.MessageScroll.Add(StringController.instance.GetFormattedObjectNameUW(objInt()));
		}
		return true;
	}

	public override float GetWeight()
	{
		return (float)GameWorldController.instance.commonObject.properties[base.item_id].mass * 0.1f;
	}
}
