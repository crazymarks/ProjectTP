using System.Collections.Generic;
using UnityEngine;

public class TimeHelper {
	public float time = 0;

	public TimeHelper ()
	{
		time = Time.realtimeSinceStartup;
	}

	static public TimeHelper Create()
	{
		return(new TimeHelper());
	}

	public float Get()
	{
		return((Time.realtimeSinceStartup - time) * 1000);
	}
}
