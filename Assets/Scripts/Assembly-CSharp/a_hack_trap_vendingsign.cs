using UnityEngine;

public class a_hack_trap_vendingsign : a_hack_trap
{
	public override void ExecuteTrap(object_base src, int triggerX, int triggerY, int State)
	{
		int num = 0;
		int num2 = 0;
		switch (Quest.instance.variables[base.owner])
		{
		default:
			return;
		case 0:
			num = 182;
			num2 = 3;
			break;
		case 1:
			num = 176;
			num2 = 3;
			break;
		case 2:
			num = 187;
			num2 = 4;
			break;
		case 3:
			num = 293;
			num2 = 4;
			break;
		case 4:
			num = 188;
			num2 = 3;
			break;
		case 5:
			num = 3;
			num2 = 11;
			break;
		case 6:
			num = 257;
			num2 = 6;
			break;
		case 7:
			num = 145;
			num2 = 4;
			break;
		}
		string simpleObjectNameUW = StringController.instance.GetSimpleObjectNameUW(num);
		UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(8, 369) + simpleObjectNameUW + StringController.instance.GetString(1, 349) + "(" + num2 + " gp)");
	}

	public override void PostActivate(object_base src)
	{
		Debug.Log("Overridden PostActivate to test " + base.name);
		base.PostActivate(src);
	}
}
