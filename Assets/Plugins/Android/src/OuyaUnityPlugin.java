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
import android.content.Intent;
import android.util.Log;
import android.view.WindowManager;
import android.widget.FrameLayout;

import java.io.File;

import com.unity3d.player.UnityPlayer;

import tv.ouya.console.api.*;

public class OuyaUnityPlugin
{
	private static final String LOG_TAG = "OuyaUnityPlugin";

	// The plugin has an instance of the OuyaFacade
	private static UnityOuyaFacade m_unityOuyaFacade = null;

	// Unity sets the developer id
	// Unity maybe set the iap test mode
	private static Boolean m_unityInitialized = false;
	public static Boolean isUnityInitialized()
	{
		return m_unityInitialized;
	}
	public static void unityInitialized()
	{
		m_unityInitialized = true;
		InitializeUnityOuyaFacade();
	}

	// java needs to tell unity that the UnityOuyaFacade has been
	// initialized
	private static Boolean m_pluginAwake = false;
	public static Boolean isPluginAwake()
	{
		return m_pluginAwake;
	}

	// the developer id is sent from Unity
	private static String m_developerId = "";

	// For debugging enable logging for testing
	private static Boolean m_enableDebugLogging = true;

	// This initializes the Unity plugin - our OuyaUnityPlugin,
	// and it gets a generic reference to the activity
	public OuyaUnityPlugin(Activity currentActivity)
	{
		Log.i(LOG_TAG, "OuyaUnityPlugin: Plugin is awake");

		m_pluginAwake = true;
	}

	public static void putGameData(String key, String val)
	{
		//Log.i(LOG_TAG, "OuyaUnityPlugin.putGameData: key=" + key + " val=" + val);

		if (null == m_unityOuyaFacade)
		{
			Log.i(LOG_TAG, "OuyaUnityPlugin.putGameData: m_unityOuyaFacade is null");
		}
		else
		{
			//Log.i(LOG_TAG, "OuyaUnityPlugin.putGameData: m_unityOuyaFacade is valid");
			m_unityOuyaFacade.putGameData(key, val);
		}
	}

	public static String getGameData(String key)
	{
		//Log.i(LOG_TAG, "OuyaUnityPlugin.getGameData");

		if (null == m_unityOuyaFacade)
		{
			Log.i(LOG_TAG, "OuyaUnityPlugin.getGameData: m_unityOuyaFacade is null");
			return "";
		}
		else
		{
			//Log.i(LOG_TAG, "OuyaUnityPlugin.getGameData: m_unityOuyaFacade is valid");
			return m_unityOuyaFacade.getGameData(key);
		}
	}

