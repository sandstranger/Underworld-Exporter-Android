using UnityEngine;

public class IceCollision : UWEBase
{
	private void OnTriggerEnter(Collider other)
	{
		if (UWCharacter.Instance.onIce)
		{
			Debug.Log("bounce off of " + other.name);
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (UWCharacter.Instance.onIce)
		{
			Debug.Log("collision off o f " + collision.gameObject.name);
		}
	}
}
