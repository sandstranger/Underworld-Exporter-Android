using System;
using UnityEngine;
using System.Collections;

// Require a character controller to be attached to the same game object
[RequireComponent (typeof (CharacterMotorC))]

//RequireComponent (CharacterMotor)
[AddComponentMenu("Character/FPS Input Controller C")]
//@script AddComponentMenu ("Character/FPS Input Controller")


public class FPSInputControllerC : MonoBehaviour
{
    private UWCharacter _uwCharacter;
    private CharacterMotorC cmotor;
    // Use this for initialization
    void Awake() {
        cmotor = GetComponent<CharacterMotorC>();
        _uwCharacter = GetComponent<UWCharacter>();
    }
    
    public void Jump(bool jump)
    {
        cmotor.inputJump = jump;
    }
    
    public void MoveCharacter(Vector3 directionVector)
    {
        if (directionVector != Vector3.zero) {
            // Get the length of the directon vector and then normalize it
            // Dividing by the length is cheaper than normalizing when we already have the length anyway
            float directionLength = directionVector.magnitude;
            directionVector = directionVector * _uwCharacter.MoveSpeed / directionLength;

            // Make sure the length is no bigger than 1
            directionLength = Mathf.Min(1 , directionLength);

            // Make the input vector more sensitive towards the extremes and less sensitive in the middle
            // This makes it easier to control slow speeds when using analog sticks
            directionLength = directionLength * directionLength;

            // Multiply the normalized direction vector by the modified length
            directionVector = directionVector * directionLength;
        }

        if (_uwCharacter.NoClipEnabled)
        {
            transform.Translate(directionVector * Time.deltaTime * 3f);
        }
        else
        {
            // Apply the direction to the CharacterMotor
            cmotor.inputMoveDirection = transform.rotation * directionVector;
        }
    }
}