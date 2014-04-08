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

public abstract class OuyaControllerCommon
{
    protected string CommonGetKeyString(int buttonNum, OuyaSDK.OuyaPlayer player)
    {
        switch (player)
        {
            case OuyaSDK.OuyaPlayer.none:
                return string.Format("JoystickButton{0}", buttonNum);
            default:
                return string.Format("Joystick{0}Button{1}", ((int)player), buttonNum);
        }
    }

    protected KeyCode CommonGetUnityKeyCode(int buttonNum, OuyaSDK.OuyaPlayer player)
    {
        string keyCode = CommonGetKeyString(buttonNum, player);
        if (string.IsNullOrEmpty(keyCode))
        {
            return (KeyCode)(-1);
        }
        OuyaKeyCode key = (OuyaKeyCode)Enum.Parse(typeof(OuyaKeyCode), keyCode);
        return (KeyCode)(int)key;
    }

    protected bool HasKeyCode(OuyaSDK.KeyEnum[] supportedCodes, OuyaSDK.KeyEnum keyCode)
    {
        foreach (OuyaSDK.KeyEnum item in supportedCodes)
        {
            if (item == keyCode)
            {
                return true;
            }
        }
        return false;
    }
}