using UnityEngine;

public class Sign : Decal
{
	protected override void Start()
	{
		base.Start();
		BoxCollider component = GetComponent<BoxCollider>();
		component.size = new Vector3(0.3f, 0.3f, 0.1f);
		component.center = new Vector3(0f, 0.15f, 0f);
		setSpriteTMOBJ(GetComponentInChildren<SpriteRenderer>(), 20 + (base.flags & 7));
	}

	public override bool use()
	{
		return Read();
	}

	public override bool LookAt()
	{
		return Read();
	}

	public virtual bool Read()
	{
		string rES = UWEBase._RES;
		if (rES != null && rES == "UW2")
		{
			if (base.isquant == 1)
			{
				UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(8, base.link - 512));
				return true;
			}
			ObjectInteraction objectIntAt = ObjectLoader.getObjectIntAt(base.link);
			if (objectIntAt != null)
			{
				switch (objectIntAt.GetItemType())
				{
				case 56:
					return objectIntAt.GetComponent<trigger_base>().Activate(base.gameObject);
				case 57:
					return objectIntAt.GetComponent<trigger_base>().Activate(base.gameObject);
				default:
					UWHUD.instance.MessageScroll.Add("You need to investigate me " + base.name);
					return true;
				}
			}
			return false;
		}
		UWHUD.instance.MessageScroll.Add(StringController.instance.GetString(8, base.link - 512));
		return true;
	}

	public override string UseVerb()
	{
		return "read";
	}

	public override string ExamineVerb()
	{
		return "read";
	}
}
