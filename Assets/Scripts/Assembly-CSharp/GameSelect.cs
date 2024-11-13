using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class GameSelect : GuiBase
{
	public string RES;

	public bool Game_Found;

	public Text PathStatus;

	public string exe;

	public override void Start()
	{
		base.Start();
		CheckPath();
	}

	private void CheckPath()
	{
		string text = "";
		switch (RES)
		{
		case "UW0":
			text = GameWorldController.instance.path_uw0;
			break;
		case "UW1":
			text = GameWorldController.instance.path_uw1;
			break;
		case "UW2":
			text = GameWorldController.instance.path_uw2;
			break;
		case "SHOCK":
			text = GameWorldController.instance.path_shock;
			break;
		case "TNOVA":
			text = GameWorldController.instance.path_tnova;
			break;
		}
		Game_Found = Directory.Exists(text);
		if (Game_Found)
		{
			PathStatus.text = RES + " found at " + text;
		}
		else
		{
			PathStatus.text = RES + " not found at " + text;
		}
	}

	public void OnClick()
	{
		if (!Game_Found)
		{
			return;
		}
		string rES = RES;
		if (rES != null && rES == "SHOCK")
		{
			GameObject gameObject = GameObject.Find("SSLevelSelect");
			if (gameObject != null)
			{
				Dropdown component = gameObject.GetComponent<Dropdown>();
				if (component != null)
				{
					GameWorldController.instance.startLevel = (short)component.value;
				}
			}
		}
		GameWorldController.instance.Begin(RES);
	}

	public void onHoverEnter()
	{
		if (Game_Found)
		{
			RawImage component = GetComponent<RawImage>();
			if (component != null)
			{
				component.color = Color.white;
			}
		}
	}

	public void onHoverExit()
	{
		if (Game_Found)
		{
			RawImage component = GetComponent<RawImage>();
			if (component != null)
			{
				component.color = Color.grey;
			}
		}
	}
}
