using UnityEngine;
using System.Collections;
using UnityEngine.Serialization;

namespace UnderworldExporter.Game
{
	/// MouseLook rotates the transform based on the mouse delta.
	/// Minimum and Maximum values can be used to constrain the possible rotation

	/// To make an FPS style character:
	/// - Create a capsule.
	/// - Add the MouseLook script to the capsule.
	///   -> Set the mouse look to use LookX. (You want to only turn character but not tilt it)
	/// - Add FPSInputController script to the capsule
	///   -> A CharacterMotor and a CharacterController component will be automatically added.

	/// - Create a camera. Make the camera a child of the capsule. Reset it's transform.
	/// - Add a MouseLook script to the camera.
	///   -> Set the mouse look to use LookY. (You want the camera to tilt up and down like a head. The character already turns.)
	[AddComponentMenu("Camera-Control/Mouse Look")]
	public class MouseLook : MonoBehaviour
	{
		public enum RotationAxes
		{
			MouseXAndY = 0,
			MouseX = 1,
			MouseY = 2
		}

		public RotationAxes axes = RotationAxes.MouseXAndY;

		public float sensitivityX = 0.3F;
		public float sensitivityY = 0.3F;

		public float touchSensitivityX = 15F;
		public float touchSensitivityY = 15F;

		public float gamepadSensitivityX = 5F;
		public float gamepadSensitivityY = 5F;

		public float gyroscopeSensitivityX = 2F;
		public float gyroscopeSensitivityY = 1F;

		public float minimumX = -360F;
		public float maximumX = 360F;

		public float minimumY = -60F;
		public float maximumY = 60F;

		float rotationY = 0F;

		void Update()
		{
			if (axes == RotationAxes.MouseX)
			{
				transform.Rotate(0, InputManager.Look.x * GetSensitivityX(), 0);
			}
			else
			{				
				rotationY += InputManager.Look.y * GetSensitivityY();
				rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);
				transform.localEulerAngles = new Vector3(-rotationY, transform.localEulerAngles.y, 0);
			}
		}

		void Start()
		{
			// Make the rigid body not change rotation
			if (GetComponent<Rigidbody>())
				GetComponent<Rigidbody>().freezeRotation = true;
		}

		private float GetSensitivityX()
		{
			if (InputManager.EnableGyroscope)
			{
				return gyroscopeSensitivityX;
			}
			
			var currentInputType = InputManager.CurrentInputType;
			
			if (currentInputType == InputManager.InputType.Touch)
			{
				return touchSensitivityX;
			}

			if (currentInputType == InputManager.InputType.Gamepad)
			{
				return gamepadSensitivityX;
			}
			
			return sensitivityX;
		}

		private float GetSensitivityY()
		{
			if (InputManager.EnableGyroscope)
			{
				return gyroscopeSensitivityY;
			}
			
			var currentInputType = InputManager.CurrentInputType;

			if (currentInputType == InputManager.InputType.Touch)
			{
				return touchSensitivityY;
			}
			
			if (currentInputType == InputManager.InputType.Gamepad)
			{
				return gamepadSensitivityY;
			}

			return sensitivityY;
		}
	}
}