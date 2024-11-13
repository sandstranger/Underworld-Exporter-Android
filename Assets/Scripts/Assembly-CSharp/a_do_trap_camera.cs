using System.Collections;
using UnityEngine;

public class a_do_trap_camera : a_hack_trap
{
	private Camera cam;

	private Light lt;

	private Rect window_uw1 = new Rect(0.163f, 0.335f, 0.54f, 0.572f);

	private Rect window_uw2 = new Rect(0.05f, 0.28f, 0.655f, 0.64f);

	private Rect fullscreen = new Rect(0f, 0f, 1f, 1f);

	protected override void Start()
	{
		base.Start();
		cam = base.gameObject.AddComponent<Camera>();
		cam.tag = "MainCamera";
		string rES = UWEBase._RES;
		if (rES != null && rES == "UW2")
		{
			cam.rect = window_uw2;
		}
		else
		{
			cam.rect = window_uw1;
		}
		cam.depth = 100f;
		cam.enabled = false;
		lt = base.gameObject.AddComponent<Light>();
		lt.range = 8f;
		lt.enabled = false;
	}

	private void SetWindowRect()
	{
		if (UWHUD.instance.window.FullScreen)
		{
			cam.rect = fullscreen;
			return;
		}
		string rES = UWEBase._RES;
		if (rES != null && rES == "UW2")
		{
			cam.rect = window_uw2;
		}
		else
		{
			cam.rect = window_uw1;
		}
	}

	public override void ExecuteTrap(object_base src, int triggerX, int triggerY, int State)
	{
		StartCoroutine(ActivateCamera());
	}

	private IEnumerator ActivateCamera()
	{
		SetWindowRect();
		UWCharacter.Instance.playerCam.tag = "Untagged";
		UWCharacter.Instance.playerCam.enabled = false;
		cam.enabled = true;
		lt.enabled = true;
		UWCharacter.Instance.isRoaming = true;
		yield return new WaitForSeconds(5f);
		UWCharacter.Instance.isRoaming = false;
		cam.enabled = false;
		lt.enabled = false;
		UWCharacter.Instance.playerCam.tag = "MainCamera";
		UWCharacter.Instance.playerCam.enabled = true;
	}

	public override void PostActivate(object_base src)
	{
		Debug.Log("Overridden PostActivate to test " + base.name);
		base.PostActivate(src);
	}
}
