using System.Collections;
using UnityEngine;

public class a_do_trap_platform : a_hack_trap
{
	public override void ExecuteTrap(object_base src, int triggerX, int triggerY, int State)
	{
		GameObject gameObject = GameWorldController.FindTile(triggerX, triggerY, 1);
		if (UWEBase.CurrentTileMap().Tiles[triggerX, triggerY].floorHeight >= 18)
		{
			StartCoroutine(MoveTile(gameObject.transform, new Vector3(0f, -2.1000001f, 0f), 0.7f));
			base.flags = (short)State;
			UWEBase.CurrentTileMap().Tiles[triggerX, triggerY].floorHeight = 2;
		}
		else
		{
			StartCoroutine(MoveTile(gameObject.transform, new Vector3(0f, 0.3f, 0f), 0.1f));
			UWEBase.CurrentTileMap().Tiles[triggerX, triggerY].floorHeight += 2;
		}
		base.flags = (short)State;
	}

	protected IEnumerator MoveTile(Transform platform, Vector3 dist, float traveltime)
	{
		float rate = 1f / traveltime;
		float index = 0f;
		Vector3 StartPos = platform.position;
		Vector3 EndPos = StartPos + dist;
		while (index < 1f)
		{
			platform.position = Vector3.Lerp(StartPos, EndPos, index);
			index += rate * Time.deltaTime;
			yield return new WaitForSeconds(0.01f);
		}
		platform.position = EndPos;
	}

	public override void PostActivate(object_base src)
	{
		Debug.Log("Overridden PostActivate to test " + base.name);
		base.PostActivate(src);
	}
}