	// most of the java functions that are called, need the ouya facade initialized
	private static void InitializeUnityOuyaFacade()
	{
		try
		{
			// check if the ouya facade is constructed
			if (null == m_unityOuyaFacade)
			{
				if (m_enableDebugLogging)
				{
					Log.i(LOG_TAG, "OuyaUnityPlugin.InitializeTest: m_unityOuyaFacade is null");
				}
			}

			if (null == IOuyaActivity.GetUnityPlayer())
			{
				Log.i(LOG_TAG, "OuyaUnityPlugin.InitializeTest: UnityPlayer is null");
				return;
			}

			if (null == IOuyaActivity.GetActivity())
			{
				if (m_enableDebugLogging)
				{
					Log.i(LOG_TAG, "OuyaUnityPlugin.InitializeTest: IOuyaActivity.GetActivity() is null");
				}
				return;
			}

			if (null == m_unityOuyaFacade)
			{
				//wait to read the application key
				if (null == IOuyaActivity.GetApplicationKey() ||

					//wait for Unity to initialize
					!m_unityInitialized)
				{
					if (m_enableDebugLogging)
					{
						if (m_developerId.equals(""))
						{
							Log.i(LOG_TAG, "InitializeTest: m_developerId is empty, requesting init");
						}
						else
						{
							Log.i(LOG_TAG, "InitializeTest: m_developerId is set, requesting init");
						}
					}

					Log.i(LOG_TAG, "OuyaUnityPlugin.InitializeTest: OuyaGameObject send RequestUnityAwake");
					IOuyaActivity.GetUnityPlayer().UnitySendMessage("OuyaGameObject", "RequestUnityAwake", "");
				}
				else
				{
					if (m_enableDebugLogging)
					{
						Log.i(LOG_TAG, "InitializeTest: Unity has initialized,  constructing TestOuyaFacade");
					}

					/*
					if (null == IOuyaActivity.GetSavedInstanceState())
					{
						Log.i(LOG_TAG, "InitializeTest: IOuyaActivity.GetSavedInstanceState() == null");
					}
					else
					{
						Log.i(LOG_TAG, "InitializeTest: m_developerId is valid,  constructing TestOuyaFacade");
						m_unityOuyaFacade = new TestOuyaFacade(IOuyaActivity.GetActivity(), IOuyaActivity.GetSavedInstanceState(), m_developerId, IOuyaActivity.GetApplicationKey());
						IOuyaActivity.SetTestOuyaFacade(m_unityOuyaFacade);
					}
					*/

					m_unityOuyaFacade = new UnityOuyaFacade(IOuyaActivity.GetActivity(), IOuyaActivity.GetSavedInstanceState(), m_developerId, IOuyaActivity.GetApplicationKey());

					//make facade accessible by activity
					IOuyaActivity.SetUnityOuyaFacade(m_unityOuyaFacade);

					Log.i(LOG_TAG, "OuyaUnityPlugin.InitializeTest: OuyaGameObject send SendIAPInitComplete");
					IOuyaActivity.GetUnityPlayer().UnitySendMessage("OuyaGameObject", "SendIAPInitComplete", "");
				}
			}
		}
		catch (Exception ex)
		{
			Log.i(LOG_TAG, "InitializeTest: InitializeTest exception: " + ex.toString());
		}
	}

	public static String setDeveloperId(String developerId)
	{
		try
		{
			Log.i(LOG_TAG, "setDeveloperId developerId: " + developerId);
			m_developerId = developerId;
		}
		catch (Exception ex)
		{
			Log.i(LOG_TAG, "setDeveloperId exception: " + ex.toString());
		}
		return "";
	}

	public static void fetchGamerInfo()
	{
		try
		{
			Log.i(LOG_TAG, "OuyaUnityPlugin.fetchGamerInfo");

			if (null == m_unityOuyaFacade)
			{
				Log.i(LOG_TAG, "OuyaUnityPlugin.fetchGamerInfo: m_unityOuyaFacade is null");
			}
			else
			{
				Log.i(LOG_TAG, "OuyaUnityPlugin.fetchGamerInfo: m_unityOuyaFacade is valid");
				m_unityOuyaFacade.fetchGamerInfo();
			}
		}
		catch (Exception ex)
		{
			Log.i(LOG_TAG, "OuyaUnityPlugin: fetchGamerInfo exception: " + ex.toString());
		}
	}

	public static void getProductsAsync()
	{
		try
		{
			Log.i(LOG_TAG, "OuyaUnityPlugin.getProductsAsync");

			if (null == m_unityOuyaFacade)
			{
				Log.i(LOG_TAG, "OuyaUnityPlugin.getProductsAsync: m_unityOuyaFacade is null");
			}
			else
			{
				Log.i(LOG_TAG, "OuyaUnityPlugin.getProductsAsync: m_unityOuyaFacade is valid");
				m_unityOuyaFacade.requestProducts();
			}
		}
		catch (Exception ex)
		{
			Log.i(LOG_TAG, "OuyaUnityPlugin: getProductsAsync exception: " + ex.toString());
		}
	}

	public static void clearGetProductList()
	{
		try
		{
			Log.i(LOG_TAG, "clearGetProductList");

			UnityOuyaFacade.PRODUCT_IDENTIFIER_LIST.clear();
		}
		catch (Exception ex)
		{
			Log.i(LOG_TAG, "clearGetProductList exception: " + ex.toString());
		}
	}

