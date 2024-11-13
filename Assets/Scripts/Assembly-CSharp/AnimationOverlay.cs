using System.Collections;
using UnityEngine;

public class AnimationOverlay : UWEBase
{
	public int StartFrame = 0;

	public int FrameNo = 0;

	public int NoOfFrames = 5;

	public bool Active = true;

	private SpriteRenderer image;

	public int OverlayIndex = 0;

	public int StartingDuration = 65535;

	public int Duration
	{
		get
		{
			return UWEBase.CurrentTileMap().Overlays[OverlayIndex].duration;
		}
		set
		{
			UWEBase.CurrentTileMap().Overlays[OverlayIndex].duration = value;
		}
	}

	public bool Looping
	{
		get
		{
			if (OverlayIndex != 0)
			{
				if (UWEBase.CurrentTileMap().Overlays[OverlayIndex].duration < 65535)
				{
					return false;
				}
				return true;
			}
			return true;
		}
	}

	public static int CreateOverlayEntry(int link, int tileX, int tileY, int duration)
	{
		TileMap.Overlay[] overlays = UWEBase.CurrentTileMap().Overlays;
		for (int i = 0; i <= overlays.GetUpperBound(0); i++)
		{
			if (overlays[i].link == 0)
			{
				overlays[i].link = link;
				overlays[i].tileX = tileX;
				overlays[i].tileY = tileY;
				overlays[i].duration = duration;
				return i;
			}
		}
		Debug.Log("Unable to create overlay control!");
		return 0;
	}

	private void Start()
	{
		TileMap.Overlay[] overlays = UWEBase.CurrentTileMap().Overlays;
		int index = GetComponent<ObjectInteraction>().objectloaderinfo.index;
		for (int i = 0; i <= overlays.GetUpperBound(0); i++)
		{
			if (overlays[i].link == index)
			{
				OverlayIndex = i;
				break;
			}
		}
		if (OverlayIndex == 0)
		{
			OverlayIndex = CreateOverlayEntry(index, GetComponent<ObjectInteraction>().ObjectTileX, GetComponent<ObjectInteraction>().ObjectTileY, StartingDuration);
		}
		image = base.gameObject.GetComponentInChildren<SpriteRenderer>();
		Go();
	}

	public void Go()
	{
		FrameNo = StartFrame;
		LoadAnimo(StartFrame);
		StartCoroutine(Animate());
	}

	public void Stop()
	{
		Active = false;
	}

	private void LoadAnimo(int index)
	{
		if (image == null)
		{
			image = base.gameObject.GetComponentInChildren<SpriteRenderer>();
		}
		if (image != null)
		{
			image.sprite = GameWorldController.instance.TmAnimo.RequestSprite(index);
		}
	}

	public IEnumerator Animate()
	{
		LoadAnimo(FrameNo);
		while (Active)
		{
			yield return new WaitForSeconds(0.2f);
			if (!Active)
			{
				break;
			}
			FrameNo++;
			if (!Looping)
			{
				Duration--;
				if (Duration <= 0)
				{
					EndAnimation();
				}
			}
			if (FrameNo >= StartFrame + NoOfFrames)
			{
				if (Looping)
				{
					FrameNo = StartFrame;
					LoadAnimo(FrameNo);
				}
				else
				{
					EndAnimation();
				}
			}
			else
			{
				LoadAnimo(FrameNo);
			}
		}
	}

	private void EndAnimation()
	{
		UWEBase.CurrentTileMap().Overlays[OverlayIndex] = default(TileMap.Overlay);
		if (GetComponent<ObjectInteraction>() != null)
		{
			GetComponent<ObjectInteraction>().objectloaderinfo.InUseFlag = 0;
			Object.Destroy(base.gameObject);
		}
	}
}
