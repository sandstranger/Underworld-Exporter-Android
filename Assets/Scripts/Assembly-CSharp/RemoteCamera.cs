using UnityEngine;

public class RemoteCamera : MonoBehaviour
{
	public GameObject ScreenToDisplayOn;

	public RemoteCameraCapture cam;

	private int FrameInterval = 30;

	private int FrameIntervalCounter = 30;

	public string Target;

	private GameObject player;

	private Material[] myMat;

	private void Start()
	{
		GameObject gameObject = GameObject.Find(Target);
		if (gameObject != null)
		{
			cam = gameObject.GetComponent<RemoteCameraCapture>();
			if (cam == null)
			{
				cam = gameObject.AddComponent<RemoteCameraCapture>();
			}
		}
		myMat = ScreenToDisplayOn.GetComponent<Renderer>().materials;
		player = UWCharacter.Instance.gameObject;
	}

	private void Update()
	{
		if (Vector3.Distance(player.transform.position, base.transform.position) <= 3f)
		{
			cam.camEnabled = true;
		}
		else
		{
			cam.camEnabled = false;
		}
		FrameIntervalCounter++;
		if (FrameIntervalCounter >= FrameInterval)
		{
			FrameIntervalCounter = 0;
			for (int i = 0; i <= myMat.GetUpperBound(0); i++)
			{
				myMat[i].mainTexture = cam.RenderedImage;
			}
		}
	}
}
