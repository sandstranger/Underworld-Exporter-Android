using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class CutsAnimator : GuiBase
{
	public bool Reset;

	public int frameIndex;

	public Texture2D anim_base;

	public Texture2D black;

	public Texture2D red;

	public Texture2D orange;

	public RawImage TargetControl;

	public string SetAnimation = "";

	public string PrevAnimation = "";

	private bool mode;

	public bool looping;

	private CutsLoader cuts;

	public Material UI_UNLIT;

	public override void Start()
	{
		base.Start();
		TargetControl.texture = anim_base;
		mode = false;
	}

	public override void Update()
	{
		if (PrevAnimation != SetAnimation)
		{
			PrevAnimation = SetAnimation;
			PlayAnimFile(SetAnimation);
		}
	}

	private IEnumerator cutscenerunner()
	{
		while (!Reset)
		{
			if (mode)
			{
				if (frameIndex > cuts.ImageCache.GetUpperBound(0))
				{
					if (looping)
					{
						frameIndex = 0;
						TargetControl.texture = cuts.ImageCache[frameIndex++];
					}
					else
					{
						TargetControl.texture = black;
						PostAnimPlay();
					}
				}
				else
				{
					TargetControl.texture = cuts.ImageCache[frameIndex++];
				}
			}
			else
			{
				Reset = true;
			}
			yield return new WaitForSeconds(0.2f);
		}
	}

	private void PostAnimPlay()
	{
		switch (SetAnimation.ToLower())
		{
		case "uw2resurrecttransition":
		case "death_with_sapling":
		case "cs402.n01":
		{
			SetAnimation = "Anim_Base";
			PrevAnimation = "x";
			string rES = UWEBase._RES;
			if (rES != null && rES == "UW2")
			{
				UWCharacter.ResurrectPlayerUW2();
			}
			else
			{
				UWCharacter.ResurrectPlayerUW1();
			}
			break;
		}
		case "death":
		case "cs403.n01":
			SetAnimation = "cs403.n02";
			break;
		case "death_final":
		case "cs403.n02":
		{
			SetAnimation = "Anim_Base";
			GameWorldController.instance.AtMainMenu = true;
			foreach (Transform item in GameWorldController.instance.LevelModel.transform)
			{
				Object.Destroy(item.gameObject);
			}
			foreach (Transform item2 in GameWorldController.instance.DynamicObjectMarker())
			{
				Object.Destroy(item2.gameObject);
			}
			foreach (Transform item3 in GameWorldController.instance.SceneryModel.transform)
			{
				Object.Destroy(item3.gameObject);
			}
			for (int i = 0; i <= GameWorldController.instance.Tilemaps.GetUpperBound(0); i++)
			{
				GameWorldController.instance.Tilemaps[i] = null;
			}
			for (int j = 0; j <= GameWorldController.instance.objectList.GetUpperBound(0); j++)
			{
				GameWorldController.instance.objectList[j] = null;
			}
			UWHUD.instance.mainmenu.gameObject.SetActive(true);
			UWHUD.instance.mainmenu.MenuMode = 0;
			UWHUD.instance.mainmenu.OpScr.SetActive(true);
			UWHUD.instance.mainmenu.CharGen.SetActive(false);
			UWHUD.instance.mainmenu.ButtonClickMainMenu(4);
			GameWorldController.instance.LevelNo = -1;
			WindowDetectUW.SwitchFromMouseLook();
			break;
		}
		default:
			UWCharacter.Instance.playerCam.cullingMask = -33;
			SetAnimation = "Anim_Base";
			break;
		}
	}

	private void PlayAnimFile(string animName)
	{
		UWCharacter.Instance.playerCam.cullingMask = 0;
		Reset = true;
		frameIndex = 0;
		StopAllCoroutines();
		switch (SetAnimation.ToLower())
		{
		case "fadetored":
			TargetControl.material = UI_UNLIT;
			TargetControl.texture = red;
			return;
		case "uw2resurrecttransition":
			TargetControl.material = UI_UNLIT;
			TargetControl.texture = black;
			mode = false;
			looping = false;
			StartCoroutine(CutsceneWait(0.5f));
			return;
		case "fadetoblacksleep":
			TargetControl.material = UI_UNLIT;
			TargetControl.texture = black;
			return;
		case "cuts_blank":
		case "":
		case "anim_base":
			TargetControl.material = null;
			UWCharacter.Instance.playerCam.cullingMask = -33;
			TargetControl.texture = anim_base;
			return;
		case "splashlookingglass":
			TargetControl.material = UI_UNLIT;
			TargetControl.texture = GameWorldController.instance.bytloader.LoadImageAt(7);
			return;
		case "splashoriginea":
			TargetControl.material = UI_UNLIT;
			TargetControl.texture = GameWorldController.instance.bytloader.LoadImageAt(6);
			return;
		case "splashorigin":
		case "pres1_0000":
			TargetControl.material = UI_UNLIT;
			TargetControl.texture = GameWorldController.instance.bytloader.LoadImageAt(5);
			return;
		case "splashbluesky":
		case "pres2_0000":
			TargetControl.material = UI_UNLIT;
			TargetControl.texture = GameWorldController.instance.bytloader.LoadImageAt(6);
			return;
		case "splashorigindemo":
			TargetControl.material = UI_UNLIT;
			TargetControl.texture = GameWorldController.instance.bytloader.LoadImageAt(9);
			return;
		case "almricsitting":
			TargetControl.material = UI_UNLIT;
			cuts = new CutsLoader("cs000.n11");
			TargetControl.texture = cuts.ImageCache[0];
			return;
		case "death_with_sapling":
			TargetControl.material = UI_UNLIT;
			cuts = new CutsLoader("cs402.n01");
			mode = true;
			Reset = false;
			TargetControl.texture = cuts.ImageCache[0];
			looping = false;
			StartCoroutine(cutscenerunner());
			return;
		case "death":
			TargetControl.material = UI_UNLIT;
			cuts = new CutsLoader("cs403.n01");
			mode = true;
			Reset = false;
			looping = false;
			TargetControl.texture = cuts.ImageCache[0];
			StartCoroutine(cutscenerunner());
			return;
		case "death_final":
			TargetControl.material = UI_UNLIT;
			cuts = new CutsLoader("cs403.n02");
			TargetControl.texture = cuts.ImageCache[0];
			looping = false;
			mode = true;
			Reset = false;
			StartCoroutine(cutscenerunner());
			return;
		case "ChasmMap":
			TargetControl.material = UI_UNLIT;
			cuts = new CutsLoader("cs410.n01");
			TargetControl.texture = cuts.ImageCache[0];
			return;
		case "Anvil":
			cuts = new CutsLoader("cs404.n01");
			TargetControl.texture = cuts.ImageCache[0];
			StartCoroutine(cutscenerunner());
			return;
		}
		if (SetAnimation.Substring(0, 7) == "Volcano")
		{
			cuts = new CutsLoader("cs400.n01");
			int num = int.Parse(SetAnimation.Substring(SetAnimation.Length - 1, 1));
			TargetControl.texture = cuts.ImageCache[num];
			TargetControl.material = UI_UNLIT;
		}
		else if (SetAnimation.Substring(0, 5) == "Grave")
		{
			TargetControl.material = UI_UNLIT;
			cuts = new CutsLoader("cs401.n01");
			string pattern = "([-+]?[0-9]*\\.?[0-9]+)";
			Match match = Regex.Match(SetAnimation, pattern);
			if (match.Success)
			{
				int num2 = int.Parse(match.Groups[0].Value);
				TargetControl.texture = cuts.ImageCache[num2];
			}
		}
		else
		{
			TargetControl.material = UI_UNLIT;
			mode = true;
			Reset = false;
			cuts = new CutsLoader(animName.Replace("_", "."));
			StartCoroutine(cutscenerunner());
		}
	}

	private IEnumerator CutsceneWait(float waittime)
	{
		yield return new WaitForSeconds(0.2f);
		PostAnimPlay();
	}
}
