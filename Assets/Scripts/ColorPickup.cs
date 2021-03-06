﻿using UnityEngine;
using System.Collections;
using Assets.Scripts.Enums;

/*
 * Class for picking up colors
 */
namespace Assets.Scripts
{
    public class ColorPickup : MonoBehaviour
    {
        //amount to add to color
        private float _amount = 50;
        //what kind of color pickup
        private ColorElement _color;

		public bool _isCollected = false;

        void Start()
        {
            //color the orb based on the assign color; stick with rgb for now
            if (_color.Equals(ColorElement.Red)) GetComponent<Renderer>().material.color = Color.red;
            else if (_color.Equals(ColorElement.Green)) GetComponent<Renderer>().material.color = Color.green;
            else if (_color.Equals(ColorElement.Blue)) GetComponent<Renderer>().material.color = Color.blue;

            else Debug.LogError("Colors for orbs should just be rgb");

			this.GetComponentInChildren<ParticleSystem>().startColor = CustomColor.GetColor(_color);

        }

		void Update()
		{
			if(!Data.GameManager.SuspendedState)
			{
				if(!this.GetComponentInChildren<ParticleSystem>().isPlaying && _isCollected)
				{
					this.GetComponent<Util.Destroyer>().DestroyImmediate();
				}
			}
		}

        //pickup collides with something
        void OnCollisionEnter2D(Collision2D col)
        {
			if(!Data.GameManager.SuspendedState)
			{
	            //if the pickup hits the ground
	            if (col.transform.tag.Equals("ground") || col.transform.tag.Equals("platform"))
	            {
	                //if falling down
	                if (GetComponent<Rigidbody2D>().velocity.y <= 0 && col.transform.position.y < transform.position.y)
	                {
	                    //stop velocity
	                    GetComponent<Rigidbody2D>().velocity = Vector2.zero;
	                    GetComponent<Rigidbody2D>().gravityScale = 0;
	                }
				}
            }
        }

		public void Collected()
		{
			_isCollected = true;
			this.GetComponentInChildren<ParticleSystem>().Play();
			Destroy (this.GetComponent<CircleCollider2D>());
			GetComponent<Rigidbody2D>().velocity = Vector2.zero;
			GetComponent<Rigidbody2D>().gravityScale = 0;
			this.GetComponent<Renderer>().enabled = false;
		}

        public float Amount
        {
            get { return _amount; }
            set { _amount = value; }
        }

        public ColorElement ColorType
        {
            get { return _color; }
            set { _color = value; }
        }
    }
}
