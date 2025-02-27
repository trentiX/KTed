using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSyncSpawn : AudioSyncer 
{
	public override void OnUpdate()
	{
		base.OnUpdate();
	}

	public override void OnBeat()
	{
		base.OnBeat();

		StartCoroutine(RythmGame.instance.SpawnArrow(0));
	}
}