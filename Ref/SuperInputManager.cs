/*
 * Based on OuyaExampleCommon.cs (June 2013) OUYA, Inc
 *
 * */

using UnityEngine;
using System;

public class SuperInputMapper {


    /// Cache joysticks
    public static string[] Joysticks = null;

    /// Query joysticks every N seconds
    private static DateTime m_timerJoysticks = DateTime.MinValue;

    #region Mapping Helpers

	public static float GreaterValue(float valIn, float compareVal){

		if (compareVal > 0f){
			if (valIn<0f && valIn*-1f < compareVal) return compareVal;
			if (valIn>=0f && valIn < compareVal) return compareVal;
		}else{
			if (valIn>0f && valIn*-1f > compareVal) return compareVal;
			if (valIn<=0f && valIn > compareVal) return compareVal;
		}

		return valIn;
	}

	public static float GetAxis(OuyaSDK.KeyEnum keyCode){
		float axisValue = 0f;
		axisValue = GreaterValue(axisValue, SuperInputMapper.GetAxis(keyCode, OuyaSDK.OuyaPlayer.player1) );
		axisValue = GreaterValue(axisValue, SuperInputMapper.GetAxis(keyCode, OuyaSDK.OuyaPlayer.player2) );
		axisValue = GreaterValue(axisValue, SuperInputMapper.GetAxis(keyCode, OuyaSDK.OuyaPlayer.player3) );
		axisValue = GreaterValue(axisValue, SuperInputMapper.GetAxis(keyCode, OuyaSDK.OuyaPlayer.player4) );
		axisValue = GreaterValue(axisValue, SuperInputMapper.GetAxis(keyCode, OuyaSDK.OuyaPlayer.player5) );
		axisValue = GreaterValue(axisValue, SuperInputMapper.GetAxis(keyCode, OuyaSDK.OuyaPlayer.player6) );
		axisValue = GreaterValue(axisValue, SuperInputMapper.GetAxis(keyCode, OuyaSDK.OuyaPlayer.player7) );
		axisValue = GreaterValue(axisValue, SuperInputMapper.GetAxis(keyCode, OuyaSDK.OuyaPlayer.player8) );
		axisValue = GreaterValue(axisValue, SuperInputMapper.GetAxis(keyCode, OuyaSDK.OuyaPlayer.player9) );
		axisValue = GreaterValue(axisValue, SuperInputMapper.GetAxis(keyCode, OuyaSDK.OuyaPlayer.player10) );
		axisValue = GreaterValue(axisValue, SuperInputMapper.GetAxis(keyCode, OuyaSDK.OuyaPlayer.player11) );

		return axisValue;
	}

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
        if (null == SuperInputMapper.Joysticks)
        {
            return 0f;
        }
        int playerIndex = (int)player - 1;
        if (playerIndex >= SuperInputMapper.Joysticks.Length)
        {
            return 0f;
        }

        string joystickName = SuperInputMapper.Joysticks[playerIndex];
        if (null == joystickName)
        {
            return 0f;
        }

        bool invert = false;
        string axisName = string.Empty;

