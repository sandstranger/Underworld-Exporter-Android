using System;

public class ObjectLoaderInfo : UWClass
{
	public int index;

	public int item_id;

	public short flags;

	public short enchantment;

	public short doordir;

	public short invis;

	public short is_quant;

	public int texture;

	public short zpos;

	public short heading;

	public short xpos;

	public short ypos;

	public short quality;

	public int next;

	public short owner;

	public int link;

	public short npc_hp;

	public short ProjectileHeadingMinor;

	public short ProjectileHeadingMajor;

	public short MobileUnk01;

	public short npc_goal;

	public short npc_gtarg;

	public short MobileUnk02;

	public short npc_level;

	public short MobileUnk03;

	public short MobileUnk04;

	public short npc_talkedto;

	public short npc_attitude;

	public short MobileUnk05;

	public short npc_height;

	public short MobileUnk06;

	public short MobileUnk07;

	public short MobileUnk08;

	public short MobileUnk09;

	public short Projectile_Speed;

	public short Projectile_Pitch;

	public short Projectile_Sign;

	public short npc_voidanim;

	public short MobileUnk11;

	public short MobileUnk12;

	public short npc_yhome;

	public short npc_xhome;

	public short npc_heading;

	public short MobileUnk13;

	public short npc_hunger;

	public short MobileUnk14;

	public short npc_whoami;

	public short npc_health = 0;

	public short npc_arms = 0;

	public short npc_power = 0;

	public short npc_name = 0;

	public short InUseFlag;

	public short levelno;

	public short ObjectTileX;

	public short ObjectTileY;

	public long address;

	public ObjectInteraction instance;

	public ObjectLoader parentList;

	public int ObjectClass;

	public int ObjectSubClass;

	public int ObjectSubClassIndex;

	public int Angle1;

	public int Angle2;

	public int Angle3;

	public int sprite;

	public int State;

	public int unk1;

	public int[] shockProperties = new int[10];

	public int[] conditions = new int[4];

	public int TriggerAction;

	public int TriggerOnce;

	public Guid guid;

	public int GetItemType()
	{
		return GameWorldController.instance.objectMaster.objProp[item_id].type;
	}

	public static void CleanUp(ObjectLoaderInfo obj)
	{
		obj.item_id = 0;
		obj.flags = 0;
		obj.enchantment = 0;
		obj.doordir = 0;
		obj.invis = 0;
		obj.is_quant = 0;
		obj.zpos = 0;
		obj.xpos = 0;
		obj.ypos = 0;
		obj.heading = 0;
		obj.quality = 0;
		obj.next = 0;
		obj.owner = 0;
		obj.link = 0;
	}

	public void Set()
	{
		InUseFlag = 1;
	}

	public void Unset()
	{
		InUseFlag = 0;
	}

	public bool isInUse()
	{
		return InUseFlag == 1;
	}

	public string getDesc()
	{
		return GameWorldController.instance.objectMaster.objProp[item_id].desc;
	}

	public int useSpriteValue()
	{
		return GameWorldController.instance.objectMaster.objProp[item_id].useSprite;
	}

	public int StartFrameValue()
	{
		return GameWorldController.instance.objectMaster.objProp[item_id].startFrame;
	}
}
