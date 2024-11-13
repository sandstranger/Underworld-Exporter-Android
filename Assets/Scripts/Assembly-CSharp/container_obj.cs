public class container_obj : object_base
{
	public override bool use()
	{
		return GetComponent<Container>().use();
	}

	public override float GetWeight()
	{
		return GetComponent<Container>().GetWeight(base.GetWeight());
	}

	public override string UseVerb()
	{
		return "open";
	}

	public override string UseObjectOnVerb_Inv()
	{
		return "place object in container";
	}

	public override bool DropEvent()
	{
		base.DropEvent();
		return GetComponent<Container>().DropEvent();
	}

	public override bool PickupEvent()
	{
		base.PickupEvent();
		return GetComponent<Container>().PickupEvent();
	}
}
