public class Cutscene_Arial : Cuts
{
	public override void Awake()
	{
		base.Awake();
		noOfImages = 3;
		ImageFrames[0] = "cs003_n01";
		ImageTimes[0] = 0f;
		ImageLoops[0] = -1;
		ImageFrames[1] = "cs003_n02";
		ImageTimes[1] = 7f;
		ImageLoops[1] = -1;
		ImageFrames[2] = "Anim_Base";
		ImageTimes[2] = 13f;
		ImageLoops[2] = -1;
		StringBlockNo = 3075;
		noOfSubs = 4;
		for (int i = 0; i < 4; i++)
		{
			SubsStringIndices[i] = i;
			SubsTimes[i] = (float)i * 3f;
			SubsDuration[i] = 2.5f;
		}
	}
}
