using System;
using UnityEngine;

public class GameClock : UWEBase
{
	public int[] gametimevals = new int[3];

	public int game_time;

	public float clockTime;

	public float clockRate = 1f;

	public int _second;

	public int _minute;

	public int _day;

	public static GameClock instance;

	private void Start()
	{
		instance = this;
	}

	private void Update()
	{
		if (ConversationVM.InConversation)
		{
			return;
		}
		clockTime += Time.deltaTime;
		if (!(clockTime >= clockRate))
		{
			return;
		}
		_second++;
		gametimevals[0]++;
		if (gametimevals[0] >= 255)
		{
			gametimevals[0] = 0;
			gametimevals[1]++;
			if (gametimevals[1] >= 255)
			{
				gametimevals[1] = 0;
				gametimevals[2]++;
				if (gametimevals[2] >= 255)
				{
					gametimevals[2] = 0;
				}
			}
		}
		clockTime = 0f;
		if (_second >= 60)
		{
			ClockTick();
			_second = 0;
		}
	}

	private static void ClockTick()
	{
		instance._minute++;
		if (instance._minute % 5 == 0)
		{
			UWCharacter.Instance.RegenMana();
			UWCharacter.Instance.UpdateHungerAndFatigue();
		}
		if (instance._minute >= 1440)
		{
			instance._minute = 0;
			instance._day++;
		}
	}

	public static void Advance()
	{
		for (int i = 0; i < 60; i++)
		{
			ClockTick();
			int num = 0;
			instance.gametimevals[0] += 60;
			if (instance.gametimevals[0] < 255)
			{
				continue;
			}
			num = Mathf.Max(0, instance.gametimevals[0] - 255);
			instance.gametimevals[0] = num;
			instance.gametimevals[1]++;
			if (instance.gametimevals[1] >= 255)
			{
				instance.gametimevals[1] = 0;
				instance.gametimevals[2]++;
				if (instance.gametimevals[2] >= 255)
				{
					instance.gametimevals[2] = 0;
				}
			}
		}
	}

	public static int AddNow(int iDay, int iHour, int iMinute)
	{
		return ConvertNow() + Convert(iDay, iHour, iMinute);
	}

	public static int DiffNow(int iDay, int iHour, int iMinute)
	{
		return Convert(iDay, iHour, iMinute) - ConvertNow();
	}

	public static int Convert(int iDay, int iHour, int iMinute)
	{
		Debug.Log("Convert no you should never use!");
		return 0;
	}

	public static int ConvertNow()
	{
		Debug.Log("Convert no you should never use!");
		return 0;
	}

	public static void setUWTime(long timevalue)
	{
		TimeSpan timeSpan = TimeSpan.FromSeconds(timevalue);
		instance._day = timeSpan.Days;
		instance._minute = timeSpan.Minutes + timeSpan.Hours * 60;
		instance._second = timeSpan.Seconds;
	}

	public static long getUWTime()
	{
		return (long)new TimeSpan(instance._day, 0, instance._minute, instance._second).TotalSeconds;
	}

	public static int second()
	{
		return instance._second;
	}

	public static int hour()
	{
		return instance._minute / 60;
	}

	public static int day()
	{
		return instance._day;
	}

	public static int minute()
	{
		return instance._minute % 24;
	}

	public static int game_min()
	{
		return instance._minute;
	}
}
