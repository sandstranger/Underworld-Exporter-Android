using UnityEngine;
using UnityEngine.UI;

public class Eyes : GuiBase
{
	private const float frameRate = 1f;

	public RawImage output;

	public int TargetSequence;

	public int currentFrame;

	public int targetFrame;

	private float currentTime;

	private float resetTimer = 0f;

	private const float resetAtTime = 5f;

	private GRLoader EyesGr;

	private int[,] ImgSequences = new int[4, 5]
	{
		{ 0, 0, 0, 0, 0 },
		{ 1, 2, 3, 2, 1 },
		{ 4, 5, 6, 5, 4 },
		{ 7, 8, 9, 8, 7 }
	};

	public override void Start()
	{
		base.Start();
		EyesGr = new GRLoader(14);
	}

	public void SetTargetFrame(short currHealth, short maxHealth)
	{
		float num = (float)currHealth / (float)maxHealth;
		if (num <= 0.15f)
		{
			TargetSequence = 3;
		}
		else if (num <= 0.6f)
		{
			TargetSequence = 2;
		}
		else
		{
			TargetSequence = 1;
		}
	}

	public override void Update()
	{
		base.Update();
		currentTime += Time.deltaTime;
		if (currentTime > 1f)
		{
			currentTime = 0f;
			targetFrame++;
			if (targetFrame >= 5)
			{
				targetFrame = 0;
			}
		}
		resetTimer += Time.deltaTime;
		if (resetTimer > 5f)
		{
			resetTimer = 0f;
			TargetSequence = 0;
		}
		if (currentFrame != targetFrame)
		{
			output.texture = EyesGr.LoadImageAt(ImgSequences[TargetSequence, targetFrame]);
			currentFrame = targetFrame;
		}
	}
}
