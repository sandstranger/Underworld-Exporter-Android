using System.Collections;

public class Combat : UWEBase
{
	protected float weaponRange = 1f;

	public bool AttackCharging;

	public bool AttackExecuting;

	public float Charge;

	public float chargeRate = 33f;

	public virtual void PlayerCombatIdle()
	{
	}

	public virtual void CombatBegin()
	{
	}

	public virtual void CombatCharging()
	{
	}

	public virtual void ExecuteRanged(float charge)
	{
	}

	public virtual void ReleaseAttack()
	{
	}

	public virtual IEnumerator ExecuteMelee(string StrikeType, float StrikeCharge)
	{
		yield break;
	}
}
