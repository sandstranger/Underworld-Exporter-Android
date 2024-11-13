using UnityEngine;

public class MoonStone : object_base
{
	protected override void Start()
	{
		base.Start();
		UWCharacter.Instance.MoonGateLevel = (short)(GameWorldController.instance.LevelNo + 1);
		UWCharacter.Instance.MoonGatePosition = base.transform.position;
	}

	public override void Update()
	{
		base.Update();
		if (!objInt().PickedUp)
		{
			UWCharacter.Instance.MoonGateLevel = (short)(GameWorldController.instance.LevelNo + 1);
			UWCharacter.Instance.MoonGatePosition = base.transform.position;
		}
		else
		{
			UWCharacter.Instance.MoonGatePosition = Vector3.zero;
		}
	}
}
