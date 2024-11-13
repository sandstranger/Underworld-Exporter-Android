using UnityEngine;

public class ComputerScreen : MonoBehaviour
{
	public int ScreenStart;

	public int NoOfFrames;

	public bool LoopFrames;

	private float AnimationTime;

	public float animationFrameTime = 0.5f;

	private float NextFrame;

	public int CurrFrame = 0;

	private short Direction = 1;

	public GameObject ScreenToDisplayOn;

	private Material[] myMat;

	private void Start()
	{
		NextFrame = animationFrameTime;
		myMat = ScreenToDisplayOn.GetComponent<Renderer>().materials;
		setSprite(0);
	}

	private void Update()
	{
		AnimationTime += Time.deltaTime;
		if (AnimationTime >= NextFrame)
		{
			AnimationTime = 0f;
			setSprite(CurrFrame + Direction);
			CurrFrame += Direction;
			if (Direction == 1 && CurrFrame >= NoOfFrames)
			{
				Direction = -1;
			}
			if (Direction == -1 && CurrFrame <= 0)
			{
				Direction = 1;
			}
		}
	}

	private void setSprite(int index)
	{
		Texture2D texture2D = new Texture2D(64, 64);
		texture2D = Resources.Load<Texture2D>("Screen/" + (ScreenStart + index + 321).ToString("D4"));
		for (int i = 0; i <= myMat.GetUpperBound(0); i++)
		{
			myMat[i].mainTexture = texture2D;
		}
	}
}
