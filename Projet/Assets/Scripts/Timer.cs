using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour
{
	private double seconds, minutes, hours = 0;
	public double time = 0;
	public bool notpause=true;

	void Awake()
	{
		seconds = time;
		StartCoroutine(Tick());
	}

	IEnumerator Tick()
	{
		for(;;)
		{
			yield return new WaitForSeconds(0.1f);
			if(notpause)
			{
				Tack();
			}

		}
	}

	void Tack()
	{
		time+=10;
		seconds++;

		if(seconds >= 60)
		{
			seconds -= 60;
			minutes++;
			if(minutes >= 60)
			{
				minutes -= 60;
				hours++;
			}
		}
	}

	public double getTime()
	{
		return time;
	}
}

