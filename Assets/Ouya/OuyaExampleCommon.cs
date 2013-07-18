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

using System;
using UnityEngine;

public class OuyaExampleCommon
{
    /// <summary>
    /// The current selected controller
    /// </summary>
    public static OuyaSDK.OuyaPlayer Player = OuyaSDK.OuyaPlayer.player1;

    /// <summary>
    /// Cache joysticks
    /// </summary>
    public static string[] Joysticks = null;

    /// <summary>
    /// Query joysticks every N seconds
    /// </summary>
    private static DateTime m_timerJoysticks = DateTime.MinValue;

    #region Mapping Helpers

    public static float GetAxis(string ouyaMapping, OuyaSDK.OuyaPlayer player)
    {
        switch (ouyaMapping)
        {
            case "LB":
                return GetAxis(OuyaSDK.KeyEnum.BUTTON_LB, player);
            case "LT":
                return GetAxis(OuyaSDK.KeyEnum.BUTTON_LT, player);
            case "LX":
                return GetAxis(OuyaSDK.KeyEnum.AXIS_LSTICK_X, player);
            case "LY":
                return GetAxis(OuyaSDK.KeyEnum.AXIS_LSTICK_Y, player);
            case "RB":
                return GetAxis(OuyaSDK.KeyEnum.BUTTON_RB, player);
            case "RT":
                return GetAxis(OuyaSDK.KeyEnum.BUTTON_RT, player);
            case "RX":
                return GetAxis(OuyaSDK.KeyEnum.AXIS_RSTICK_X, player);
            case "RY":
                return GetAxis(OuyaSDK.KeyEnum.AXIS_RSTICK_Y, player);
            case "DL":
                return GetAxis(OuyaSDK.KeyEnum.BUTTON_DPAD_LEFT, player);
            case "DR":
                return GetAxis(OuyaSDK.KeyEnum.BUTTON_DPAD_RIGHT, player);
            case "DU":
                return GetAxis(OuyaSDK.KeyEnum.BUTTON_DPAD_UP, player);
            case "DD":
                return GetAxis(OuyaSDK.KeyEnum.BUTTON_DPAD_DOWN, player);
            default:
                return 0f;
        }
    }

