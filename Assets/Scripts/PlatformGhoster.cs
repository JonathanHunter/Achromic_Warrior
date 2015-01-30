﻿using UnityEngine;
using System.Collections;

/*
 * This class goes on a platform and tells the player he can jump through or not
 */
public class PlatformGhoster : MonoBehaviour
{
	void OnTriggerEnter2D(Collider2D col)
	{
		if(col.transform.tag.Equals("Player"))
		{
			col.GetComponent<PlayerController>().Ghost = true;
		}
	}

	void OnTriggerStay2D(Collider2D col)
	{
		if(col.transform.tag.Equals("Player"))
		{
			col.GetComponent<PlayerController>().Ghost = true;
		}
	}

	void OnTriggerExit2D(Collider2D col)
	{
		if(col.transform.tag.Equals("Player"))
		{
			col.GetComponent<PlayerController>().Ghost = false;
		}
	}
}
