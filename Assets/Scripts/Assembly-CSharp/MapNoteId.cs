using System;
using UnityEngine;

public class MapNoteId : MonoBehaviour
{
	public Guid guid;

	public void OnClick()
	{
		if (MapInteraction.InteractionMode != 1)
		{
			return;
		}
		for (int i = 0; i < GameWorldController.instance.AutoMaps[MapInteraction.MapNo].MapNotes.Count; i++)
		{
			if (guid == GameWorldController.instance.AutoMaps[MapInteraction.MapNo].MapNotes[i].guid)
			{
				GameWorldController.instance.AutoMaps[MapInteraction.MapNo].MapNotes.RemoveAt(i);
				UnityEngine.Object.Destroy(base.gameObject);
				break;
			}
		}
	}
}
