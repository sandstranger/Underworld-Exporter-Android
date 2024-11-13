using System.Collections;
using UnityEngine;

public class Action_Activate : MonoBehaviour
{
	public string[] ObjectsToActivate = new string[4];

	public int[] ActivationDelay = new int[4];

	public void PerformAction()
	{
		Debug.Log(base.name + " Action Activate");
		for (int i = 0; i < 4; i++)
		{
			if (!(ObjectsToActivate[i] != ""))
			{
				continue;
			}
			GameObject gameObject = GameObject.Find(ObjectsToActivate[i]);
			if (gameObject != null)
			{
				if (ActivationDelay[i] != 0)
				{
					StartCoroutine(ActivationWait(gameObject, ActivationDelay[i]));
				}
				else
				{
					gameObject.SendMessage("Activate");
				}
			}
		}
	}

	private IEnumerator ActivationWait(GameObject objToActivate, float waittime)
	{
		float index = 0f;
		while (index < waittime)
		{
			index += Time.deltaTime;
			yield return new WaitForSeconds(0.01f);
		}
		objToActivate.SendMessage("Activate");
	}
}
