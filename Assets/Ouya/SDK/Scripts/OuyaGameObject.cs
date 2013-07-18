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
using LitJson;
using System.Collections.Generic;
using UnityEngine;

public class OuyaGameObject : MonoBehaviour
{
    #region Public Visible Variables

    public string DEVELOPER_ID = "310a8f51-4d6e-4ae5-bda0-b93878e5f5d0";
    public bool debugOff = false;
    public bool showRawAxis = false;

    public string[] Purchasables =
    {
        "long_sword",
        "sharp_axe",
        "__DECLINED__THIS_PURCHASE",
    };

    public bool UseLegacyInput = true;

    [@HideInInspector]
    private static string m_inputData = null;
    public static string InputData{
        get{
            return m_inputData;
        }
        set{
            m_inputData = value;
        }
    }
    #endregion

    #region Public Not Visible Variables
    public static List<Device> devices = new List<OuyaGameObject.Device>();
    #endregion 

    #region Private Variables
    //private string m_inputData = string.Empty;
    private static OuyaGameObject m_instance = null;
#if UNITY_EDITOR || UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX
    private bool axisSetTriggers = false;
    private bool axisSet = false;
#endif
    #endregion 

    #region Singleton Accessor Class
    /// <summary>
    /// Singleton interface
    /// </summary>
    public static OuyaGameObject Singleton
    {
        get
        {
            if (null == m_instance)
            {
                GameObject ouyaGameObject = GameObject.Find("OuyaGameObject");
                if (ouyaGameObject)
                {
                    m_instance = ouyaGameObject.GetComponent<OuyaGameObject>();
                }
            }
            return m_instance;
        }
    }
    #endregion 
     
    #region Java To Unity Event Handlers

    [Obsolete("No longer needed in the next version 1.0.3.13")]
    public void onKeyDown(string jsonData)
    {
        InputListener(OuyaSDK.InputAction.KeyDown, jsonData);
    }

    [Obsolete("No longer needed in the next version 1.0.3.13")]
    public void onKeyUp(string jsonData)
    {
        InputListener(OuyaSDK.InputAction.KeyUp, jsonData);
    }

    [Obsolete("No longer needed in the next version 1.0.3.13")]
    public void onGenericMotionEvent(string jsonData)
    {
        InputListener(OuyaSDK.InputAction.GenericMotionEvent, jsonData);
    }

    [Obsolete("No longer needed in the next version 1.0.3.13")]
    public void onTouchEvent(string jsonData)
    {
        InputListener(OuyaSDK.InputAction.TouchEvent, jsonData);
    }

    [Obsolete("No longer needed in the next version 1.0.3.13")]
    public void onTrackballEvent(string jsonData)
    {
        InputListener(OuyaSDK.InputAction.TrackballEvent, jsonData);
    }

    [Obsolete("No longer needed in the next version 1.0.3.13")]
    public void onSetDevices(string jsonData)
    {
		//Debug.Log(string.Format("Devices jsonData={0}", jsonData));

        devices.Clear();
		
        List<Device> deviceList = new List<Device>();
        deviceList = JsonMapper.ToObject<List<Device>>(jsonData);
        foreach(Device d in deviceList){
            //Debug.Log("DeviceID:" + d.id + " DevicePlayer:" + d.player + " DeviceName:" + d.name);
            devices.Add(d);
        }
        //Debug.Log("DeviceCount:" + devices.Count);
    }

    public void onMenuButtonUp(string ignore)
    {
        //Debug.Log("OuyaGameObject: onMenuButtonUp");
        foreach (OuyaSDK.IMenuButtonUpListener listener in OuyaSDK.getMenuButtonUpListeners())
        {
            //Debug.Log("OuyaGameObject: Invoke OuyaMenuButtonUp");
            listener.OuyaMenuButtonUp();
        }
    }

    public void onMenuAppearing(string ignore)
    {
        //Debug.Log("onMenuAppearing");
        foreach (OuyaSDK.IMenuAppearingListener listener in OuyaSDK.getMenuAppearingListeners())
        {
            listener.OuyaMenuAppearing();
        }
    }

    public void onPause()
    {
        //Debug.Log("onPause");
        foreach (OuyaSDK.IPauseListener listener in OuyaSDK.getPauseListeners())
        {
            listener.OuyaOnPause();
        }
    }

    public void onResume()
    {
        //Debug.Log("onResume");
        foreach (OuyaSDK.IResumeListener listener in OuyaSDK.getResumeListeners())
        {
            listener.OuyaOnResume();
        }
    }

    #endregion
    
    #region JSON Data Listeners

    public void FetchGamerUUIDSuccessListener(string gamerUUID)
    {
        Debug.LogError(string.Format("FetchGamerUUIDSuccessListener gamerUUID={0}", gamerUUID));
        InvokeOuyaFetchGamerUUIDOnSuccess(gamerUUID);
    }
    public void FetchGamerUUIDFailureListener(string jsonData)
    {
        Debug.LogError(string.Format("FetchGamerUUIDFailureListener jsonData={0}", jsonData));
        InvokeOuyaFetchGamerUUIDOnFailure(0, jsonData);
    }
    public void FetchGamerUUIDCancelListener(string ignore)
    {
        InvokeOuyaFetchGamerUUIDOnCancel();
    }

    private List<OuyaSDK.Product> m_products = new List<OuyaSDK.Product>();

    public void ProductListClearListener(string ignore)
    {
        m_products.Clear();
    }
    public void ProductListListener(string jsonData)
    {
        if (string.IsNullOrEmpty(jsonData))
        {
            Debug.LogError("OuyaSDK.ProductListListener: received empty jsondata");
            return;
        }

        Debug.Log(string.Format("OuyaSDK.ProductListListener: jsonData={0}", jsonData));
        OuyaSDK.Product product = JsonMapper.ToObject<OuyaSDK.Product>(jsonData);
        m_products.Add(product);
    }
    public void ProductListFailureListener(string jsonData)
    {
        Debug.LogError(string.Format("ProductListFailureListener jsonData={0}", jsonData));
        InvokeOuyaGetReceiptsOnFailure(0, jsonData);
    }
    public void ProductListCompleteListener(string ignore)
    {
        foreach (OuyaSDK.Product product in m_products)
        {
            Debug.Log(string.Format("ProductListCompleteListener Product id={0} name={1} price={2}",
                product.getIdentifier(), product.getName(), product.getPriceInCents()));
        }
        InvokeOuyaGetProductsOnSuccess(m_products);
    }

    public void PurchaseSuccessListener(string jsonData)
    {
        Debug.LogError(string.Format("PurchaseSuccessListener jsonData={0}", jsonData));
        OuyaSDK.Product product = JsonMapper.ToObject<OuyaSDK.Product>(jsonData);
        InvokeOuyaPurchaseOnSuccess(product);
    }
    public void PurchaseFailureListener(string jsonData)
    {
        Debug.LogError(string.Format("PurchaseFailureListener jsonData={0}", jsonData));
        InvokeOuyaPurchaseOnFailure(0, jsonData);
    }
    public void PurchaseCancelListener(string ignore)
    {
        InvokeOuyaPurchaseOnCancel();
    }
    
    private List<OuyaSDK.Receipt> m_receipts = new List<OuyaSDK.Receipt>();

    public void ReceiptListClearListener(string ignore)
    {
        m_receipts.Clear();
    }
    public void ReceiptListListener(string jsonData)
    {
        if (string.IsNullOrEmpty(jsonData))
        {
            Debug.LogError("OuyaSDK.ReceiptListListener: received empty jsondata");
            return;
        }

        Debug.Log(string.Format("OuyaSDK.ReceiptListListener: jsonData={0}", jsonData));
        OuyaSDK.Receipt receipt = JsonMapper.ToObject<OuyaSDK.Receipt>(jsonData);
        m_receipts.Add(receipt);
    }
    public void ReceiptListCompleteListener(string ignore)
    {
        foreach (OuyaSDK.Receipt receipt in m_receipts)
        {
            Debug.Log(string.Format("ReceiptListCompleteListener Receipt id={0} price={1} purchaseDate={2} generatedDate={3}",
                receipt.getIdentifier(),
                receipt.getPriceInCents(),
                receipt.getPurchaseDate(),
                receipt.getGeneratedDate()));
        }
        InvokeOuyaGetReceiptsOnSuccess(m_receipts);
    }
    public void ReceiptListFailureListener(string jsonData)
    {
        Debug.LogError(string.Format("ReceiptListFailureListener jsonData={0}", jsonData));
        InvokeOuyaGetReceiptsOnFailure(0, jsonData);
    }
    public void ReceiptListCancelListener(string ignore)
    {
        InvokeOuyaGetReceiptsOnCancel();
    }
    
