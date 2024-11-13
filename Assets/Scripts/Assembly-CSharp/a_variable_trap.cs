public class a_variable_trap : trap_base
{
	public int value;

	protected override void Start()
	{
		base.Start();
		value = VariableValue();
	}

	public virtual int VariableValue()
	{
		return ((base.quality & 0x3F) << 8) | (((base.owner & 0x1F) << 3) | (base.ypos & 7));
	}
}
