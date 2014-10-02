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
import android.content.res.AssetManager;
import android.content.res.Configuration;
import android.os.Bundle;
import android.util.Log;
import android.view.KeyEvent;
import android.view.MotionEvent;
import android.view.InputDevice;
import android.view.View;
import android.view.Window;
import android.view.WindowManager;
import android.widget.FrameLayout;
import android.widget.LinearLayout;
import android.widget.LinearLayout.LayoutParams;
import android.widget.RelativeLayout;

import com.unity3d.player.UnityPlayer;

import tv.ouya.console.api.*;

import java.io.InputStream;
import java.io.IOException;
import java.util.ArrayList;

public class MainActivity extends OuyaUnityActivity
{
	//indicates the Unity player has loaded
	private Boolean m_enableUnity = true;

	//indicates use logging in one place
	private Boolean m_enableLogging = false;

	protected void onCreate(Bundle savedInstanceState)
	{
		super.onCreate(savedInstanceState);

		//make activity accessible to Unity
		IOuyaActivity.SetActivity(this);

		//make bundle accessible to Unity
		IOuyaActivity.SetSavedInstanceState(savedInstanceState);

		// load the signing key from assets
		try {
			Context context = getApplicationContext();
			AssetManager assetManager = context.getAssets();
			InputStream inputStream = assetManager.open("key.der", AssetManager.ACCESS_BUFFER);
			byte[] applicationKey = new byte[inputStream.available()];
			inputStream.read(applicationKey);
			inputStream.close();
			IOuyaActivity.SetApplicationKey(applicationKey);
		} catch (IOException e) {
			e.printStackTrace();
		}

		// Create the View
		RelativeLayout mainLayout = new RelativeLayout(this);
		LayoutParams lp = new LayoutParams (LayoutParams.MATCH_PARENT, LayoutParams.MATCH_PARENT);
		mainLayout.setLayoutParams(lp);

		LinearLayout linearLayout = new LinearLayout(this);
		lp = new LayoutParams (LayoutParams.MATCH_PARENT, LayoutParams.MATCH_PARENT);
		linearLayout.setOrientation(LinearLayout.VERTICAL);
		mainLayout.addView(linearLayout, 0, lp);
		
		FrameLayout unityLayout = new FrameLayout(this);
		lp = new LayoutParams (LayoutParams.MATCH_PARENT, LayoutParams.MATCH_PARENT);
		linearLayout.addView(unityLayout, 0, lp);
		
		// Create the UnityPlayer
        IOuyaActivity.SetUnityPlayer(new UnityPlayer(this));
        int glesMode = IOuyaActivity.GetUnityPlayer().getSettings().getInt("gles_mode", 1);
        boolean trueColor8888 = false;
        IOuyaActivity.GetUnityPlayer().init(glesMode, trueColor8888);
        setContentView(mainLayout);

        // Add the Unity view
        lp = new LayoutParams (LayoutParams.MATCH_PARENT, LayoutParams.MATCH_PARENT);
        unityLayout.addView(IOuyaActivity.GetUnityPlayer().getView(), 0, lp);

		// Set the focus
		mainLayout.setFocusableInTouchMode(true);
	}

    /**
     * Broadcast listener to handle re-requesting the receipts when a user has re-authenticated
     */

    private BroadcastReceiver mAuthChangeReceiver = new BroadcastReceiver() {
        @Override
        public void onReceive(Context context, Intent intent) {
			UnityOuyaFacade unityOuyaFacade = IOuyaActivity.GetUnityOuyaFacade();
			if (null != unityOuyaFacade)
			{
				unityOuyaFacade.requestReceipts();
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
		finish();
    }

    /**
     * Check for the result from a call through to the authentication intent. If the authentication was
     * successful then re-try the purchase.
     */

    @Override
    protected void onActivityResult(final int requestCode, final int resultCode, final Intent data) {
        if(resultCode == RESULT_OK) {
			UnityOuyaFacade unityOuyaFacade = IOuyaActivity.GetUnityOuyaFacade();
			if (null != unityOuyaFacade)
			{
				switch (requestCode) {
					case UnityOuyaFacade.GAMER_UUID_AUTHENTICATION_ACTIVITY_ID:
						unityOuyaFacade.fetchGamerInfo();
						break;
					case UnityOuyaFacade.PURCHASE_AUTHENTICATION_ACTIVITY_ID:
						unityOuyaFacade.restartInterruptedPurchase();
						break;
				}
            }
        }
    }

	@Override
    protected void onSaveInstanceState(final Bundle outState)
	{
		UnityOuyaFacade unityOuyaFacade = IOuyaActivity.GetUnityOuyaFacade();
		if (null != unityOuyaFacade)
		{
			unityOuyaFacade.onSaveInstanceState(outState);
		}
	}

	@Override
    protected void onDestroy()
	{
		UnityOuyaFacade unityOuyaFacade = IOuyaActivity.GetUnityOuyaFacade();
		if (null != unityOuyaFacade)
		{
			unityOuyaFacade.onDestroy();
		}

		if (null != IOuyaActivity.GetUnityPlayer())
		{
			IOuyaActivity.GetUnityPlayer().quit();
		}

        super.onDestroy();
    }

    @Override
    public void onResume()
	{
		if (m_enableUnity)
		{
			UnityPlayer.UnitySendMessage("OuyaGameObject", "onResume", "");
		}

		super.onResume();

		IOuyaActivity.GetUnityPlayer().resume();
    }

    @Override
    public void onPause()
	{
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
}
