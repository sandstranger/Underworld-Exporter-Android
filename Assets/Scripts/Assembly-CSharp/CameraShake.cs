using UnityEngine;

public class CameraShake : UWEBase
{
	public Transform camTransform;

	private float shakeDuration = 0f;

	private float shakeAmount = 0.7f;

	private float decreaseFactor = 1f;

	public static Vector3 CurrentShake = Vector3.zero;

	public bool shaking = false;

	public static CameraShake instance;

	private void Awake()
	{
		instance = this;
		if (camTransform == null)
		{
			camTransform = GetComponent(typeof(Transform)) as Transform;
		}
	}

	public void ShakeEarthQuake(float duration)
	{
		if (!shaking)
		{
			shakeAmount = 0.7f;
			shakeDuration = duration;
		}
	}

	public void ShakeCombat(float duration)
	{
		if (!shaking)
		{
			shakeAmount = 0.1f;
			shakeDuration = duration;
		}
	}

	private void Update()
	{
		if (shakeDuration > 0f)
		{
			shaking = true;
			CurrentShake = Random.insideUnitSphere * shakeAmount;
			shakeDuration -= Time.deltaTime * decreaseFactor;
			return;
		}
		if (shaking)
		{
		}
		shaking = false;
		shakeDuration = 0f;
		CurrentShake = Vector3.zero;
	}
}
