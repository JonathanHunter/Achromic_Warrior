﻿using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Util
{
    class Bullet : MonoBehaviour
    {
		private bool dest=false;
        Vector3 dir;
        //speed is in units/sec
        public float speed;
        //liftime is in sec
        public float lifeTime;

        public Vector3 Direction
        {
            get { return dir; }
            set { dir = value; }
        }

        void Start()
        {
        }

        void Update()
        {
            if (Data.GameManager.State != Enums.GameState.Pause)
            {
                //move bullet
                transform.Translate(dir * speed * Time.deltaTime);
                //check if too old
                if ((lifeTime -= Time.deltaTime) < 0 || dest)
                    Destroy(this.gameObject);
            }
        }

        //standard destory self on collision
        void OnCollisionEnter2D(Collision2D col)
        {
            dest = true;
        }
        void OnTriggerEnter2D(Collider2D col)
        {
            dest = true;
        }
    }
}
