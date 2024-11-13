using System;
using UnityEngine;

public class MapNote : UWClass
{
	public string NoteText;

	public Guid guid;

	public int PosX;

	public int PosY;

	public MapNote(int posX, int posY, string noteText)
	{
		PosX = posX;
		PosY = posY;
		NoteText = noteText;
		guid = Guid.NewGuid();
	}

	public Vector2 NotePosition()
	{
		return new Vector2(PosX, (float)PosY - 100f);
	}
}
