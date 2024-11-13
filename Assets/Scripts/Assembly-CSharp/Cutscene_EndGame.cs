using UnityEngine;

public class Cutscene_EndGame : Cuts
{
	public override void Awake()
	{
		base.Awake();
		noOfImages = 15;
		ImageFrames[0] = "cs001_n01";
		ImageTimes[0] = 0f;
		ImageLoops[0] = 0;
		ImageFrames[1] = "cs001_n02";
		ImageTimes[1] = 5.8f;
		ImageLoops[1] = 0;
		ImageFrames[2] = "cs001_n03";
		ImageTimes[2] = 13.8f;
		ImageLoops[2] = -1;
		ImageFrames[3] = "cs001_n04";
		ImageTimes[3] = 18f;
		ImageLoops[3] = -1;
		ImageFrames[4] = "cs001_n05";
		ImageTimes[4] = 23.8f;
		ImageLoops[4] = -1;
		ImageFrames[5] = "cs001_n06";
		ImageTimes[5] = 33.8f;
		ImageLoops[5] = -1;
		ImageFrames[6] = "cs001_n07";
		ImageTimes[6] = 38f;
		ImageLoops[6] = -1;
		ImageFrames[7] = "cs001_n10";
		ImageTimes[7] = 45.8f;
		ImageLoops[7] = -1;
		ImageFrames[8] = "cs000_n01";
		ImageTimes[8] = 55f;
		ImageLoops[8] = -1;
		ImageFrames[9] = "cs000_n06";
		ImageTimes[9] = 63f;
		ImageLoops[9] = -1;
		ImageFrames[10] = "cs000_n03";
		ImageTimes[10] = 67f;
		ImageLoops[10] = -1;
		ImageFrames[11] = "cs000_n04";
		ImageTimes[11] = 69.6f;
		ImageLoops[11] = -1;
		ImageFrames[12] = "cs000_n05";
		ImageTimes[12] = 74.8f;
		ImageLoops[12] = -1;
		ImageFrames[13] = "Anim_Base";
		ImageTimes[13] = 90.6f;
		ImageLoops[13] = -1;
		StringBlockNo = 3073;
		noOfSubs = 20;
		SubsStringIndices[0] = 0;
		SubsTimes[0] = 0.2f;
		SubsDuration[0] = 2.5f;
		SubsStringIndices[1] = 1;
		SubsTimes[1] = ImageTimes[1];
		SubsDuration[1] = 2.9f;
		SubsStringIndices[2] = 2;
		SubsTimes[2] = SubsTimes[1] + 2.5f;
		SubsDuration[2] = 2.5f;
		SubsStringIndices[3] = 3;
		SubsTimes[3] = ImageTimes[2];
		SubsDuration[3] = 2.9f;
		SubsStringIndices[4] = 4;
		SubsTimes[4] = SubsTimes[3] + 4f;
		SubsDuration[4] = 2.9f;
		SubsStringIndices[5] = 5;
		SubsTimes[5] = ImageTimes[4];
		SubsDuration[5] = 2.5f;
		SubsStringIndices[6] = 6;
		SubsTimes[6] = ImageTimes[5];
		SubsDuration[6] = 2.9f;
		SubsStringIndices[7] = 7;
		SubsTimes[7] = SubsTimes[6] + 3f;
		SubsDuration[7] = 2.9f;
		SubsStringIndices[8] = 8;
		SubsTimes[8] = SubsTimes[7] + 3f;
		SubsDuration[8] = 2.9f;
		SubsStringIndices[9] = 9;
		SubsTimes[9] = ImageTimes[7];
		SubsDuration[9] = 2.9f;
		SubsStringIndices[10] = 10;
		SubsTimes[10] = ImageTimes[8];
		SubsDuration[10] = 2.9f;
		SubsStringIndices[11] = 11;
		SubsTimes[11] = SubsTimes[10] + 3f;
		SubsDuration[11] = 2.9f;
		SubsStringIndices[12] = 12;
		SubsTimes[12] = ImageTimes[9];
		SubsDuration[12] = 2.9f;
		SubsStringIndices[13] = 13;
		SubsTimes[13] = ImageTimes[10];
		SubsDuration[13] = 2.9f;
		SubsStringIndices[14] = 14;
		SubsTimes[14] = SubsTimes[13] + 3f;
		SubsDuration[14] = 2.9f;
		SubsStringIndices[15] = 15;
		SubsTimes[15] = SubsTimes[14] + 3f;
		SubsDuration[15] = 2.9f;
		SubsStringIndices[16] = 16;
		SubsTimes[16] = SubsTimes[15] + 3f;
		SubsDuration[16] = 2.9f;
		SubsStringIndices[17] = 17;
		SubsTimes[17] = SubsTimes[16] + 3f;
		SubsDuration[17] = 2.9f;
		SubsStringIndices[18] = 18;
		SubsTimes[18] = SubsTimes[17] + 3f;
		SubsDuration[18] = 2.9f;
		SubsStringIndices[19] = 19;
		SubsTimes[19] = SubsTimes[18] + 3f;
		SubsDuration[19] = 2.9f;
	}

	public override void PreCutsceneEvent()
	{
		base.PreCutsceneEvent();
		UWCharacter.Instance.transform.position = Vector3.zero;
		MusicController.instance.EndGame = true;
	}

	public override void PostCutSceneEvent()
	{
		GotoMainMenu();
	}
}
