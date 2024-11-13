using UnityEngine;

public class Action_Message : MonoBehaviour
{
	public int SuccessMessage;

	public int FailMessage;

	public bool state = false;

	public void PerformAction()
	{
		AudioClip clip = Resources.Load("ss1/sfx/shock_barks/bark" + FailMessage) as AudioClip;
		AudioSource component = GetComponent<AudioSource>();
		if (component != null)
		{
			component.clip = clip;
			component.Play();
		}
		Debug.Log("Action Message");
	}
}
