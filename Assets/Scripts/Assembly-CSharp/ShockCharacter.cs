using UnityEngine;

public class ShockCharacter : MonoBehaviour
{
	public static int InteractionMode;

	public float weaponRange = 1f;

	public float pickupRange = 3f;

	public float useRange = 3f;

	public float talkRange = 20f;

	public float lookRange = 25f;

	public Texture2D CursorIcon;

	public Texture2D CursorIconDefault;

	public Texture2D CursorIconBlank;

	private int cursorSizeX = 64;

	private int cursorSizeY = 64;

	private MouseLook XAxis;

	private MouseLook YAxis;

	private bool MouseLookEnabled;

	private GameObject MainCam;

	public bool CursorInMainWindow;

	public StringController StringControl;

	public int AttackCharging;

	public float Charge;

	public float chargeRate = 33f;

	public string ReadiedSpell;

	public bool[] Runes = new bool[24];

	public int[] ActiveRunes = new int[3];

	public bool isFemale;

	public bool isLefty;

	public static GameObject InvMarker;

	private void Start()
	{
		XAxis = GetComponent<MouseLook>();
		YAxis = base.transform.Find("Main Camera").GetComponent<MouseLook>();
		Cursor.SetCursor(CursorIconBlank, Vector2.zero, CursorMode.ForceSoftware);
	}

	public void PickupMode()
	{
		PlayerInventory component = GetComponent<PlayerInventory>();
		if (InvMarker == null)
		{
			InvMarker = GameWorldController.instance.InventoryMarker;
		}
		if (!(component.ObjectInHand == null))
		{
			return;
		}
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hitInfo = default(RaycastHit);
		if (Physics.Raycast(ray, out hitInfo, pickupRange))
		{
			ObjectInteraction component2 = hitInfo.transform.GetComponent<ObjectInteraction>();
			if (component2 != null)
			{
				component.ObjectInHand = component2;
				component.JustPickedup = true;
				component2.transform.position = InvMarker.transform.position;
			}
		}
	}

	private void OnGUI()
	{
		if (Event.current.Equals(Event.KeyboardEvent("e")))
		{
			if (!MouseLookEnabled)
			{
				XAxis.enabled = true;
				YAxis.enabled = true;
				MouseLookEnabled = true;
			}
			else
			{
				XAxis.enabled = false;
				YAxis.enabled = false;
				MouseLookEnabled = false;
			}
		}
		if (MouseLookEnabled)
		{
			Rect position = new Rect(Screen.width / 2 - cursorSizeX / 2, Screen.height / 2 - cursorSizeY / 2, cursorSizeX, cursorSizeY);
			GUI.DrawTexture(position, CursorIcon);
		}
		else
		{
			Rect position2 = new Rect(Event.current.mousePosition.x - (float)(cursorSizeX / 2), Event.current.mousePosition.y - (float)(cursorSizeY / 2), cursorSizeX, cursorSizeY);
			GUI.DrawTexture(position2, CursorIcon);
		}
	}
}
