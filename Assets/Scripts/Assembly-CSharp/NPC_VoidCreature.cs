public class NPC_VoidCreature : NPC
{
	protected override void Start()
	{
		base.Start();
		playAnimation(GetAnimIndex(), false);
	}

	public override void Update()
	{
	}

	public override bool LookAt()
	{
		string @string = StringController.instance.GetString(1, StringController.str_you_see_);
		if (UWEBase._RES == "UW2")
		{
			string text = StringController.instance.GetString(1, 277 + base.npc_voidanim).Replace("_", " ");
			UWHUD.instance.MessageScroll.Add(@string + text);
			return true;
		}
		return base.LookAt();
	}

	private int GetAnimIndex()
	{
		int result = 0;
		if (UWEBase._RES == "UW2")
		{
			switch (base.npc_voidanim)
			{
			case 0:
				result = 10;
				break;
			case 1:
				result = 18;
				break;
			case 2:
				result = 1;
				break;
			case 3:
				result = 0;
				break;
			case 4:
				result = 3;
				break;
			case 5:
				result = 5;
				break;
			case 6:
				result = 6;
				break;
			case 7:
				result = 8;
				break;
			}
		}
		else
		{
			switch (base.npc_whoami)
			{
			case 240:
			case 241:
			case 242:
				result = 10;
				break;
			case 243:
			case 244:
				result = 0;
				break;
			case 245:
				result = 20;
				break;
			case 246:
				result = 32;
				break;
			case 247:
				result = 34;
				break;
			}
		}
		return result;
	}
}
