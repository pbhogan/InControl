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

public interface IOuyaController
{
    /// <summary>
    /// Return the supported Joystick names
    /// </summary>
    /// <returns></returns>
    string[] GetSupportedJoysicks();

    /// <summary>
    /// Return the supported axises
    /// </summary>
    /// <returns></returns>
    OuyaSDK.KeyEnum[] GetSupportedAxises();

    /// <summary>
    /// Return the supported buttons
    /// </summary>
    /// <returns></returns>
    OuyaSDK.KeyEnum[] GetSupportedButtons();

    /// <summary>
    /// Check if the controller supports the axis
    /// </summary>
    /// <param name="keyCode"></param>
    /// <returns></returns>
    bool HasAxis(OuyaSDK.KeyEnum keyCode);

    /// <summary>
    /// Check if the controller supports the button
    /// </summary>
    /// <param name="keyCode"></param>
    /// <returns></returns>
    bool HasButton(OuyaSDK.KeyEnum keyCode);

    /// <summary>
    /// Given the keycode and player, check if the axis is inverted
    /// </summary>
    /// <param name="keyCode"></param>
    /// <returns></returns>
    bool GetAxisInverted(OuyaSDK.KeyEnum keyCode);

    /// <summary>
    /// Given the keycode and player, return the AxisName used with the Unity API
    /// </summary>
    /// <param name="keyCode"></param>
    /// <param name="player"></param>
    /// <returns></returns>
    string GetUnityAxisName(OuyaSDK.KeyEnum keyCode, OuyaSDK.OuyaPlayer player);

    /// <summary>
    /// Given the keycode and player, return the KeyCode used with the Unity API
    /// </summary>
    /// <param name="keyCode"></param>
    /// <param name="player"></param>
    /// <returns></returns>
    KeyCode GetUnityKeyCode(OuyaSDK.KeyEnum keyCode, OuyaSDK.OuyaPlayer player);
}