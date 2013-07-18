/*
 * Copyright (C) 2012, 2013 OUYA, Inc.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using UnityEngine;
using System.Collections.Generic;

public static class OuyaInputManager
{
    #region KeyState Management
    private static List<OuyaInputManager.OuyaKeyState> keyStates;

    static OuyaInputManager()
    {
        OuyaInputManager.initKeyStates();
    }

    public class OuyaKeyState
    {
        public OuyaSDK.OuyaPlayer player;
        public float m_axisLeftStickX = 0f;
        public float m_axisLeftStickY = 0f;
        public float m_axisRightStickX = 0f;
        public float m_axisRightStickY = 0f;
        public float m_axisLeftTrigger = 0f;
        public float m_axisRightTrigger = 0f;

        public bool m_buttonDPadCenter = false;
        public bool m_buttonDPadDown = false;
        public bool m_buttonDPadLeft = false;
        public bool m_buttonDPadRight = false;
        public bool m_buttonDPadUp = false;
        public bool m_buttonSystem = false;
        public bool m_buttonO = false;
        public bool m_buttonU = false;
        public bool m_buttonY = false;
        public bool m_buttonA = false;
        public bool m_buttonLB = false;
        public bool m_buttonLT = false;
        public bool m_buttonRB = false;
        public bool m_buttonRT = false;
        public bool m_buttonL3 = false;
        public bool m_buttonR3 = false;
    }


    public static void initKeyStates()
    {
        OuyaInputManager.OuyaKeyState keyState;
        keyStates = new List<OuyaInputManager.OuyaKeyState>();

        keyState = new OuyaInputManager.OuyaKeyState();
        keyState.player = OuyaSDK.OuyaPlayer.player1;
        keyStates.Add(keyState);

        keyState = new OuyaInputManager.OuyaKeyState();
        keyState.player = OuyaSDK.OuyaPlayer.player2;
        keyStates.Add(keyState);

        keyState = new OuyaInputManager.OuyaKeyState();
        keyState.player = OuyaSDK.OuyaPlayer.player3;
        keyStates.Add(keyState);

        keyState = new OuyaInputManager.OuyaKeyState();
        keyState.player = OuyaSDK.OuyaPlayer.player4;
        keyStates.Add(keyState);

        keyState = new OuyaInputManager.OuyaKeyState();
        keyState.player = OuyaSDK.OuyaPlayer.player5;
        keyStates.Add(keyState);

        keyState = new OuyaInputManager.OuyaKeyState();
        keyState.player = OuyaSDK.OuyaPlayer.player6;
        keyStates.Add(keyState);

        keyState = new OuyaInputManager.OuyaKeyState();
        keyState.player = OuyaSDK.OuyaPlayer.player7;
        keyStates.Add(keyState);

        keyState = new OuyaInputManager.OuyaKeyState();
        keyState.player = OuyaSDK.OuyaPlayer.player8;
        keyStates.Add(keyState);

        keyState = new OuyaInputManager.OuyaKeyState();
        keyState.player = OuyaSDK.OuyaPlayer.player9;
        keyStates.Add(keyState);

        keyState = new OuyaInputManager.OuyaKeyState();
        keyState.player = OuyaSDK.OuyaPlayer.player10;
        keyStates.Add(keyState);

        keyState = new OuyaInputManager.OuyaKeyState();
        keyState.player = OuyaSDK.OuyaPlayer.player11;
        keyStates.Add(keyState);
    }
    public static OuyaInputManager.OuyaKeyState getPlayerKeyState(OuyaSDK.OuyaPlayer player)
    {
        OuyaInputManager.OuyaKeyState keyState = keyStates.Find(delegate(OuyaInputManager.OuyaKeyState key) { return key.player.Equals(player); });
        return keyState;
    }
    #endregion

    #region Button Event Class
    /// <summary>
    /// Button Event Handler
    /// </summary>
    public static class OuyaButtonEvent
    {
        //Delegate for the button event
        public delegate void ButtonEventHandler(OuyaSDK.OuyaPlayer player, OuyaSDK.KeyEnum button, OuyaSDK.InputAction buttonState);
        //acutal button event, this is where you subscribte to the event using the += / -=
        public static event ButtonEventHandler ButtonsEvent;

        //Call this event ( trigger )
        public static void buttonPressEvent(OuyaSDK.OuyaPlayer player, OuyaSDK.KeyEnum button, OuyaSDK.InputAction buttonState)
        {
            if (ButtonsEvent != null)
            {
                ButtonsEvent(player, button, buttonState);
            }
        }

        //Subscribte to the event 
        public static void addButtonEventListener(ButtonEventHandler handler)
        {
            OuyaButtonEvent.ButtonsEvent += handler;
        }

        //UnSubscribte to the event
        public static void removeButtonEventListener(ButtonEventHandler handler)
        {
            OuyaButtonEvent.ButtonsEvent -= handler;
        }
    }
    #endregion

    #region Button & Axis Event Handlers

    /// <summary>
    /// Get Devices
    /// </summary>
    /// <returns>List<Device></returns>
    public static void HandleAxisEvent(OuyaSDK.InputAxisEvent inputEvent)
    {
        switch (inputEvent.getAxisCode())
        {
            case OuyaSDK.AxisEnum.AXIS_LSTICK_X:
                OuyaInputManager.getPlayerKeyState(inputEvent.getPlayer()).m_axisLeftStickX = inputEvent.getAxis();
                break;
            case OuyaSDK.AxisEnum.AXIS_LSTICK_Y:
                OuyaInputManager.getPlayerKeyState(inputEvent.getPlayer()).m_axisLeftStickY = inputEvent.getAxis();
                break;
            case OuyaSDK.AxisEnum.AXIS_RSTICK_X:
                OuyaInputManager.getPlayerKeyState(inputEvent.getPlayer()).m_axisRightStickX = inputEvent.getAxis();
                break;
            case OuyaSDK.AxisEnum.AXIS_RSTICK_Y:
                OuyaInputManager.getPlayerKeyState(inputEvent.getPlayer()).m_axisRightStickY = inputEvent.getAxis();
                break;
            case OuyaSDK.AxisEnum.AXIS_LTRIGGER:
                OuyaInputManager.getPlayerKeyState(inputEvent.getPlayer()).m_axisLeftTrigger = inputEvent.getAxis();
                break;
            case OuyaSDK.AxisEnum.AXIS_RTRIGGER:
                OuyaInputManager.getPlayerKeyState(inputEvent.getPlayer()).m_axisRightTrigger = inputEvent.getAxis();
                break;
        }
    }

    public static void HandleButtonEvent(OuyaSDK.InputButtonEvent inputEvent)
    {
        OuyaInputManager.OuyaKeyState keyState = OuyaInputManager.getPlayerKeyState(inputEvent.getPlayer());
        if (null == keyState)
        {
            return;
        }

        switch (inputEvent.getKeyAction())
        {
            case OuyaSDK.InputAction.KeyDown:
            case OuyaSDK.InputAction.KeyUp:
                switch (inputEvent.getKeyCode())
                {
                    case OuyaSDK.KeyEnum.BUTTON_O:
                        keyState.m_buttonO = inputEvent.getKeyAction() == OuyaSDK.InputAction.KeyDown;
                        break;
                    case OuyaSDK.KeyEnum.BUTTON_U:
                        keyState.m_buttonU = inputEvent.getKeyAction() == OuyaSDK.InputAction.KeyDown;
                        break;
                    case OuyaSDK.KeyEnum.BUTTON_Y:
                        keyState.m_buttonY = inputEvent.getKeyAction() == OuyaSDK.InputAction.KeyDown;
                        break;
                    case OuyaSDK.KeyEnum.BUTTON_A:
                        keyState.m_buttonA = inputEvent.getKeyAction() == OuyaSDK.InputAction.KeyDown;
                        break;
                    case OuyaSDK.KeyEnum.BUTTON_LB:
                        keyState.m_buttonLB = inputEvent.getKeyAction() == OuyaSDK.InputAction.KeyDown;
                        break;
                    case OuyaSDK.KeyEnum.BUTTON_LT:
                        keyState.m_buttonLT = inputEvent.getKeyAction() == OuyaSDK.InputAction.KeyDown;
                        break;
                    case OuyaSDK.KeyEnum.BUTTON_L3:
                        keyState.m_buttonL3 = inputEvent.getKeyAction() == OuyaSDK.InputAction.KeyDown;
                        break;
                    case OuyaSDK.KeyEnum.BUTTON_RB:
                        keyState.m_buttonRB = inputEvent.getKeyAction() == OuyaSDK.InputAction.KeyDown;
                        break;
                    case OuyaSDK.KeyEnum.BUTTON_RT:
                        keyState.m_buttonRT = inputEvent.getKeyAction() == OuyaSDK.InputAction.KeyDown;
                        break;
                    case OuyaSDK.KeyEnum.BUTTON_R3:
                        keyState.m_buttonR3 = inputEvent.getKeyAction() == OuyaSDK.InputAction.KeyDown;
                        break;
                    case OuyaSDK.KeyEnum.BUTTON_SYSTEM:
                        keyState.m_buttonSystem = inputEvent.getKeyAction() == OuyaSDK.InputAction.KeyDown;
                        break;
                    case OuyaSDK.KeyEnum.BUTTON_DPAD_DOWN:
                        keyState.m_buttonDPadDown = inputEvent.getKeyAction() == OuyaSDK.InputAction.KeyDown;
                        break;
                    case OuyaSDK.KeyEnum.BUTTON_DPAD_LEFT:
                        keyState.m_buttonDPadLeft = inputEvent.getKeyAction() == OuyaSDK.InputAction.KeyDown;
                        break;
                    case OuyaSDK.KeyEnum.BUTTON_DPAD_RIGHT:
                        keyState.m_buttonDPadRight = inputEvent.getKeyAction() == OuyaSDK.InputAction.KeyDown;
                        break;
                    case OuyaSDK.KeyEnum.BUTTON_DPAD_UP:
                        keyState.m_buttonDPadUp = inputEvent.getKeyAction() == OuyaSDK.InputAction.KeyDown;
                        break;
                    default:
                        return;
                }
                OuyaButtonEvent.buttonPressEvent(inputEvent.getPlayer(), inputEvent.getKeyCode(), inputEvent.getKeyAction());
                break;
        }
    }
    #endregion

    #region Button States
    public static void SetButton(string inputName, OuyaSDK.OuyaPlayer player, bool state)
    {
        switch (inputName)
        {
            case "SYS": //arbitrary name and mapping
                OuyaInputManager.getPlayerKeyState(player).m_buttonSystem = state;
                break;
            case "DPC": //arbitrary name and mapping
                OuyaInputManager.getPlayerKeyState(player).m_buttonDPadCenter = state;
                break;
            case "DPD": //arbitrary name and mapping
                OuyaInputManager.getPlayerKeyState(player).m_buttonDPadDown = state;
                break;
            case "DPL": //arbitrary name and mapping
                OuyaInputManager.getPlayerKeyState(player).m_buttonDPadLeft = state;
                break;
            case "DPR": //arbitrary name and mapping
                OuyaInputManager.getPlayerKeyState(player).m_buttonDPadRight = state;
                break;
            case "DPU": //arbitrary name and mapping
                OuyaInputManager.getPlayerKeyState(player).m_buttonDPadUp = state;
                break;
            case "O": //arbitrary name and mapping
                OuyaInputManager.getPlayerKeyState(player).m_buttonO = state;
                break;
            case "U": //arbitrary name and mapping
                OuyaInputManager.getPlayerKeyState(player).m_buttonU = state;
                break;
            case "Y": //arbitrary name and mapping
                OuyaInputManager.getPlayerKeyState(player).m_buttonY = state;
                break;
            case "A": //arbitrary name and mapping
                OuyaInputManager.getPlayerKeyState(player).m_buttonA = state;
                break;
            case "LT": //arbitrary name and mapping
                OuyaInputManager.getPlayerKeyState(player).m_buttonLT = state;
                break;
            case "RT": //arbitrary name and mapping
                OuyaInputManager.getPlayerKeyState(player).m_buttonRT = state;
                break;
            case "LB": //arbitrary name and mapping
                OuyaInputManager.getPlayerKeyState(player).m_buttonLB = state;
                break;
            case "RB": //arbitrary name and mapping
                OuyaInputManager.getPlayerKeyState(player).m_buttonRB = state;
                break;
            case "L3": //arbitrary name and mapping
                OuyaInputManager.getPlayerKeyState(player).m_buttonL3 = state;
                break;
            case "R3": //arbitrary name and mapping
                OuyaInputManager.getPlayerKeyState(player).m_buttonR3 = state;
                break;

        }
    }

    /// <summary>
    /// Wrap Unity's method
    /// </summary>
    /// <param name="inputName"></param>
    /// <returns></returns>
    public static bool GetButton(string inputName, OuyaSDK.OuyaPlayer player)
    {

        switch (inputName)
        {
            case "SYS": //arbitrary name and mapping
                return OuyaInputManager.getPlayerKeyState(player).m_buttonSystem;
            case "DPC": //arbitrary name and mapping
                return OuyaInputManager.getPlayerKeyState(player).m_buttonDPadCenter;
            case "DPD": //arbitrary name and mapping
                return OuyaInputManager.getPlayerKeyState(player).m_buttonDPadDown;
            case "DPL": //arbitrary name and mapping
                return OuyaInputManager.getPlayerKeyState(player).m_buttonDPadLeft;
            case "DPR": //arbitrary name and mapping
                return OuyaInputManager.getPlayerKeyState(player).m_buttonDPadRight;
            case "DPU": //arbitrary name and mapping
                return OuyaInputManager.getPlayerKeyState(player).m_buttonDPadUp;
            case "O": //arbitrary name and mapping
                return OuyaInputManager.getPlayerKeyState(player).m_buttonO;
            case "U": //arbitrary name and mapping
                return OuyaInputManager.getPlayerKeyState(player).m_buttonU;
            case "Y": //arbitrary name and mapping
                return OuyaInputManager.getPlayerKeyState(player).m_buttonY;
            case "A": //arbitrary name and mapping
                return OuyaInputManager.getPlayerKeyState(player).m_buttonA;
            case "LT": //arbitrary name and mapping
                return OuyaInputManager.getPlayerKeyState(player).m_buttonLT;
            case "RT": //arbitrary name and mapping
                return OuyaInputManager.getPlayerKeyState(player).m_buttonRT;
            case "LB": //arbitrary name and mapping
                return OuyaInputManager.getPlayerKeyState(player).m_buttonLB;
            case "RB": //arbitrary name and mapping
                return OuyaInputManager.getPlayerKeyState(player).m_buttonRB;
            case "L3":
                return OuyaInputManager.getPlayerKeyState(player).m_buttonL3;
            case "R3":
                return OuyaInputManager.getPlayerKeyState(player).m_buttonR3;
        }

        return false;
    }

    /// <summary>
    /// Wrap Unity's method
    /// </summary>
    /// <param name="inputName"></param>
    /// <returns></returns>
    public static bool GetButtonDown(string inputName, OuyaSDK.OuyaPlayer player)
    {
        // these will map to the Unity game's existing button names
        // the text cases are placeholders
        return OuyaInputManager.GetButton(inputName, player);
    }

    /// <summary>
    /// Wrap Unity's method
    /// </summary>
    /// <param name="inputName"></param>
    /// <returns></returns>
    public static bool GetButtonUp(string inputName, OuyaSDK.OuyaPlayer player)
    {
        // these will map to the Unity game's existing button names
        // the text cases are placeholders
        return !OuyaInputManager.GetButton(inputName, player);
    }
    #endregion

    #region Handle Axis Input

    /// <summary>
    /// Wrap Unity's method
    /// </summary>
    /// <param name="inputName"></param>
    /// <returns></returns>
    public static float GetAxis(string inputName, OuyaSDK.OuyaPlayer player)
    {


#if !UNITY_EDITOR && !UNITY_STANDALONE_OSX && !UNITY_STANDALONE_WIN && !UNITY_STANDALONE_LINUX
        switch (inputName)
        {
            case "LT":
                return OuyaInputManager.getPlayerKeyState(player).m_axisLeftTrigger;
            case "RT":
                return OuyaInputManager.getPlayerKeyState(player).m_axisRightTrigger;
            case "RX":
                return OuyaInputManager.getPlayerKeyState(player).m_axisRightStickX;
            case "RY":
                return OuyaInputManager.getPlayerKeyState(player).m_axisRightStickY;
            case "LX":
                return OuyaInputManager.getPlayerKeyState(player).m_axisLeftStickX;
            case "LY":
                return -OuyaInputManager.getPlayerKeyState(player).m_axisLeftStickY;
        }
        return 0f;
#else
        string axisName = string.Empty;
        int invertFactor = GetInvertedFactor(GetControllerType(player), inputName);
        switch (inputName)
        {
            case "LT":
                axisName = OuyaInputManager.GetInput(player, AxisTypes.Horizontal, AnalogTypes.LTRT);
                break;
            case "RT":
                axisName = OuyaInputManager.GetInput(player, AxisTypes.Vertical, AnalogTypes.LTRT);
                break;
            case "RX":
                axisName = OuyaInputManager.GetInput(player, AxisTypes.Horizontal, AnalogTypes.Right);
                break;
            case "RY":
                axisName = OuyaInputManager.GetInput(player, AxisTypes.Vertical, AnalogTypes.Right);
                break;
            case "LX":
                axisName = OuyaInputManager.GetInput(player, AxisTypes.Horizontal, AnalogTypes.Left);
                break;
            case "LY":
                axisName = OuyaInputManager.GetInput(player, AxisTypes.Vertical, AnalogTypes.Left);
                break;
        }
        if (!string.IsNullOrEmpty(axisName))
        {
            return invertFactor * Input.GetAxis(axisName);
        }
        return 0f;

#endif
    }

    public static JoystickType GetControllerType(string deviceName)
    {
        /// @todo: ADD_CONTROLLER_NAME

        switch (deviceName.ToUpper())
        {
            case "BLUETOOTH JOYSTICK":
            case "OUYA GAME CONTROLLER":
                return JoystickType.ouya;
            case "B":
                return JoystickType.ps2;
            case "MOTIONINJOY VIRTUAL GAME CONTROLLER":
                return JoystickType.ps3;
            case "USB CONTROLLER":
                return JoystickType.usb;
            case "XBOX 360 WIRELESS RECEIVER":
            case "CONTROLLER (AFTERGLOW GAMEPAD FOR XBOX 360)":
            case "CONTROLLER (ROCK CANDY GAMEPAD FOR XBOX 360)":
            case "CONTROLLER (XBOX 360 WIRELESS RECEIVER FOR WINDOWS)":
            case "MICROSOFT X-BOX 360 PAD":
            case "CONTROLLER (XBOX 360 FOR WINDOWS)":
            case "CONTROLLER (XBOX360 GAMEPAD)":
            case "XBOX 360 FOR WINDOWS (CONTROLLER)":
                return JoystickType.xbox;
            default:
                Debug.Log(string.Format("Controller name: {0}", deviceName));
                return JoystickType.none;
        }
    }

    public static int GetInvertedFactor(JoystickType joystickType, string axis)
    {
        switch (joystickType)
        {
            case JoystickType.ps3:
                switch (axis)
                {
                    case "RY":
                        return -1;
                    case "DPL":
                        return -1;
                }
                break;
            case JoystickType.xbox:
                switch (axis)
                {
                    case "RX":
                        return -1;
                }
                break;
            case JoystickType.ouya:
                switch (axis)
                {
                    case "RX":
                    //case "RY":
                        switch (OuyaGameObject.Singleton.GetDeviceType())
                        {
                            case OuyaGameObject.DEVICE_OUYA:
                            case OuyaGameObject.DEVICE_ANDROID:
                                return 1;
                            default:
                                return -1;
                        }
                }
                break;
            case JoystickType.none:
                switch (axis)
                {
                    case "RX":
                        return -1;
                }
                break;
        }

        return 1;
    }

    public static JoystickType GetControllerType(OuyaSDK.OuyaPlayer player)
    {
        // check the player to see what joystick type they have
        OuyaGameObject.Device device = OuyaGameObject.devices.Find(delegate(OuyaGameObject.Device d) { return (null == d) ? false : (d.player == player); });
        if (null == device)
        {
            return JoystickType.none;
        }
        return GetControllerType(device.name);
    }

    /// <summary>
    /// Builds a string to get the controller by player.
    /// </summary>
    /// <param name="player"></param>
    /// <param name="axis"></param>
    /// <param name="type"></param>
    /// <param name="atype"></param>
    /// <returns></returns>
    public static string GetInput(OuyaSDK.OuyaPlayer player, AxisTypes axis)
	{
		return GetInput(player, axis, AnalogTypes.none);
	}
		

    /// <summary>
    /// Builds a string to get the controller by player.
    /// </summary>
    /// <param name="player"></param>
    /// <param name="axis"></param>
    /// <param name="type"></param>
    /// <param name="atype"></param>
    /// <returns></returns>
    public static string GetInput(OuyaSDK.OuyaPlayer player, AxisTypes axis, AnalogTypes atype)
    {
        Vector2 point;
        JoystickType joystickType = GetControllerType(player);
        switch (joystickType)
        {
            case JoystickType.ps3:
                //Note We don't care if the axis is X or Y,  we just want to check for input from normalized.
                #region PS3 Controller
                point = new Vector2(Input.GetAxis(CheckInput(player, axis, JoystickType.ps3, AnalogTypes.Left)), Input.GetAxis(CheckInput(player, axis, JoystickType.ps3, AnalogTypes.Right)));
                if (point.x != 0f || point.y != 0f)
                {
                    return CheckInput(player, axis, JoystickType.ps3, atype);
                }
                if (atype.Equals(AnalogTypes.DPad)){
                    return CheckInput(player, axis, JoystickType.ps3, atype);
                }
                #endregion
                return string.Empty;

            case JoystickType.xbox:
                #region xbox360 
                point = new Vector2(Input.GetAxis(CheckInput(player, axis, JoystickType.xbox, AnalogTypes.Left)), Input.GetAxis(CheckInput(player, axis, JoystickType.xbox, AnalogTypes.Right)));
                if (point.x != 0f || point.y != 0f)
                {
                    return CheckInput(player, axis, JoystickType.xbox, atype);
                }
                if (atype.Equals(AnalogTypes.DPad))
                {
                    return CheckInput(player, axis, JoystickType.xbox, atype);
                }
                if (atype.Equals(AnalogTypes.LTRT))
                {
                    return CheckInput(player, axis, JoystickType.xbox, atype);
                }
                #endregion
                return string.Empty;

            case JoystickType.ouya:
                point = new Vector2(Input.GetAxis(CheckInput(player, axis, JoystickType.ouya, AnalogTypes.Left)), Input.GetAxis(CheckInput(player, axis, JoystickType.ouya, AnalogTypes.Right)));
                if (point.x != 0f || point.y != 0f)
                {
                    return CheckInput(player, axis, JoystickType.ouya, atype);
                }
                if (atype.Equals(AnalogTypes.DPad))
                {
                    return CheckInput(player, axis, JoystickType.ouya, atype);
                }
                if (atype.Equals(AnalogTypes.LTRT))
                {
                    return CheckInput(player, axis, JoystickType.ouya, atype);
                }               
                return string.Empty;

            default:
                return "Horizontal";
        }
    }

    /// <summary>
    /// This needs to be setup for each joystick type being supported.  I have not setup the USB controller or the I:Droid:CON  
    /// For the most part you won't need to modify this section.
    /// </summary>
    /// <param name="player"></param>
    /// <param name="axis"></param>
    /// <param name="joystick"></param>
    /// <param name="atype"></param>
    /// <returns></returns>
    private static string CheckInput(OuyaSDK.OuyaPlayer player, AxisTypes axis, JoystickType joystick, AnalogTypes atype)
    {
       //Debug.Log(string.Format("Player:{0} Axis:{1} Joystick:{2} AnalogType:{3}",player,axis,joystick,atype));
        //REF: player1, DPadH, PS3, Dpad

        //Note:  It is your responsibility to make sure that  Unity Inputs are setup correctly or you will get an error on your controller.
        
        int axisNumber=0;
        
        switch (joystick)
        {
            case JoystickType.ps3:
                //Get The Joystick name from  Project Settings --> Input for Controllers
                #region PS3 Controller 
                if (atype.Equals(AnalogTypes.Left))
                {
                    ControllerType cType = OuyaControllerMapping.getController("ps3");
                    if (axis.Equals(AxisTypes.Horizontal)) { axisNumber = cType.leftAnalogH; } else { axisNumber = cType.leftAnalogV; }
                    return "Joy" + (int)player + " Axis " + axisNumber.ToString();
                }
                else if (atype.Equals(AnalogTypes.Right))
                {
                    ControllerType cType = OuyaControllerMapping.getController("ps3");
                    if (axis.Equals(AxisTypes.Horizontal)) { axisNumber = cType.rightAnalogH; } else { axisNumber = cType.rightAnalogV; }
                    return "Joy" + (int)player + " Axis " + axisNumber.ToString();
                }
                else if (atype.Equals(AnalogTypes.DPad))
                {
                    ControllerType cType = OuyaControllerMapping.getController("ps3");
                    if (axis.Equals(AxisTypes.DPadH)) { axisNumber = cType.dpadH; } else { axisNumber = cType.dpadV; }
                    return "Joy" + (int)player + " Axis " + axisNumber.ToString();
                }
                #endregion
                break;

            case JoystickType.xbox:
                #region xbox ( usually 360 / untested with normal xbox )
                if (atype.Equals(AnalogTypes.Left))
                {
                    ControllerType cType = OuyaControllerMapping.getController("xbox360");
                    if (axis.Equals(AxisTypes.Horizontal)) { axisNumber = cType.leftAnalogH; } else { axisNumber = cType.leftAnalogV; }
                    return "Joy" + (int)player + " Axis " + axisNumber.ToString();
                }
                else if (atype.Equals(AnalogTypes.Right))
                {
                    ControllerType cType = OuyaControllerMapping.getController("xbox360");
                    if (axis.Equals(AxisTypes.Horizontal)) { axisNumber = cType.rightAnalogH; } else { axisNumber = cType.rightAnalogV; }
                    return "Joy" + (int)player + " Axis " + axisNumber.ToString();
                }
                else if (atype.Equals(AnalogTypes.DPad))
                {
                    ControllerType cType = OuyaControllerMapping.getController("xbox360");
                    if (axis.Equals(AxisTypes.DPadH)) { axisNumber = cType.dpadH; } else { axisNumber = cType.dpadV; }
                    return "Joy" + (int)player + " Axis " + axisNumber.ToString();
                }
                else if (atype.Equals(AnalogTypes.LTRT))
                {
                    ControllerType cType = OuyaControllerMapping.getController("xbox360");
                    axisNumber = cType.triggers;
                    return "Joy" + (int)player + " Axis " + axisNumber.ToString();
                }
                #endregion
                break;
            
            case JoystickType.ouya:
                #region OUYA - Not Tested until I get a bluetooth dongle
                if (atype.Equals(AnalogTypes.Left))
                {
                    ControllerType cType = OuyaControllerMapping.getController("ouya");
                    if (axis.Equals(AxisTypes.Horizontal)) { axisNumber = cType.leftAnalogH; } else { axisNumber = cType.leftAnalogV; }
                    return "Joy" + (int)player + " Axis " + axisNumber.ToString();
                }
                else if (atype.Equals(AnalogTypes.Right))
                {
                    ControllerType cType = OuyaControllerMapping.getController("ouya");
                    if (axis.Equals(AxisTypes.Horizontal)) { axisNumber = cType.rightAnalogH; } else { axisNumber = cType.rightAnalogV; }
                    return "Joy" + (int)player + " Axis " + axisNumber.ToString();
                }
                else if (atype.Equals(AnalogTypes.DPad))
                {
                    ControllerType cType = OuyaControllerMapping.getController("ouya");
                    if (axis.Equals(AxisTypes.DPadH)) { axisNumber = cType.dpadH; } else { axisNumber = cType.dpadV; }
                    return "Joy" + (int)player + " Axis " + axisNumber.ToString();
                }
                #endregion
                break;
        }

        //most likely we will never reach here, but If we do Return basic Horizontal Axis
        return "Horizontal";
    }

    #endregion

    #region Device Management
    /// <summary>
    /// Get Device list from the OuyaGameObject.
    /// </summary>
    /// <returns></returns>
    public static List<OuyaGameObject.Device> GetDevices()
    {
        //Return a list of devices.
        return OuyaGameObject.devices;
    }
    #endregion

}