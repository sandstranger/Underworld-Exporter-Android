using UnityEngine;

public class Instrument : object_base
{
	public static bool PlayingInstrument;

	private static string CurrentInstrument;

	private static string NoteRecord;

	protected override void Start()
	{
		base.Start();
	}

	public override bool use()
	{
		if (objInt().PickedUp)
		{
			if (UWEBase.CurrentObjectInHand == null)
			{
				if (!PlayingInstrument)
				{
					PlayInstrument();
				}
				return true;
			}
			return ActivateByObject(UWEBase.CurrentObjectInHand);
		}
		return false;
	}

	public void PlayInstrument()
	{
		WindowDetect.WaitingForInput = true;
		UWCharacter.Instance.playerMotor.enabled = false;
		PlayingInstrument = true;
		CurrentInstrument = base.name;
		MusicController.instance.Stop();
		NoteRecord = "";
		UWHUD.instance.MessageScroll.Set(StringController.instance.GetString(1, 250));
	}

	private void MusicInstrumentInteaction()
	{
		if (Input.GetKeyDown("1"))
		{
			PlayNote(1);
		}
		if (Input.GetKeyDown("2"))
		{
			PlayNote(2);
		}
		if (Input.GetKeyDown("3"))
		{
			PlayNote(3);
		}
		if (Input.GetKeyDown("4"))
		{
			PlayNote(4);
		}
		if (Input.GetKeyDown("5"))
		{
			PlayNote(5);
		}
		if (Input.GetKeyDown("6"))
		{
			PlayNote(6);
		}
		if (Input.GetKeyDown("7"))
		{
			PlayNote(7);
		}
		if (Input.GetKeyDown("8"))
		{
			PlayNote(8);
		}
		if (Input.GetKeyDown("9"))
		{
			PlayNote(9);
		}
		if (Input.GetKeyDown("0"))
		{
			PlayNote(10);
		}
		if (!Input.GetKeyDown(KeyCode.Escape))
		{
			return;
		}
		PlayingInstrument = false;
		CurrentInstrument = "";
		WindowDetect.WaitingForInput = false;
		UWCharacter.Instance.playerMotor.enabled = true;
		UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, StringController.str_you_put_the_instrument_down_));
		MusicController.instance.Resume();
		if (UWEBase._RES == "UW1" && NoteRecord == "354237875" && base.item_id == 292 && GameWorldController.instance.LevelNo == 2 && !Quest.instance.isCupFound && base.item_id == 292)
		{
			int visitTileX = TileMap.visitTileX;
			int visitTileY = TileMap.visitTileY;
			if (visitTileX >= 23 && visitTileX <= 27 && visitTileY >= 43 && visitTileY <= 45)
			{
				ObjectLoaderInfo currObj = ObjectLoader.newObject(174, 0, 0, 0, 256);
				ObjectInteraction objectInteraction = ObjectInteraction.CreateNewObject(UWEBase.CurrentTileMap(), currObj, UWEBase.CurrentObjectList().objInfo, GameWorldController.instance.InventoryMarker.gameObject, GameWorldController.instance.InventoryMarker.transform.position);
				GameWorldController.MoveToInventory(objectInteraction);
				UWEBase.CurrentObjectInHand = objectInteraction;
				Character.InteractionMode = 2;
				InteractionModeControl.UpdateNow = true;
				Quest.instance.isCupFound = true;
				UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(1, 136));
			}
		}
	}

	public override void Update()
	{
		base.Update();
		if (PlayingInstrument && CurrentInstrument == base.name)
		{
			MusicInstrumentInteaction();
		}
	}

	private void PlayNote(int note)
	{
		if (note == 10)
		{
			NoteRecord += "0";
		}
		else
		{
			NoteRecord += note;
		}
		if (NoteRecord.Length > 9)
		{
			NoteRecord = NoteRecord.Remove(0, 1);
		}
		MusicController.instance.MusicalInstruments.pitch = Mathf.Pow(2f, (float)note / 12f);
		MusicController.instance.MusicalInstruments.Play();
	}

	public override string UseVerb()
	{
		return "play";
	}
}