    private void InputListener(OuyaSDK.InputAction inputAction, string jsonData)
    {
        #region Error Handling
        if (string.IsNullOrEmpty(jsonData))
        {
            Debug.LogError("OuyaSDK.InputListener: received invalid jsondata");
            return;
        }
        OuyaGameObject.InputData = jsonData;

        if (string.IsNullOrEmpty(OuyaSDK.getDeveloperId()))
        {
            Debug.LogError("SDK is not initialized");
            return;
        }

        //Debug.Log(string.Format("OuyaSDK.InputListener: inputAction={0} jsonData={1}", inputAction, jsonData));

        if (null == OuyaSDK.getInputAxisListener())
        {
            Debug.LogError("OuyaSDK.InputListener: Input axis listener is not set");
            return;
        }

        if (null == OuyaSDK.getInputAxisListener().onSuccess)
        {
            Debug.LogError("OuyaSDK.InputListener: Input axis listener onSuccess is not set");
            return;
        }

        if (null == OuyaSDK.getInputButtonListener())
        {
            Debug.LogError("OuyaSDK.InputListener: Input button listener is not set");
            return;
        }

        if (null == OuyaSDK.getInputButtonListener().onSuccess)
        {
            Debug.LogError("OuyaSDK.InputListener: Input button listener onSuccess is not set");
            return;
        }
        #endregion

        InputContainer container = JsonMapper.ToObject<InputContainer>(jsonData);
        OuyaSDK.InputAxisEvent inputAxis;
        OuyaSDK.InputButtonEvent inputButton;

        if (null == container)
        {
            return;
        }
        
        Device device = devices.Find(delegate(Device d) { return (null == d || null == container) ? false : (d.id == container.DeviceId); });
        if (null == device)
        {
            return;
        }

        if (!debugOff)
        {
            Debug.Log("Device:" + device.id + " Player" + device.player);
        }

        /// @todo: ADD_CONTROLLER_NAME

        switch (container.DeviceName.ToUpper())
        {
            #region OUYA Game Controller
            case "BLUETOOTH JOYSTICK":
            case "OUYA GAME CONTROLLER":
                switch (inputAction)
                {
                    #region KeyDown
                    case OuyaSDK.InputAction.KeyDown:
                        if (container.KeyEvent.mRepeatCount == 0 || container.KeyEvent.mRepeatCount > 5)
                        {
                            switch (container.KeyEvent.mKeyCode)
                            {
                                case 97:
                                    inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_A, device.player);
                                    OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                                    break;
                                case 96:
                                    inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_O, device.player);
                                    OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                                    break;
                                case 99:
                                    inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_U, device.player);
                                    OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                                    break;
                                case 100:
                                    inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_Y, device.player);
                                    OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                                    break;

                                case 102:
                                    inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_LB, device.player);
                                    OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                                    break;
                                case 104:
                                    inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_LT, device.player);
                                    OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                                    break;
                                case 106:
                                    inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_L3, device.player);
                                    OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                                    break;

                                case 103:
                                    inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_RB, device.player);
                                    OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                                    break;
                                case 105:
                                    inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_RT, device.player);
                                    OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                                    break;
                                case 107:
                                    inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_R3, device.player);
                                    OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                                    break;
                                case 108:
                                    inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_SYSTEM, device.player);
                                    OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                                    break;
                                case 19:
                                    inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_DPAD_UP, device.player);
                                    OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                                    break;
                                case 20:
                                    inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_DPAD_DOWN, device.player);
                                    OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                                    break;
                                case 21:
                                    inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_DPAD_LEFT, device.player);
                                    OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                                    break;
                                case 22:
                                    inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_DPAD_RIGHT, device.player);
                                    OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                                    break;

                                case 82:
                                    inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_SYSTEM, device.player);
                                    OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                                    break;

                                default:
                                    Debug.Log("Unhandled " + inputAction + ": " + container.KeyEvent.mKeyCode);
                                    break;
                            }
                        }
                        break;
                    #endregion

                    #region KeyUp
                    case OuyaSDK.InputAction.KeyUp:
                        switch (container.KeyEvent.mKeyCode)
                        {
                            case 97:
                                inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_A, device.player);
                                OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                                break;
                            case 96:
                                inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_O, device.player);
                                OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                                break;
                            case 99:
                                inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_U, device.player);
                                OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                                break;
                            case 100:
                                inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_Y, device.player);
                                OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                                break;

                            case 102:
                                inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_LB, device.player);
                                OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                                break;
                            case 104:
                                inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_LT, device.player);
                                OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                                break;
                            case 106:
                                inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_L3, device.player);
                                OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                                break;

                            case 103:
                                inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_RB, device.player);
                                OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                                break;
                            case 105:
                                inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_RT, device.player);
                                OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                                break;
                            case 107:
                                inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_R3, device.player);
                                OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                                break;
                            case 108:
                                inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_SYSTEM, device.player);
                                OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                                break;
                            case 19:
                                inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_DPAD_UP, device.player);
                                OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                                break;
                            case 20:
                                inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_DPAD_DOWN, device.player);
                                OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                                break;
                            case 21:
                                inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_DPAD_LEFT, device.player);
                                OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                                break;
                            case 22:
                                inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_DPAD_RIGHT, device.player);
                                OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                                break;

                            case 82:
                                inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_SYSTEM, device.player);
                                OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                                break;

                            default:
                                Debug.Log("Unhandled " + inputAction + ": " + container.KeyEvent.mKeyCode);
                                break;
                        }
                        break;
                    #endregion

                    #region GenericMotionEvent
                    case OuyaSDK.InputAction.GenericMotionEvent:
                        inputAxis = new OuyaSDK.InputAxisEvent(inputAction, OuyaSDK.AxisEnum.AXIS_LSTICK_X, container.AxisX, device.player);
                        OuyaSDK.getInputAxisListener().onSuccess(inputAxis);

                        inputAxis = new OuyaSDK.InputAxisEvent(inputAction, OuyaSDK.AxisEnum.AXIS_LSTICK_Y, container.AxisY, device.player);
                        OuyaSDK.getInputAxisListener().onSuccess(inputAxis);

                        inputAxis = new OuyaSDK.InputAxisEvent(inputAction, OuyaSDK.AxisEnum.AXIS_RSTICK_X, container.AxisZ, device.player);
                        OuyaSDK.getInputAxisListener().onSuccess(inputAxis);

                        inputAxis = new OuyaSDK.InputAxisEvent(inputAction, OuyaSDK.AxisEnum.AXIS_RSTICK_Y, container.AxisRZ, device.player);
                        OuyaSDK.getInputAxisListener().onSuccess(inputAxis);

                        inputAxis = new OuyaSDK.InputAxisEvent(inputAction, OuyaSDK.AxisEnum.AXIS_LTRIGGER, container.AxisLTrigger, device.player);
                        OuyaSDK.getInputAxisListener().onSuccess(inputAxis);

                        inputAxis = new OuyaSDK.InputAxisEvent(inputAction, OuyaSDK.AxisEnum.AXIS_RTRIGGER, container.AxisRTrigger, device.player);
                        OuyaSDK.getInputAxisListener().onSuccess(inputAxis);

                        break;
                    #endregion
                }
                break;
            #endregion

            #region XBOX 360 WIRELESS RECEIVER & Microsoft X-Box 360 pad
            case "XBOX 360 WIRELESS RECEIVER":
            case "CONTROLLER (AFTERGLOW GAMEPAD FOR XBOX 360)":
            case "CONTROLLER (ROCK CANDY GAMEPAD FOR XBOX 360)":
            case "CONTROLLER (XBOX 360 WIRELESS RECEIVER FOR WINDOWS)":
            case "MICROSOFT X-BOX 360 PAD":
            case "CONTROLLER (XBOX 360 FOR WINDOWS)":
            case "CONTROLLER (XBOX360 GAMEPAD)":
            case "XBOX 360 FOR WINDOWS (CONTROLLER)":
                switch (inputAction)
                {
                    #region KeyDown
                    case OuyaSDK.InputAction.KeyDown:
                        if (container.KeyEvent.mRepeatCount == 0 || container.KeyEvent.mRepeatCount > 5)
                        {
                            if (container.KeyEvent.mKeyCode == 97)
                            {
                                inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_A, device.player);
                                OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                            }
                            if (container.KeyEvent.mKeyCode == 96)
                            {
                                inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_O, device.player);
                                OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                            }
                            if (container.KeyEvent.mKeyCode == 99)
                            {
                                inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_U, device.player);
                                OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                            }
                            if (container.KeyEvent.mKeyCode == 100)
                            {
                                inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_Y, device.player);
                                OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                            }
                            if (container.KeyEvent.mKeyCode == 102)
                            {
                                inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_LB, device.player);
                                OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                            }
                            if (container.KeyEvent.mKeyCode == 103)
                            {
                                inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_RB, device.player);
                                OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                            }
                            if (container.KeyEvent.mKeyCode == 106)
                            {
                                inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_L3, device.player);
                                OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                            }
                            if (container.KeyEvent.mKeyCode == 107)
                            {
                                inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_R3, device.player);
                                OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                            }
                        }
                        break;
                    #endregion
                    
                    #region KeyUp
                    case OuyaSDK.InputAction.KeyUp:
                        if (container.KeyEvent.mKeyCode == 97)
                        {
                            inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_A, device.player);
                            OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                        }
                        if (container.KeyEvent.mKeyCode == 96)
                        {
                            inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_O, device.player);
                            OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                        }
                        if (container.KeyEvent.mKeyCode == 99)
                        {
                            inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_U, device.player);
                            OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                        }
                        if (container.KeyEvent.mKeyCode == 100)
                        {
                            inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_Y, device.player);
                            OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                        }
                        if (container.KeyEvent.mKeyCode == 102)
                        {
                            inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_LB, device.player);
                            OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                        }
                        if (container.KeyEvent.mKeyCode == 103)
                        {
                            inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_RB, device.player);
                            OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                        }
                        if (container.KeyEvent.mKeyCode == 106)
                        {
                            inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_L3, device.player);
                            OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                        }
                        if (container.KeyEvent.mKeyCode == 107)
                        {
                            inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_R3, device.player);
                            OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                        }
                        break;
                    #endregion

                    #region GenericMotionEvent
                    case OuyaSDK.InputAction.GenericMotionEvent:
                        if (container.AxisHatY == -1)
                        {
                            inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_DPAD_UP, device.player);
                            OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                        }

                        if (container.AxisHatY == 1)
                        {
                            inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_DPAD_DOWN, device.player);
                            OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                        }

                        if (container.AxisHatX == -1)
                        {
                            inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_DPAD_LEFT, device.player);
                            OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                        }

                        if (container.AxisHatX == 1)
                        {
                            inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_DPAD_RIGHT, device.player);
                            OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                        }

                        if (container.AxisHatX == 0 &&
                            container.AxisHatY == 0)
                        {
                            inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_DPAD_CENTER, device.player);
                            OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                        }

                        inputAxis = new OuyaSDK.InputAxisEvent(inputAction, OuyaSDK.AxisEnum.AXIS_LSTICK_X, container.AxisX, device.player);
                        OuyaSDK.getInputAxisListener().onSuccess(inputAxis);

                        inputAxis = new OuyaSDK.InputAxisEvent(inputAction, OuyaSDK.AxisEnum.AXIS_LSTICK_Y, container.AxisY, device.player);
                        OuyaSDK.getInputAxisListener().onSuccess(inputAxis);

                        inputAxis = new OuyaSDK.InputAxisEvent(inputAction, OuyaSDK.AxisEnum.AXIS_RSTICK_X, container.AxisZ, device.player);
                        OuyaSDK.getInputAxisListener().onSuccess(inputAxis);

                        inputAxis = new OuyaSDK.InputAxisEvent(inputAction, OuyaSDK.AxisEnum.AXIS_RSTICK_Y, container.AxisRZ, device.player);
                        OuyaSDK.getInputAxisListener().onSuccess(inputAxis);

                        inputAxis = new OuyaSDK.InputAxisEvent(inputAction, OuyaSDK.AxisEnum.AXIS_LTRIGGER, container.AxisLTrigger, device.player);
                        OuyaSDK.getInputAxisListener().onSuccess(inputAxis);

                        inputAxis = new OuyaSDK.InputAxisEvent(inputAction, OuyaSDK.AxisEnum.AXIS_RTRIGGER, container.AxisRTrigger, device.player);
                        OuyaSDK.getInputAxisListener().onSuccess(inputAxis);

                        /*
                        if (container.AxisLTrigger > 0.1f)
                        {
                            inputButton = new OuyaSDK.InputButtonEvent(OuyaSDK.InputAction.KeyDown, OuyaSDK.KeyEnum.BUTTON_LT, device.player);
                            OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                            m_detectLTriggerDown = true;
                        }
                        else if (m_detectLTriggerDown)
                        {
                            m_detectLTriggerDown = false;
                            inputButton = new OuyaSDK.InputButtonEvent(OuyaSDK.InputAction.KeyUp, OuyaSDK.KeyEnum.BUTTON_LT, device.player);
                            OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                        }

                        if (container.AxisRTrigger > 0.1f)
                        {
                            inputButton = new OuyaSDK.InputButtonEvent(OuyaSDK.InputAction.KeyDown, OuyaSDK.KeyEnum.BUTTON_RT, device.player);
                            OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                            m_detectRTriggerDown = true;
                        }
                        else if (m_detectRTriggerDown)
                        {
                            m_detectRTriggerDown = false;
                            inputButton = new OuyaSDK.InputButtonEvent(OuyaSDK.InputAction.KeyUp, OuyaSDK.KeyEnum.BUTTON_RT, device.player);
                            OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                        }
                         */ 

                        break;
                    #endregion
                }
                break;
            #endregion

            #region idroid:con
            case "IDROID:CON":
                switch (inputAction)
                {
                    #region KeyDown
                    case OuyaSDK.InputAction.KeyDown:
                        if (container.KeyEvent.mRepeatCount == 0 || container.KeyEvent.mRepeatCount > 5)
                        {
                            if (container.KeyEvent.mKeyCode == 189)
                            {
                                inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_A, device.player);
                                OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                            }
                            if (container.KeyEvent.mKeyCode == 188)
                            {
                                inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_O, device.player);
                                OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                            }
                            if (container.KeyEvent.mKeyCode == 190)
                            {
                                inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_U, device.player);
                                OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                            }
                            if (container.KeyEvent.mKeyCode == 191)
                            {
                                inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_Y, device.player);
                                OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                            }
                            if (container.KeyEvent.mKeyCode == 192)
                            {
                                inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_LB, device.player);
                                OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                            }
                            if (container.KeyEvent.mKeyCode == 193)
                            {
                                inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_RB, device.player);
                                OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                            }
                        }
                        break;
                    #endregion

                    #region KeyUp
                    case OuyaSDK.InputAction.KeyUp:
                        if (container.KeyEvent.mKeyCode == 189)
                        {
                            inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_A, device.player);
                            OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                        }
                        if (container.KeyEvent.mKeyCode == 188)
                        {
                            inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_O, device.player);
                            OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                        }
                        if (container.KeyEvent.mKeyCode == 190)
                        {
                            inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_U, device.player);
                            OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                        }
                        if (container.KeyEvent.mKeyCode == 191)
                        {
                            inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_Y, device.player);
                            OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                        }
                        if (container.KeyEvent.mKeyCode == 192)
                        {
                            inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_LB, device.player);
                            OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                        }
                        if (container.KeyEvent.mKeyCode == 193)
                        {
                            inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_RB, device.player);
                            OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                        }
                        break;
                    #endregion

                    #region GenericMotionEvent
                    case OuyaSDK.InputAction.GenericMotionEvent:
                        if (container.AxisHatY == -1)
                        {
                            inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_DPAD_UP, device.player);
                            OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                        }

                        if (container.AxisHatY == 1)
                        {
                            inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_DPAD_DOWN, device.player);
                            OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                        }

                        if (container.AxisHatX == -1)
                        {
                            inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_DPAD_LEFT, device.player);
                            OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                        }

                        if (container.AxisHatX == 1)
                        {
                            inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_DPAD_RIGHT, device.player);
                            OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                        }

                        if (container.AxisHatX == 0 &&
                            container.AxisHatY == 0)
                        {
                            inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_DPAD_CENTER, device.player);
                            OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                        }

                        inputAxis = new OuyaSDK.InputAxisEvent(inputAction, OuyaSDK.AxisEnum.AXIS_LSTICK_X, container.AxisX, device.player);
                        OuyaSDK.getInputAxisListener().onSuccess(inputAxis);

                        inputAxis = new OuyaSDK.InputAxisEvent(inputAction, OuyaSDK.AxisEnum.AXIS_LSTICK_Y, container.AxisY, device.player);
                        OuyaSDK.getInputAxisListener().onSuccess(inputAxis);

                        inputAxis = new OuyaSDK.InputAxisEvent(inputAction, OuyaSDK.AxisEnum.AXIS_RSTICK_X, container.AxisRX, device.player);
                        OuyaSDK.getInputAxisListener().onSuccess(inputAxis);

                        inputAxis = new OuyaSDK.InputAxisEvent(inputAction, OuyaSDK.AxisEnum.AXIS_RSTICK_Y, container.AxisRY, device.player);
                        OuyaSDK.getInputAxisListener().onSuccess(inputAxis);

                        inputAxis = new OuyaSDK.InputAxisEvent(inputAction, OuyaSDK.AxisEnum.AXIS_LTRIGGER, container.AxisZ, device.player);
                        OuyaSDK.getInputAxisListener().onSuccess(inputAxis);

                        inputAxis = new OuyaSDK.InputAxisEvent(inputAction, OuyaSDK.AxisEnum.AXIS_RTRIGGER, container.AxisZ, device.player);
                        OuyaSDK.getInputAxisListener().onSuccess(inputAxis);
                        /*
                        if (container.AxisZ > 0.1f)
                        {
                            inputButton = new OuyaSDK.InputButtonEvent(OuyaSDK.InputAction.KeyDown, OuyaSDK.KeyEnum.BUTTON_LT, device.player);
                            OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                            m_detectLTriggerDown = true;
                        }
                        else if (m_detectLTriggerDown)
                        {
                            m_detectLTriggerDown = false;
                            inputButton = new OuyaSDK.InputButtonEvent(OuyaSDK.InputAction.KeyUp, OuyaSDK.KeyEnum.BUTTON_LT, device.player);
                            OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                        }

                        if (container.AxisZ < -0.1f)
                        {
                            inputButton = new OuyaSDK.InputButtonEvent(OuyaSDK.InputAction.KeyDown, OuyaSDK.KeyEnum.BUTTON_RT, device.player);
                            OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                            m_detectRTriggerDown = true;
                        }
                        else if (m_detectRTriggerDown)
                        {
                            m_detectRTriggerDown = false;
                            inputButton = new OuyaSDK.InputButtonEvent(OuyaSDK.InputAction.KeyUp, OuyaSDK.KeyEnum.BUTTON_RT, device.player);
                            OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                        }
                         */

                        break;
                    #endregion
                }
                break;
            #endregion

            #region PS3 MotionInJoy Game Controller ( Driver required )
            case "MOTIONINJOY VIRTUAL GAME CONTROLLER":
                switch (inputAction)
                {
                    #region KeyDown
                    case OuyaSDK.InputAction.KeyDown:
                        if (container.KeyEvent.mRepeatCount == 0 || container.KeyEvent.mRepeatCount > 5)
                        {
                            switch (container.KeyEvent.mKeyCode)
                            {
                                case 97:
                                    inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_A, device.player);
                                    OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                                    break;
                                case 96:
                                    inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_O, device.player);
                                    OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                                    break;
                                case 99:
                                    inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_U, device.player);
                                    OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                                    break;
                                case 100:
                                    inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_Y, device.player);
                                    OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                                    break;

                                case 102:
                                    inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_LB, device.player);
                                    OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                                    break;
                                case 104:
                                    inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_LT, device.player);
                                    OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                                    break;
                                case 106:
                                    inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_L3, device.player);
                                    OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                                    break;

                                case 103:
                                    inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_RB, device.player);
                                    OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                                    break;
                                case 105:
                                    inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_RT, device.player);
                                    OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                                    break;
                                case 107:
                                    inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_R3, device.player);
                                    OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                                    break;

                                case 108:
                                    inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_SYSTEM, device.player);
                                    OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                                    break;

                                case 19:
                                    inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_DPAD_UP, device.player);
                                    OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                                    break;
                                case 20:
                                    inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_DPAD_DOWN, device.player);
                                    OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                                    break;
                                case 21:
                                    inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_DPAD_LEFT, device.player);
                                    OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                                    break;
                                case 22:
                                    inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_DPAD_RIGHT, device.player);
                                    OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                                    break;

                                default:
                                    Debug.Log("Unhandled " + inputAction + ": " + container.KeyEvent.mKeyCode);
                                    break;
                            }
                        }
                        break;
                    #endregion

                    #region KeyUp
                    case OuyaSDK.InputAction.KeyUp:
                        switch (container.KeyEvent.mKeyCode)
                        {
                            case 97:
                                inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_A, device.player);
                                OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                                break;
                            case 96:
                                inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_O, device.player);
                                OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                                break;
                            case 99:
                                inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_U, device.player);
                                OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                                break;
                            case 100:
                                inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_Y, device.player);
                                OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                                break;

                            case 102:
                                inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_LB, device.player);
                                OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                                break;
                            case 104:
                                inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_LT, device.player);
                                OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                                break;
                            case 106:
                                inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_L3, device.player);
                                OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                                break;

                            case 103:
                                inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_RB, device.player);
                                OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                                break;
                            case 105:
                                inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_RT, device.player);
                                OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                                break;
                            case 107:
                                inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_R3, device.player);
                                OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                                break;

                            case 108:
                                inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_SYSTEM, device.player);
                                OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                                break;

                            case 19:
                                inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_DPAD_UP, device.player);
                                OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                                break;
                            case 20:
                                inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_DPAD_DOWN, device.player);
                                OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                                break;
                            case 21:
                                inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_DPAD_LEFT, device.player);
                                OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                                break;
                            case 22:
                                inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_DPAD_RIGHT, device.player);
                                OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                                break;

                            default:
                                Debug.Log("Unhandled " + inputAction + ": " + container.KeyEvent.mKeyCode);
                                break;
                        }
                        break;
                    #endregion

                    #region GenericMotionEvent
                    case OuyaSDK.InputAction.GenericMotionEvent:

                        inputAxis = new OuyaSDK.InputAxisEvent(inputAction, OuyaSDK.AxisEnum.AXIS_LSTICK_X, container.AxisX, device.player);
                        OuyaSDK.getInputAxisListener().onSuccess(inputAxis);

                        inputAxis = new OuyaSDK.InputAxisEvent(inputAction, OuyaSDK.AxisEnum.AXIS_LSTICK_Y, container.AxisY, device.player);
                        OuyaSDK.getInputAxisListener().onSuccess(inputAxis);

                        inputAxis = new OuyaSDK.InputAxisEvent(inputAction, OuyaSDK.AxisEnum.AXIS_RSTICK_X, container.AxisZ, device.player);
                        OuyaSDK.getInputAxisListener().onSuccess(inputAxis);

                        inputAxis = new OuyaSDK.InputAxisEvent(inputAction, OuyaSDK.AxisEnum.AXIS_RSTICK_Y, container.AxisRZ, device.player);
                        OuyaSDK.getInputAxisListener().onSuccess(inputAxis);

                        //inputAxis = new OuyaSDK.InputAxisEvent(inputAction, OuyaSDK.AxisEnum.AXIS_LTRIGGER,container.AxisLTrigger, device.player);
                        //OuyaSDK.getInputAxisListener().onSuccess(inputAxis);

                        //inputAxis = new OuyaSDK.InputAxisEvent(inputAction, OuyaSDK.AxisEnum.AXIS_RTRIGGER,container.AxisRTrigger, device.player);
                        //OuyaSDK.getInputAxisListener().onSuccess(inputAxis);
                        /*
                        if (container.AxisLTrigger > 0.13f)
                        {
                            inputButton = new OuyaSDK.InputButtonEvent(OuyaSDK.InputAction.KeyDown, OuyaSDK.KeyEnum.BUTTON_LT, device.player);
                            OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                            m_detectLTriggerDown = true;
                        }
                        else //if (m_detectLTriggerDown)
                        {
                            m_detectLTriggerDown = false;
                            inputButton = new OuyaSDK.InputButtonEvent(OuyaSDK.InputAction.KeyUp, OuyaSDK.KeyEnum.BUTTON_LT, device.player);
                            container.AxisLTrigger = 0; //override for deadzone
                            OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                        }

                        if (container.AxisRTrigger > 0.13f)
                        {
                            inputButton = new OuyaSDK.InputButtonEvent(OuyaSDK.InputAction.KeyDown, OuyaSDK.KeyEnum.BUTTON_RT, device.player);
                            OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                            m_detectRTriggerDown = true;
                        }
                        else //if (m_detectRTriggerDown)
                        {
                            m_detectRTriggerDown = false;
                            inputButton = new OuyaSDK.InputButtonEvent(OuyaSDK.InputAction.KeyUp, OuyaSDK.KeyEnum.BUTTON_RT, device.player);
                            container.AxisRTrigger = 0; //override for deadzone
                            OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                        }*/

                        break;
                    #endregion
                }
                break;
            #endregion

            #region SONY PLAYSTATION(R)3 CONTROLLER

            case "SONY PLAYSTATION(R)3 CONTROLLER":

                switch (inputAction)
                {

                    #region KeyDown

                    case OuyaSDK.InputAction.KeyDown:

                        if (container.KeyEvent.mRepeatCount == 0 || container.KeyEvent.mRepeatCount > 5)
                        {

                            switch (container.KeyEvent.mKeyCode)
                            {

                                case 21:

                                    inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_A, device.player);

                                    OuyaSDK.getInputButtonListener().onSuccess(inputButton);

                                    break;

                                case 22:

                                    inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_O, device.player);

                                    OuyaSDK.getInputButtonListener().onSuccess(inputButton);

                                    break;

                                case 19:

                                    inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_U, device.player);

                                    OuyaSDK.getInputButtonListener().onSuccess(inputButton);

                                    break;

                                case 108:

                                    inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_Y, device.player);

                                    OuyaSDK.getInputButtonListener().onSuccess(inputButton);

                                    break;


                                case 107:

                                    inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_LB, device.player);

                                    OuyaSDK.getInputButtonListener().onSuccess(inputButton);

                                    break;

                                case -1:

                                    inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_LT, device.player);

                                    OuyaSDK.getInputButtonListener().onSuccess(inputButton);

                                    break;


                                case -2:

                                    inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_RB, device.player);

                                    OuyaSDK.getInputButtonListener().onSuccess(inputButton);

                                    break;

                                case 106:

                                    inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_RT, device.player);

                                    OuyaSDK.getInputButtonListener().onSuccess(inputButton);

                                    break;


                                case 20:

                                    inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_SYSTEM, device.player);

                                    OuyaSDK.getInputButtonListener().onSuccess(inputButton);

                                    break;


                                case 102:

                                    inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_DPAD_UP, device.player);

                                    OuyaSDK.getInputButtonListener().onSuccess(inputButton);

                                    break;

                                case 104:

                                    inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_DPAD_DOWN, device.player);

                                    OuyaSDK.getInputButtonListener().onSuccess(inputButton);

                                    break;

                                case 105:

                                    inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_DPAD_LEFT, device.player);

                                    OuyaSDK.getInputButtonListener().onSuccess(inputButton);

                                    break;

                                case 103:

                                    inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_DPAD_RIGHT, device.player);

                                    OuyaSDK.getInputButtonListener().onSuccess(inputButton);

                                    break;


                                default:

                                    Debug.Log("Unhandled " + inputAction + ": " + container.KeyEvent.mKeyCode);

                                    break;

                            }

                        }

                        break;

                    #endregion


                    #region KeyUp

                    case OuyaSDK.InputAction.KeyUp:

                        switch (container.KeyEvent.mKeyCode)
                        {

                            case 21:

                                inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_A, device.player);

                                OuyaSDK.getInputButtonListener().onSuccess(inputButton);

                                break;

                            case 22:

                                inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_O, device.player);

                                OuyaSDK.getInputButtonListener().onSuccess(inputButton);

                                break;

                            case 19:

                                inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_U, device.player);

                                OuyaSDK.getInputButtonListener().onSuccess(inputButton);

                                break;

                            case 108:

                                inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_Y, device.player);

                                OuyaSDK.getInputButtonListener().onSuccess(inputButton);

                                break;

                            case 107:

                                inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_LB, device.player);

                                OuyaSDK.getInputButtonListener().onSuccess(inputButton);

                                break;

                            case -1:

                                inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_LT, device.player);

                                OuyaSDK.getInputButtonListener().onSuccess(inputButton);

                                break;

                            case -2:

                                inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_RB, device.player);

                                OuyaSDK.getInputButtonListener().onSuccess(inputButton);

                                break;

                            case 106:

                                inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_RT, device.player);

                                OuyaSDK.getInputButtonListener().onSuccess(inputButton);

                                break;

                            case 20:

                                inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_SYSTEM, device.player);

                                OuyaSDK.getInputButtonListener().onSuccess(inputButton);

                                break;

                            case 102:

                                inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_DPAD_UP, device.player);

                                OuyaSDK.getInputButtonListener().onSuccess(inputButton);

                                break;

                            case 104:

                                inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_DPAD_DOWN, device.player);

                                OuyaSDK.getInputButtonListener().onSuccess(inputButton);

                                break;

                            case 105:

                                inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_DPAD_LEFT, device.player);

                                OuyaSDK.getInputButtonListener().onSuccess(inputButton);

                                break;

                            case 103:

                                inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_DPAD_RIGHT, device.player);

                                OuyaSDK.getInputButtonListener().onSuccess(inputButton);

                                break;


                            default:

                                Debug.Log("Unhandled " + inputAction + ": " + container.KeyEvent.mKeyCode);

                                break;

                        }

                        break;

                    #endregion


                    #region GenericMotionEvent

                    case OuyaSDK.InputAction.GenericMotionEvent:


                        inputAxis = new OuyaSDK.InputAxisEvent(inputAction, OuyaSDK.AxisEnum.AXIS_LSTICK_X, container.AxisX, device.player);

                        OuyaSDK.getInputAxisListener().onSuccess(inputAxis);


                        inputAxis = new OuyaSDK.InputAxisEvent(inputAction, OuyaSDK.AxisEnum.AXIS_LSTICK_Y, container.AxisY, device.player);

                        OuyaSDK.getInputAxisListener().onSuccess(inputAxis);


                        inputAxis = new OuyaSDK.InputAxisEvent(inputAction, OuyaSDK.AxisEnum.AXIS_RSTICK_X, container.AxisZ, device.player);

                        OuyaSDK.getInputAxisListener().onSuccess(inputAxis);


                        inputAxis = new OuyaSDK.InputAxisEvent(inputAction, OuyaSDK.AxisEnum.AXIS_RSTICK_Y, container.AxisRZ, device.player);

                        OuyaSDK.getInputAxisListener().onSuccess(inputAxis);

                        break;

                    #endregion

                }

                break;

            #endregion

            #region USB Controller
            case "USB CONTROLLER":
            default:
                switch (inputAction)
                {
                    #region KeyDown
                    case OuyaSDK.InputAction.KeyDown:
                        if (container.KeyEvent.mRepeatCount == 0 || container.KeyEvent.mRepeatCount > 5)
                        {
                            if (container.KeyEvent.mKeyCode == 97)
                            {
                                inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_A, device.player);
                                OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                            }
                            if (container.KeyEvent.mKeyCode == 98)
                            {
                                inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_O, device.player);
                                OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                            }
                            if (container.KeyEvent.mKeyCode == 99)
                            {
                                inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_U, device.player);
                                OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                            }
                            if (container.KeyEvent.mKeyCode == 96)
                            {
                                inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_Y, device.player);
                                OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                            }
                            if (container.KeyEvent.mKeyCode == 100)
                            {
                                inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_LB, device.player);
                                OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                            }
                            if (container.KeyEvent.mKeyCode == 101)
                            {
                                inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_RB, device.player);
                                OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                            }
                            if (container.KeyEvent.mKeyCode == 102)
                            {
                                inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_LT, device.player);
                                OuyaSDK.getInputButtonListener().onSuccess(inputButton);

                                switch (inputAction)
                                {
                                    case OuyaSDK.InputAction.KeyDown:
                                        inputAxis = new OuyaSDK.InputAxisEvent(inputAction, OuyaSDK.AxisEnum.AXIS_LTRIGGER, 1, device.player);
                                        OuyaSDK.getInputAxisListener().onSuccess(inputAxis);
                                        break;
                                    case OuyaSDK.InputAction.KeyUp:
                                        inputAxis = new OuyaSDK.InputAxisEvent(inputAction, OuyaSDK.AxisEnum.AXIS_LTRIGGER, 0, device.player);
                                        OuyaSDK.getInputAxisListener().onSuccess(inputAxis);
                                        break;
                                }
                            }
                            if (container.KeyEvent.mKeyCode == 103)
                            {
                                inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_RT, device.player);
                                OuyaSDK.getInputButtonListener().onSuccess(inputButton);

                                switch (inputAction)
                                {
                                    case OuyaSDK.InputAction.KeyDown:
                                        inputAxis = new OuyaSDK.InputAxisEvent(inputAction, OuyaSDK.AxisEnum.AXIS_RTRIGGER, -1, device.player);
                                        OuyaSDK.getInputAxisListener().onSuccess(inputAxis);
                                        break;
                                    case OuyaSDK.InputAction.KeyUp:
                                        inputAxis = new OuyaSDK.InputAxisEvent(inputAction, OuyaSDK.AxisEnum.AXIS_RTRIGGER, 0, device.player);
                                        OuyaSDK.getInputAxisListener().onSuccess(inputAxis);
                                        break;
                                }
                            }
                        }
                        break;
                    #endregion

                    #region KeyUp
                    case OuyaSDK.InputAction.KeyUp:
                        if (container.KeyEvent.mKeyCode == 97)
                        {
                            inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_A, device.player);
                            OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                        }
                        if (container.KeyEvent.mKeyCode == 98)
                        {
                            inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_O, device.player);
                            OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                        }
                        if (container.KeyEvent.mKeyCode == 99)
                        {
                            inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_U, device.player);
                            OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                        }
                        if (container.KeyEvent.mKeyCode == 96)
                        {
                            inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_Y, device.player);
                            OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                        }
                        if (container.KeyEvent.mKeyCode == 100)
                        {
                            inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_LB, device.player);
                            OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                        }
                        if (container.KeyEvent.mKeyCode == 101)
                        {
                            inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_RB, device.player);
                            OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                        }
                        if (container.KeyEvent.mKeyCode == 102)
                        {
                            inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_LT, device.player);
                            OuyaSDK.getInputButtonListener().onSuccess(inputButton);

                            switch (inputAction)
                            {
                                case OuyaSDK.InputAction.KeyDown:
                                    inputAxis = new OuyaSDK.InputAxisEvent(inputAction, OuyaSDK.AxisEnum.AXIS_LTRIGGER, 1, device.player);
                                    OuyaSDK.getInputAxisListener().onSuccess(inputAxis);
                                    break;
                                case OuyaSDK.InputAction.KeyUp:
                                    inputAxis = new OuyaSDK.InputAxisEvent(inputAction, OuyaSDK.AxisEnum.AXIS_LTRIGGER, 0, device.player);
                                    OuyaSDK.getInputAxisListener().onSuccess(inputAxis);
                                    break;
                            }
                        }
                        if (container.KeyEvent.mKeyCode == 103)
                        {
                            inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_RT, device.player);
                            OuyaSDK.getInputButtonListener().onSuccess(inputButton);

                            switch (inputAction)
                            {
                                case OuyaSDK.InputAction.KeyDown:
                                    inputAxis = new OuyaSDK.InputAxisEvent(inputAction, OuyaSDK.AxisEnum.AXIS_RTRIGGER, -1, device.player);
                                    OuyaSDK.getInputAxisListener().onSuccess(inputAxis);
                                    break;
                                case OuyaSDK.InputAction.KeyUp:
                                    inputAxis = new OuyaSDK.InputAxisEvent(inputAction, OuyaSDK.AxisEnum.AXIS_RTRIGGER, 0, device.player);
                                    OuyaSDK.getInputAxisListener().onSuccess(inputAxis);
                                    break;
                            }
                        }
                        break;
                    #endregion

                    #region GenericMotionEvent
                    case OuyaSDK.InputAction.GenericMotionEvent:
                        if (container.AxisHatY == -1)
                        {
                            inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_DPAD_UP, device.player);
                            OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                        }

                        if (container.AxisHatY == 1)
                        {
                            inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_DPAD_DOWN, device.player);
                            OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                        }

                        if (container.AxisHatX == -1)
                        {
                            inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_DPAD_LEFT, device.player);
                            OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                        }

                        if (container.AxisHatX == 1)
                        {
                            inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_DPAD_RIGHT, device.player);
                            OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                        }

                        if (container.AxisHatX == 0 &&
                            container.AxisHatY == 0)
                        {
                            inputButton = new OuyaSDK.InputButtonEvent(inputAction, OuyaSDK.KeyEnum.BUTTON_DPAD_CENTER, device.player);
                            OuyaSDK.getInputButtonListener().onSuccess(inputButton);
                        }

                        inputAxis = new OuyaSDK.InputAxisEvent(inputAction, OuyaSDK.AxisEnum.AXIS_LSTICK_X, container.AxisX, device.player);
                        OuyaSDK.getInputAxisListener().onSuccess(inputAxis);

                        inputAxis = new OuyaSDK.InputAxisEvent(inputAction, OuyaSDK.AxisEnum.AXIS_LSTICK_Y, container.AxisY, device.player);
                        OuyaSDK.getInputAxisListener().onSuccess(inputAxis);

                        inputAxis = new OuyaSDK.InputAxisEvent(inputAction, OuyaSDK.AxisEnum.AXIS_RSTICK_X, container.AxisZ, device.player);
                        OuyaSDK.getInputAxisListener().onSuccess(inputAxis);

                        inputAxis = new OuyaSDK.InputAxisEvent(inputAction, OuyaSDK.AxisEnum.AXIS_RSTICK_Y, container.AxisRZ, device.player);
                        OuyaSDK.getInputAxisListener().onSuccess(inputAxis);

                        inputAxis = new OuyaSDK.InputAxisEvent(inputAction, OuyaSDK.AxisEnum.AXIS_LTRIGGER, container.AxisZ, device.player);
                        OuyaSDK.getInputAxisListener().onSuccess(inputAxis);

                        inputAxis = new OuyaSDK.InputAxisEvent(inputAction, OuyaSDK.AxisEnum.AXIS_RTRIGGER, container.AxisZ, device.player);
                        OuyaSDK.getInputAxisListener().onSuccess(inputAxis);

                        break;
                    #endregion
                }
                break;
            #endregion

        }
    }
    #endregion

    #region Data Classes
    public class InputContainer
    {
        public string Method = string.Empty;
        public int KeyCode = 0;
        public KeyEvent KeyEvent = null;
        public MotionEvent MotionEvent = null;

        public int Action = 0;
        public int ActionCode = 0;
        public int ActionIndex = 0;
        public int ActionMasked = 0;
        public int ButtonState = 0;

        public float AxisBrake = 0;
        public float AxisDistance = 0;
        public float AxisGas = 0;
        public float AxisGeneric1 = 0;
        public float AxisGeneric2 = 0;
        public float AxisGeneric3 = 0;
        public float AxisGeneric4 = 0;
        public float AxisGeneric5 = 0;
        public float AxisGeneric6 = 0;
        public float AxisGeneric7 = 0;
        public float AxisGeneric8 = 0;
        public float AxisGeneric9 = 0;
        public float AxisGeneric10 = 0;
        public float AxisGeneric11 = 0;
        public float AxisGeneric12 = 0;
        public float AxisGeneric13 = 0;
        public float AxisGeneric14 = 0;
        public float AxisGeneric15 = 0;
        public float AxisGeneric16 = 0;
        public float AxisHatX = 0;
        public float AxisHatY = 0;
        public float AxisHScroll = 0;
        public float AxisLTrigger = 0;
        public float AxisOrientation = 0;
        public float AxisPressire = 0;
        public float AxisRTrigger = 0;
        public float AxisRudder = 0;
        public float AxisRX = 0;
        public float AxisRY = 0;
        public float AxisRZ = 0;
        public float AxisSize = 0;
        public float AxisThrottle = 0;
        public float AxisTilt = 0;
        public float AxisToolMajor = 0;
        public float AxisToolMinor = 0;
        public float AxisVScroll = 0;
        public float AxisWheel = 0;
        public float AxisX = 0;
        public float AxisY = 0;
        public float AxisZ = 0;
        public int DeviceId = 0;
        public string DeviceName = string.Empty;
        public int EdgeFlags = 0;
        public int Flags = 0;
        public int MetaState = 0;
        public int PointerCount = 0;
        public float Pressure = 0;
        public float X = 0;
        public float Y = 0;
    }

    public class KeyEvent
    {
        public int mEventTime;
        public int mDownTime;
        public int mDeviceId;
        public int mFlags;
        public int mKeyCode;
        public int mMetaState;
        public int mAction;
        public bool mRecycled;
        public int mRepeatCount;
        public int mScanCode;
        public int mSource;
        public int mSeq;
    }

    public class MotionEvent
    {
        public bool mRecycled;
        public int mNativePtr;
        public int mSeq;
    }

    public class Device
    {
        public int id = 0;
        public OuyaSDK.OuyaPlayer player = OuyaSDK.OuyaPlayer.none;
        public int playerNum = 0;
        public string name = string.Empty;
    }
    #endregion

    public const string DEVICE_ANDROID = "Android";
    public const string DEVICE_OUYA = "Ouya";

    public string GetDeviceType()
    {
        string deviceType = string.Empty;
        if (SystemInfo.deviceType == DeviceType.Handheld && SystemInfo.deviceModel.Contains("NVIDIA Cardhu"))
        {
            deviceType = DEVICE_OUYA;
        }
        else if (SystemInfo.deviceType == DeviceType.Handheld && SystemInfo.operatingSystem.ToUpper().Contains("ANDROID"))
        {
            deviceType = DEVICE_ANDROID;
        }
        else
        {
            deviceType = SystemInfo.deviceType.ToString();
        }
        return deviceType;
    }

    #region UNITY Awake, Start & Update
    void Awake()
    {
        m_instance = this;
    }
    void Start()
    {
        string deviceType = GetDeviceType();
        PlayerPrefs.SetString("device_type", deviceType);

        Input.ResetInputAxes();
        Application.targetFrameRate = 60;
        DontDestroyOnLoad(transform.gameObject);
        #region Init  Devices if were not on Android
#if UNITY_EDITOR || UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX
        //Get Devices attached to computer:
        devices = new List<OuyaGameObject.Device>();
        int deviceCount = 0;
        int controllerCounter = 1;
        foreach (string joystick in Input.GetJoystickNames())
        {
            OuyaGameObject.Device device = new OuyaGameObject.Device();
            device.id = deviceCount;
            device.player = (OuyaSDK.OuyaPlayer)controllerCounter;
            device.name = joystick;
            devices.Add(device);
            deviceCount++;
            controllerCounter++;
        }
#endif
        #endregion

        #region Init OUYA And Handlers
        try
        {
            //First Initalize Ouya SDK;
            #region Initialize OUYA

            //Initialize OuyaSDK with your developer ID
            //Get your developer_id from the ouya developer portal @ http://developer.ouya.tv
            OuyaSDK.initialize(DEVELOPER_ID, UseLegacyInput);

            #endregion
        }
        catch (System.Exception ex)
        {
            Debug.LogError(string.Format("Failed to initialize OuyaSDK exception={0}", ex));
        }
        try
        {
            #region Register button listener

            //Register the for Button Input handling through the static class 
            OuyaSDK.registerInputButtonListener(new OuyaSDK.InputButtonListener<OuyaSDK.InputButtonEvent>()
            {
                onSuccess = (OuyaSDK.InputButtonEvent inputEvent) =>
                {
                    //Assign our handler in the static class.
                    OuyaInputManager.HandleButtonEvent(inputEvent);
                },

                onFailure = (int errorCode, string errorMessage) =>
                {
                    // Your app probably wants to do something more sophisticated than popping a Toast. This is
                    // here to tell you that your app needs to handle this case: if your app doesn't display
                    // something, the user won't know of the failure.
                    //Debug.Log(string.Format("Could not fetch input (error {0}: {1})", errorCode, errorMessage));
                }
            });

            #endregion
        }
        catch (System.Exception ex)
        {
            Debug.LogError(string.Format("Failed to register button listener exception={0}", ex));
        }
        try
        {
            #region Register axis events

            //Register the for Axis Input handling through the static class
            OuyaSDK.registerInputAxisListener(new OuyaSDK.InputAxisListener<OuyaSDK.InputAxisEvent>()
            {
                onSuccess = (OuyaSDK.InputAxisEvent inputEvent) =>
                {
                    //Assign our handler in the static class.
                    OuyaInputManager.HandleAxisEvent(inputEvent);
                },

                onFailure = (int errorCode, string errorMessage) =>
                {
                    // Your app probably wants to do something more sophisticated than popping a Toast. This is
                    // here to tell you that your app needs to handle this case: if your app doesn't display
                    // something, the user won't know of the failure.
                    //Debug.Log(string.Format("Could not fetch input (error {0}: {1})", errorCode, errorMessage));
                }
            });

            #endregion
        }
        catch (System.Exception ex)
        {
            Debug.LogError(string.Format("Failed to register axis events exception={0}", ex));
        }
        #endregion
    }


    void Update()
    {
        if (!UseLegacyInput)
        {
            return;
        }

#if UNITY_EDITOR || UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX
        //Monitor for player input:

        if (!debugOff)
        {
            foreach (Device d in devices)
            {
                Debug.Log(string.Format("player={0} name={1}", d.player, d.name));
            }
        }

        for (int iPlayerJoystick = 1; iPlayerJoystick <= devices.Count; iPlayerJoystick++)
        {
            Device device = devices.Find(delegate(Device d) { return (null == d || null == devices) ? false : (d.id == devices[iPlayerJoystick-1].id); });
            for (int i = 0; i < 20; i++)
            {
                //Controller Name:
                string fireBtnName = string.Format("Joystick{0}Button{1}", iPlayerJoystick,i);
                OuyaKeyCode keycode = (OuyaKeyCode)System.Enum.Parse(typeof(OuyaKeyCode), fireBtnName); //missing more than 4 joystick buttons

                //Controller Action KeyDown
                if (Input.GetKeyDown((KeyCode)(int)keycode))
                {
                    //@watch for new keycodes and new controller mappings
                    //Debug.Log((int)keycode);

                    string jsonData = buildInputPackage((KeyCode)(int)keycode, device);
                    InputListener(OuyaSDK.InputAction.KeyDown, jsonData);
                    if (!this.debugOff){Debug.Log(jsonData);}//Debug
                }
                
                //Controller Action KeyUp
                if (Input.GetKeyUp((KeyCode)(int)keycode))
                {
                    string jsonData = buildInputPackage((KeyCode)(int)keycode, device);
                    InputListener(OuyaSDK.InputAction.KeyUp, jsonData);
                    if (!this.debugOff) { Debug.Log(jsonData); }//Debug
                }
            }

            #region DPAD
            string dpadHps3 = OuyaInputManager.GetInput(device.player, AxisTypes.DPadH, AnalogTypes.DPad);
            string dpadVps3 = OuyaInputManager.GetInput(device.player, AxisTypes.DPadV, AnalogTypes.DPad);


            OuyaSDK.KeyEnum dpad = getDpadPress(dpadVps3, dpadHps3, device);
            if (dpad != OuyaSDK.KeyEnum.NONE)
            {
                bool doEvent = false;
                //Check which button was pressed.
                switch (dpad)
                {
                    case OuyaSDK.KeyEnum.BUTTON_DPAD_DOWN:
                        if (!OuyaInputManager.GetButton("DPD", device.player)) { doEvent = true; axisSet = true; OuyaInputManager.SetButton("DPD", device.player, true); }
                        break;
                    case OuyaSDK.KeyEnum.BUTTON_DPAD_UP:
                        if (!OuyaInputManager.GetButton("DPU", device.player)) { doEvent = true; axisSet = true; OuyaInputManager.SetButton("DPU", device.player, true); }
                        break;
                    case OuyaSDK.KeyEnum.BUTTON_DPAD_LEFT:
                        if (!OuyaInputManager.GetButton("DPL", device.player)) { doEvent = true; axisSet = true; OuyaInputManager.SetButton("DPL", device.player, true); }
                        break;
                    case OuyaSDK.KeyEnum.BUTTON_DPAD_RIGHT:
                        if (!OuyaInputManager.GetButton("DPR", device.player)) { doEvent = true; axisSet = true; OuyaInputManager.SetButton("DPR", device.player, true); }
                        break;
                }
                if (doEvent)
                {
                    OuyaInputManager.OuyaButtonEvent.buttonPressEvent(device.player, dpad, OuyaSDK.InputAction.KeyDown);
                }
            }
            else
            {

                //Check each dpad value.. if it was previously down then triggger dpad up.
                if (OuyaInputManager.GetButton("DPD", device.player) &&  axisSet)
                {
                    OuyaInputManager.OuyaButtonEvent.buttonPressEvent(device.player, OuyaSDK.KeyEnum.BUTTON_DPAD_DOWN, OuyaSDK.InputAction.KeyUp);
                    OuyaInputManager.SetButton("DPD", device.player, false);
                    axisSet = false;
                }

                if (OuyaInputManager.GetButton("DPU", device.player) && axisSet)
                {
                    OuyaInputManager.OuyaButtonEvent.buttonPressEvent(device.player, OuyaSDK.KeyEnum.BUTTON_DPAD_UP, OuyaSDK.InputAction.KeyUp);
                    OuyaInputManager.SetButton("DPU", device.player, false);
                    axisSet = false;
                }

                if (OuyaInputManager.GetButton("DPL", device.player) && axisSet)
                {
                    OuyaInputManager.OuyaButtonEvent.buttonPressEvent(device.player, OuyaSDK.KeyEnum.BUTTON_DPAD_LEFT, OuyaSDK.InputAction.KeyUp);
                    OuyaInputManager.SetButton("DPL", device.player, false);
                    axisSet = false;
                }

                if (OuyaInputManager.GetButton("DPR", device.player) && axisSet)
                {
                    OuyaInputManager.OuyaButtonEvent.buttonPressEvent(device.player, OuyaSDK.KeyEnum.BUTTON_DPAD_RIGHT, OuyaSDK.InputAction.KeyUp);
                    OuyaInputManager.SetButton("DPR", device.player, false);
                    axisSet = false;
                }
            }

            #endregion

            #region handle triggers
            //KeyDown States only for the PC and Editor Platform.
            string trigger = OuyaInputManager.GetInput(device.player, AxisTypes.none, AnalogTypes.LTRT);
            if (!getTriggerPress(trigger).Equals(OuyaSDK.KeyEnum.NONE))
            {
                if(getTriggerPress(trigger).Equals(OuyaSDK.KeyEnum.BUTTON_LT) && !OuyaInputManager.GetButton("LT",device.player)){
                    OuyaInputManager.OuyaButtonEvent.buttonPressEvent(device.player, getTriggerPress(trigger), OuyaSDK.InputAction.KeyDown);
                    OuyaInputManager.SetButton("LT",device.player,true);
                    axisSetTriggers = true;
                }

                if (getTriggerPress(trigger).Equals(OuyaSDK.KeyEnum.BUTTON_RT) && !OuyaInputManager.GetButton("RT", device.player))
                {
                    OuyaInputManager.OuyaButtonEvent.buttonPressEvent(device.player, getTriggerPress(trigger), OuyaSDK.InputAction.KeyDown);
                    OuyaInputManager.SetButton("RT",device.player,true);
                    axisSetTriggers = true;
                }

            }
            else
            {
                //We can try to do up state, but no gaurentee it will work.
                if (OuyaInputManager.GetButton("LT", device.player) && axisSetTriggers)
                {
                    OuyaInputManager.OuyaButtonEvent.buttonPressEvent(device.player, OuyaSDK.KeyEnum.BUTTON_LT, OuyaSDK.InputAction.KeyUp);
                    OuyaInputManager.SetButton("LT", device.player, false);
                    axisSetTriggers = false;
                }
                if (OuyaInputManager.GetButton("RT", device.player) && axisSetTriggers)
                {
                    OuyaInputManager.OuyaButtonEvent.buttonPressEvent(device.player, OuyaSDK.KeyEnum.BUTTON_RT, OuyaSDK.InputAction.KeyUp);
                    OuyaInputManager.SetButton("RT", device.player, false);
                    axisSetTriggers = false;
                }
            }
            #endregion

            #region AXIS DEBUG CODE UNITY EDITOR / STANDALONE Platforms.
            if (showRawAxis)
            {
                for (int i = 1; i <= 5; i++)
                {
                    for (int a = 1; a <= 10; a++)
                    {
                        string axisName = string.Format("Joy{0} Axis {1}", i, a);
                        Debug.Log(string.Format("Name: {0}, AxisValue:{1}", axisName, Input.GetAxis(axisName)));
                    }
                }
            }
            #endregion

        }

        

#endif
    }
    #endregion

    #region Helper Methods
    private OuyaSDK.KeyEnum getTriggerPress(string axis)
    {
        //If we do not have this axis for this controller type return none.
        if (axis.Length == 0) { return OuyaSDK.KeyEnum.NONE; }

        float trigger = Input.GetAxis(axis);
        if (trigger == 0) { return OuyaSDK.KeyEnum.NONE; }
        if(trigger > 0){
            return OuyaSDK.KeyEnum.BUTTON_LT;
        }
        else if (trigger < 0)
        {
            return OuyaSDK.KeyEnum.BUTTON_RT;
        }
        return OuyaSDK.KeyEnum.NONE;
    }

    private OuyaSDK.KeyEnum getDpadPress(string V, string H, Device device)
    {
        if (Input.GetAxis(V) == 0 && Input.GetAxis(H) == 0) { return OuyaSDK.KeyEnum.NONE; }

        JoystickType joystickType = OuyaInputManager.GetControllerType(device.player);

        float fInputVert = InterpolateDeadZone(OuyaInputManager.GetInvertedFactor(joystickType, "DPU") * Input.GetAxis(V));
        float fInputHoriz = InterpolateDeadZone(OuyaInputManager.GetInvertedFactor(joystickType, "DPL") * Input.GetAxis(H));

        //Debug.Log("DPAD X:" + fInputHoriz + " | DPAD Y:" + fInputVert);
        if (!debugOff)
        {
            Debug.Log("DPAD X:" + fInputHoriz + " | DPAD Y:" + fInputVert);
        }

        OuyaSDK.KeyEnum dpad;

        if (fInputVert > 0f)
        {
            dpad = OuyaSDK.KeyEnum.BUTTON_DPAD_UP;
        }
        else if (fInputVert < 0f)
        {
            dpad = OuyaSDK.KeyEnum.BUTTON_DPAD_DOWN;
        }
        else if (fInputHoriz > 0f)
        {
            dpad = OuyaSDK.KeyEnum.BUTTON_DPAD_LEFT;
        }
        else if (fInputHoriz < 0f)
        {
            dpad = OuyaSDK.KeyEnum.BUTTON_DPAD_RIGHT;
        }
        else
        {
            dpad = OuyaSDK.KeyEnum.NONE;
        }

        return dpad;
    }


    private float m_innerDeadZone = 0.25f;
    private float m_outerDeadZone = 0.9f;
    public float InterpolateDeadZone(float rawInput)
    {
        if (Mathf.Abs(rawInput) < m_innerDeadZone)
        {
            return 0;
        }
        if (rawInput < 0f)
        {
            return -Mathf.InverseLerp(-m_innerDeadZone, -m_outerDeadZone, rawInput);
        }
        else
        {
            return Mathf.InverseLerp(m_innerDeadZone, m_outerDeadZone, rawInput);
        }
    }    

    private string buildInputPackage(KeyCode key, Device device)
    {
        KeyEvent keyEvent = new KeyEvent();
        keyEvent.mAction = 0;
        keyEvent.mDeviceId = device.id;
        keyEvent.mDownTime = 0;
        keyEvent.mFlags = 0;
        keyEvent.mRepeatCount = 0;
        keyEvent.mKeyCode = OuyaControllerMapping.findButton((int)key, OuyaInputManager.GetControllerType(device.name));
        keyEvent.mMetaState = 0;
        keyEvent.mScanCode = 0;
        keyEvent.mSource = 0;
        keyEvent.mSeq = 0;
        keyEvent.mEventTime = 0;
        keyEvent.mDownTime = 0;
        keyEvent.mRecycled = false;

        MotionEvent motionEvent = new MotionEvent();
        
        InputContainer inputContainer = new InputContainer();
        inputContainer.KeyEvent = keyEvent;
        inputContainer.MotionEvent = motionEvent;
        inputContainer.DeviceId = device.id;
        inputContainer.DeviceName = device.name;

        string jsonData = JsonMapper.ToJson(inputContainer);
        return jsonData;
    }

    #endregion

    #region IAP

    public void RequestUnityAwake(string ignore)
    {
        OuyaSDK.initialize(DEVELOPER_ID, UseLegacyInput);
    }

    public void SendIAPInitComplete(string ignore)
    {
        OuyaSDK.setIAPInitComplete();
    }

    public void InvokeOuyaFetchGamerUUIDOnSuccess(string gamerUUID)
    {
        foreach (OuyaSDK.IFetchGamerUUIDListener listener in OuyaSDK.getFetchGamerUUIDListeners())
        {
            if (null != listener)
            {
                listener.OuyaFetchGamerUUIDOnSuccess(gamerUUID);
            }
        }
    }

    public void InvokeOuyaFetchGamerUUIDOnFailure(int errorCode, string errorMessage)
    {
        Debug.LogError(string.Format("InvokeOuyaFetchGamerUUIDOnFailure: error={0} errorMessage={1}", errorCode, errorMessage));
        foreach (OuyaSDK.IFetchGamerUUIDListener listener in OuyaSDK.getFetchGamerUUIDListeners())
        {
            if (null != listener)
            {
                listener.OuyaFetchGamerUUIDOnFailure(errorCode, errorMessage);
            }
        }
    }

    public void InvokeOuyaFetchGamerUUIDOnCancel()
    {
        //Debug.Log("InvokeOuyaFetchGamerUUIDOnCancel");
        foreach (OuyaSDK.IFetchGamerUUIDListener listener in OuyaSDK.getFetchGamerUUIDListeners())
        {
            if (null != listener)
            {
                listener.OuyaFetchGamerUUIDOnCancel();
            }
        }
    }

    public void InvokeOuyaGetProductsOnSuccess(List<OuyaSDK.Product> products)
    {
        foreach (OuyaSDK.IGetProductsListener listener in OuyaSDK.getGetProductsListeners())
        {
            if (null != listener)
            {
                listener.OuyaGetProductsOnSuccess(products);
            }
        }
    }

    public void InvokeOuyaGetProductsOnFailure(int errorCode, string errorMessage)
    {
        Debug.LogError(string.Format("InvokeOuyaGetProductsOnFailure: error={0} errorMessage={1}", errorCode, errorMessage));
        foreach (OuyaSDK.IGetProductsListener listener in OuyaSDK.getGetProductsListeners())
        {
            if (null != listener)
            {
                listener.OuyaGetProductsOnFailure(errorCode, errorMessage);
            }
        }
    }

    public void InvokeOuyaPurchaseOnSuccess(OuyaSDK.Product product)
    {
        foreach (OuyaSDK.IPurchaseListener listener in OuyaSDK.getPurchaseListeners())
        {
            if (null != listener)
            {
                listener.OuyaPurchaseOnSuccess(product);
            }
        }
    }

    public void InvokeOuyaPurchaseOnFailure(int errorCode, string errorMessage)
    {
        Debug.LogError(string.Format("InvokeOuyaPurchaseOnFailure: error={0} errorMessage={1}", errorCode, errorMessage));
        foreach (OuyaSDK.IPurchaseListener listener in OuyaSDK.getPurchaseListeners())
        {
            if (null != listener)
            {
                listener.OuyaPurchaseOnFailure(errorCode, errorMessage);
            }
        }
    }

    public void InvokeOuyaPurchaseOnCancel()
    {
        //Debug.Log("InvokeOuyaPurchaseOnCancel");
        foreach (OuyaSDK.IPurchaseListener listener in OuyaSDK.getPurchaseListeners())
        {
            if (null != listener)
            {
                listener.OuyaPurchaseOnCancel();
            }
        }
    }

    public void InvokeOuyaGetReceiptsOnSuccess(List<OuyaSDK.Receipt> receipts)
    {
        foreach (OuyaSDK.IGetReceiptsListener listener in OuyaSDK.getGetReceiptsListeners())
        {
            if (null != listener)
            {
                listener.OuyaGetReceiptsOnSuccess(receipts);
            }
        }
    }

    public void InvokeOuyaGetReceiptsOnFailure(int errorCode, string errorMessage)
    {
        Debug.LogError(string.Format("InvokeOuyaGetReceiptsOnFailure: error={0} errorMessage={1}", errorCode, errorMessage));
        foreach (OuyaSDK.IGetReceiptsListener listener in OuyaSDK.getGetReceiptsListeners())
        {
            if (null != listener)
            {
                listener.OuyaGetReceiptsOnFailure(errorCode, errorMessage);
            }
        }
    }

    public void InvokeOuyaGetReceiptsOnCancel()
    {
        //Debug.Log("InvokeOuyaGetReceiptsOnCancel");
        foreach (OuyaSDK.IGetReceiptsListener listener in OuyaSDK.getGetReceiptsListeners())
        {
            if (null != listener)
            {
                listener.OuyaGetReceiptsOnCancel();
            }
        }
    }

    #endregion

    #region Debug Logs
    public void DebugLog(string message)
    {
        Debug.Log(message);
    }

    public void DebugLogError(string message)
    {
        Debug.LogError(message);
    }
    #endregion

}