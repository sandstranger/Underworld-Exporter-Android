using UnityEngine;
using UnityEngine.UI;

public class TextureMapButton : GuiBase
{
	public const short TextureTypeFloor = 0;

	public const short TextureTypeWall = 1;

	public const short TextureTypeDoor = 2;

	public int MapIndex;

	public int TextureIndex;

	public RawImage img;

	public static Color ColorOn = new Color(1f, 1f, 1f);

	public static Color ColorOff = new Color(0.5f, 0.5f, 0.5f);

	public short textureType = 0;

	private static short currentTextureType = 0;

	private static int SelectedTextureMapIndex;

	private static TextureMapButton selectedButton;

	public void OnClick()
	{
		SelectedTextureMapIndex = MapIndex;
		IngameEditor.instance.SelectedTextureMap.texture = img.texture;
		selectedButton = this;
		currentTextureType = textureType;
		switch (textureType)
		{
		case 0:
			IngameEditor.instance.WallTextureMapSelect.gameObject.SetActive(false);
			IngameEditor.instance.DoorTextureMapSelect.gameObject.SetActive(false);
			IngameEditor.instance.FloorTextureMapSelect.gameObject.SetActive(true);
			if (UWEBase._RES == "UW2")
			{
				IngameEditor.instance.FloorTextureMapSelect.value = TextureIndex;
			}
			else
			{
				IngameEditor.instance.FloorTextureMapSelect.value = TextureIndex - 210;
			}
			break;
		case 1:
			IngameEditor.instance.WallTextureMapSelect.gameObject.SetActive(true);
			IngameEditor.instance.DoorTextureMapSelect.gameObject.SetActive(false);
			IngameEditor.instance.FloorTextureMapSelect.gameObject.SetActive(false);
			IngameEditor.instance.WallTextureMapSelect.value = TextureIndex;
			break;
		case 2:
			IngameEditor.instance.WallTextureMapSelect.gameObject.SetActive(false);
			IngameEditor.instance.DoorTextureMapSelect.gameObject.SetActive(true);
			IngameEditor.instance.FloorTextureMapSelect.gameObject.SetActive(false);
			IngameEditor.instance.DoorTextureMapSelect.value = TextureIndex;
			break;
		}
	}

	public void OnHoverEnter()
	{
		img.color = ColorOn;
	}

	public void OnHoverExit()
	{
		img.color = ColorOff;
	}

	public void AcceptChange()
	{
		int num = 0;
		Texture2D texture = GameWorldController.instance.texLoader.LoadImageAt(0);
		switch (currentTextureType)
		{
		case 0:
			num = IngameEditor.instance.FloorTextureMapSelect.value + 210;
			texture = GameWorldController.instance.texLoader.LoadImageAt(num);
			break;
		case 1:
			num = IngameEditor.instance.WallTextureMapSelect.value;
			texture = GameWorldController.instance.texLoader.LoadImageAt(num);
			break;
		case 2:
			num = IngameEditor.instance.DoorTextureMapSelect.value;
			texture = (Texture2D)GameWorldController.instance.MaterialDoors[num].mainTexture;
			break;
		}
		UWEBase.CurrentTileMap().texture_map[SelectedTextureMapIndex] = (short)num;
		selectedButton.img.texture = texture;
		selectedButton.TextureIndex = num;
		GameWorldController.WorldReRenderPending = true;
		GameWorldController.ObjectReRenderPending = true;
		GameWorldController.FullReRender = true;
	}
}
