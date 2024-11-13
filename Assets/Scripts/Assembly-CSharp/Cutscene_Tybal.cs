public class Cutscene_Tybal : Cuts
{
	public override void Awake()
	{
		base.Awake();
		noOfImages = 5;
		ImageFrames[0] = "cs002_n01";
		ImageTimes[0] = 0f;
		ImageLoops[0] = -1;
		ImageFrames[1] = "cs002_n02";
		ImageTimes[1] = 10f;
		ImageLoops[1] = -1;
		ImageFrames[2] = "cs002_n03";
		ImageTimes[2] = 20f;
		ImageLoops[2] = -1;
		ImageFrames[3] = "cs002_n04";
		ImageTimes[3] = 45.9f;
		ImageLoops[3] = 0;
		ImageFrames[4] = "Anim_Base";
		ImageTimes[4] = 49.2f;
		ImageLoops[4] = -1;
		StringBlockNo = 3074;
		noOfSubs = 15;
		for (int i = 0; i < 15; i++)
		{
			SubsStringIndices[i] = i;
			SubsTimes[i] = (float)i * 3f;
			SubsDuration[i] = 2.5f;
		}
	}
}
