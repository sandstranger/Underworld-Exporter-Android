using UnityEngine;
using UnityEngine.UI;

public class Power : GuiBase
{
	public float PreviousCharge = -1f;

	public int RepeatCounter = 0;

	public int PreviousIndex = -1;

	public RawImage uiPowerGem;

	private Texture2D[] PowerGemArt = new Texture2D[14];

	public override void Start()
	{
		base.Start();
		GRLoader gRLoader = new GRLoader(25);
		if (uiPowerGem == null)
		{
			uiPowerGem = base.gameObject.GetComponent<RawImage>();
		}
		PowerGemArt = new Texture2D[gRLoader.NoOfFileImages()];
		for (int i = 0; i <= PowerGemArt.GetUpperBound(0); i++)
		{
			PowerGemArt[i] = gRLoader.LoadImageAt(i);
		}
		if (UWEBase._RES == "UW2")
		{
			uiPowerGem.GetComponent<GuiBase_Draggable>().rectT = new RectTransform[2];
			uiPowerGem.GetComponent<GuiBase_Draggable>().rectT[0] = GetComponent<RectTransform>();
			uiPowerGem.GetComponent<GuiBase_Draggable>().rectT[1] = UWHUD.instance.HudCompass.GetComponent<RectTransform>();
		}
	}

	public override void Update()
	{
		base.Update();
		if (PreviousCharge != UWCharacter.Instance.PlayerCombat.Charge || UWCharacter.Instance.PlayerCombat.AttackCharging)
		{
			PreviousCharge = UWCharacter.Instance.PlayerCombat.Charge;
			int num = (int)UWCharacter.Instance.PlayerCombat.Charge / 10;
			if (num == 10)
			{
				if (!IsInvoking("UpdateMaxCharge"))
				{
					InvokeRepeating("UpdateMaxCharge", 0f, 0.1f);
				}
			}
			else if (num != PreviousIndex)
			{
				RepeatCounter = 0;
				uiPowerGem.texture = PowerGemArt[num];
			}
		}
		else
		{
			if (IsInvoking("UpdateMaxCharge"))
			{
				CancelInvoke("UpdateMaxCharge");
				uiPowerGem.texture = PowerGemArt[0];
			}
			RepeatCounter = 0;
		}
	}

	public void UpdateMaxCharge()
	{
		if (UWEBase._RES != "UW2")
		{
			uiPowerGem.texture = PowerGemArt[10 + RepeatCounter];
		}
		else
		{
			uiPowerGem.texture = PowerGemArt[10];
		}
		RepeatCounter++;
		if (RepeatCounter > 3)
		{
			RepeatCounter = 0;
		}
	}
}
