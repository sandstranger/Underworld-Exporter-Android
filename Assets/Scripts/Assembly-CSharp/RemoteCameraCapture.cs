using UnityEngine;

public class RemoteCameraCapture : MonoBehaviour
{
	public Texture2D RenderedImage;

	public int FrameInterval = 100;

	private int FrameIntervalCounter = 100;

	public Camera cam;

	public bool camEnabled = true;

	private Texture2D captured;

	private void Start()
	{
		captured = new Texture2D(Screen.width, Screen.height);
		cam = GetComponent<Camera>();
		if (cam == null)
		{
			cam = base.gameObject.AddComponent<Camera>();
		}
		cam.depth = -10f;
	}

	private void OnPostRender()
	{
		if (camEnabled)
		{
			FrameIntervalCounter++;
			if (FrameIntervalCounter >= FrameInterval)
			{
				FrameIntervalCounter = 0;
				RenderedImage = CaptureImage(cam, Screen.width, Screen.height);
			}
		}
	}

	public Texture2D CaptureImage(Camera camera, int width, int height)
	{
		camera.Render();
		RenderTexture.active = camera.targetTexture;
		captured.ReadPixels(new Rect(0f, 0f, width, height), 0, 0);
		captured.Apply();
		RenderTexture.active = null;
		return captured;
	}
}
