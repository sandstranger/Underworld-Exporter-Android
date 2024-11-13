using UnityEngine;

public class BillboardNPC : Billboard
{
	private void Update()
	{
		if (Vector3.Distance(base.transform.position, UWCharacter.Instance.CameraPos) <= 8f && UWCharacter.Instance.dir != Vector3.zero)
		{
			base.transform.rotation = Quaternion.LookRotation(UWCharacter.Instance.dirForNPC);
		}
	}
}