    public static float GetAxis(OuyaSDK.KeyEnum keyCode, OuyaSDK.OuyaPlayer player)
    {
        if (null == OuyaExampleCommon.Joysticks)
        {
            return 0f;
        }
        int playerIndex = (int)player - 1;
        if (playerIndex >= OuyaExampleCommon.Joysticks.Length)
        {
            return 0f;
        }

        string joystickName = OuyaExampleCommon.Joysticks[playerIndex];
        if (null == joystickName)
        {
            return 0f;
        }

        bool invert = false;
        string axisName = string.Empty;

        switch (joystickName.ToUpper())
        {
            case "HARMONIX ROCK BAND DRUMKIT":
                return 0f;
            case "HARMONIX ROCK BAND GUITAR":
                switch (keyCode)
                {
                    case OuyaSDK.KeyEnum.HARMONIX_ROCK_BAND_GUITAR_PICKUP:
                    case OuyaSDK.KeyEnum.BUTTON_LT:
                        axisName = string.Format("Joy{0} Axis 3", (int)player);
                        break;
                    case OuyaSDK.KeyEnum.HARMONIX_ROCK_BAND_GUITAR_WHAMMI:
                    case OuyaSDK.KeyEnum.BUTTON_RT:
                        axisName = string.Format("Joy{0} Axis 4", (int)player);
                        break;
                    case OuyaSDK.KeyEnum.BUTTON_DPAD_LEFT:
                    case OuyaSDK.KeyEnum.BUTTON_DPAD_RIGHT:
                        axisName = string.Format("Joy{0} Axis 7", (int)player);
                        break;
                    case OuyaSDK.KeyEnum.HARMONIX_ROCK_BAND_GUITAR_STRUM:
                    case OuyaSDK.KeyEnum.AXIS_LSTICK_Y:
                    case OuyaSDK.KeyEnum.BUTTON_DPAD_UP:
                    case OuyaSDK.KeyEnum.BUTTON_DPAD_DOWN:
                        axisName = string.Format("Joy{0} Axis 8", (int)player);
                        break;
                    default:
                        return 0f;
                }
                break;

            case "BROADCOM BLUETOOTH HID":
            case "MOGA PRO HID":
#if !UNITY_EDITOR && UNITY_ANDROID
            switch (keyCode)
            {
                case OuyaSDK.KeyEnum.AXIS_LSTICK_X:
                    axisName = string.Format("Joy{0} Axis 1", (int)player);
                    break;
                case OuyaSDK.KeyEnum.AXIS_LSTICK_Y:
                    axisName = string.Format("Joy{0} Axis 2", (int)player);
                    invert = true;
                    break;
                case OuyaSDK.KeyEnum.AXIS_RSTICK_X:
                    axisName = string.Format("Joy{0} Axis 3", (int)player);
                    break;
                case OuyaSDK.KeyEnum.AXIS_RSTICK_Y:
                    axisName = string.Format("Joy{0} Axis 4", (int)player);
                    break;
                case OuyaSDK.KeyEnum.BUTTON_LT:
                    axisName = string.Format("Joy{0} Axis 8", (int)player);
                    break;
                case OuyaSDK.KeyEnum.BUTTON_RT:
                    axisName = string.Format("Joy{0} Axis 7", (int)player);
                    break;
                case OuyaSDK.KeyEnum.BUTTON_DPAD_LEFT:
                    axisName = string.Format("Joy{0} Axis 5", (int)player);
                    break;
                case OuyaSDK.KeyEnum.BUTTON_DPAD_RIGHT:
                    axisName = string.Format("Joy{0} Axis 5", (int)player);
                    break;
                case OuyaSDK.KeyEnum.BUTTON_DPAD_UP:
                    axisName = string.Format("Joy{0} Axis 6", (int)player);
                    break;
                case OuyaSDK.KeyEnum.BUTTON_DPAD_DOWN:
                    axisName = string.Format("Joy{0} Axis 6", (int)player);
                    break;
                default:
                    return 0f;
            }
#endif
                break;
            case "OUYA GAME CONTROLLER":

#if !UNITY_EDITOR && UNITY_ANDROID
            switch (keyCode)
            {
                case OuyaSDK.KeyEnum.AXIS_LSTICK_X:
                    axisName = string.Format("Joy{0} Axis 1", (int)player);
                    break;
                case OuyaSDK.KeyEnum.AXIS_LSTICK_Y:
                    axisName = string.Format("Joy{0} Axis 2", (int)player);
                    break;
                case OuyaSDK.KeyEnum.AXIS_RSTICK_X:
                    axisName = string.Format("Joy{0} Axis 3", (int)player);
                    break;
                case OuyaSDK.KeyEnum.AXIS_RSTICK_Y:
                    axisName = string.Format("Joy{0} Axis 4", (int)player);
                    break;
                case OuyaSDK.KeyEnum.BUTTON_LT:
                    axisName = string.Format("Joy{0} Axis 5", (int)player);
                    break;
                case OuyaSDK.KeyEnum.BUTTON_RT:
                    axisName = string.Format("Joy{0} Axis 6", (int)player);
                    break;
                default:
                    return 0f;
            }
#else
                switch (keyCode)
                {
                    case OuyaSDK.KeyEnum.AXIS_LSTICK_X:
                        axisName = string.Format("Joy{0} Axis 1", (int)player);
                        break;
                    case OuyaSDK.KeyEnum.AXIS_LSTICK_Y:
                        axisName = string.Format("Joy{0} Axis 2", (int)player);
                        invert = true;
                        break;
                    case OuyaSDK.KeyEnum.AXIS_RSTICK_X:
                        axisName = string.Format("Joy{0} Axis 4", (int)player);
                        invert = true;
                        break;
                    case OuyaSDK.KeyEnum.AXIS_RSTICK_Y:
                        axisName = string.Format("Joy{0} Axis 5", (int)player);
                        break;
                    case OuyaSDK.KeyEnum.BUTTON_LT:
                        axisName = string.Format("Joy{0} Axis 9", (int)player);
                        break;
                    case OuyaSDK.KeyEnum.BUTTON_RT:
                        axisName = string.Format("Joy{0} Axis 10", (int)player);
                        break;
                    default:
                        return 0f;
                }
#endif
                break;

            case "XBOX 360 WIRELESS RECEIVER":

#if !UNITY_EDITOR && UNITY_ANDROID

                switch (keyCode)
                {
                    case OuyaSDK.KeyEnum.AXIS_LSTICK_X:
                        axisName = string.Format("Joy{0} Axis 1", (int)player);
                        break;
                    case OuyaSDK.KeyEnum.AXIS_LSTICK_Y:
                        axisName = string.Format("Joy{0} Axis 2", (int)player);
                        break;
                    case OuyaSDK.KeyEnum.AXIS_RSTICK_X:
                        axisName = string.Format("Joy{0} Axis 3", (int)player);
                        break;
                    case OuyaSDK.KeyEnum.AXIS_RSTICK_Y:
                        axisName = string.Format("Joy{0} Axis 4", (int)player);
                        break;
                    case OuyaSDK.KeyEnum.BUTTON_LT:
                        axisName = string.Format("Joy{0} Axis 5", (int)player);
                        break;
                    case OuyaSDK.KeyEnum.BUTTON_RT:
                        axisName = string.Format("Joy{0} Axis 6", (int)player);
                        break;
                    default:
                        return 0f;
                }

#else
                if (Application.platform == RuntimePlatform.OSXEditor)
                {
                    switch (keyCode)
                    {
                        case OuyaSDK.KeyEnum.AXIS_LSTICK_X:
                            axisName = string.Format("Joy{0} Axis 1", (int)player);
                            break;
                        case OuyaSDK.KeyEnum.AXIS_LSTICK_Y:
                            axisName = string.Format("Joy{0} Axis 2", (int)player);
                            break;
                        case OuyaSDK.KeyEnum.AXIS_RSTICK_X:
                            axisName = string.Format("Joy{0} Axis 3", (int)player);
                            break;
                        case OuyaSDK.KeyEnum.AXIS_RSTICK_Y:
                            axisName = string.Format("Joy{0} Axis 4", (int)player);
                            break;
                        case OuyaSDK.KeyEnum.BUTTON_LT:
                            axisName = string.Format("Joy{0} Axis 5", (int)player);
                            break;
                        case OuyaSDK.KeyEnum.BUTTON_RT:
                            axisName = string.Format("Joy{0} Axis 6", (int)player);
                            break;
                        default:
                            return 0f;
                    }
                }
                else
                {
                    switch (keyCode)
                    {
                        case OuyaSDK.KeyEnum.AXIS_LSTICK_X:
                            axisName = string.Format("Joy{0} Axis 1", (int)player);
                            break;
                        case OuyaSDK.KeyEnum.AXIS_LSTICK_Y:
                            axisName = string.Format("Joy{0} Axis 2", (int)player);
                            break;
                        case OuyaSDK.KeyEnum.AXIS_RSTICK_X:
                            axisName = string.Format("Joy{0} Axis 4", (int)player);
                            break;
                        case OuyaSDK.KeyEnum.AXIS_RSTICK_Y:
                            axisName = string.Format("Joy{0} Axis 5", (int)player);
                            break;
                        case OuyaSDK.KeyEnum.BUTTON_LT:
                            axisName = string.Format("Joy{0} Axis 9", (int)player);
                            break;
                        case OuyaSDK.KeyEnum.BUTTON_RT:
                            axisName = string.Format("Joy{0} Axis 10", (int)player);
                            break;
                        case OuyaSDK.KeyEnum.BUTTON_DPAD_LEFT:
                            axisName = string.Format("Joy{0} Axis 6", (int)player);
                            break;
                        case OuyaSDK.KeyEnum.BUTTON_DPAD_RIGHT:
                            axisName = string.Format("Joy{0} Axis 6", (int)player);
                            break;
                        case OuyaSDK.KeyEnum.BUTTON_DPAD_UP:
                            axisName = string.Format("Joy{0} Axis 7", (int)player);
                            break;
                        case OuyaSDK.KeyEnum.BUTTON_DPAD_DOWN:
                            axisName = string.Format("Joy{0} Axis 7", (int)player);
                            break;
                        default:
                            return 0f;
                    }
                }
#endif

                break;

            case "CONTROLLER (AFTERGLOW GAMEPAD FOR XBOX 360)":
            case "CONTROLLER (ROCK CANDY GAMEPAD FOR XBOX 360)":
            case "CONTROLLER (XBOX 360 WIRELESS RECEIVER FOR WINDOWS)":
            case "MICROSOFT X-BOX 360 PAD":
            case "CONTROLLER (XBOX 360 FOR WINDOWS)":
            case "CONTROLLER (XBOX360 GAMEPAD)":
            case "XBOX 360 FOR WINDOWS (CONTROLLER)":
            case "MICROSOFT WIRELESS 360 CONTROLLER":

#if !UNITY_EDITOR && UNITY_ANDROID

                switch (keyCode)
                {
                    case OuyaSDK.KeyEnum.AXIS_LSTICK_X:
                        axisName = string.Format("Joy{0} Axis 1", (int)player);
                        break;
                    case OuyaSDK.KeyEnum.AXIS_LSTICK_Y:
                        axisName = string.Format("Joy{0} Axis 2", (int)player);
                        break;
                    case OuyaSDK.KeyEnum.AXIS_RSTICK_X:
                        axisName = string.Format("Joy{0} Axis 3", (int)player);
                        break;
                    case OuyaSDK.KeyEnum.AXIS_RSTICK_Y:
                        axisName = string.Format("Joy{0} Axis 4", (int)player);
                        break;
                    case OuyaSDK.KeyEnum.BUTTON_LT:
                        axisName = string.Format("Joy{0} Axis 7", (int)player);
                        break;
                    case OuyaSDK.KeyEnum.BUTTON_RT:
                        axisName = string.Format("Joy{0} Axis 8", (int)player);
                        break;
                    case OuyaSDK.KeyEnum.BUTTON_DPAD_LEFT:
                        axisName = string.Format("Joy{0} Axis 5", (int)player);
                        break;
                    case OuyaSDK.KeyEnum.BUTTON_DPAD_RIGHT:
                        axisName = string.Format("Joy{0} Axis 5", (int)player);
                        break;
                    case OuyaSDK.KeyEnum.BUTTON_DPAD_UP:
                        axisName = string.Format("Joy{0} Axis 6", (int)player);
                        break;
                    case OuyaSDK.KeyEnum.BUTTON_DPAD_DOWN:
                        axisName = string.Format("Joy{0} Axis 6", (int)player);
                        break;
                    default:
                        return 0f;
                }

#else
                if (Application.platform == RuntimePlatform.OSXEditor)
                {
                    switch (keyCode)
                    {
                        case OuyaSDK.KeyEnum.AXIS_LSTICK_X:
                            axisName = string.Format("Joy{0} Axis 1", (int)player);
                            break;
                        case OuyaSDK.KeyEnum.AXIS_LSTICK_Y:
                            axisName = string.Format("Joy{0} Axis 2", (int)player);
                            break;
                        case OuyaSDK.KeyEnum.AXIS_RSTICK_X:
                            axisName = string.Format("Joy{0} Axis 3", (int)player);
                            break;
                        case OuyaSDK.KeyEnum.AXIS_RSTICK_Y:
                            axisName = string.Format("Joy{0} Axis 4", (int)player);
                            break;
                        case OuyaSDK.KeyEnum.BUTTON_LT:
                            axisName = string.Format("Joy{0} Axis 5", (int)player);
                            break;
                        case OuyaSDK.KeyEnum.BUTTON_RT:
                            axisName = string.Format("Joy{0} Axis 6", (int)player);
                            break;
                        default:
                            return 0f;
                    }
                }
                else
                {
                    switch (keyCode)
                    {
                        case OuyaSDK.KeyEnum.AXIS_LSTICK_X:
                            axisName = string.Format("Joy{0} Axis 1", (int) player);
                            break;
                        case OuyaSDK.KeyEnum.AXIS_LSTICK_Y:
                            axisName = string.Format("Joy{0} Axis 2", (int) player);
                            break;
                        case OuyaSDK.KeyEnum.AXIS_RSTICK_X:
                            axisName = string.Format("Joy{0} Axis 4", (int) player);
                            break;
                        case OuyaSDK.KeyEnum.AXIS_RSTICK_Y:
                            axisName = string.Format("Joy{0} Axis 5", (int) player);
                            break;
                        case OuyaSDK.KeyEnum.BUTTON_LT:
                            axisName = string.Format("Joy{0} Axis 9", (int) player);
                            break;
                        case OuyaSDK.KeyEnum.BUTTON_RT:
                            axisName = string.Format("Joy{0} Axis 10", (int) player);
                            break;
                        case OuyaSDK.KeyEnum.BUTTON_DPAD_LEFT:
                            axisName = string.Format("Joy{0} Axis 6", (int) player);
                            break;
                        case OuyaSDK.KeyEnum.BUTTON_DPAD_RIGHT:
                            axisName = string.Format("Joy{0} Axis 6", (int) player);
                            break;
                        case OuyaSDK.KeyEnum.BUTTON_DPAD_UP:
                            axisName = string.Format("Joy{0} Axis 7", (int) player);
                            break;
                        case OuyaSDK.KeyEnum.BUTTON_DPAD_DOWN:
                            axisName = string.Format("Joy{0} Axis 7", (int) player);
                            break;
                        default:
                            return 0f;
                    }
                }
#endif

                break;

            case "": //the driver is reporting the controller as blank

#if !UNITY_EDITOR && UNITY_ANDROID

                switch (keyCode)
                {
                    case OuyaSDK.KeyEnum.AXIS_LSTICK_X:
                        axisName = string.Format("Joy{0} Axis 1", (int)player);
                        break;
                    case OuyaSDK.KeyEnum.AXIS_LSTICK_Y:
                        axisName = string.Format("Joy{0} Axis 2", (int)player);
                        invert = true;
                        break;
                    case OuyaSDK.KeyEnum.AXIS_RSTICK_X:
                        axisName = string.Format("Joy{0} Axis 3", (int)player);
                        break;
                    case OuyaSDK.KeyEnum.AXIS_RSTICK_Y:
                        axisName = string.Format("Joy{0} Axis 4", (int)player);
                        invert = true;
                        break;
                    case OuyaSDK.KeyEnum.BUTTON_LT:
                        axisName = string.Format("Joy{0} Axis 7", (int)player);
                        break;
                    case OuyaSDK.KeyEnum.BUTTON_RT:
                        axisName = string.Format("Joy{0} Axis 8", (int)player);
                        break;
                    case OuyaSDK.KeyEnum.BUTTON_DPAD_LEFT:
                        axisName = string.Format("Joy{0} Axis 5", (int)player);
                        break;
                    case OuyaSDK.KeyEnum.BUTTON_DPAD_RIGHT:
                        axisName = string.Format("Joy{0} Axis 5", (int)player);
                        break;
                    case OuyaSDK.KeyEnum.BUTTON_DPAD_UP:
                        axisName = string.Format("Joy{0} Axis 6", (int)player);
                        break;
                    case OuyaSDK.KeyEnum.BUTTON_DPAD_DOWN:
                        axisName = string.Format("Joy{0} Axis 6", (int)player);
                        break;
                    default:
                        return 0f;
                }

#else

                switch (keyCode)
                {
                    case OuyaSDK.KeyEnum.AXIS_LSTICK_X:
                        axisName = string.Format("Joy{0} Axis 1", (int)player);
                        break;
                    case OuyaSDK.KeyEnum.AXIS_LSTICK_Y:
                        axisName = string.Format("Joy{0} Axis 2", (int)player);
                        break;
                    case OuyaSDK.KeyEnum.AXIS_RSTICK_X:
                        axisName = string.Format("Joy{0} Axis 3", (int)player);
                        break;
                    case OuyaSDK.KeyEnum.AXIS_RSTICK_Y:
                        axisName = string.Format("Joy{0} Axis 4", (int)player);
                        break;
                    case OuyaSDK.KeyEnum.BUTTON_LT:
                        axisName = string.Format("Joy{0} Axis 5", (int)player);
                        break;
                    case OuyaSDK.KeyEnum.BUTTON_RT:
                        axisName = string.Format("Joy{0} Axis 6", (int)player);
                        break;
                    case OuyaSDK.KeyEnum.BUTTON_DPAD_LEFT:
                        if (GetButton(7, player))
                        {
                            return -1;
                        }
                        else
                        {
                            return 0;
                        }
                    case OuyaSDK.KeyEnum.BUTTON_DPAD_RIGHT:
                        if (GetButton(8, player))
                        {
                            return 1;
                        }
                        else
                        {
                            return 0;
                        }
                    case OuyaSDK.KeyEnum.BUTTON_DPAD_UP:
                        if (GetButton(5, player))
                        {
                            return 1;
                        }
                        else
                        {
                            return 0;
                        }
                    case OuyaSDK.KeyEnum.BUTTON_DPAD_DOWN:
                        if (GetButton(6, player))
                        {
                            return -1;
                        }
                        else
                        {
                            return 0;
                        }
                    default:
                        return 0f;
                }
#endif

                break;
        }
        if (string.IsNullOrEmpty(axisName))
        {
            return 0f;
        }
        if (invert)
        {
            return -Input.GetAxisRaw(axisName);
        }
        else
        {
            return Input.GetAxisRaw(axisName);
        }
    }

