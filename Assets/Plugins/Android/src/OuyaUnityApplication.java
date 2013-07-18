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

package tv.ouya.demo.OuyaUnityApplication;

import tv.ouya.console.api.OuyaController;
import tv.ouya.sdk.*;

import android.accounts.AccountManager;
import android.app.Activity;
import android.content.*;
import android.content.res.Configuration;
import android.hardware.input.InputManager; //API 16
import android.hardware.input.InputManager.InputDeviceListener; //API 16
import android.os.Bundle;
import android.util.Log;
import android.view.KeyEvent;
import android.view.MotionEvent;
import android.view.InputDevice;
import android.widget.FrameLayout;
import android.widget.LinearLayout.LayoutParams;
import android.widget.RelativeLayout;

import com.google.gson.Gson;

import com.unity3d.player.UnityPlayer;
import com.unity3d.player.UnityPlayerActivity;
import com.unity3d.player.UnityPlayerNativeActivity;
import com.unity3d.player.UnityPlayerProxyActivity;

import tv.ouya.console.api.*;

import java.io.InputStream;
import java.io.IOException;
import java.util.ArrayList;

public class OuyaUnityApplication extends Activity
	implements InputDeviceListener
{
	//indicates the Unity player has loaded
	private Boolean m_enableUnity = true;

	//indicates use logging in one place
	private Boolean m_enableLogging = false;

	private InputManager m_inputManager = null;
	private InputManager.InputDeviceListener m_inputDeviceListener = null;

	protected void onCreate(Bundle savedInstanceState) 
	{
		//make activity accessible to Unity
		IOuyaActivity.SetActivity(this);

		//make bundle accessible to Unity
		IOuyaActivity.SetSavedInstanceState(savedInstanceState);

		super.onCreate(savedInstanceState);

		// load the raw resource for the application key
		try {
			InputStream inputStream = getResources().openRawResource(R.raw.key);
			byte[] applicationKey = new byte[inputStream.available()];
			inputStream.read(applicationKey);
			inputStream.close();
			IOuyaActivity.SetApplicationKey(applicationKey);
		} catch (IOException e) {
			e.printStackTrace();
		}

		// Create the UnityPlayer
        IOuyaActivity.SetUnityPlayer(new UnityPlayer(this));
        int glesMode = IOuyaActivity.GetUnityPlayer().getSettings().getInt("gles_mode", 1);
        boolean trueColor8888 = false;
        IOuyaActivity.GetUnityPlayer().init(glesMode, trueColor8888);
        setContentView(R.layout.main);

        // Add the Unity view
        FrameLayout layout = (FrameLayout) findViewById(R.id.unityLayout);
        LayoutParams lp = new LayoutParams (LayoutParams.MATCH_PARENT, LayoutParams.MATCH_PARENT);
        layout.addView(IOuyaActivity.GetUnityPlayer().getView(), 0, lp);
		IOuyaActivity.SetLayout(layout);

		// Set the focus
        RelativeLayout mainLayout = (RelativeLayout) findViewById(R.id.mainLayout);
		mainLayout.setFocusableInTouchMode(true);

		Context context = getBaseContext();

		// listen for controller changes - http://developer.android.com/reference/android/hardware/input/InputManager.html#registerInputDeviceListener%28android.hardware.input.InputManager.InputDeviceListener,%20android.os.Handler%29
		m_inputManager = (InputManager)context.getSystemService(Context.INPUT_SERVICE);
		m_inputManager.registerInputDeviceListener (this, null);

		// Init the controller
		OuyaController.init(context);

		sendDevices();		
	}

    /**
     * Broadcast listener to handle re-requesting the receipts when a user has re-authenticated
     */

    private BroadcastReceiver mAuthChangeReceiver = new BroadcastReceiver() {
        @Override
        public void onReceive(Context context, Intent intent) {
			TestOuyaFacade test = IOuyaActivity.GetTestOuyaFacade();
			if (null != test)
			{
				test.requestReceipts();
			}
        }
    };

    /**
     * Broadcast listener to handle menu appearing
     */

    private BroadcastReceiver mMenuAppearingReceiver = new BroadcastReceiver() {
        @Override
        public void onReceive(Context context, Intent intent) {
			Log.i("Unity", "BroadcastReceiver intent=" + intent.getAction());
			if(intent.getAction().equals(OuyaIntent.ACTION_MENUAPPEARING)) {
				//pause music, free up resources, etc.

				//invoke a unity callback
				if (m_enableUnity)
				{
					Log.i("Unity", "BroadcastReceiver tell Unity we see the menu appearing");
					UnityPlayer.UnitySendMessage("OuyaGameObject", "onMenuAppearing", "");
					Log.i("Unity", "BroadcastReceiver notified Unity onMenuAppearing");
				}
			}
        }
    };

    /**
     * Request an up to date list of receipts and start listening for any account changes
     * whilst the application is running.
     */
    @Override
    public void onStart() {
        super.onStart();

        // Request an up to date list of receipts for the user.
        //requestReceipts();

        // Register to receive notifications about account changes. This will re-query
        // the receipt list in order to ensure it is always up to date for whomever
        // is logged in.
        IntentFilter accountsChangedFilter = new IntentFilter();
        accountsChangedFilter.addAction(AccountManager.LOGIN_ACCOUNTS_CHANGED_ACTION);
        registerReceiver(mAuthChangeReceiver, accountsChangedFilter);

		IntentFilter menuAppearingFilter = new IntentFilter();
		menuAppearingFilter.addAction(OuyaIntent.ACTION_MENUAPPEARING);
		registerReceiver(mMenuAppearingReceiver, menuAppearingFilter);
    }

    /**
     * Unregister the account change listener when the application is stopped.
     */
    @Override
    public void onStop() {
		unregisterReceiver(mAuthChangeReceiver);
		unregisterReceiver(mMenuAppearingReceiver);
        super.onStop();
    }

    /**
     * Check for the result from a call through to the authentication intent. If the authentication was
     * successful then re-try the purchase.
     */

    @Override
    protected void onActivityResult(final int requestCode, final int resultCode, final Intent data) {
        if(resultCode == RESULT_OK) {
			TestOuyaFacade test = IOuyaActivity.GetTestOuyaFacade();
			if (null != test)
			{
				switch (requestCode) {
					case TestOuyaFacade.GAMER_UUID_AUTHENTICATION_ACTIVITY_ID:
						test.fetchGamerUUID();
						break;
					case TestOuyaFacade.PURCHASE_AUTHENTICATION_ACTIVITY_ID:
						test.restartInterruptedPurchase();
						break;
				}
            }
        }
    }

	@Override
    protected void onSaveInstanceState(final Bundle outState)
	{
		TestOuyaFacade test = IOuyaActivity.GetTestOuyaFacade();
		if (null != test)
		{
			test.onSaveInstanceState(outState);
		}
	}

	@Override
    protected void onDestroy()
	{
		TestOuyaFacade test = IOuyaActivity.GetTestOuyaFacade();
		if (null != test)
		{
			test.onDestroy();
		}

		if (null != IOuyaActivity.GetUnityPlayer())
		{
			IOuyaActivity.GetUnityPlayer().quit();
		}

        super.onDestroy();
    }

	/// Implements InputDeviceListener
	public @Override void onInputDeviceAdded(int deviceId)
	{
		if (m_enableLogging)
		{
			Log.i("Unity", "void onInputDeviceAdded(int deviceId) " + deviceId);
		}
		sendDevices();
	}
	public @Override void onInputDeviceChanged(int deviceId)
	{
		if (m_enableLogging)
		{
			Log.i("Unity", "void onInputDeviceAdded(int deviceId) " + deviceId);
		}
		sendDevices();
	}
	public @Override void onInputDeviceRemoved(int deviceId)
	{
		if (m_enableLogging)
		{
			Log.i("Unity", "void onInputDeviceRemoved(int deviceId) " + deviceId);
		}
		sendDevices();
	}

    @Override
    public void onResume()
	{
		if (null != m_inputManager)
		{
			m_inputManager.registerInputDeviceListener(this, null);
		}

		if (m_enableUnity)
		{
			UnityPlayer.UnitySendMessage("OuyaGameObject", "onResume", "");
		}

		sendDevices();

		super.onResume();

		IOuyaActivity.GetUnityPlayer().resume();
    }

    @Override
    public void onPause()
	{
		if (null != m_inputManager)
		{
			m_inputManager.unregisterInputDeviceListener(this);
		}

		if (m_enableUnity)
		{
			UnityPlayer.UnitySendMessage("OuyaGameObject", "onPause", "");
		}

		Boolean isFinishing = isFinishing();
		if (m_enableLogging)
		{
			Log.i("Unity", "isFinishing=" + isFinishing);
		}
		if (isFinishing)
		{
			IOuyaActivity.GetUnityPlayer().quit();
		}

		IOuyaActivity.GetUnityPlayer().pause();

		super.onPause();
    }

	void sendDevices()
	{
		//Get a list of all device id's and assign them to players.
		ArrayList<Device> devices = checkDevices();
		GenericSendMessage("onSetDevices", devices);
	}

	void inputDeviceListener()
	{
		sendDevices();
	}

	private ArrayList<Device> checkDevices(){
		//Get a list of all device id's and assign them to players.
		ArrayList<Device> devices = new ArrayList<Device>();
		int[] deviceIds = InputDevice.getDeviceIds();
		
		if (m_enableLogging)
		{
			//Log.i("Unity-Devices", "length:" + deviceIds.length );
		}

		int controllerCount = 1;
		for (int count=0; count < deviceIds.length; count++)
		{
			InputDevice d = InputDevice.getDevice(deviceIds[count]);
			if (!d.isVirtual())
			{
				if (d.getName().toUpperCase().indexOf("XBOX 360 WIRELESS RECEIVER") != -1 ||
					d.getName().toUpperCase().indexOf("OUYA GAME CONTROLLER") != -1 ||
					d.getName().toUpperCase().indexOf("MICROSOFT X-BOX 360 PAD") != -1 ||
					d.getName().toUpperCase().indexOf("IDROID:CON") != -1 ||
					d.getName().toUpperCase().indexOf("USB CONTROLLER") != -1)
				{
					Device device = new Device();
					device.id = d.getId();
					device.player = controllerCount;
					device.name = d.getName();
					device.playerNum = OuyaController.getPlayerNumByDeviceId(d.getId());
					devices.add(device);
					controllerCount++;
				}
				else
				{
					Device device = new Device();
					device.id = d.getId();
					device.player = 0;
					device.playerNum = OuyaController.getPlayerNumByDeviceId(d.getId());
					device.name = d.getName();
					devices.add(device);
				}
			}
		}
		return devices;
	}

	private void ExtractDataMotionEvent (InputContainer box, MotionEvent event)
	{
		box.MotionEvent = event;
		
		box.ActionCode = event.getAction();
		box.ActionIndex = event.getActionIndex();
		box.ActionMasked = event.getActionMasked();
		
		box.AxisBrake = event.getAxisValue(MotionEvent.AXIS_BRAKE);
        box.AxisDistance = event.getAxisValue(MotionEvent.AXIS_DISTANCE);
        box.AxisGas = event.getAxisValue(MotionEvent.AXIS_GAS);
		box.AxisGeneric1 = event.getAxisValue(MotionEvent.AXIS_GENERIC_1);
		box.AxisGeneric2 = event.getAxisValue(MotionEvent.AXIS_GENERIC_2);
		box.AxisGeneric3 = event.getAxisValue(MotionEvent.AXIS_GENERIC_3);
		box.AxisGeneric4 = event.getAxisValue(MotionEvent.AXIS_GENERIC_4);
		box.AxisGeneric5 = event.getAxisValue(MotionEvent.AXIS_GENERIC_5);
		box.AxisGeneric6 = event.getAxisValue(MotionEvent.AXIS_GENERIC_6);
		box.AxisGeneric7 = event.getAxisValue(MotionEvent.AXIS_GENERIC_7);
		box.AxisGeneric8 = event.getAxisValue(MotionEvent.AXIS_GENERIC_8);
		box.AxisGeneric9 = event.getAxisValue(MotionEvent.AXIS_GENERIC_9);
		box.AxisGeneric10 = event.getAxisValue(MotionEvent.AXIS_GENERIC_10);
		box.AxisGeneric11 = event.getAxisValue(MotionEvent.AXIS_GENERIC_11);
		box.AxisGeneric12 = event.getAxisValue(MotionEvent.AXIS_GENERIC_12);
		box.AxisGeneric13 = event.getAxisValue(MotionEvent.AXIS_GENERIC_13);
		box.AxisGeneric14 = event.getAxisValue(MotionEvent.AXIS_GENERIC_14);
		box.AxisGeneric15 = event.getAxisValue(MotionEvent.AXIS_GENERIC_15);
		box.AxisGeneric16 = event.getAxisValue(MotionEvent.AXIS_GENERIC_16);
		box.AxisHatX = event.getAxisValue(MotionEvent.AXIS_HAT_X);
        box.AxisHatY = event.getAxisValue(MotionEvent.AXIS_HAT_Y);
        box.AxisHScroll = event.getAxisValue(MotionEvent.AXIS_HSCROLL);
        box.AxisLTrigger = event.getAxisValue(MotionEvent.AXIS_LTRIGGER);
        box.AxisOrientation = event.getAxisValue(MotionEvent.AXIS_ORIENTATION);
        box.AxisPressire = event.getAxisValue(MotionEvent.AXIS_PRESSURE);
        box.AxisRTrigger = event.getAxisValue(MotionEvent.AXIS_RTRIGGER);
        box.AxisRudder = event.getAxisValue(MotionEvent.AXIS_RUDDER);
        box.AxisRX = event.getAxisValue(MotionEvent.AXIS_RX);
        box.AxisRY = event.getAxisValue(MotionEvent.AXIS_RY);
        box.AxisRZ = event.getAxisValue(MotionEvent.AXIS_RZ);
        box.AxisSize = event.getAxisValue(MotionEvent.AXIS_SIZE);
        box.AxisThrottle = event.getAxisValue(MotionEvent.AXIS_THROTTLE);
        box.AxisTilt = event.getAxisValue(MotionEvent.AXIS_TILT);
        box.AxisToolMajor = event.getAxisValue(MotionEvent.AXIS_TOUCH_MAJOR);
        box.AxisToolMinor = event.getAxisValue(MotionEvent.AXIS_TOUCH_MINOR);
        box.AxisVScroll = event.getAxisValue(MotionEvent.AXIS_VSCROLL);
        box.AxisWheel = event.getAxisValue(MotionEvent.AXIS_WHEEL);
        box.AxisX = event.getAxisValue(MotionEvent.AXIS_X);
		box.AxisY = event.getAxisValue(MotionEvent.AXIS_Y);
		box.AxisZ = event.getAxisValue(MotionEvent.AXIS_Z);

		box.ButtonState = event.getButtonState();
		box.EdgeFlags = event.getEdgeFlags();
		box.Flags = event.getFlags();
		box.DeviceId = event.getDeviceId();
		InputDevice device = InputDevice.getDevice(box.DeviceId);
		if (null != device)
		{
			box.DeviceName = device.getName();
		}
		box.MetaState = event.getMetaState();
		box.PointerCount = event.getPointerCount();
		box.Pressure = event.getPressure();
		box.X = event.getX();
		box.Y = event.getY();
	}

	private void ExtractDataKeyEvent (InputContainer box, KeyEvent event)
	{
		box.KeyEvent = event;
		
		box.ActionCode = event.getAction();
		box.Flags = event.getFlags();
		box.DeviceId = event.getDeviceId();
		InputDevice device = InputDevice.getDevice(box.DeviceId);
		if (null != device)
		{
			box.DeviceName = device.getName();
		}
		box.MetaState = event.getMetaState();
	}

	private void GenericSendMessage (String method, InputContainer box)
	{
		Gson gson = new Gson();
		String jsonData = gson.toJson(box);

		if (m_enableLogging)
		{
			//Log.i("Unity", method + " jsonData=" + jsonData);
		}

		if (m_enableUnity)
		{
			UnityPlayer.UnitySendMessage("OuyaGameObject", method, jsonData);
		}
	}

	private void GenericSendMessage (String method, ArrayList<Device> devices)
	{
		Gson gson = new Gson();
		String jsonData = gson.toJson(devices);

		if (m_enableLogging)
		{
			Log.i("Unity", method + " jsonData=" + jsonData);
		}
		
		if (m_enableUnity)
		{
			UnityPlayer.UnitySendMessage("OuyaGameObject", method, jsonData);
		}
	}
	public void onConfigurationChanged(Configuration newConfig)
	{
		super.onConfigurationChanged(newConfig);
		if (null == IOuyaActivity.GetUnityPlayer())
		{
			Log.i("Unity", "IOuyaActivity.GetUnityPlayer() is null");
			return;
		}
		IOuyaActivity.GetUnityPlayer().configurationChanged(newConfig);
	}
	public void onWindowFocusChanged(boolean hasFocus)
	{
		super.onWindowFocusChanged(hasFocus);
		if (null == IOuyaActivity.GetUnityPlayer())
		{
			Log.i("Unity", "IOuyaActivity.GetUnityPlayer() is null");
			return;
		}
		IOuyaActivity.GetUnityPlayer().windowFocusChanged(hasFocus);
	}
	// Pass any keys not handled by (unfocused) views straight to UnityPlayer
	public boolean onKeyMultiple(int keyCode, int count, KeyEvent event)
	{
		if (null == IOuyaActivity.GetUnityPlayer())
		{
			Log.i("Unity", "IOuyaActivity.GetUnityPlayer() is null");
			return false;
		}
		return IOuyaActivity.GetUnityPlayer().onKeyMultiple(keyCode, count, event);
	}

	@Override
	public boolean onKeyDown (int keyCode, KeyEvent event)
	{
		if (OuyaUnityPlugin.getUseLegacyInput())
		{
			InputContainer box = new InputContainer();
			ExtractDataKeyEvent(box, event);
			box.KeyCode = keyCode;
			GenericSendMessage("onKeyDown", box);
			return true;
		}

		if (null == IOuyaActivity.GetUnityPlayer())
		{
			Log.i("Unity", "IOuyaActivity.GetUnityPlayer() is null");
			return false;
		}

		return IOuyaActivity.GetUnityPlayer().onKeyDown(keyCode, event);
	}

	@Override
	public boolean onKeyUp (int keyCode, KeyEvent event)
	{
		//Log.i("Unity", "onKeyUp keyCode=" + keyCode);
		if (keyCode == OuyaController.BUTTON_MENU) {
			Log.i("Unity", "BroadcastReceiver tell Unity we see the menu button up");
			UnityPlayer.UnitySendMessage("OuyaGameObject", "onMenuButtonUp", "");
			Log.i("Unity", "BroadcastReceiver notified Unity onMenuButtonUp");
			 
		}

		if (OuyaUnityPlugin.getUseLegacyInput())
		{
			InputContainer box = new InputContainer();
			ExtractDataKeyEvent(box, event);
			box.KeyCode = keyCode;
			GenericSendMessage("onKeyUp", box);
			return true;
		}

		if (null == IOuyaActivity.GetUnityPlayer())
		{
			Log.i("Unity", "IOuyaActivity.GetUnityPlayer() is null");
			return false;
		}
		
		return IOuyaActivity.GetUnityPlayer().onKeyUp(keyCode, event);
	}

	@Override
	public boolean onGenericMotionEvent (MotionEvent event)
	{
		if (OuyaUnityPlugin.getUseLegacyInput())
		{
			InputContainer box = new InputContainer();
			ExtractDataMotionEvent(box, event);
			GenericSendMessage("onGenericMotionEvent", box);
			return true;
		}

		if (null == IOuyaActivity.GetUnityPlayer())
		{
			Log.i("Unity", "IOuyaActivity.GetUnityPlayer() is null");
			return false;
		}

		//return IOuyaActivity.GetUnityPlayer().onGenericMotionEvent(event);

		//rupert is awesome!!! workaround to not detecting axis input (3.5.7)
		return IOuyaActivity.GetUnityPlayer().onTouchEvent(event);

		/*

		//yes we have the data

		//Get the player #
		int player = OuyaController.getPlayerNumByDeviceId(event.getDeviceId());    

		//Get all the axis for the event
		float LS_X = event.getAxisValue(OuyaController.AXIS_LS_X);
		float LS_Y = event.getAxisValue(OuyaController.AXIS_LS_Y);
		float RS_X = event.getAxisValue(OuyaController.AXIS_RS_X);
		float RS_Y = event.getAxisValue(OuyaController.AXIS_RS_Y);
		float L2 = event.getAxisValue(OuyaController.AXIS_L2);
		float R2 = event.getAxisValue(OuyaController.AXIS_R2);

		//Do something with the input
		//updatePlayerInput(player, LS_X, LS_Y, RS_X, RS_Y, L2, R2);
		Log.i("Unity", "LS_X="+LS_X+", LS_Y"+LS_Y+", RS_X="+RS_X+", RS_Y"+RS_Y+", L2="+L2+", R2"+R2);

		return IOuyaActivity.GetUnityPlayer().onGenericMotionEvent(m);
		*/
	}

	@Override
	public boolean onTouchEvent (MotionEvent event)
	{
		if (OuyaUnityPlugin.getUseLegacyInput())
		{
			InputContainer box = new InputContainer();
			ExtractDataMotionEvent(box, event);
			GenericSendMessage("onTouchEvent", box);
			return true;
		}

		if (null == IOuyaActivity.GetUnityPlayer())
		{
			Log.i("Unity", "IOuyaActivity.GetUnityPlayer() is null");
			return false;
		}
		
		return IOuyaActivity.GetUnityPlayer().onTouchEvent(event);
	}

	@Override
	public boolean onTrackballEvent (MotionEvent event)
	{
		if (OuyaUnityPlugin.getUseLegacyInput())
		{
			InputContainer box = new InputContainer();
			ExtractDataMotionEvent(box, event);
			GenericSendMessage("onTrackballEvent", box);
			return true;
		}

		if (null == IOuyaActivity.GetUnityPlayer())
		{
			Log.i("Unity", "IOuyaActivity.GetUnityPlayer() is null");
			return false;
		}
		
		return IOuyaActivity.GetUnityPlayer().onTrackballEvent(event);
	}

	public class Device
	{
		public int id;
		public int player;
		public int playerNum;
		public String name;
	}

	public class InputContainer
	{
		public int KeyCode = 0;
		public KeyEvent KeyEvent = null;
		public MotionEvent MotionEvent = null;
		
		public int Action = 0;
        public int ActionCode = 0;
        public int ActionIndex = 0;
        public int ActionMasked = 0;
        
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

		public int ButtonState = 0;
        public int DeviceId = 0;
        public String DeviceName = "";
        public int EdgeFlags = 0;
        public int Flags = 0;
        public int MetaState = 0;
        public int PointerCount = 0;
        public float Pressure = 0;
        public float X = 0;
        public float Y = 0;
	}
}
