﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Assets.Scripts.Data;
/*
 * 
 */
namespace Assets.Scripts.UI
{
	public class Credits : MonoBehaviour
	{
		//list of the lines in credits
		private List<string> _credits;
		//list of the gamobjects representing text
		private List<GameObject> _creditObjects;

		//empty prefab to load into
		public GameObject _creditTextItem;

		//starting location of the credits
		public Transform _start;

		//spaceing between lines
		private float _spacing = -2.5f;
		//speed for the text to flow upward
		private float _speed = 2f;

		public TextAsset _file;



		void Start () 
		{
			//init lists
			_credits = new List<string>();
			_creditObjects = new List<GameObject>();

			string[] _arr = _file.text.Split(new char[] {'\n'});
			for(int i = 0; i < _arr.Length; i++)
			{
				_credits.Add(_arr[i]);
			}

			//init the credits
			this.CreateCredits();
		}
		
		void Update () 
		{
			if(!GameManager.Paused)
			{
				//if list is not empty
				if (_creditObjects.Count > 0)
				{
					//update the position upward
					for (int i = 0; i < _creditObjects.Count; i++)
					{
						_creditObjects[i].transform.Translate(Vector2.up * _speed * Time.deltaTime);
					}
				}
			}
		}

		//inits or reinits the text objects
		public void CreateCredits()
		{
			//for each line of text
			for (int i = 0; i < _credits.Count; i++)
			{
				//get the line of text
				string _c = _credits[i];
				//create new text object
				GameObject _text = (GameObject)GameObject.Instantiate(_creditTextItem, _start.position + (new Vector3(0, _spacing * i, 0)), Quaternion.identity);
				//formatting text
				if(_c.EndsWith(" <Title>"))
				{
					//make biggest
					_text.GetComponent<TextMesh>().fontSize = 125;
					//don't include the title tag in the name
					_text.GetComponent<TextMesh>().text = _c.Substring(0, _c.LastIndexOf(" <Title>"));
					//change text color
					_text.GetComponent<TextMesh>().color = Color.black;
				}
				//formatting text
				else if(_c.EndsWith(" <Position>"))
				{
					//make smaller than title
					_text.GetComponent<TextMesh>().fontSize = 100;
					//don't include the position tag in the name
					_text.GetComponent<TextMesh>().text = _c.Substring(0, _c.LastIndexOf(" <Position>"));
					//change text color
					_text.GetComponent<TextMesh>().color = new Color(0.05f, 0.05f, 0.05f);
				}
				//text is normal name
				else
				{
					_text.GetComponent<TextMesh>().text = _c;
					//change text color
					_text.GetComponent<TextMesh>().color = new Color(0.7f, 0.7f, 0.7f);
				}

				//assign this handler to the individual object
				_text.GetComponent<CreditBehaviour>().AssignHandler(this);
				//add to the list of objects
				_creditObjects.Add(_text);
			}
		}

		//list of the text objects
		public List<GameObject> TextObjects
		{
			get{return _creditObjects;}
		}

		//removing from the front of the list
		public void RemoveFirstObject()
		{
			if(_creditObjects[0] != null) _creditObjects.RemoveAt(0);
		}
	}
}
