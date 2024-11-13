public class Cutscene_Dream_2 : Cuts
{
	public override void Awake()
	{
		base.Awake();
		noOfImages = 4;
		ImageFrames[0] = "cs000_n06";
		ImageTimes[0] = 0f;
		ImageLoops[0] = -1;
		ImageFrames[1] = "cs000_n03";
		ImageTimes[1] = 8f;
		ImageLoops[1] = -1;
		ImageFrames[2] = "cs000_n03";
		ImageTimes[2] = 22f;
		ImageLoops[2] = -1;
		ImageFrames[3] = "Anim_Base";
		ImageTimes[3] = 32.2f;
		ImageLoops[3] = -1;
		StringBlockNo = 3097;
		noOfSubs = 5;
		for (int i = 0; i < noOfSubs; i++)
		{
			SubsStringIndices[i] = i;
			SubsTimes[i] = 5f + (float)i * 5f;
			SubsDuration[i] = 4.5f;
		}
	}
}
