using System.Collections;
using UnityEngine;

public class ShrineLava : UWEBase
{
	private void OnTriggerEnter(Collider other)
	{
		if (!(other.gameObject.GetComponent<ObjectInteraction>() != null) || !Quest.instance.isGaramonBuried)
		{
			return;
		}
		ObjectInteraction component = other.gameObject.GetComponent<ObjectInteraction>();
		int item_id = component.item_id;
		if (item_id == 54 || item_id == 55 || item_id == 10 || item_id == 147 || item_id == 151 || item_id == 174 || item_id == 191 || item_id == 287 || item_id == 310)
		{
			Quest.instance.TalismansRemaining--;
			Impact.SpawnHitImpact(Impact.ImpactMagic(), component.GetImpactPoint(), 40, 44);
			component.consumeObject();
			if (Quest.instance.TalismansRemaining <= 0)
			{
				StartCoroutine(SuckItAvatar());
			}
		}
	}

	private IEnumerator SuckItAvatar()
	{
		ObjectInteraction slasher = UWEBase.CurrentObjectList().objInfo[129].instance;
		Vector3 slasherPos = Vector3.zero;
		if (slasher != null)
		{
			slasherPos = slasher.transform.position;
		}
		GameObject myObj = ObjectInteraction.CreateNewObject(currObj: ObjectLoader.newObject(346, 0, 0, 0, 256), tm: UWEBase.CurrentTileMap(), objList: UWEBase.CurrentObjectList().objInfo, parent: GameWorldController.instance.DynamicObjectMarker().gameObject, position: GameWorldController.instance.InventoryMarker.transform.position).gameObject;
		GameWorldController.MoveToWorld(myObj.GetComponent<ObjectInteraction>());
		myObj.transform.localPosition = base.transform.position + new Vector3(0f, 0.5f, 0f);
		Quaternion playerRot = UWCharacter.Instance.transform.rotation;
		Quaternion EndRot = new Quaternion(playerRot.x, playerRot.y, playerRot.z + 1.2f, playerRot.w);
		Vector3 StartPos = UWCharacter.Instance.transform.position;
		Vector3 EndPos = myObj.transform.localPosition;
		float rate = 0.5f;
		float index = 0f;
		while (index < 1f)
		{
			UWCharacter.Instance.transform.position = Vector3.Lerp(StartPos, EndPos, index);
			UWCharacter.Instance.transform.rotation = Quaternion.Lerp(playerRot, EndRot, index);
			if (slasher != null)
			{
				slasher.transform.position = Vector3.Lerp(slasherPos, EndPos, index);
			}
			index += rate * Time.deltaTime;
			yield return new WaitForSeconds(0.01f);
		}
		UWCharacter.Instance.transform.rotation = playerRot;
		GameWorldController.instance.SwitchLevel(8, 26, 24);
	}
}
