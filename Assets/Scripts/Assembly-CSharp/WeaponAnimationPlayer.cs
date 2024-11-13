using UnityEngine;
using UnityEngine.UI;

public class WeaponAnimationPlayer : UWEBase
{
	private static float frameRate = 0.2f;

	public int AnimationPos;

	public float animationCounter;

	public RawImage TargetControl;

	public int PreviousAnimation;

	public int SetAnimation;

	public Texture2D blank;

	public int RetrievedImage;

	private void Update()
	{
		if (SetAnimation != PreviousAnimation)
		{
			PreviousAnimation = SetAnimation;
			AnimationPos = 0;
			animationCounter = 0f;
			if (SetAnimation == -1)
			{
				TargetControl.texture = blank;
				return;
			}
		}
		if (GameWorldController.instance.weapongr == null || SetAnimation == -1)
		{
			return;
		}
		animationCounter += Time.deltaTime;
		if (!(animationCounter >= frameRate) || !(UWEBase._RES != "UW0"))
		{
			return;
		}
		animationCounter = 0f;
		if (AnimationPos <= GameWorldController.instance.weaps.frames.GetUpperBound(1))
		{
			if (GameWorldController.instance.weaps.frames[SetAnimation, AnimationPos] != -1)
			{
				RetrievedImage = GameWorldController.instance.weaps.frames[SetAnimation, AnimationPos];
				TargetControl.texture = GameWorldController.instance.weapongr.LoadImageAt(GameWorldController.instance.weaps.frames[SetAnimation, AnimationPos]);
			}
			AnimationPos++;
		}
	}
}
