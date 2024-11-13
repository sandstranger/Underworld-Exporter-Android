using System.Collections;
using UnityEngine;

public class Repulsor : MonoBehaviour
{
	public bool RepulsorOn;

	public bool PlayerInside;

	public float TargetHeight;

	public Vector3 TargetPos;

	public bool PlayerStillInside;

	public Collider play;

	public float TravelSpeed = 1f;

	public Vector3 StartPos;

	public Vector3 EndPos;

	public float index;

	private void Start()
	{
		TargetPos = base.transform.position;
		CalcTargetZ();
		play = GameObject.Find("Gronk").GetComponent<Collider>();
	}

	private void Update()
	{
		if (PlayerStillInside)
		{
			PlayerStillInside = false;
			StartMove();
		}
	}

	private void Activate()
	{
		RepulsorOn = !RepulsorOn;
		if (PlayerInside)
		{
			Debug.Log("Cancelling");
			StopCoroutine("MovePlayer");
			StartMove();
		}
	}

	private void CalcTargetZ()
	{
		if (RepulsorOn)
		{
			Vector3 vector = new Vector3(0f, TargetHeight * 1.2f, 0f);
			TargetPos = base.transform.position + vector;
		}
		else
		{
			TargetPos = base.transform.position;
		}
	}

	private IEnumerator MovePlayer(GameObject player, Vector3 dist, float traveltime)
	{
		float rate = 1f / traveltime;
		index = 0f;
		StartPos = player.transform.position;
		EndPos = StartPos + dist;
		while (StartPos.y != EndPos.y)
		{
			if (PlayerInside)
			{
				Vector3 nextPosition = new Vector3(player.transform.position.x, Mathf.Lerp(StartPos.y, EndPos.y, index), player.transform.position.z);
				player.transform.position = nextPosition;
				index += rate * Time.deltaTime;
				yield return new WaitForSeconds(0.01f);
			}
			else
			{
				yield return 0;
			}
		}
		if (PlayerInside)
		{
			PlayerStillInside = true;
		}
	}

	private void StartMove()
	{
		CalcTargetZ();
		float num = Mathf.Abs(play.transform.position.y - TargetPos.y) * TravelSpeed;
		if (!RepulsorOn)
		{
			num *= 4f;
		}
		StartCoroutine(MovePlayer(play.gameObject, TargetPos - play.transform.position, num));
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.name == UWCharacter.Instance.name)
		{
			PlayerInside = true;
			StartMove();
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.name == UWCharacter.Instance.name)
		{
			play = other;
			PlayerInside = false;
			PlayerStillInside = false;
		}
	}
}