        switch (joystickName.ToUpper())
        {

			case "SONY PLAYSTATION(R)3 CONTROLLER":
			case "PLAYSTATION(R)3 CONTROLLER":
			case "SONY PLAYSTATION[R]3 CONTROLLER":
			case "PLAYSTATION[R]3 CONTROLLER":
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

	            break;


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

	public static bool GetButton(OuyaSDK.KeyEnum keyCode){

		return (
			GetButton(keyCode, OuyaSDK.OuyaPlayer.player1) ||
           GetButton(keyCode, OuyaSDK.OuyaPlayer.player2) ||
           GetButton(keyCode, OuyaSDK.OuyaPlayer.player3) ||
           GetButton(keyCode, OuyaSDK.OuyaPlayer.player4) ||
           GetButton(keyCode, OuyaSDK.OuyaPlayer.player5) ||
           GetButton(keyCode, OuyaSDK.OuyaPlayer.player6) ||
           GetButton(keyCode, OuyaSDK.OuyaPlayer.player7) ||
           GetButton(keyCode, OuyaSDK.OuyaPlayer.player8) ||
           GetButton(keyCode, OuyaSDK.OuyaPlayer.player9) ||
           GetButton(keyCode, OuyaSDK.OuyaPlayer.player10) ||
           GetButton(keyCode, OuyaSDK.OuyaPlayer.player11)
			);
	}

    public static bool GetButton(OuyaSDK.KeyEnum keyCode, OuyaSDK.OuyaPlayer player)
    {

		UpdateJoysticks();

		//return false if no known gamepads
		if (null == SuperInputMapper.Joysticks) return false;

		//return false if this player's gamepad doesn't exist
        int playerIndex = (int) player - 1;
        if (playerIndex >= SuperInputMapper.Joysticks.Length) return false;

		//return false if we can't get a joystick name
        string joystickName = SuperInputMapper.Joysticks[playerIndex];
        if (null == joystickName) return false;


		//figure out button Index based on what controller type it is (mapping stuff wheeee!)
		ButtonIndexObj buttonIndex = OuyakeyToButtonIndex(keyCode, joystickName);

		if (buttonIndex.buttonIndex == -1) return false; //no such button
		if (buttonIndex.buttonIndex == -10) return Input.GetKey( (KeyCode)319 ); //OUYA home button

		if (buttonIndex.buttonIndex == -20){
			// button is an axis
			//return false; // FIX THIS!

			if (buttonIndex.compareType == 1){
				return GetAxis(keyCode, player) > buttonIndex.compareVal;
			}else{
				return GetAxis(keyCode, player) < buttonIndex.compareVal;
			}


		}else{
			// button is a regular button
			return ( GetButton(buttonIndex.buttonIndex, player) );
		}

    }

	public static bool GetButtonDown(int buttonNum, OuyaSDK.OuyaPlayer player)
    {
        string keyCode = GetKeyCode(buttonNum, player);
        if (string.IsNullOrEmpty(keyCode))
        {
            return false;
        }
        OuyaKeyCode key = (OuyaKeyCode)Enum.Parse(typeof(OuyaKeyCode), keyCode);
        return Input.GetKeyDown((KeyCode)(int)key);
    }

	public static bool GetButtonDown(OuyaSDK.KeyEnum keyCode){

		return (
			GetButtonDown(keyCode, OuyaSDK.OuyaPlayer.player1) ||
           GetButtonDown(keyCode, OuyaSDK.OuyaPlayer.player2) ||
           GetButtonDown(keyCode, OuyaSDK.OuyaPlayer.player3) ||
           GetButtonDown(keyCode, OuyaSDK.OuyaPlayer.player4) ||
           GetButtonDown(keyCode, OuyaSDK.OuyaPlayer.player5) ||
           GetButtonDown(keyCode, OuyaSDK.OuyaPlayer.player6) ||
           GetButtonDown(keyCode, OuyaSDK.OuyaPlayer.player7) ||
           GetButtonDown(keyCode, OuyaSDK.OuyaPlayer.player8) ||
           GetButtonDown(keyCode, OuyaSDK.OuyaPlayer.player9) ||
           GetButtonDown(keyCode, OuyaSDK.OuyaPlayer.player10) ||
           GetButtonDown(keyCode, OuyaSDK.OuyaPlayer.player11)
			);
	}

	public static bool GetButtonDown(OuyaSDK.KeyEnum keyCode, OuyaSDK.OuyaPlayer player)
    {
		UpdateJoysticks();

		//return false if no known gamepads
		if (null == SuperInputMapper.Joysticks) return false;

		//return false if this player's gamepad doesn't exist
        int playerIndex = (int) player - 1;
        if (playerIndex >= SuperInputMapper.Joysticks.Length) return false;

		//return false if we can't get a joystick name
        string joystickName = SuperInputMapper.Joysticks[playerIndex];
        if (null == joystickName) return false;


		//figure out button Index based on what controller type it is (mapping stuff wheeee!)
		ButtonIndexObj buttonIndex = OuyakeyToButtonIndex(keyCode, joystickName);

		if (buttonIndex.buttonIndex == -1) return false; //no such button
		if (buttonIndex.buttonIndex == -10) return Input.GetKeyDown( (KeyCode)319 ); //OUYA home button

		if (buttonIndex.buttonIndex == -20){
			// button is an axis
			return false; // FIX THIS!

		}else{
			// button is a regular button
			return ( GetButtonDown(buttonIndex.buttonIndex, player) );
		}
    }

	public static ButtonIndexObj ButtonIndex(int bindex, int ctype, float cval){
		ButtonIndexObj buttonIndexObj = new ButtonIndexObj();
			buttonIndexObj.buttonIndex = bindex;
			buttonIndexObj.compareType = ctype;
			buttonIndexObj.compareVal = cval;

		//Debug.Log(bindex);
		return buttonIndexObj;
	}

	public struct ButtonIndexObj{

		public int buttonIndex; // (-1 is non-existant button, -10 is OUYA home button, -20 is axis button)

		//if the button is really an axis, pass info needed to use axis as button
		public int compareType; //(1 is >, 2 is <, 3 is >=, 4 is <=)
		public float compareVal;


	}

	public static ButtonIndexObj OuyakeyToButtonIndex(OuyaSDK.KeyEnum keyCode, string joystickName){


		switch (joystickName.ToUpper())
        {

			case "SONY PLAYSTATION(R)3 CONTROLLER":
			case "PLAYSTATION(R)3 CONTROLLER":
			case "SONY PLAYSTATION[R]3 CONTROLLER":
			case "PLAYSTATION[R]3 CONTROLLER":
				// this code for all cases? might pay to have non-ouya mappings too. later tho
				//return true;
				switch (keyCode) {
                    case OuyaSDK.KeyEnum.BUTTON_LB:
                        return ButtonIndex(10,0,0f);
                    case OuyaSDK.KeyEnum.BUTTON_RB:
                        return ButtonIndex(11,0,0f);
                    case OuyaSDK.KeyEnum.BUTTON_O:
                        return ButtonIndex(14,0,0f);
                    case OuyaSDK.KeyEnum.BUTTON_U:
                        return ButtonIndex(15,0,0f);
                    case OuyaSDK.KeyEnum.BUTTON_Y:
                        return ButtonIndex(12,0,0f);
                    case OuyaSDK.KeyEnum.BUTTON_A:
                        return ButtonIndex(13,0,0f);
                    case OuyaSDK.KeyEnum.BUTTON_L3:
                        return ButtonIndex(1,0,0f);
                    case OuyaSDK.KeyEnum.BUTTON_R3:
                        return ButtonIndex(2,0,0f);
                    case OuyaSDK.KeyEnum.BUTTON_DPAD_UP:
                        return ButtonIndex(4,0,0f);
                    case OuyaSDK.KeyEnum.BUTTON_DPAD_DOWN:
                        return ButtonIndex(6,0,0f);
                    case OuyaSDK.KeyEnum.BUTTON_DPAD_LEFT:
                        return ButtonIndex(7,0,0f);
                    case OuyaSDK.KeyEnum.BUTTON_DPAD_RIGHT:
                        return ButtonIndex(5,0,0f);
                    case OuyaSDK.KeyEnum.BUTTON_LT:
                        return ButtonIndex(8,0,0f);
                    case OuyaSDK.KeyEnum.BUTTON_RT:
                        return ButtonIndex(9,0,0f);
                    case OuyaSDK.KeyEnum.BUTTON_SYSTEM:
                        return ButtonIndex(-10,0,0f);//319;
                    default:
                        return ButtonIndex(-1,0,0f);
                }
            case "HARMONIX ROCK BAND DRUMKIT":
                switch (keyCode)
                {
                    case OuyaSDK.KeyEnum.HARMONIX_ROCK_BAND_DRUMKIT_GREEN:
                    case OuyaSDK.KeyEnum.HARMONIX_ROCK_BAND_DRUMKIT_A:
                    case OuyaSDK.KeyEnum.BUTTON_O:
                        return ButtonIndex(0,0,0f);
                    case OuyaSDK.KeyEnum.HARMONIX_ROCK_BAND_DRUMKIT_RED:
                    case OuyaSDK.KeyEnum.HARMONIX_ROCK_BAND_DRUMKIT_B:
                    case OuyaSDK.KeyEnum.BUTTON_A:
                        return ButtonIndex(1,0,0f);
                    case OuyaSDK.KeyEnum.HARMONIX_ROCK_BAND_DRUMKIT_YELLOW:
                    case OuyaSDK.KeyEnum.HARMONIX_ROCK_BAND_DRUMKIT_Y:
                    case OuyaSDK.KeyEnum.BUTTON_Y:
                        return ButtonIndex(4,0,0f);
                    case OuyaSDK.KeyEnum.HARMONIX_ROCK_BAND_DRUMKIT_BLUE:
                    case OuyaSDK.KeyEnum.HARMONIX_ROCK_BAND_DRUMKIT_X:
                    case OuyaSDK.KeyEnum.BUTTON_U:
                        return ButtonIndex(3,0,0f);
                    case OuyaSDK.KeyEnum.HARMONIX_ROCK_BAND_DRUMKIT_ORANGE:
                    case OuyaSDK.KeyEnum.BUTTON_L3:
                        return ButtonIndex(6,0,0f);
                    case OuyaSDK.KeyEnum.BUTTON_BACK:
                    case OuyaSDK.KeyEnum.BUTTON_R3:
                        return ButtonIndex(10,0,0f);
                    case OuyaSDK.KeyEnum.BUTTON_START:
                    case OuyaSDK.KeyEnum.BUTTON_LB:
                        return ButtonIndex(11,0,0f);
                    case OuyaSDK.KeyEnum.BUTTON_SYSTEM:
                    case OuyaSDK.KeyEnum.BUTTON_RB:
                        return ButtonIndex(12,0,0f);
                    default:
                        return ButtonIndex(-1,0,0f);
                }
            case "HARMONIX ROCK BAND GUITAR":
                switch (keyCode)
                {
                    case OuyaSDK.KeyEnum.HARMONIX_ROCK_BAND_GUITAR_GREEN:
                    case OuyaSDK.KeyEnum.BUTTON_O:
                        return ButtonIndex(0,0,0f);
                    case OuyaSDK.KeyEnum.HARMONIX_ROCK_BAND_GUITAR_RED:
                    case OuyaSDK.KeyEnum.BUTTON_A:
                        return ButtonIndex(1,0,0f);
                    case OuyaSDK.KeyEnum.HARMONIX_ROCK_BAND_GUITAR_YELLOW:
                    case OuyaSDK.KeyEnum.BUTTON_Y:
                        return ButtonIndex(4,0,0f);
                    case OuyaSDK.KeyEnum.HARMONIX_ROCK_BAND_GUITAR_BLUE:
                    case OuyaSDK.KeyEnum.BUTTON_U:
                        return ButtonIndex(3,0,0f);
                    case OuyaSDK.KeyEnum.HARMONIX_ROCK_BAND_GUITAR_ORANGE:
                    case OuyaSDK.KeyEnum.BUTTON_L3:
                        return ButtonIndex(6,0,0f);
                    case OuyaSDK.KeyEnum.HARMONIX_ROCK_BAND_GUITAR_LOWER:
                        return ButtonIndex(13,0,0f);
                    case OuyaSDK.KeyEnum.BUTTON_BACK:
                    case OuyaSDK.KeyEnum.BUTTON_R3:
                        return ButtonIndex(10,0,0f);
                    case OuyaSDK.KeyEnum.BUTTON_START:
                    case OuyaSDK.KeyEnum.BUTTON_LB:
                        return ButtonIndex(11,0,0f);
                    case OuyaSDK.KeyEnum.BUTTON_SYSTEM:
                    case OuyaSDK.KeyEnum.BUTTON_RB:
                        return ButtonIndex(12,0,0f);
                    case OuyaSDK.KeyEnum.HARMONIX_ROCK_BAND_GUITAR_PICKUP:
                    case OuyaSDK.KeyEnum.BUTTON_LT:
                        return ButtonIndex(-1,0,0f);
                    case OuyaSDK.KeyEnum.HARMONIX_ROCK_BAND_GUITAR_WHAMMI:
                    case OuyaSDK.KeyEnum.BUTTON_RT:
                        return ButtonIndex(-1,0,0f);
                    case OuyaSDK.KeyEnum.BUTTON_DPAD_UP:
                        return ButtonIndex(-20, 1, 0f);//GetAxis(keyCode, player) > 0f;
                    case OuyaSDK.KeyEnum.BUTTON_DPAD_DOWN:
                        return ButtonIndex(-20, 2, 0f);//GetAxis(keyCode, player) < 0f;
                    case OuyaSDK.KeyEnum.BUTTON_DPAD_LEFT:
                        return ButtonIndex(-20, 1, 0f);//GetAxis(keyCode, player) > 0f;
                    case OuyaSDK.KeyEnum.BUTTON_DPAD_RIGHT:
                        return ButtonIndex(-20, 2, 0f);//GetAxis(keyCode, player) < 0f;
                    default:
                        return ButtonIndex(-1,0,0f);
                }

            case "BROADCOM BLUETOOTH HID":
            case "MOGA PRO HID":
                switch (keyCode)
                {
                    case OuyaSDK.KeyEnum.BUTTON_LB:
                        return ButtonIndex(6,0,0f);
                    case OuyaSDK.KeyEnum.BUTTON_RB:
                        return ButtonIndex(7,0,0f);
                    case OuyaSDK.KeyEnum.BUTTON_O:
                        return ButtonIndex(0,0,0f);
                    case OuyaSDK.KeyEnum.BUTTON_U:
                        return ButtonIndex(3,0,0f);
                    case OuyaSDK.KeyEnum.BUTTON_Y:
                        return ButtonIndex(4,0,0f);
                    case OuyaSDK.KeyEnum.BUTTON_A:
                        return ButtonIndex(1,0,0f);
                    case OuyaSDK.KeyEnum.BUTTON_L3:
                        return ButtonIndex(13,0,0f);
                    case OuyaSDK.KeyEnum.BUTTON_R3:
                        return ButtonIndex(14,0,0f);
                    case OuyaSDK.KeyEnum.BUTTON_DPAD_UP:
                        return ButtonIndex(-20, 1, 0f);//GetAxis(keyCode, player) > 0f;
                    case OuyaSDK.KeyEnum.BUTTON_DPAD_DOWN:
                        return ButtonIndex(-20, 2, 0f);//GetAxis(keyCode, player) < 0f;
                    case OuyaSDK.KeyEnum.BUTTON_DPAD_LEFT:
                        return ButtonIndex(-20, 2, 0f);//GetAxis(keyCode, player) < 0f;
                    case OuyaSDK.KeyEnum.BUTTON_DPAD_RIGHT:
                        return ButtonIndex(-20, 1, 0f);//GetAxis(keyCode, player) > 0f;
                    case OuyaSDK.KeyEnum.BUTTON_LT:
                        return ButtonIndex(-20, 1, 0f);//GetAxis(keyCode, player) > 0f;
                    case OuyaSDK.KeyEnum.BUTTON_RT:
                        return ButtonIndex(-20, 1, 0f);//GetAxis(keyCode, player) > 0f;
                    case OuyaSDK.KeyEnum.BUTTON_SYSTEM:
                        return ButtonIndex(-10,0,0f);//Input.GetKey((KeyCode)319);
                    default:
                        return ButtonIndex(-1,0,0f);
                }

            case "OUYA GAME CONTROLLER":

                switch (keyCode)
                {
                    case OuyaSDK.KeyEnum.BUTTON_LB:
                        return ButtonIndex(4,0,0f);
                    case OuyaSDK.KeyEnum.BUTTON_RB:
                        return ButtonIndex(5,0,0f);
                    case OuyaSDK.KeyEnum.BUTTON_O:
                        return ButtonIndex(0,0,0f);
                    case OuyaSDK.KeyEnum.BUTTON_U:
                        return ButtonIndex(1,0,0f);
                    case OuyaSDK.KeyEnum.BUTTON_Y:
                        return ButtonIndex(2,0,0f);
                    case OuyaSDK.KeyEnum.BUTTON_A:
                        return ButtonIndex(3,0,0f);
                    case OuyaSDK.KeyEnum.BUTTON_L3:
                        return ButtonIndex(6,0,0f);
                    case OuyaSDK.KeyEnum.BUTTON_R3:
                        return ButtonIndex(7,0,0f);
                    case OuyaSDK.KeyEnum.BUTTON_DPAD_UP:
                        return ButtonIndex(8,0,0f);
                    case OuyaSDK.KeyEnum.BUTTON_DPAD_DOWN:
                        return ButtonIndex(9,0,0f);
                    case OuyaSDK.KeyEnum.BUTTON_DPAD_LEFT:
                        return ButtonIndex(10,0,0f);
                    case OuyaSDK.KeyEnum.BUTTON_DPAD_RIGHT:
                        return ButtonIndex(11,0,0f);
                    case OuyaSDK.KeyEnum.BUTTON_LT:
                        return ButtonIndex(12,0,0f);
                    case OuyaSDK.KeyEnum.BUTTON_RT:
                        return ButtonIndex(13,0,0f);
                    case OuyaSDK.KeyEnum.BUTTON_SYSTEM:
                        return ButtonIndex(-10,0,0f);//Input.GetKey((KeyCode)319);
                    default:
                        return ButtonIndex(-1,0,0f);
                }


            case "XBOX 360 WIRELESS RECEIVER":

#if !UNITY_EDITOR && UNITY_ANDROID

                switch (keyCode)
                {
                    case OuyaSDK.KeyEnum.BUTTON_LB:
                        return ButtonIndex(6,0,0f);
                    case OuyaSDK.KeyEnum.BUTTON_RB:
                        return ButtonIndex(7,0,0f);
                    case OuyaSDK.KeyEnum.BUTTON_O:
                        return ButtonIndex(0,0,0f);
                    case OuyaSDK.KeyEnum.BUTTON_U:
                        return ButtonIndex(3,0,0f);
                    case OuyaSDK.KeyEnum.BUTTON_Y:
                        return ButtonIndex(4,0,0f);
                    case OuyaSDK.KeyEnum.BUTTON_A:
                        return ButtonIndex(1,0,0f);
                    case OuyaSDK.KeyEnum.BUTTON_L3:
                        return ButtonIndex(13,0,0f);
                    case OuyaSDK.KeyEnum.BUTTON_R3:
                        return ButtonIndex(14,0,0f);
                    case OuyaSDK.KeyEnum.BUTTON_DPAD_UP:
                        return ButtonIndex(2,0,0f);
                    case OuyaSDK.KeyEnum.BUTTON_DPAD_DOWN:
                        return ButtonIndex(3,0,0f);
                    case OuyaSDK.KeyEnum.BUTTON_DPAD_LEFT:
                        return ButtonIndex(0,0,0f);
                    case OuyaSDK.KeyEnum.BUTTON_DPAD_RIGHT:
                        return ButtonIndex(1,0,0f);
                    case OuyaSDK.KeyEnum.BUTTON_LT:
                        return ButtonIndex(-20, 1, 0f);//GetAxis(keyCode, player) > 0f;
                    case OuyaSDK.KeyEnum.BUTTON_RT:
                        return ButtonIndex(-20, 1, 0f);//GetAxis(keyCode, player) > 0f;
                    case OuyaSDK.KeyEnum.BUTTON_SYSTEM:
                        return ButtonIndex(-10,0,0f);//Input.GetKey((KeyCode)319);
                    default:
                        return ButtonIndex(-1,0,0f);
                }
#else
                if (Application.platform == RuntimePlatform.OSXEditor)
                {
                    switch (keyCode)
                    {
                        case OuyaSDK.KeyEnum.BUTTON_LB:
                            return ButtonIndex(13,0,0f);
                        case OuyaSDK.KeyEnum.BUTTON_RB:
                            return ButtonIndex(14,0,0f);
                        case OuyaSDK.KeyEnum.BUTTON_O:
                            return ButtonIndex(16,0,0f);
                        case OuyaSDK.KeyEnum.BUTTON_U:
                            return ButtonIndex(18,0,0f);
                        case OuyaSDK.KeyEnum.BUTTON_Y:
                            return ButtonIndex(19,0,0f);
                        case OuyaSDK.KeyEnum.BUTTON_A:
                            return ButtonIndex(17,0,0f);
                        case OuyaSDK.KeyEnum.BUTTON_L3:
                            return ButtonIndex(11,0,0f);
                        case OuyaSDK.KeyEnum.BUTTON_R3:
                            return ButtonIndex(12,0,0f);
                        case OuyaSDK.KeyEnum.BUTTON_DPAD_UP:
                            return ButtonIndex(5,0,0f);
                        case OuyaSDK.KeyEnum.BUTTON_DPAD_DOWN:
                            return ButtonIndex(6,0,0f);
                        case OuyaSDK.KeyEnum.BUTTON_DPAD_LEFT:
                            return ButtonIndex(7,0,0f);
                        case OuyaSDK.KeyEnum.BUTTON_DPAD_RIGHT:
                            return ButtonIndex(8,0,0f);
                        case OuyaSDK.KeyEnum.BUTTON_LT:
                            return ButtonIndex(-20, 1, -1f);//GetAxis(keyCode, player) > -1f;
                        case OuyaSDK.KeyEnum.BUTTON_RT:
                            return ButtonIndex(-20, 1, -1f);//GetAxis(keyCode, player) > -1f;
                        default:
                            return ButtonIndex(-1,0,0f);
                    }
                }
                else
                {
                    switch (keyCode)
                    {
                        case OuyaSDK.KeyEnum.BUTTON_LB:
                            return ButtonIndex(4,0,0f);
                        case OuyaSDK.KeyEnum.BUTTON_RB:
                            return ButtonIndex(5,0,0f);
                        case OuyaSDK.KeyEnum.BUTTON_O:
                            return ButtonIndex(0,0,0f);
                        case OuyaSDK.KeyEnum.BUTTON_U:
                            return ButtonIndex(2,0,0f);
                        case OuyaSDK.KeyEnum.BUTTON_Y:
                            return ButtonIndex(3,0,0f);
                        case OuyaSDK.KeyEnum.BUTTON_A:
                            return ButtonIndex(1,0,0f);
                        case OuyaSDK.KeyEnum.BUTTON_L3:
                            return ButtonIndex(8,0,0f);
                        case OuyaSDK.KeyEnum.BUTTON_R3:
                            return ButtonIndex(9,0,0f);
                        case OuyaSDK.KeyEnum.BUTTON_DPAD_UP:
                            return ButtonIndex(-20, 1, 0f);//GetAxis(keyCode, player) > 0f;
                        case OuyaSDK.KeyEnum.BUTTON_DPAD_DOWN:
                            return ButtonIndex(-20, 2, 0f);//GetAxis(keyCode, player) < 0f;
                        case OuyaSDK.KeyEnum.BUTTON_DPAD_LEFT:
                            return ButtonIndex(-20, 2, 0f);//GetAxis(keyCode, player) < 0f;
                        case OuyaSDK.KeyEnum.BUTTON_DPAD_RIGHT:
                            return ButtonIndex(-20, 1, 0f);//GetAxis(keyCode, player) > 0f;
                        case OuyaSDK.KeyEnum.BUTTON_LT:
                            return ButtonIndex(-20, 1, 0f);//GetAxis(keyCode, player) > 0f;
                        case OuyaSDK.KeyEnum.BUTTON_RT:
                            return ButtonIndex(-20, 1, 0f);//GetAxis(keyCode, player) > 0f;
                        default:
                            return ButtonIndex(-1,0,0f);
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
                        return ButtonIndex(6,0,0f);
                    case OuyaSDK.KeyEnum.BUTTON_RB:
                        return ButtonIndex(7,0,0f);
                    case OuyaSDK.KeyEnum.BUTTON_O:
                        return ButtonIndex(0,0,0f);
                    case OuyaSDK.KeyEnum.BUTTON_U:
                        return ButtonIndex(3,0,0f);
                    case OuyaSDK.KeyEnum.BUTTON_Y:
                        return ButtonIndex(4,0,0f);
                    case OuyaSDK.KeyEnum.BUTTON_A:
                        return ButtonIndex(1,0,0f);
                    case OuyaSDK.KeyEnum.BUTTON_L3:
                        return ButtonIndex(13,0,0f);
                    case OuyaSDK.KeyEnum.BUTTON_R3:
                        return ButtonIndex(14,0,0f);
                    case OuyaSDK.KeyEnum.BUTTON_DPAD_UP:
                        return ButtonIndex(-20, 1, 0f);//GetAxis(keyCode, player) > 0f;
                    case OuyaSDK.KeyEnum.BUTTON_DPAD_DOWN:
                        return ButtonIndex(-20, 2, 0f);//GetAxis(keyCode, player) < 0f;
                    case OuyaSDK.KeyEnum.BUTTON_DPAD_LEFT:
                        return ButtonIndex(-20, 2, 0f);//GetAxis(keyCode, player) < 0f;
                    case OuyaSDK.KeyEnum.BUTTON_DPAD_RIGHT:
                        return ButtonIndex(-20, 1, 0f);//GetAxis(keyCode, player) > 0f;
                    case OuyaSDK.KeyEnum.BUTTON_LT:
                        return ButtonIndex(-20, 1, 0f);//GetAxis(keyCode, player) > 0f;
                    case OuyaSDK.KeyEnum.BUTTON_RT:
                        return ButtonIndex(-20, 1, 0f);//GetAxis(keyCode, player) > 0f;
                    case OuyaSDK.KeyEnum.BUTTON_SYSTEM:
                        return ButtonIndex(-10,0,0f);//Input.GetKey((KeyCode)319);
                    default:
                        return ButtonIndex(-1,0,0f);
                }
#else
                if (Application.platform == RuntimePlatform.OSXEditor)
                {
                    switch (keyCode)
                    {
                        case OuyaSDK.KeyEnum.BUTTON_LB:
                            return ButtonIndex(13,0,0f);
                        case OuyaSDK.KeyEnum.BUTTON_RB:
                            return ButtonIndex(14,0,0f);
                        case OuyaSDK.KeyEnum.BUTTON_O:
                            return ButtonIndex(16,0,0f);
                        case OuyaSDK.KeyEnum.BUTTON_U:
                            return ButtonIndex(18,0,0f);
                        case OuyaSDK.KeyEnum.BUTTON_Y:
                            return ButtonIndex(19,0,0f);
                        case OuyaSDK.KeyEnum.BUTTON_A:
                            return ButtonIndex(17,0,0f);
                        case OuyaSDK.KeyEnum.BUTTON_L3:
                            return ButtonIndex(11,0,0f);
                        case OuyaSDK.KeyEnum.BUTTON_R3:
                            return ButtonIndex(12,0,0f);
                        case OuyaSDK.KeyEnum.BUTTON_DPAD_UP:
                            return ButtonIndex(5,0,0f);
                        case OuyaSDK.KeyEnum.BUTTON_DPAD_DOWN:
                            return ButtonIndex(6,0,0f);
                        case OuyaSDK.KeyEnum.BUTTON_DPAD_LEFT:
                            return ButtonIndex(7,0,0f);
                        case OuyaSDK.KeyEnum.BUTTON_DPAD_RIGHT:
                            return ButtonIndex(8,0,0f);
                        case OuyaSDK.KeyEnum.BUTTON_LT:
                            return ButtonIndex(-20, 1, -1f);//GetAxis(keyCode, player) > -1f;
                        case OuyaSDK.KeyEnum.BUTTON_RT:
                            return ButtonIndex(-20, 1, -1f);//GetAxis(keyCode, player) > -1f;
                        default:
                            return ButtonIndex(-1,0,0f);
                    }
                }
                else
                {
                    switch (keyCode)
                    {
                        case OuyaSDK.KeyEnum.BUTTON_LB:
                            return ButtonIndex(4,0,0f);
                        case OuyaSDK.KeyEnum.BUTTON_RB:
                            return ButtonIndex(5,0,0f);
                        case OuyaSDK.KeyEnum.BUTTON_O:
                            return ButtonIndex(0,0,0f);
                        case OuyaSDK.KeyEnum.BUTTON_U:
                            return ButtonIndex(2,0,0f);
                        case OuyaSDK.KeyEnum.BUTTON_Y:
                            return ButtonIndex(3,0,0f);
                        case OuyaSDK.KeyEnum.BUTTON_A:
                            return ButtonIndex(1,0,0f);
                        case OuyaSDK.KeyEnum.BUTTON_L3:
                            return ButtonIndex(8,0,0f);
                        case OuyaSDK.KeyEnum.BUTTON_R3:
                            return ButtonIndex(9,0,0f);
                        case OuyaSDK.KeyEnum.BUTTON_DPAD_UP:
                            return ButtonIndex(-20, 1, 0f);//GetAxis(keyCode, player) > 0f;
                        case OuyaSDK.KeyEnum.BUTTON_DPAD_DOWN:
                            return ButtonIndex(-20, 2, 0f);//GetAxis(keyCode, player) < 0f;
                        case OuyaSDK.KeyEnum.BUTTON_DPAD_LEFT:
                            return ButtonIndex(-20, 2, 0f);//GetAxis(keyCode, player) < 0f;
                        case OuyaSDK.KeyEnum.BUTTON_DPAD_RIGHT:
                            return ButtonIndex(-20, 1, 0f);//GetAxis(keyCode, player) > 0f;
                        case OuyaSDK.KeyEnum.BUTTON_LT:
                            return ButtonIndex(-20, 1, 0f);//GetAxis(keyCode, player) > 0f;
                        case OuyaSDK.KeyEnum.BUTTON_RT:
                            return ButtonIndex(-20, 1, 0f);//GetAxis(keyCode, player) > 0f;
                        default:
                            return ButtonIndex(-1,0,0f);
                    }
                }
#endif

            case "": //the driver is reporting the controller as blank

#if !UNITY_EDITOR && UNITY_ANDROID

                switch (keyCode)
                {
                    case OuyaSDK.KeyEnum.BUTTON_LB:
                        return ButtonIndex(6,0,0f);
                    case OuyaSDK.KeyEnum.BUTTON_RB:
                        return ButtonIndex(7,0,0f);
                    case OuyaSDK.KeyEnum.BUTTON_O:
                        return ButtonIndex(0,0,0f);
                    case OuyaSDK.KeyEnum.BUTTON_U:
                        return ButtonIndex(3,0,0f);
                    case OuyaSDK.KeyEnum.BUTTON_Y:
                        return ButtonIndex(4,0,0f);
                    case OuyaSDK.KeyEnum.BUTTON_A:
                        return ButtonIndex(1,0,0f);
                    case OuyaSDK.KeyEnum.BUTTON_L3:
                        return ButtonIndex(13,0,0f);
                    case OuyaSDK.KeyEnum.BUTTON_R3:
                        return ButtonIndex(14,0,0f);
                    case OuyaSDK.KeyEnum.BUTTON_DPAD_UP:
                        return ButtonIndex(-20, 1, 0f);//GetAxis(keyCode, player) > 0f;
                    case OuyaSDK.KeyEnum.BUTTON_DPAD_DOWN:
                        return ButtonIndex(-20, 2, 0f);//GetAxis(keyCode, player) < 0f;
                    case OuyaSDK.KeyEnum.BUTTON_DPAD_LEFT:
                        return ButtonIndex(-20, 2, 0f);//GetAxis(keyCode, player) < 0f;
                    case OuyaSDK.KeyEnum.BUTTON_DPAD_RIGHT:
                        return ButtonIndex(-20, 1, 0f);//GetAxis(keyCode, player) > 0f;
                    case OuyaSDK.KeyEnum.BUTTON_LT:
                        return ButtonIndex(-20, 1, 0f);//GetAxis(keyCode, player) > 0f;
                    case OuyaSDK.KeyEnum.BUTTON_RT:
                        return ButtonIndex(-20, 1, 0f);//GetAxis(keyCode, player) > 0f;
                    case OuyaSDK.KeyEnum.BUTTON_SYSTEM:
                        return ButtonIndex(-10,0,0f);//Input.GetKey((KeyCode)319);
                    default:
                        return ButtonIndex(-1,0,0f);
                }
#else
                switch (keyCode)
                {
                    case OuyaSDK.KeyEnum.BUTTON_LB:
                        return ButtonIndex(13,0,0f);
                    case OuyaSDK.KeyEnum.BUTTON_RB:
                        return ButtonIndex(14,0,0f);
                    case OuyaSDK.KeyEnum.BUTTON_O:
                        return ButtonIndex(16,0,0f);
                    case OuyaSDK.KeyEnum.BUTTON_U:
                        return ButtonIndex(18,0,0f);
                    case OuyaSDK.KeyEnum.BUTTON_Y:
                        return ButtonIndex(19,0,0f);
                    case OuyaSDK.KeyEnum.BUTTON_A:
                        return ButtonIndex(17,0,0f);
                    case OuyaSDK.KeyEnum.BUTTON_L3:
                        return ButtonIndex(11,0,0f);
                    case OuyaSDK.KeyEnum.BUTTON_R3:
                        return ButtonIndex(12,0,0f);
                    case OuyaSDK.KeyEnum.BUTTON_DPAD_UP:
                        return ButtonIndex(5,0,0f);
                    case OuyaSDK.KeyEnum.BUTTON_DPAD_DOWN:
                        return ButtonIndex(6,0,0f);
                    case OuyaSDK.KeyEnum.BUTTON_DPAD_LEFT:
                        return ButtonIndex(7,0,0f);
                    case OuyaSDK.KeyEnum.BUTTON_DPAD_RIGHT:
                        return ButtonIndex(8,0,0f);
                    case OuyaSDK.KeyEnum.BUTTON_LT:
                        return ButtonIndex(-20, 1, 0f);//GetAxis(keyCode, player) > 0f;
                    case OuyaSDK.KeyEnum.BUTTON_RT:
                        return ButtonIndex(-20, 1, 0f);//GetAxis(keyCode, player) > 0f;
                    case OuyaSDK.KeyEnum.BUTTON_SELECT:
                        return ButtonIndex(10,0,0f);
                    case OuyaSDK.KeyEnum.BUTTON_START:
                        return ButtonIndex(9,0,0f);
                    case OuyaSDK.KeyEnum.BUTTON_SYSTEM:
                        return ButtonIndex(15,0,0f);
                    default:
                        return ButtonIndex(-1,0,0f);
                }
#endif
        }
		return ButtonIndex(-1, 0, 0f);
	}


    #endregion

    public static void UpdateJoysticks()
    {
        if (m_timerJoysticks < DateTime.Now)
        {
            //check for new joysticks every N seconds
            m_timerJoysticks = DateTime.Now + TimeSpan.FromSeconds(3);

            Joysticks = Input.GetJoystickNames();
			//Debug.Log(Joysticks.Length);
        }
    }
}