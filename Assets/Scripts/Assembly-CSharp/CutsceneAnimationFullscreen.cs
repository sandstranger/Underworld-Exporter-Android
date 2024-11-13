using System.Collections;
using UnityEngine;

public class CutsceneAnimationFullscreen : HudAnimation
{
	private bool PlayingSequence;

	private bool isFullScreen;

	private int currentFrameLoops;

	public Cuts cs;

	public string CurrentSpriteName;

	private Sprite CurrentSpriteLoaded;

	public ScrollController mlCuts;

	public AudioSource aud;

	public bool SkipAnim = false;

	private float CutsceneTime;

	public Sprite filler;

	public string SetAnimationFile;

	public string PreviousAnimationFile;

	public string currentCutsFile;

	public string previousCutsFile;

	public void End()
	{
		UWCharacter.Instance.playerCam.cullingMask = -33;
		if (cs != null)
		{
			cs.PostCutSceneEvent();
			PlayingSequence = false;
			UWHUD.instance.EnableDisableControl(UWHUD.instance.CutsceneFullPanel.gameObject, false);
			Object.Destroy(cs);
		}
	}

	public void Begin()
	{
		if (!(cs == null))
		{
			CutsceneTime = 0f;
			UWHUD.instance.EnableDisableControl(UWHUD.instance.CutsceneFullPanel, true);
			PlayingSequence = true;
			SkipAnim = false;
			cs.PreCutsceneEvent();
			StartCoroutine(PlayCutsImageSequence());
			StartCoroutine(PlayCutsSubtitle());
			StartCoroutine(PlayCutsAudio());
		}
	}

	private IEnumerator PlayCutsImageSequence()
	{
		float currTime = 0f;
		for (int i = 0; i < cs.NoOfImages(); i++)
		{
			yield return new WaitForSeconds(cs.getImageTime(i) - currTime);
			currTime = cs.getImageTime(i);
			currentFrameLoops = cs.getImageLoops(i);
			if (currentFrameLoops != 0)
			{
				anim.looping = true;
			}
			else
			{
				anim.looping = false;
			}
			SetAnimationFile = cs.getImageFrame(i);
		}
		SetAnimationFile = "Anim_Base";
		PlayingSequence = false;
		PostAnimPlay();
		End();
	}

	public void Loop()
	{
		switch (currentFrameLoops)
		{
		case -1:
			break;
		case 0:
			SetAnimationFile = cs.getFillerAnim();
			break;
		default:
			currentFrameLoops--;
			break;
		}
	}

	private IEnumerator PlayCutsSubtitle()
	{
		mlCuts.Set("");
		float currTime = 0f;
		for (int i = 0; i <= cs.getNoOfSubs(); i++)
		{
			yield return new WaitForSeconds(cs.getSubTime(i) - currTime);
			currTime = cs.getSubTime(i) + cs.getSubDuration(i);
			if (i < cs.getNoOfSubs())
			{
				currTime = Mathf.Min(currTime, cs.getSubTime(i + 1));
			}
			mlCuts.Set(StringController.instance.GetString(cs.StringBlockNo, cs.getSubIndex(i)));
			yield return new WaitForSeconds(cs.getSubDuration(i));
			mlCuts.Set("");
		}
	}

	private IEnumerator PlayCutsAudio()
	{
		float currTime = 0f;
		for (int i = 0; i < cs.getNoOfAudioClips(); i++)
		{
			yield return new WaitForSeconds(cs.getAudioTime(i) - currTime);
			currTime = cs.getAudioTime(i);
			aud.clip = Resources.Load<AudioClip>(cs.getAudioClip(i));
			aud.loop = false;
			aud.Play();
		}
	}

	public void PreAnimPlay()
	{
	}

	public void PostAnimPlay()
	{
		if (!PlayingSequence || cs == null)
		{
			UWCharacter.Instance.playerCam.cullingMask = -33;
			SetAnimationFile = "Anim_Base";
			mlCuts.Set("");
			End();
			UWHUD.instance.EnableDisableControl(UWHUD.instance.CutsceneFullPanel, false);
		}
		else
		{
			Loop();
		}
	}

	public override void Update()
	{
		if (SetAnimationFile != PreviousAnimationFile)
		{
			anim.SetAnimation = SetAnimationFile;
			PreviousAnimationFile = SetAnimationFile;
			if (SetAnimationFile.ToLower() == "anim_base")
			{
				UWCharacter.Instance.playerCam.cullingMask = -33;
			}
		}
		if (PlayingSequence)
		{
			CutsceneTime += Time.deltaTime;
			if (Input.anyKey && CutsceneTime >= 3f)
			{
				UWCharacter.Instance.playerCam.cullingMask = -33;
				SetAnimationFile = "Anim_Base";
				PlayingSequence = false;
				PostAnimPlay();
				StopAllCoroutines();
				UWHUD.instance.EnableDisableControl(UWHUD.instance.CutsceneFullPanel.gameObject, false);
				Object.Destroy(cs);
			}
		}
	}

	private void InitCutsFile()
	{
		if (SetAnimationFile != "Anim_Base")
		{
			GameWorldController.instance.cutsLoader = new CutsLoader(SetAnimationFile.Replace("_", "."));
		}
	}
}
