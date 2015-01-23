﻿using UnityEngine;
using System.Collections;
using Assets.Scripts.Enums;

/*
 * Base enemy class
 */
public class Enemy : MonoBehaviour
{
	//color element associated with the enemy
	public ColorElement _color;

	//time before enemy can be hit again
	public float _hitTime = 1f;
	private float _timer;
	public bool _hit = false;

	//orbs to spawn when hit
	[HideInInspector]
	public GameObject _orb;

	//reference to the player
	public GameObject _player;

	//knockback force
	public float _force = 75f;
	
	void Start () {
		Init();
	}

	void Update()
	{
		UpdateHit();
	}

	// Produces the orbs
	public void ProduceOrbs(int _num)
	{
		for(int i = 0; i < _num; i++)
		{
			//create a new orb
			GameObject _newOrb = (GameObject)GameObject.Instantiate(_orb, transform.position, Quaternion.identity);
			//set the orb color
			_newOrb.GetComponent<ColorPickup>().ColorType = _color;
			//throw it in the air
			_newOrb.rigidbody2D.AddRelativeForce(new Vector2(Random.Range(-1f, 1f), Random.Range(5, 10)), ForceMode2D.Impulse);
		}
	}

	//Initialize the enemy
	public void Init()
	{
		//set the enemy color
		renderer.material.color = CustomColor.GetColor(_color);

		//load the orb
		_orb = (GameObject)Resources.Load("Prefabs/orb");

		//find the player
		_player = GameObject.Find("player");
	}

	//what happens when hit
	public void Hit(int _damage, Vector3 _hitPos)
	{
		//if not already hit
		if(!_hit)
		{
			//reset hit
			_hit = true;
			_timer = 0f;
			//if player is not achromic produce more orbs ------- CHANGE TO PLAYER STATE LATER
			if(!_player.GetComponent<Player>().Color.Equals(ColorElement.Black))
				ProduceOrbs(1);
			//show the damage taken
			DamageDisplay.instance.ShowDamage(_damage, _hitPos, _color);
			//apply knockback force based on position
			if(_player.GetComponent<PlayerController>()._facingRight)
				rigidbody2D.AddRelativeForce((new Vector2(1, 0))* _force * _player.GetComponent<Player>().Attack);
			else{
				rigidbody2D.AddRelativeForce((new Vector2(-1, 0))* _force * _player.GetComponent<Player>().Attack);
			}
		}
	}

	//Updates the hit
	public void UpdateHit()
	{
		if(_hit)
		{
			_timer += Time.deltaTime;
			if(_timer > _hitTime)
			{
				_hit = false;
			}
		}
	}

	//Gets or sets the color
	public ColorElement Color
	{
		get{return _color;}
		set{_color = value;}
	}
}
