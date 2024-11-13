using UnityEngine;

public class a_teleport_trap : trap_base
{
	public override void ExecuteTrap(object_base src, int triggerX, int triggerY, int State)
	{
		if (UWEBase._RES == "UW0" && base.zpos != 0)
		{
			ExecuteForUWDemo();
		}
		else
		{
			if (UWEBase.EditorMode || (UWEBase._RES == "UW2" && UWCharacter.Instance.JustTeleported))
			{
				return;
			}
			float x = (float)base.quality * 1.2f + 0.6f;
			float z = (float)base.owner * 1.2f + 0.6f;
			UWCharacter.Instance.transform.eulerAngles = new Vector3(0f, (float)base.heading * 45f, 0f);
			UWCharacter.Instance.playerCam.transform.localRotation = Quaternion.identity;
			UWCharacter.Instance.JustTeleported = true;
			UWCharacter.Instance.teleportedTimer = 0f;
			if (base.zpos == 0)
			{
				float num = (float)UWEBase.CurrentTileMap().GetFloorHeight(base.quality, base.owner) * 0.15f;
				UWCharacter.Instance.transform.position = new Vector3(x, num + 0.5f, z);
				UWCharacter.Instance.TeleportPosition = UWCharacter.Instance.transform.position;
				return;
			}
			UWCharacter.Instance.teleportedTimer = -1f;
			if (UWEBase._RES == "UW1")
			{
				UWCharacter.ResetTrueMana();
			}
			UWCharacter.Instance.playerMotor.movement.velocity = Vector3.zero;
			if ((base.xpos & 1) == 1)
			{
				GameWorldController.instance.SwitchLevel((short)(base.zpos - 1), base.quality, base.owner, 24);
			}
			else
			{
				GameWorldController.instance.SwitchLevel((short)(base.zpos - 1), base.quality, base.owner);
			}
		}
	}

	public void ExecuteForUWDemo()
	{
		UWHUD.instance.MessageScroll.Add("You have reached level 2 of the Underworld. This level is not in the demo.");
	}

	public override bool WillFireRepeatedly()
	{
		return true;
	}
}
