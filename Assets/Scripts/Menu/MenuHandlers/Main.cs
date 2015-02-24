﻿using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Menu.MenuHandlers
{
    class Main : ButtonHandler
    {
        public GameObject[] cursors;

        private MainStateMachine machine = new MainStateMachine();
        private delegate void state();
        private state[] doState;
        private MainStateMachine.main currState;

        void Start()
        {
            doState = new state[] { Sleep, Play, Settings, Credits };
        }

        void Update()
        {
            MainStateMachine.main prevState = currState;
            currState = machine.update();
            if (prevState != currState)
            {
                foreach (GameObject g in cursors)
                    g.SetActive(false);
                int cursor = (int)currState - 1;
                if (cursor >= 0)
                    cursors[cursor].SetActive(true);
            }
            doState[(int)currState]();

        }

        public override void wake()
        {
            machine.wake();
            foreach (GameObject g in cursors)
                g.SetActive(false);
            int cursor = (int)currState - 1;
            if (cursor >= 0)
                cursors[cursor].SetActive(true);
        }

        public override void sleep()
        {
            machine.sleep();
        }

        private static void Sleep()
        {
        }

        private static void Play()
        {
            if (CustomInput.AcceptFreshPress)
                doPlay();
        }
        private static void doPlay()
        {
            Application.LoadLevel(2);
        }

        private static void Settings()
        {
            if (CustomInput.AcceptFreshPress)
                doSettings();
        }
        private static void doSettings()
        {
            Debug.Log("settings");
        }

        private static void Credits()
        {
            if (CustomInput.AcceptFreshPress)
                doCredits();
        }
        private static void doCredits()
        {
            Application.LoadLevel(2);
        }

        public void PlayClick()
        {
            if (currState == MainStateMachine.main.sleep)
                Kernel.interrupt(isLeft);
            machine.goTo(MainStateMachine.main.play);
            foreach (GameObject g in cursors)
                g.SetActive(false);
            cursors[(int)MainStateMachine.main.play - 1].SetActive(true);
            doPlay();
        }

        public void SettingsClick()
        {
            if (currState == MainStateMachine.main.sleep)
                Kernel.interrupt(isLeft);
            machine.goTo(MainStateMachine.main.settings);
            foreach (GameObject g in cursors)
                g.SetActive(false);
            cursors[(int)MainStateMachine.main.settings - 1].SetActive(true);
            doSettings();
        }

        public void CreditsClick()
        {
            if (currState == MainStateMachine.main.sleep)
                Kernel.interrupt(isLeft);
            machine.goTo(MainStateMachine.main.credits);
            foreach (GameObject g in cursors)
                g.SetActive(false);
            cursors[(int)MainStateMachine.main.credits - 1].SetActive(true);
            doCredits();
        }
    }
    class MainStateMachine
    {
        internal enum main { sleep, play, settings, credits };
        private delegate main machine();//function pointer
        private machine[] getNextState;//array of function pointers
        private main currState;
        private static float hold = 0;//used for delays
        private static bool die = false;
        private static bool doubleJumped = false;
        private main sleepState = main.play;

        internal MainStateMachine()
        {
            currState = main.sleep;
            //fill array with functions
            getNextState = new machine[] { Sleep, Play, Settings, Credits };
        }

        internal main update()
        {
            return currState = getNextState[((int)currState)]();//gets te next Enums.PlayerState
        }

        internal void wake()
        {
            currState = sleepState;
        }

        internal void sleep()
        {
            sleepState = currState;
            currState = main.sleep;
        }

        internal void goTo(main state)
        {
            currState = state;
        }

        //The following methods control when and how you can transition between states
        private static main Sleep()
        {
            return main.sleep;
        }

        private static main Play()
        {
            if (CustomInput.UpFreshPress)
                return main.credits;
            if (CustomInput.DownFreshPress)
                return main.settings;
            return main.play;
        }
        private static main Settings()
        {
            if (CustomInput.UpFreshPress)
                return main.play;
            if (CustomInput.DownFreshPress)
                return main.credits;
            return main.settings;
        }
        private static main Credits()
        {
            if (CustomInput.UpFreshPress)
                return main.settings;
            if (CustomInput.DownFreshPress)
                return main.play;
            return main.credits;
        }
    }
}
