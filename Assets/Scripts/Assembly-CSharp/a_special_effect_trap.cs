using System.Collections;
using UnityEngine;

public class a_special_effect_trap : trap_base
{
	public override void ExecuteTrap(object_base src, int triggerX, int triggerY, int State)
	{
		switch (base.quality)
		{
		case 2:
			UWCharacter.Instance.aud.clip = MusicController.instance.SoundEffects[49];
			UWCharacter.Instance.aud.Play();
			return;
		case 4:
			CameraShake.instance.ShakeEarthQuake((float)base.owner * 0.05f);
			return;
		case 5:
			StartCoroutine(Flash("FadeToRed"));
			return;
		}
		Debug.Log("unimplemented special effect " + base.name + " q=" + base.quality);
	}

	private IEnumerator Flash(string anim)
	{
		UWHUD.instance.EnableDisableControl(UWHUD.instance.CutsceneFullPanel.gameObject, true);
		UWHUD.instance.CutScenesSmall.anim.SetAnimation = anim;
		yield return new WaitForSeconds(0.2f);
		UWHUD.instance.CutScenesSmall.anim.SetAnimation = "Anim_Base";
		UWHUD.instance.EnableDisableControl(UWHUD.instance.CutsceneFullPanel.gameObject, false);
	}
}
