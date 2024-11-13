using UnityEngine;
using UnityEngine.UI;

public class HUD : UWEBase
{
	[Header("Cursor Icons")]
	public RawImage FreeLookCursor;

	public RawImage MouseLookCursor;

	private Texture2D _CursorIcon;

	public Texture2D CursorIconDefault;

	public Texture2D CursorIconBlank;

	public Texture2D CursorIconTarget;

	public Text LoadingProgress;

	public Texture2D CursorIcon
	{
		get
		{
			return _CursorIcon;
		}
		set
		{
			_CursorIcon = value;
			FreeLookCursor.texture = value;
			MouseLookCursor.texture = value;
		}
	}
}
