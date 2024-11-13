using UnityEngine;

public class Words : MonoBehaviour
{
	public int font;

	public int BlockNo;

	public int colour;

	public int StringNo;

	public int size;

	private float fontSpacing;

	public static StringController sc;

	private void Start()
	{
		int num = font;
		if (num == 606 || num == 609)
		{
			fontSpacing = 0.15f;
		}
		else
		{
			fontSpacing = 0.05f;
		}
		CreateString();
	}

	private void CreateString()
	{
		char[] array = sc.GetString(BlockNo, StringNo).ToCharArray();
		for (int i = 0; i <= array.GetUpperBound(0); i++)
		{
			AddLetter(i, array[i]);
		}
	}

	private void AddLetter(int index, int Asciichar)
	{
		GameObject gameObject = new GameObject();
		gameObject.name = base.name + "_letter_" + index;
		gameObject.transform.parent = base.transform;
		gameObject.transform.position = base.transform.position;
		gameObject.transform.localPosition = new Vector3(fontSpacing * (float)index, 0f, 0f);
		gameObject.transform.localRotation = new Quaternion(0f, 0f, 0f, 0f);
		SpriteRenderer spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
		Sprite sprite = Resources.Load<Sprite>("Fonts/font_" + font.ToString("0000") + "_" + Asciichar.ToString("0000"));
		spriteRenderer.sprite = sprite;
	}
}
