public class traptrigger_base : object_base
{
	public virtual bool WillFire()
	{
		return ((base.flags >> 2) & 1) == 1;
	}

	public virtual bool WillFireRepeatedly()
	{
		return ((base.flags >> 1) & 1) == 1;
	}
}