	public static void addGetProduct(String productId)
	{
		try
		{
			Log.i(LOG_TAG, "addGetProduct productId: " + productId);

			boolean found = false;
			for (Purchasable purchasable : UnityOuyaFacade.PRODUCT_IDENTIFIER_LIST)
			{
				//Log.i(LOG_TAG, "addGetProduct " + purchasable.getProductId() + "==" + productId);
				if (purchasable.getProductId().equals(productId))
				{
					//Log.i(LOG_TAG, "addGetProduct equals: " + purchasable.getProductId() + "==" + productId + "=" + purchasable.getProductId().equals(productId));
					found = true;
					break;
				}
			}
			if (found)
			{
				//Log.i(LOG_TAG, "addGetProduct found productId: " + productId);
			}
			else
			{
				//Log.i(LOG_TAG, "addGetProduct added productId: " + productId);
				Purchasable newPurchasable = new Purchasable(new String(productId));
				UnityOuyaFacade.PRODUCT_IDENTIFIER_LIST.add(newPurchasable);
			}
		}
		catch (Exception ex)
		{
			Log.i(LOG_TAG, "addGetProduct exception: " + ex.toString());
		}
	}

	public static void debugGetProductList()
	{
		try
		{
			int count = 0;
			for (Purchasable purchasable : UnityOuyaFacade.PRODUCT_IDENTIFIER_LIST)
			{
				++count;
			}
			Log.i(LOG_TAG, "debugProductList TestOuyaFacade.PRODUCT_IDENTIFIER_LIST has " + count + " elements");
			for (Purchasable purchasable : UnityOuyaFacade.PRODUCT_IDENTIFIER_LIST)
			{
				Log.i(LOG_TAG, "debugProductList TestOuyaFacade.PRODUCT_IDENTIFIER_LIST has: " + purchasable.getProductId());
			}
		}
		catch (Exception ex)
		{
			Log.i(LOG_TAG, "debugProductList exception: " + ex.toString());
		}
	}

	public static String requestPurchaseAsync(String sku)
	{
		try
		{
			Log.i(LOG_TAG, "requestPurchaseAsync sku: " + sku);

			if (null == m_unityOuyaFacade)
			{
				Log.i(LOG_TAG, "OuyaUnityPlugin.requestPurchaseAsync: m_unityOuyaFacade is null");
			}
			else
			{
				Log.i(LOG_TAG, "OuyaUnityPlugin.getReceiptsAsync: m_unityOuyaFacade is valid");
				Product product = new Product(sku, "", 0, 0, "", 0, 0, "", "", Product.Type.ENTITLEMENT);
				m_unityOuyaFacade.requestPurchase(product);
			}
		}
		catch (Exception ex)
		{
			Log.i(LOG_TAG, "requestPurchaseAsync exception: " + ex.toString());
		}
		return "";
	}

	public static void getReceiptsAsync()
	{
		try
		{
			Log.i(LOG_TAG, "OuyaUnityPlugin.getReceiptsAsync");

			if (null == m_unityOuyaFacade)
			{
				Log.i(LOG_TAG, "OuyaUnityPlugin.getReceiptsAsync: m_unityOuyaFacade is null");
			}
			else
			{
				Log.i(LOG_TAG, "OuyaUnityPlugin.getReceiptsAsync: m_unityOuyaFacade is valid");
				m_unityOuyaFacade.requestReceipts();
			}
		}
		catch (Exception ex)
		{
			Log.i(LOG_TAG, "OuyaUnityPlugin: getProductsAsync exception: " + ex.toString());
		}
	}

	public static boolean isRunningOnOUYASupportedHardware()
	{
		boolean result = false;
		try
		{
			//Log.i(LOG_TAG, "OuyaUnityPlugin.isRunningOnOUYASupportedHardware");
			if (null == m_unityOuyaFacade)
			{
				Log.i(LOG_TAG, "OuyaUnityPlugin.isRunningOnOUYASupportedHardware: m_unityOuyaFacade is null");
			}
			else
			{
				//Log.i(LOG_TAG, "OuyaUnityPlugin.isRunningOnOUYASupportedHardware: m_unityOuyaFacade is valid");
				result = m_unityOuyaFacade.isRunningOnOUYASupportedHardware();
			}
		}
		catch (Exception ex)
		{
			Log.i(LOG_TAG, "OuyaUnityPlugin: isRunningOnOUYASupportedHardware exception: " + ex.toString());
		}
		return result;
	}
}