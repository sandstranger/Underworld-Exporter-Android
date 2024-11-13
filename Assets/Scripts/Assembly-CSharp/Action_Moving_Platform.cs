using System.Collections;
using UnityEngine;

public class Action_Moving_Platform : MonoBehaviour
{
	public int TileX;

	public int TileY;

	public int TargetFloorHeight;

	public int TargetCeilingHeight;

	public int Flag;

	private GameObject level;

	public static TileMap tm;

	public void PerformAction()
	{
		level = GameObject.Find("level");
		int num = 32 - tm.GetCeilingHeight(TileX, TileY);
		int floorHeight = tm.GetFloorHeight(TileX, TileY);
		int num2 = TargetFloorHeight - floorHeight;
		int num3 = 32 - TargetCeilingHeight - num;
		GameObject gameObject = FindTile(TileX, TileY, 1);
		GameObject gameObject2 = FindTile(TileX, TileY, 2);
		switch (Flag)
		{
		case 1:
			Debug.Log("Displacement is " + num2);
			StartCoroutine(MoveTile(gameObject.transform, new Vector3(0f, 0.15f * (float)num2, 0f), 0.5f));
			tm.SetFloorHeight(TileX, TileY, (short)TargetFloorHeight);
			break;
		case 2:
			Debug.Log("Displacement is " + num3);
			StartCoroutine(MoveTile(gameObject2.transform, new Vector3(0f, 0.15f * (float)num3, 0f), 0.5f));
			tm.SetCeilingHeight(TileX, TileY, (short)TargetCeilingHeight);
			break;
		case 3:
			Debug.Log("Displacement is " + num2);
			StartCoroutine(MoveTile(gameObject.transform, new Vector3(0f, 0.15f * (float)num2, 0f), 0.5f));
			Debug.Log("Displacement is " + num3);
			StartCoroutine(MoveTile(gameObject2.transform, new Vector3(0f, 0.15f * (float)num3, 0f), 0.5f));
			tm.SetFloorHeight(TileX, TileY, (short)TargetCeilingHeight);
			break;
		}
		Debug.Log(base.name + " Action Moving Platform");
	}

	private IEnumerator MoveTile(Transform platform, Vector3 dist, float traveltime)
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

	public GameObject FindTile(int x, int y, int surface)
	{
		string tileName = GetTileName(x, y, surface);
		return level.transform.Find(tileName).gameObject;
	}

	public string GetTileName(int x, int y, int surface)
	{
		string text = x.ToString("D2");
		string text2 = y.ToString("D2");
		switch (surface)
		{
		case 3:
			return "Wall_" + text + "_" + text2;
		case 2:
			return "Ceiling_" + text + "_" + text2;
		default:
			return "Tile_" + text + "_" + text2;
		}
	}
}
