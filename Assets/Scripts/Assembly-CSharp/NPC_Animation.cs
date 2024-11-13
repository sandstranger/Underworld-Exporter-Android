using UnityEngine;

public class NPC_Animation : MonoBehaviour
{
	private const float frameRate = 0.2f;

	public bool FreezeAnimFrame;

	public int AnimationIndex;

	public int AnimationPos;

	public float animationCounter;

	public CritterAnimInfo critAnim;

	public SpriteRenderer output;

	public bool ConstantAnim;

	private void Update()
	{
		animationCounter += Time.deltaTime;
		if (!(animationCounter >= 0.2f))
		{
			return;
		}
		animationCounter = 0f;
		if (AnimationPos <= critAnim.animIndices.GetUpperBound(1))
		{
			if (critAnim.animIndices[AnimationIndex, AnimationPos] != -1)
			{
				output.sprite = critAnim.animSprites[critAnim.animIndices[AnimationIndex, AnimationPos]];
			}
			else if (!ConstantAnim)
			{
				AnimationPos = 0;
			}
		}
		if (FreezeAnimFrame || UWCharacter.Instance.isTimeFrozen)
		{
			AnimationPos = 0;
		}
		else
		{
			AnimationPos++;
		}
		if (AnimationPos >= critAnim.animIndices.GetUpperBound(1) && !ConstantAnim)
		{
			AnimationPos = 0;
		}
	}

	public void Play(int anim, bool isConstant)
	{
		if (anim != AnimationIndex)
		{
			ConstantAnim = isConstant;
			AnimationPos = 0;
			animationCounter = 0f;
			if (critAnim.animIndices[AnimationIndex, AnimationPos++] != -1)
			{
				output.sprite = critAnim.animSprites[critAnim.animIndices[AnimationIndex, AnimationPos++]];
			}
		}
		if (FreezeAnimFrame || UWCharacter.Instance.isTimeFrozen)
		{
			AnimationPos = 0;
		}
		AnimationIndex = anim;
	}
}
