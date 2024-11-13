using UnityEngine;

public class PortcullisInteraction : MonoBehaviour
{
	public ObjectInteraction getParentObjectInteraction()
	{
		return base.transform.parent.parent.GetComponent<ObjectInteraction>();
	}
}
