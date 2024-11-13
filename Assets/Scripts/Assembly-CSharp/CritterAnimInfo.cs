using UnityEngine;

public class CritterAnimInfo
{
	public const int NoOfAnims = 44;

	public string[,] animSequence;

	public int[,] animIndices;

	public Sprite[] animSprites;

	public string[] animName;

	public CritterAnimInfo()
	{
		animSequence = new string[44, 8];
		animIndices = new int[44, 8];
		string rES = UWClass._RES;
		if (rES != null && rES == "UW2")
		{
			animSprites = new Sprite[180];
		}
		else
		{
			animSprites = new Sprite[128];
		}
		animName = new string[44];
	}
}
