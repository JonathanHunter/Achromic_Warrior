﻿using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Menu.MenuHandlers
{
    class Settings : ButtonHandler
    {
        public GameObject[] cursors;

        private SettingsStateMachine machine = new SettingsStateMachine();
        private delegate void state();
        private state[] doState;
        private SettingsStateMachine.setting currState;

        private static bool isLeft;
        public override void setLeft()
        {
            isLeft = true;
        }

        public override void setRight()
        {
            isLeft = false;
        }

        void Start()
        {
            doState = new state[] { Sleep, Audio, Video, Controls, Exit };
        }

        void Update()
        {
            SettingsStateMachine.setting prevState = currState;
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

        private static void Audio()
        {
            if (CustomInput.AcceptFreshPressDeleteOnRead)
                doAudio();
        }
        private static void doAudio()
        {
            Kernel.transition(true, isLeft, 0);
        }

        private static void Video()
        {
            if (CustomInput.AcceptFreshPressDeleteOnRead)
                doVideo();
        }
        private static void doVideo()
        {
            Kernel.transition(true, isLeft, 1);
        }

        private static void Controls()
        {
            if (CustomInput.AcceptFreshPressDeleteOnRead)
                doControls();
        }
        private static void doControls()
        {
            Kernel.transition(true, isLeft, 2);
        }

        private static void Exit()
        {
            if (CustomInput.AcceptFreshPressDeleteOnRead)
                doExit();
        }
        private static void doExit()
        {
            Kernel.transition(false, isLeft, 0);
        }

        public void AudioClick()
        {
            if (currState == SettingsStateMachine.setting.sleep)
                Kernel.interrupt(isLeft);
            machine.goTo(SettingsStateMachine.setting.audio);
            foreach (GameObject g in cursors)
                g.SetActive(false);
            cursors[(int)SettingsStateMachine.setting.audio - 1].SetActive(true);
            doAudio();
        }

        public void VideoClick()
        {
            if (currState == SettingsStateMachine.setting.sleep)
                Kernel.interrupt(isLeft);
            machine.goTo(SettingsStateMachine.setting.video);
            foreach (GameObject g in cursors)
                g.SetActive(false);
            cursors[(int)SettingsStateMachine.setting.video - 1].SetActive(true);
            doVideo();
        }

        public void ControlsClick()
        {
            if (currState == SettingsStateMachine.setting.sleep)
                Kernel.interrupt(isLeft);
            machine.goTo(SettingsStateMachine.setting.controls);
            foreach (GameObject g in cursors)
                g.SetActive(false);
            cursors[(int)SettingsStateMachine.setting.controls - 1].SetActive(true);
            doControls();
        }
        public void ExitClick()
        {
            if (currState == SettingsStateMachine.setting.sleep)
                Kernel.interrupt(isLeft);
            machine.goTo(SettingsStateMachine.setting.exit);
            foreach (GameObject g in cursors)
                g.SetActive(false);
            //cursors[(int)SettingsStateMachine.setting.exit - 1].SetActive(true);
            doExit();
        }
    }
    class SettingsStateMachine
    {
        internal enum setting { sleep, audio, video, controls, exit };
        private delegate setting machine();//function pointer
        private machine[] getNextState;//array of function pointers
        private setting currState;
        private setting sleepState = setting.audio;

        internal SettingsStateMachine()
        {
            currState = setting.sleep;
            //fill array with functions
            getNextState = new machine[] { Sleep, Audio, Video, Controls, Exit };
        }

        internal setting update()
        {
            return currState = getNextState[((int)currState)]();
        }

        internal void wake()
        {
            currState = sleepState;
        }

        internal void sleep()
        {
            if (currState != setting.sleep)
            {
                sleepState = currState;
                currState = setting.sleep;
            }
        }

        internal void goTo(setting state)
        {
            currState = state;
        }

        //The following methods control when and how you can transition between states
        private static setting Sleep()
        {
            return setting.sleep;
        }

        private static setting Audio()
        {
			if (CustomInput.UpFreshPressDeleteOnRead || Input.GetKeyDown(KeyCode.UpArrow))
                return setting.exit;
			if (CustomInput.DownFreshPressDeleteOnRead || Input.GetKeyDown(KeyCode.DownArrow))
                return setting.video;
            return setting.audio;
        }
        private static setting Video()
        {
			if (CustomInput.UpFreshPressDeleteOnRead || Input.GetKeyDown(KeyCode.UpArrow))
                return setting.audio;
			if (CustomInput.DownFreshPressDeleteOnRead || Input.GetKeyDown(KeyCode.DownArrow))
                return setting.controls;
            return setting.video;
        }
        private static setting Controls()
        {
			if (CustomInput.UpFreshPressDeleteOnRead || Input.GetKeyDown(KeyCode.UpArrow))
                return setting.video;
			if (CustomInput.DownFreshPressDeleteOnRead || Input.GetKeyDown(KeyCode.DownArrow))
                return setting.exit;
            return setting.controls;
        }
        private static setting Exit()
        {
			if (CustomInput.UpFreshPressDeleteOnRead || Input.GetKeyDown(KeyCode.UpArrow))
                return setting.controls;
			if (CustomInput.DownFreshPressDeleteOnRead || Input.GetKeyDown(KeyCode.DownArrow))
                return setting.audio;
            return setting.exit;
        }
    }
}
