using UnityEngine;

public class Cuts : GuiBase
{
	protected float[] ImageTimes = new float[50];

	protected string[] ImageFrames = new string[50];

	protected int[] ImageLoops = new int[50];

	protected float[] SubsTimes = new float[50];

	protected int[] SubsStringIndices = new int[50];

	protected float[] SubsDuration = new float[50];

	protected float[] AudioTimes = new float[50];

	protected string[] AudioClipName = new string[50];

	public int StringBlockNo;

	protected int noOfImages;

	protected int noOfSubs;

	protected int noOfAudioClips;

	public virtual string getFillerAnim()
	{
		string rES = UWEBase._RES;
		if (rES != null && rES == "UW0")
		{
			return "CS011.N00";
		}
		return "cs000_n01";
	}

	public int NoOfImages()
	{
		return noOfImages;
	}

	public float getImageTime(int index)
	{
		return ImageTimes[index];
	}

	public int getImageLoops(int index)
	{
		return ImageLoops[index];
	}

	public string getImageFrame(int index)
	{
		return ImageFrames[index];
	}

	public int getNoOfSubs()
	{
		return noOfSubs;
	}

	public float getSubTime(int index)
	{
		return SubsTimes[index];
	}

	public int getSubIndex(int index)
	{
		return SubsStringIndices[index];
	}

	public float getSubDuration(int index)
	{
		return SubsDuration[index];
	}

	public int getNoOfAudioClips()
	{
		return noOfAudioClips;
	}

	public float getAudioTime(int index)
	{
		return AudioTimes[index];
	}

	public string getAudioClip(int index)
	{
		return AudioClipName[index];
	}

	public virtual void Awake()
	{
	}

	public virtual void PreCutsceneEvent()
	{
	}

	public virtual void PostCutSceneEvent()
	{
	}

	public void GotoMainMenu()
	{
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
		MainMenuHud.instance.gameObject.SetActive(true);
		MainMenuHud.instance.MenuMode = 0;
		MainMenuHud.instance.OpScr.SetActive(true);
		MainMenuHud.instance.CharGen.SetActive(false);
		MainMenuHud.instance.ButtonClickMainMenu(4);
		WindowDetectUW.SwitchFromMouseLook();
	}

	protected void SyncSubtitles()
	{
		for (int i = 0; i <= AudioTimes.GetUpperBound(0); i++)
		{
			SubsTimes[i] = AudioTimes[i];
			AudioClip audioClip = (AudioClip)Resources.Load(AudioClipName[i]);
			if (audioClip != null)
			{
				SubsDuration[i] = audioClip.length;
			}
		}
	}
}
