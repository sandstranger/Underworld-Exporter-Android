using UnityEngine;

public class a_do_trap_conversation : a_hack_trap
{
	protected override void Start()
	{
		base.Start();
		NPC_Door nPC_Door = base.gameObject.AddComponent<NPC_Door>();
		nPC_Door.npc_whoami = 25;
	}

	public override bool Activate(object_base src, int triggerX, int triggerY, int State)
	{
		if (!ConversationVM.InConversation)
		{
			NPC_Door component = GetComponent<NPC_Door>();
			if (component != null)
			{
				Character.InteractionMode = 1;
				InteractionModeControl.UpdateNow = true;
				return component.TalkTo();
			}
		}
		return false;
	}

	public override void PostActivate(object_base src)
	{
		Debug.Log("Overridden PostActivate to test " + base.name);
		base.PostActivate(src);
	}
}