    public static string GetKeyCode(int buttonNum, OuyaSDK.OuyaPlayer player)
    {
        switch (player)
        {
            case OuyaSDK.OuyaPlayer.none:
                return string.Format("JoystickButton{0}", buttonNum);
            default:
                return string.Format("Joystick{0}Button{1}", ((int)player), buttonNum);
        }
    }

    public static bool GetButton(int buttonNum, OuyaSDK.OuyaPlayer player)
    {
        string keyCode = GetKeyCode(buttonNum, player);
        if (string.IsNullOrEmpty(keyCode))
        {
            return false;
        }
        OuyaKeyCode key = (OuyaKeyCode)Enum.Parse(typeof(OuyaKeyCode), keyCode);
        return Input.GetKey((KeyCode)(int)key);
    }

    public static bool GetButton(OuyaSDK.KeyEnum keyCode)
    {
        return (
                   GetButton(keyCode, OuyaSDK.OuyaPlayer.player1) ||
                   GetButton(keyCode, OuyaSDK.OuyaPlayer.player2) ||
                   GetButton(keyCode, OuyaSDK.OuyaPlayer.player3) ||
                   GetButton(keyCode, OuyaSDK.OuyaPlayer.player4) ||
                   GetButton(keyCode, OuyaSDK.OuyaPlayer.player5) ||
                   GetButton(keyCode, OuyaSDK.OuyaPlayer.player6) ||
                   GetButton(keyCode, OuyaSDK.OuyaPlayer.player7) ||
                   GetButton(keyCode, OuyaSDK.OuyaPlayer.player8));
    }

