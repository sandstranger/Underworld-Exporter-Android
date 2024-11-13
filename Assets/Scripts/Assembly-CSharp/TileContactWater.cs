using UnityEngine;

public class TileContactWater : TileContact
{
	protected override void TileContactEvent(ObjectInteraction obj, Vector3 position)
	{
		int item_id = obj.item_id;
		if (item_id != 453)
		{
			ObjectHitsWater(obj, position);
		}
	}

	private void ObjectHitsWater(ObjectInteraction obj, Vector3 position)
	{
		if (IsObjectDestroyable(obj))
		{
			GameObject gameObject = Impact.SpawnHitImpact(453, position, splashanimstart(), splashanimstart() + 4);
			if (ObjectInteraction.PlaySoundEffects)
			{
				gameObject.GetComponent<ObjectInteraction>().aud.clip = MusicController.instance.SoundEffects[24];
				gameObject.GetComponent<ObjectInteraction>().aud.Play();
			}
			DestroyObject(obj);
		}
	}

	private int splashanimstart()
	{
		string rES = UWEBase._RES;
		if (rES != null && rES == "UW2")
		{
			return 34;
		}
		return 36;
	}
}
