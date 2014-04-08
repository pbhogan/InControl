package tv.ouya.demo.OuyaUnityApplication;

import tv.ouya.console.api.*;
import tv.ouya.console.api.OuyaController;
import tv.ouya.sdk.*;

import android.accounts.AccountManager;
import android.app.Activity;
import android.app.NativeActivity;
import android.content.*;
import android.content.res.Configuration;
import android.graphics.PixelFormat;
import android.os.Bundle;
import android.util.Log;
import android.view.KeyEvent;
import android.view.MotionEvent;
import android.view.InputDevice;
import android.view.View;
import android.view.Window;
import android.view.WindowManager;
import android.widget.FrameLayout;
import android.widget.LinearLayout.LayoutParams;
import android.widget.RelativeLayout;

import com.unity3d.player.UnityPlayer;
import com.unity3d.player.UnityPlayerActivity;
import com.unity3d.player.UnityPlayerProxyActivity;

import java.io.InputStream;
import java.io.IOException;
import java.util.ArrayList;

public class OuyaNativeActivity extends NativeActivity
{
	protected UnityPlayer mUnityPlayer;		// don't change the name of this variable; referenced from native code

	// UnityPlayer.init() should be called before attaching the view to a layout - it will load the native code.
	// UnityPlayer.quit() should be the last thing called - it will unload the native code.
	protected void onCreate (Bundle savedInstanceState)
	{
		Log.i("Unity", "***Starting OUYA Native Activity*********");

		//make activity accessible to Unity
		IOuyaActivity.SetActivity(this);

		//make bundle accessible to Unity
		IOuyaActivity.SetSavedInstanceState(savedInstanceState);

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

		requestWindowFeature(Window.FEATURE_NO_TITLE);
		super.onCreate(savedInstanceState);

		getWindow().takeSurface(null);
		setTheme(android.R.style.Theme_NoTitleBar_Fullscreen);
		getWindow().setFormat(PixelFormat.RGB_565);

		mUnityPlayer = new UnityPlayer(this);
		if (mUnityPlayer.getSettings ().getBoolean ("hide_status_bar", true))
			getWindow ().setFlags (WindowManager.LayoutParams.FLAG_FULLSCREEN,
			                       WindowManager.LayoutParams.FLAG_FULLSCREEN);

		int glesMode = mUnityPlayer.getSettings().getInt("gles_mode", 1);
		boolean trueColor8888 = false;
		mUnityPlayer.init(glesMode, trueColor8888);

		View playerView = mUnityPlayer.getView();
		setContentView(playerView);
		playerView.requestFocus();

		// Create the UnityPlayer
        IOuyaActivity.SetUnityPlayer(mUnityPlayer);

		Context context = getBaseContext();

		// Init the controller
		OuyaController.init(context);
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
				Log.i("Unity", "BroadcastReceiver tell Unity we see the menu appearing");
				UnityPlayer.UnitySendMessage("OuyaGameObject", "onMenuAppearing", "");
				Log.i("Unity", "BroadcastReceiver notified Unity onMenuAppearing");
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

	protected void onDestroy ()
	{
		mUnityPlayer.quit();
		super.onDestroy();
	}

	// onPause()/onResume() must be sent to UnityPlayer to enable pause and resource recreation on resume.
	protected void onPause()
	{
		UnityPlayer.UnitySendMessage("OuyaGameObject", "onPause", "");

		super.onPause();
		mUnityPlayer.pause();
	}
	protected void onResume()
	{
		UnityPlayer.UnitySendMessage("OuyaGameObject", "onResume", "");

		super.onResume();
		mUnityPlayer.resume();
	}
	public void onConfigurationChanged(Configuration newConfig)
	{
		super.onConfigurationChanged(newConfig);
		mUnityPlayer.configurationChanged(newConfig);
	}
	public void onWindowFocusChanged(boolean hasFocus)
	{
		super.onWindowFocusChanged(hasFocus);
		mUnityPlayer.windowFocusChanged(hasFocus);
	}
	public boolean dispatchKeyEvent(KeyEvent event)
	{
		if (event.getAction() == KeyEvent.ACTION_MULTIPLE)
			return mUnityPlayer.onKeyMultiple(event.getKeyCode(), event.getRepeatCount(), event);
		return super.dispatchKeyEvent(event);
	}
}
