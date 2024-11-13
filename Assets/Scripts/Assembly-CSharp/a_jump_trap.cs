using UnityEngine;

public class a_jump_trap : trap_base
{
	private Vector3 impact = Vector3.zero;

	private const float mass = 3f;

	public float force = 60f;

	public float Timer;

	public CharacterController current;

	protected override void Start()
	{
		base.gameObject.layer = LayerMask.NameToLayer("NPCTraps");
		base.Start();
		BoxCollider component = GetComponent<BoxCollider>();
		if (component != null)
		{
			component.center = new Vector3(0.6f, 0.08f, 0.6f);
			component.size = new Vector3(1.2f, 0.4f, 1.2f);
			component.isTrigger = true;
		}
	}

	public override void ExecuteTrap(object_base src, int triggerX, int triggerY, int State)
	{
		if (Timer <= 0f && current == null)
		{
			ExecuteJumpOnController(UWCharacter.Instance.transform);
		}
	}

	private void ExecuteJumpOnController(Transform t)
	{
		current = t.gameObject.GetComponent<CharacterController>();
		Timer = 2f;
		AddImpact(2f * t.forward + 5f * Vector3.up, (float)base.quality * 2f);
	}

	private void AddImpact(Vector3 dir, float force)
	{
		dir.Normalize();
		if (dir.y < 0f)
		{
			dir.y = 0f - dir.y;
		}
		impact += dir.normalized * force / 3f;
	}

	public override void Update()
	{
		ApplyToController();
		if (Timer > 0f)
		{
			Timer -= Time.deltaTime;
			if (Timer < 0f)
			{
				Timer = 0f;
			}
		}
	}

	private void ApplyToController()
	{
		if ((double)impact.magnitude > 0.2 && current != null)
		{
			current.Move(impact * Time.deltaTime);
		}
		else
		{
			current = null;
		}
		impact = Vector3.Lerp(impact, Vector3.zero, 5f * Time.deltaTime);
	}

	public override void PostActivate(object_base src)
	{
		Debug.Log("Overridden PostActivate to test " + base.name);
		base.PostActivate(src);
	}

	protected virtual void OnTriggerEnter(Collider other)
	{
		if (current == null)
		{
			ExecuteJumpOnController(other.transform);
		}
	}
}
