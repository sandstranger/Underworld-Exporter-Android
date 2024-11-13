public class Cutscene_Splash : Cuts
{
	public override void Awake()
	{
		base.Awake();
		switch (UWEBase._RES)
		{
		case "UW0":
			noOfImages = 3;
			ImageTimes[0] = 0f;
			ImageTimes[1] = 2f;
			ImageTimes[2] = 4f;
			ImageFrames[0] = "SplashOriginDemo";
			ImageFrames[1] = "cs011_n01";
			ImageFrames[2] = "Anim_Base";
			ImageLoops[0] = -1;
			ImageLoops[1] = -1;
			ImageLoops[2] = -1;
			noOfSubs = 0;
			noOfAudioClips = 0;
			break;
		case "UW1":
			noOfImages = 4;
			ImageTimes[0] = 0f;
			ImageTimes[1] = 1f;
			ImageTimes[2] = 2f;
			ImageTimes[3] = 5f;
			ImageFrames[0] = "SplashOrigin";
			ImageFrames[1] = "SplashBlueSky";
			ImageFrames[2] = "cs011_n01";
			ImageFrames[3] = "Anim_Base";
			ImageLoops[0] = -1;
			ImageLoops[1] = -1;
			ImageLoops[2] = -1;
			ImageLoops[3] = -1;
			noOfSubs = 0;
			noOfAudioClips = 0;
			break;
		case "UW2":
			noOfImages = 4;
			ImageTimes[0] = 0f;
			ImageTimes[1] = 1f;
			ImageTimes[2] = 2f;
			ImageTimes[3] = 6f;
			ImageFrames[0] = "SplashOriginEa";
			ImageFrames[1] = "SplashLookingGlass";
			ImageFrames[2] = "cs011_n01";
			ImageFrames[3] = "Anim_Base";
			ImageLoops[0] = -1;
			ImageLoops[1] = -1;
			ImageLoops[2] = -1;
			ImageLoops[3] = -1;
			noOfSubs = 0;
			noOfAudioClips = 0;
			break;
		}
	}

	public override void PostCutSceneEvent()
	{
		GotoMainMenu();
	}
}
