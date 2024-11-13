using UnityEngine;

[RequireComponent(typeof(CharacterMotorC))]
[AddComponentMenu("Character/FPS Input Controller C")]
public class FPSInputControllerC : MonoBehaviour
{
	private CharacterMotorC cmotor;

	private void Awake()
	{
		cmotor = GetComponent<CharacterMotorC>();
	}

	private void Update()
	{
		Vector3 vector = new Vector3(Input.GetAxis("Horizontal"), 0.5f, Input.GetAxis("Vertical"));
		if (vector != Vector3.zero)
		{
			float magnitude = vector.magnitude;
			vector /= magnitude;
			magnitude = Mathf.Min(1f, magnitude);
			magnitude *= magnitude;
			vector *= magnitude;
		}
		cmotor.inputMoveDirection = base.transform.rotation * vector;
		cmotor.inputJump = Input.GetButton("Jump");
	}
}
