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

package tv.ouya.sdk;

import android.app.Activity;
import android.os.Bundle;
import android.widget.FrameLayout;
import com.unity3d.player.UnityPlayer;

public class IOuyaActivity
{
	// save reference to the activity
	protected static Activity m_activity = null;
	public static Activity GetActivity()
	{
		return m_activity;
	}
	public static void SetActivity(Activity activity)
	{
		m_activity = activity;
	}

	// save reference to the unity player
	protected static UnityPlayer m_unityPlayer = null;
	public static UnityPlayer GetUnityPlayer()
	{
		return m_unityPlayer;
	}
	public static void SetUnityPlayer(UnityPlayer unityPlayer)
	{
		m_unityPlayer = unityPlayer;
	}

	// save reference to the bundle
	protected static Bundle m_savedInstanceState = null;
	public static Bundle GetSavedInstanceState()
	{
		return m_savedInstanceState;
	}
	public static void SetSavedInstanceState(Bundle savedInstanceState)
	{
		m_savedInstanceState = savedInstanceState;
	}

	// save reference to the UnityOuyaFacade
	protected static UnityOuyaFacade m_unityOuyaFacade = null;
	public static UnityOuyaFacade GetUnityOuyaFacade()
	{
		return m_unityOuyaFacade;
	}
	public static void SetUnityOuyaFacade(UnityOuyaFacade unityOuyaFacade)
	{
		m_unityOuyaFacade = unityOuyaFacade;
	}

	/*
	* The application key. This is used to decrypt encrypted receipt responses. This should be replaced with the
	* application key obtained from the OUYA developers website.
	*/
	protected static byte[] m_applicationKey = null;
	public static byte[] GetApplicationKey()
	{
		return m_applicationKey;
	}
	public static void SetApplicationKey(byte[] applicationKey)
	{
		m_applicationKey = applicationKey;
	}
}