using UnityEngine;

public class CnvStack
{
	public int[] StackValues;

	public int stackptr = 0;

	public int instrp = 0;

	public int basep = 0;

	public int TopValue = 0;

	public int result_register;

	public int call_level = 1;

	public CnvStack(int stackSize)
	{
		StackValues = new int[stackSize];
		stackptr = 0;
		result_register = 1;
		instrp = 0;
		TopValue = 0;
		call_level = 1;
		for (int i = 0; i < StackValues.GetUpperBound(0); i++)
		{
			StackValues[i] = 0;
		}
	}

	public int Pop()
	{
		stackptr--;
		if (stackptr > StackValues.GetUpperBound(0))
		{
			Debug.Log("POP Stack out of bounds- At (" + stackptr + ") instrp:" + instrp);
			return 0;
		}
		if (stackptr < 0)
		{
			Debug.Log("POP Stack out of bounds (neg)- At (" + stackptr + ") instrp:" + instrp);
			return 0;
		}
		int result = StackValues[stackptr];
		StackValues[stackptr] = 0;
		if (stackptr - 1 >= 0)
		{
			TopValue = StackValues[stackptr - 1];
		}
		else
		{
			TopValue = 0;
		}
		return result;
	}

	public void Push(int newValue)
	{
		StackValues[stackptr++] = newValue;
		TopValue = newValue;
	}

	public void Set(int index, int val)
	{
		StackValues[index] = val;
	}

	public int get_stackp()
	{
		return stackptr;
	}

	public void set_stackp(int ptr)
	{
		stackptr = ptr;
		if (stackptr - 1 >= 0)
		{
			TopValue = StackValues[stackptr - 1];
		}
		else
		{
			TopValue = 0;
		}
	}

	public int at(int index)
	{
		if (index > StackValues.GetUpperBound(0))
		{
			Debug.Log("Stack out of bounds- At (" + index + ")");
			return 0;
		}
		if (index < 0)
		{
			Debug.Log("Stack out of bounds (neg)- At (" + index + ")");
			return 0;
		}
		return StackValues[index];
	}

	public int Upperbound()
	{
		return StackValues.GetUpperBound(0);
	}
}
