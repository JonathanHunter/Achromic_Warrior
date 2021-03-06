﻿using UnityEngine;
using System.Collections;
using Assets.Scripts.Enums;
using Assets.Scripts.Util;

namespace Assets.Scripts.Data
{

	public class GameManager : MonoBehaviour
	{
		public static GameManager _instance;
		private static GameState _state = GameState.Unpause;

		void Awake()
		{
			if(_instance == null)
			{
				DontDestroyOnLoad(gameObject);
				_instance = this;
			}
			else if(_instance != this)
			{
				Destroy(gameObject);
			}
		}

		void OnLevelWasLoaded(int i)
		{
			_state = GameState.Unpause;
		}

		void Update ()
		{
			if(CustomInput.PauseFreshPress && _state.Equals(GameState.Unpause))
			{
				_state = GameState.Pause;
				Pause();
			}
			else if(CustomInput.PauseFreshPress && _state.Equals(GameState.Pause))
			{
				_state = GameState.Unpause;
				Unpause();
			}
		}

		public static void Pause()
		{
			VelocityInfo[] _bodies = GameObject.FindObjectsOfType<VelocityInfo>();
			for(int i = 0; i < _bodies.Length; i++)
			{
				_bodies[i].PauseMotion();
			}
		}

		public static void Unpause()
        {
			VelocityInfo[] _bodies = GameObject.FindObjectsOfType<VelocityInfo>();
			for(int i = 0; i < _bodies.Length; i++)
			{
				_bodies[i].UnpauseMotion();
			}
		}

        public static void GotoLevel(string level)
        {
            if (_state.Equals(GameState.Pause))
                Unpause();
            try
            {
                Application.LoadLevel(level);
            }
            catch(System.Exception e)
            {
                Debug.Log(e);
                Application.LoadLevel("Menu");
            }
        }

		public static GameState State
		{
			get{return _state;}
			set{_state = value;}
		}

        public static bool Paused
        {
            get { return _state == GameState.Pause; }
        }

		public static bool End
		{
			get{ return _state == GameState.Lose || _state == GameState.Win; }
		}

		public static bool SuspendedState
		{
			get{ return _state == GameState.Lose || _state == GameState.Win || _state == GameState.Pause; }
		}

		public static bool Game
		{
			get { return _state == GameState.Unpause; }
		}
	}
}
