using UnityEngine;
using UnityEngine.UI;

public class StatsDisplay : GuiBase_Draggable
{
	public Text CharName;

	public Text CharClass;

	public Text CharClassLevel;

	public Text CharStr;

	public Text CharDex;

	public Text CharInt;

	public Text CharVIT;

	public Text CharMana;

	public Text CharEXP;

	public Text CharSkills;

	public Text CharSkillLevels;

	public static int Offset;

	private string[] Skillnames = new string[20]
	{
		"ATTACK", "DEFENSE", "UNARMED", "SWORD", "AXE", "MACE", "MISSILE", "MANA", "LORE", "CASTING",
		"TRAPS", "SEARCH", "TRACK", "SNEAK", "REPAIR", "CHARM", "PICKLOCK", "ACROBAT", "APPRAISE", "SWIMMING"
	};

	public static bool UpdateNow = true;

	private string tmpSkillNames;

	private string tmpSkillValues;

	public override void Start()
	{
		base.Start();
		if (UWEBase._RES == "UW2")
		{
			CharName.color = Color.white;
			CharClass.color = Color.white;
			CharClassLevel.color = Color.white;
			CharStr.color = Color.white;
			CharDex.color = Color.white;
			CharInt.color = Color.white;
			CharVIT.color = Color.white;
			CharMana.color = Color.white;
			CharEXP.color = Color.white;
			CharSkills.color = Color.white;
			CharSkillLevels.color = Color.white;
		}
	}

	public override void Update()
	{
		base.Update();
		if (GameWorldController.instance.AtMainMenu || !UpdateNow)
		{
			return;
		}
		UpdateNow = false;
		CharName.text = UWCharacter.Instance.CharName;
		CharClass.text = StringController.instance.GetString(2, 23 + UWCharacter.Instance.CharClass);
		CharClassLevel.text = UWCharacter.Instance.CharLevel.ToString();
		CharStr.text = UWCharacter.Instance.PlayerSkills.STR.ToString();
		CharDex.text = UWCharacter.Instance.PlayerSkills.DEX.ToString();
		CharInt.text = UWCharacter.Instance.PlayerSkills.INT.ToString();
		CharVIT.text = UWCharacter.Instance.CurVIT + "/" + UWCharacter.Instance.MaxVIT;
		CharMana.text = UWCharacter.Instance.PlayerMagic.CurMana + "/" + UWCharacter.Instance.PlayerMagic.MaxMana;
		CharEXP.text = UWCharacter.Instance.EXP.ToString();
		tmpSkillNames = "";
		tmpSkillValues = "";
		if (Offset > 15)
		{
			Offset = 15;
		}
		if (Offset < 0)
		{
			Offset = 0;
		}
		for (int i = 0; i <= 5; i++)
		{
			tmpSkillNames += Skillnames[i + Offset];
			tmpSkillValues += UWCharacter.Instance.PlayerSkills.GetSkill(i + Offset + 1);
			if (i != 5)
			{
				tmpSkillNames += "\n";
				tmpSkillValues += "\n";
			}
		}
		CharSkills.text = tmpSkillNames;
		CharSkillLevels.text = tmpSkillValues;
	}
}