    public static bool GetButton(OuyaSDK.KeyEnum keyCode, OuyaSDK.OuyaPlayer player)
    {
        if (null == OuyaExampleCommon.Joysticks)
        {
            return false;
        }
        int playerIndex = (int) player - 1;
        if (playerIndex >= OuyaExampleCommon.Joysticks.Length)
        {
            return false;
        }

        string joystickName = OuyaExampleCommon.Joysticks[playerIndex];
        if (null == joystickName)
        {
            return false;
        }

        switch (joystickName.ToUpper())
        {
            case "HARMONIX ROCK BAND DRUMKIT":
                switch (keyCode)
                {
                    case OuyaSDK.KeyEnum.HARMONIX_ROCK_BAND_DRUMKIT_GREEN:
                    case OuyaSDK.KeyEnum.HARMONIX_ROCK_BAND_DRUMKIT_A:
                    case OuyaSDK.KeyEnum.BUTTON_O:
                        return GetButton(0, player);
                    case OuyaSDK.KeyEnum.HARMONIX_ROCK_BAND_DRUMKIT_RED:
                    case OuyaSDK.KeyEnum.HARMONIX_ROCK_BAND_DRUMKIT_B:
                    case OuyaSDK.KeyEnum.BUTTON_A:
                        return GetButton(1, player);
                    case OuyaSDK.KeyEnum.HARMONIX_ROCK_BAND_DRUMKIT_YELLOW:
                    case OuyaSDK.KeyEnum.HARMONIX_ROCK_BAND_DRUMKIT_Y:
                    case OuyaSDK.KeyEnum.BUTTON_Y:
                        return GetButton(4, player);
                    case OuyaSDK.KeyEnum.HARMONIX_ROCK_BAND_DRUMKIT_BLUE:
                    case OuyaSDK.KeyEnum.HARMONIX_ROCK_BAND_DRUMKIT_X:
                    case OuyaSDK.KeyEnum.BUTTON_U:
                        return GetButton(3, player);
                    case OuyaSDK.KeyEnum.HARMONIX_ROCK_BAND_DRUMKIT_ORANGE:
                    case OuyaSDK.KeyEnum.BUTTON_L3:
                        return GetButton(6, player);
                    case OuyaSDK.KeyEnum.BUTTON_BACK:
                    case OuyaSDK.KeyEnum.BUTTON_R3:
                        return GetButton(10, player);
                    case OuyaSDK.KeyEnum.BUTTON_START:
                    case OuyaSDK.KeyEnum.BUTTON_LB:
                        return GetButton(11, player);
                    case OuyaSDK.KeyEnum.BUTTON_SYSTEM:
                    case OuyaSDK.KeyEnum.BUTTON_RB:
                        return GetButton(12, player);
                    default:
                        return false;
                }
            case "HARMONIX ROCK BAND GUITAR":
                switch (keyCode)
                {
                    case OuyaSDK.KeyEnum.HARMONIX_ROCK_BAND_GUITAR_GREEN:
                    case OuyaSDK.KeyEnum.BUTTON_O:
                        return GetButton(0, player);
                    case OuyaSDK.KeyEnum.HARMONIX_ROCK_BAND_GUITAR_RED:
                    case OuyaSDK.KeyEnum.BUTTON_A:
                        return GetButton(1, player);
                    case OuyaSDK.KeyEnum.HARMONIX_ROCK_BAND_GUITAR_YELLOW:
                    case OuyaSDK.KeyEnum.BUTTON_Y:
                        return GetButton(4, player);
                    case OuyaSDK.KeyEnum.HARMONIX_ROCK_BAND_GUITAR_BLUE:
                    case OuyaSDK.KeyEnum.BUTTON_U:
                        return GetButton(3, player);
                    case OuyaSDK.KeyEnum.HARMONIX_ROCK_BAND_GUITAR_ORANGE:
                    case OuyaSDK.KeyEnum.BUTTON_L3:
                        return GetButton(6, player);
                    case OuyaSDK.KeyEnum.HARMONIX_ROCK_BAND_GUITAR_LOWER:
                        return GetButton(13, player);
                    case OuyaSDK.KeyEnum.BUTTON_BACK:
                    case OuyaSDK.KeyEnum.BUTTON_R3:
                        return GetButton(10, player);
                    case OuyaSDK.KeyEnum.BUTTON_START:
                    case OuyaSDK.KeyEnum.BUTTON_LB:
                        return GetButton(11, player);
                    case OuyaSDK.KeyEnum.BUTTON_SYSTEM:
                    case OuyaSDK.KeyEnum.BUTTON_RB:
                        return GetButton(12, player);
                    case OuyaSDK.KeyEnum.HARMONIX_ROCK_BAND_GUITAR_PICKUP:
                    case OuyaSDK.KeyEnum.BUTTON_LT:
                        return false;
                    case OuyaSDK.KeyEnum.HARMONIX_ROCK_BAND_GUITAR_WHAMMI:
                    case OuyaSDK.KeyEnum.BUTTON_RT:
                        return false;
                    case OuyaSDK.KeyEnum.BUTTON_DPAD_UP:
                        return GetAxis(keyCode, player) > 0f;
                    case OuyaSDK.KeyEnum.BUTTON_DPAD_DOWN:
                        return GetAxis(keyCode, player) < 0f;
                    case OuyaSDK.KeyEnum.BUTTON_DPAD_LEFT:
                        return GetAxis(keyCode, player) > 0f;
                    case OuyaSDK.KeyEnum.BUTTON_DPAD_RIGHT:
                        return GetAxis(keyCode, player) < 0f;
                    default:
                        return false;
                }

            case "BROADCOM BLUETOOTH HID":
            case "MOGA PRO HID":
                switch (keyCode)
                {
                    case OuyaSDK.KeyEnum.BUTTON_LB:
                        return GetButton(6, player);
                    case OuyaSDK.KeyEnum.BUTTON_RB:
                        return GetButton(7, player);
                    case OuyaSDK.KeyEnum.BUTTON_O:
                        return GetButton(0, player);
                    case OuyaSDK.KeyEnum.BUTTON_U:
                        return GetButton(3, player);
                    case OuyaSDK.KeyEnum.BUTTON_Y:
                        return GetButton(4, player);
                    case OuyaSDK.KeyEnum.BUTTON_A:
                        return GetButton(1, player);
                    case OuyaSDK.KeyEnum.BUTTON_L3:
                        return GetButton(13, player);
                    case OuyaSDK.KeyEnum.BUTTON_R3:
                        return GetButton(14, player);
                    case OuyaSDK.KeyEnum.BUTTON_DPAD_UP:
                        return GetAxis(keyCode, player) > 0f;
                    case OuyaSDK.KeyEnum.BUTTON_DPAD_DOWN:
                        return GetAxis(keyCode, player) < 0f;
                    case OuyaSDK.KeyEnum.BUTTON_DPAD_LEFT:
                        return GetAxis(keyCode, player) < 0f;
                    case OuyaSDK.KeyEnum.BUTTON_DPAD_RIGHT:
                        return GetAxis(keyCode, player) > 0f;
                    case OuyaSDK.KeyEnum.BUTTON_LT:
                        return GetAxis(keyCode, player) > 0f;
                    case OuyaSDK.KeyEnum.BUTTON_RT:
                        return GetAxis(keyCode, player) > 0f;
                    default:
                        return false;
                }

            case "OUYA GAME CONTROLLER":

                switch (keyCode)
                {
                    case OuyaSDK.KeyEnum.BUTTON_LB:
                        return GetButton(4, player);
                    case OuyaSDK.KeyEnum.BUTTON_RB:
                        return GetButton(5, player);
                    case OuyaSDK.KeyEnum.BUTTON_O:
                        return GetButton(0, player);
                    case OuyaSDK.KeyEnum.BUTTON_U:
                        return GetButton(1, player);
                    case OuyaSDK.KeyEnum.BUTTON_Y:
                        return GetButton(2, player);
                    case OuyaSDK.KeyEnum.BUTTON_A:
                        return GetButton(3, player);
                    case OuyaSDK.KeyEnum.BUTTON_L3:
                        return GetButton(6, player);
                    case OuyaSDK.KeyEnum.BUTTON_R3:
                        return GetButton(7, player);
                    case OuyaSDK.KeyEnum.BUTTON_DPAD_UP:
                        return GetButton(8, player);
                    case OuyaSDK.KeyEnum.BUTTON_DPAD_DOWN:
                        return GetButton(9, player);
                    case OuyaSDK.KeyEnum.BUTTON_DPAD_LEFT:
                        return GetButton(10, player);
                    case OuyaSDK.KeyEnum.BUTTON_DPAD_RIGHT:
                        return GetButton(11, player);
                    case OuyaSDK.KeyEnum.BUTTON_LT:
                        return GetButton(12, player);
                    case OuyaSDK.KeyEnum.BUTTON_RT:
                        return GetButton(13, player);
                    default:
                        return false;
                }

            case "XBOX 360 WIRELESS RECEIVER":

#if !UNITY_EDITOR && UNITY_ANDROID

                switch (keyCode)
                {
                    case OuyaSDK.KeyEnum.BUTTON_LB:
                        return GetButton(6, player);
                    case OuyaSDK.KeyEnum.BUTTON_RB:
                        return GetButton(7, player);
                    case OuyaSDK.KeyEnum.BUTTON_O:
                        return GetButton(0, player);
                    case OuyaSDK.KeyEnum.BUTTON_U:
                        return GetButton(3, player);
                    case OuyaSDK.KeyEnum.BUTTON_Y:
                        return GetButton(4, player);
                    case OuyaSDK.KeyEnum.BUTTON_A:
                        return GetButton(1, player);
                    case OuyaSDK.KeyEnum.BUTTON_L3:
                        return GetButton(13, player);
                    case OuyaSDK.KeyEnum.BUTTON_R3:
                        return GetButton(14, player);
                    case OuyaSDK.KeyEnum.BUTTON_DPAD_UP:
                        return GetButton(2, player);
                    case OuyaSDK.KeyEnum.BUTTON_DPAD_DOWN:
                        return GetButton(3, player);
                    case OuyaSDK.KeyEnum.BUTTON_DPAD_LEFT:
                        return GetButton(0, player);
                    case OuyaSDK.KeyEnum.BUTTON_DPAD_RIGHT:
                        return GetButton(1, player);
                    case OuyaSDK.KeyEnum.BUTTON_LT:
                        return GetAxis(keyCode, player) > 0f;
                    case OuyaSDK.KeyEnum.BUTTON_RT:
                        return GetAxis(keyCode, player) > 0f;
                    default:
                        return false;
                }
#else
                if (Application.platform == RuntimePlatform.OSXEditor)
                {
                    switch (keyCode)
                    {
                        case OuyaSDK.KeyEnum.BUTTON_LB:
                            return GetButton(13, player);
                        case OuyaSDK.KeyEnum.BUTTON_RB:
                            return GetButton(14, player);
                        case OuyaSDK.KeyEnum.BUTTON_O:
                            return GetButton(16, player);
                        case OuyaSDK.KeyEnum.BUTTON_U:
                            return GetButton(18, player);
                        case OuyaSDK.KeyEnum.BUTTON_Y:
                            return GetButton(19, player);
                        case OuyaSDK.KeyEnum.BUTTON_A:
                            return GetButton(17, player);
                        case OuyaSDK.KeyEnum.BUTTON_L3:
                            return GetButton(11, player);
                        case OuyaSDK.KeyEnum.BUTTON_R3:
                            return GetButton(12, player);
                        case OuyaSDK.KeyEnum.BUTTON_DPAD_UP:
                            return GetButton(5, player);
                        case OuyaSDK.KeyEnum.BUTTON_DPAD_DOWN:
                            return GetButton(6, player);
                        case OuyaSDK.KeyEnum.BUTTON_DPAD_LEFT:
                            return GetButton(7, player);
                        case OuyaSDK.KeyEnum.BUTTON_DPAD_RIGHT:
                            return GetButton(8, player);
                        case OuyaSDK.KeyEnum.BUTTON_LT:
                            return GetAxis(keyCode, player) > -1f;
                        case OuyaSDK.KeyEnum.BUTTON_RT:
                            return GetAxis(keyCode, player) > -1f;
                        default:
                            return false;
                    }
                }
                else
                {
                    switch (keyCode)
                    {
                        case OuyaSDK.KeyEnum.BUTTON_LB:
                            return GetButton(4, player);
                        case OuyaSDK.KeyEnum.BUTTON_RB:
                            return GetButton(5, player);
                        case OuyaSDK.KeyEnum.BUTTON_O:
                            return GetButton(0, player);
                        case OuyaSDK.KeyEnum.BUTTON_U:
                            return GetButton(2, player);
                        case OuyaSDK.KeyEnum.BUTTON_Y:
                            return GetButton(3, player);
                        case OuyaSDK.KeyEnum.BUTTON_A:
                            return GetButton(1, player);
                        case OuyaSDK.KeyEnum.BUTTON_L3:
                            return GetButton(8, player);
                        case OuyaSDK.KeyEnum.BUTTON_R3:
                            return GetButton(9, player);
                        case OuyaSDK.KeyEnum.BUTTON_DPAD_UP:
                            return GetAxis(keyCode, player) > 0f;
                        case OuyaSDK.KeyEnum.BUTTON_DPAD_DOWN:
                            return GetAxis(keyCode, player) < 0f;
                        case OuyaSDK.KeyEnum.BUTTON_DPAD_LEFT:
                            return GetAxis(keyCode, player) < 0f;
                        case OuyaSDK.KeyEnum.BUTTON_DPAD_RIGHT:
                            return GetAxis(keyCode, player) > 0f;
                        case OuyaSDK.KeyEnum.BUTTON_LT:
                            return GetAxis(keyCode, player) > 0f;
                        case OuyaSDK.KeyEnum.BUTTON_RT:
                            return GetAxis(keyCode, player) > 0f;
                        default:
                            return false;
                    }
                }
#endif

            case "CONTROLLER (AFTERGLOW GAMEPAD FOR XBOX 360)":
            case "CONTROLLER (ROCK CANDY GAMEPAD FOR XBOX 360)":
            case "CONTROLLER (XBOX 360 WIRELESS RECEIVER FOR WINDOWS)":
            case "MICROSOFT X-BOX 360 PAD":
            case "CONTROLLER (XBOX 360 FOR WINDOWS)":
            case "CONTROLLER (XBOX360 GAMEPAD)":
            case "XBOX 360 FOR WINDOWS (CONTROLLER)":
            case "MICROSOFT WIRELESS 360 CONTROLLER":

#if !UNITY_EDITOR && UNITY_ANDROID

                switch (keyCode)
                {
                    case OuyaSDK.KeyEnum.BUTTON_LB:
                        return GetButton(6, player);
                    case OuyaSDK.KeyEnum.BUTTON_RB:
                        return GetButton(7, player);
                    case OuyaSDK.KeyEnum.BUTTON_O:
                        return GetButton(0, player);
                    case OuyaSDK.KeyEnum.BUTTON_U:
                        return GetButton(3, player);
                    case OuyaSDK.KeyEnum.BUTTON_Y:
                        return GetButton(4, player);
                    case OuyaSDK.KeyEnum.BUTTON_A:
                        return GetButton(1, player);
                    case OuyaSDK.KeyEnum.BUTTON_L3:
                        return GetButton(13, player);
                    case OuyaSDK.KeyEnum.BUTTON_R3:
                        return GetButton(14, player);
                    case OuyaSDK.KeyEnum.BUTTON_DPAD_UP:
                        return GetAxis(keyCode, player) > 0f;
                    case OuyaSDK.KeyEnum.BUTTON_DPAD_DOWN:
                        return GetAxis(keyCode, player) < 0f;
                    case OuyaSDK.KeyEnum.BUTTON_DPAD_LEFT:
                        return GetAxis(keyCode, player) < 0f;
                    case OuyaSDK.KeyEnum.BUTTON_DPAD_RIGHT:
                        return GetAxis(keyCode, player) > 0f;
                    case OuyaSDK.KeyEnum.BUTTON_LT:
                        return GetAxis(keyCode, player) > 0f;
                    case OuyaSDK.KeyEnum.BUTTON_RT:
                        return GetAxis(keyCode, player) > 0f;
                    default:
                        return false;
                }
#else
                if (Application.platform == RuntimePlatform.OSXEditor)
                {
                    switch (keyCode)
                    {
                        case OuyaSDK.KeyEnum.BUTTON_LB:
                            return GetButton(13, player);
                        case OuyaSDK.KeyEnum.BUTTON_RB:
                            return GetButton(14, player);
                        case OuyaSDK.KeyEnum.BUTTON_O:
                            return GetButton(16, player);
                        case OuyaSDK.KeyEnum.BUTTON_U:
                            return GetButton(18, player);
                        case OuyaSDK.KeyEnum.BUTTON_Y:
                            return GetButton(19, player);
                        case OuyaSDK.KeyEnum.BUTTON_A:
                            return GetButton(17, player);
                        case OuyaSDK.KeyEnum.BUTTON_L3:
                            return GetButton(11, player);
                        case OuyaSDK.KeyEnum.BUTTON_R3:
                            return GetButton(12, player);
                        case OuyaSDK.KeyEnum.BUTTON_DPAD_UP:
                            return GetButton(5, player);
                        case OuyaSDK.KeyEnum.BUTTON_DPAD_DOWN:
                            return GetButton(6, player);
                        case OuyaSDK.KeyEnum.BUTTON_DPAD_LEFT:
                            return GetButton(7, player);
                        case OuyaSDK.KeyEnum.BUTTON_DPAD_RIGHT:
                            return GetButton(8, player);
                        case OuyaSDK.KeyEnum.BUTTON_LT:
                            return GetAxis(keyCode, player) > -1f;
                        case OuyaSDK.KeyEnum.BUTTON_RT:
                            return GetAxis(keyCode, player) > -1f;
                        default:
                            return false;
                    }
                }
                else
                {
                    switch (keyCode)
                    {
                        case OuyaSDK.KeyEnum.BUTTON_LB:
                            return GetButton(4, player);
                        case OuyaSDK.KeyEnum.BUTTON_RB:
                            return GetButton(5, player);
                        case OuyaSDK.KeyEnum.BUTTON_O:
                            return GetButton(0, player);
                        case OuyaSDK.KeyEnum.BUTTON_U:
                            return GetButton(2, player);
                        case OuyaSDK.KeyEnum.BUTTON_Y:
                            return GetButton(3, player);
                        case OuyaSDK.KeyEnum.BUTTON_A:
                            return GetButton(1, player);
                        case OuyaSDK.KeyEnum.BUTTON_L3:
                            return GetButton(8, player);
                        case OuyaSDK.KeyEnum.BUTTON_R3:
                            return GetButton(9, player);
                        case OuyaSDK.KeyEnum.BUTTON_DPAD_UP:
                            return GetAxis(keyCode, player) > 0f;
                        case OuyaSDK.KeyEnum.BUTTON_DPAD_DOWN:
                            return GetAxis(keyCode, player) < 0f;
                        case OuyaSDK.KeyEnum.BUTTON_DPAD_LEFT:
                            return GetAxis(keyCode, player) < 0f;
                        case OuyaSDK.KeyEnum.BUTTON_DPAD_RIGHT:
                            return GetAxis(keyCode, player) > 0f;
                        case OuyaSDK.KeyEnum.BUTTON_LT:
                            return GetAxis(keyCode, player) > 0f;
                        case OuyaSDK.KeyEnum.BUTTON_RT:
                            return GetAxis(keyCode, player) > 0f;
                        default:
                            return false;
                    }
                }
#endif

            case "": //the driver is reporting the controller as blank

#if !UNITY_EDITOR && UNITY_ANDROID

                switch (keyCode)
                {
                    case OuyaSDK.KeyEnum.BUTTON_LB:
                        return GetButton(6, player);
                    case OuyaSDK.KeyEnum.BUTTON_RB:
                        return GetButton(7, player);
                    case OuyaSDK.KeyEnum.BUTTON_O:
                        return GetButton(0, player);
                    case OuyaSDK.KeyEnum.BUTTON_U:
                        return GetButton(3, player);
                    case OuyaSDK.KeyEnum.BUTTON_Y:
                        return GetButton(4, player);
                    case OuyaSDK.KeyEnum.BUTTON_A:
                        return GetButton(1, player);
                    case OuyaSDK.KeyEnum.BUTTON_L3:
                        return GetButton(13, player);
                    case OuyaSDK.KeyEnum.BUTTON_R3:
                        return GetButton(14, player);
                    case OuyaSDK.KeyEnum.BUTTON_DPAD_UP:
                        return GetAxis(keyCode, player) > 0f;
                    case OuyaSDK.KeyEnum.BUTTON_DPAD_DOWN:
                        return GetAxis(keyCode, player) < 0f;
                    case OuyaSDK.KeyEnum.BUTTON_DPAD_LEFT:
                        return GetAxis(keyCode, player) < 0f;
                    case OuyaSDK.KeyEnum.BUTTON_DPAD_RIGHT:
                        return GetAxis(keyCode, player) > 0f;
                    case OuyaSDK.KeyEnum.BUTTON_LT:
                        return GetAxis(keyCode, player) > 0f;
                    case OuyaSDK.KeyEnum.BUTTON_RT:
                        return GetAxis(keyCode, player) > 0f;
                    default:
                        return false;
                }
#else
                switch (keyCode)
                {
                    case OuyaSDK.KeyEnum.BUTTON_LB:
                        return GetButton(13, player);
                    case OuyaSDK.KeyEnum.BUTTON_RB:
                        return GetButton(14, player);
                    case OuyaSDK.KeyEnum.BUTTON_O:
                        return GetButton(16, player);
                    case OuyaSDK.KeyEnum.BUTTON_U:
                        return GetButton(18, player);
                    case OuyaSDK.KeyEnum.BUTTON_Y:
                        return GetButton(19, player);
                    case OuyaSDK.KeyEnum.BUTTON_A:
                        return GetButton(17, player);
                    case OuyaSDK.KeyEnum.BUTTON_L3:
                        return GetButton(11, player);
                    case OuyaSDK.KeyEnum.BUTTON_R3:
                        return GetButton(12, player);
                    case OuyaSDK.KeyEnum.BUTTON_DPAD_UP:
                        return GetButton(5, player);
                    case OuyaSDK.KeyEnum.BUTTON_DPAD_DOWN:
                        return GetButton(6, player);
                    case OuyaSDK.KeyEnum.BUTTON_DPAD_LEFT:
                        return GetButton(7, player);
                    case OuyaSDK.KeyEnum.BUTTON_DPAD_RIGHT:
                        return GetButton(8, player);
                    case OuyaSDK.KeyEnum.BUTTON_LT:
                        return GetAxis(keyCode, player) > 0f;
                    case OuyaSDK.KeyEnum.BUTTON_RT:
                        return GetAxis(keyCode, player) > 0f;
                    case OuyaSDK.KeyEnum.BUTTON_SELECT:
                        return GetButton(10, player);
                    case OuyaSDK.KeyEnum.BUTTON_START:
                        return GetButton(9, player);
                    case OuyaSDK.KeyEnum.BUTTON_SYSTEM:
                        return GetButton(15, player);
                    default:
                        return false;
                }
#endif
        }

        return false;
    }

    #endregion

    public static void UpdateJoysticks()
    {
        if (m_timerJoysticks < DateTime.Now)
        {
            //check for new joysticks every N seconds
            m_timerJoysticks = DateTime.Now + TimeSpan.FromSeconds(3);

            Joysticks = Input.GetJoystickNames();
        }
    }
}