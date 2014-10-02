#include <jni.h>
#include <android/log.h>

#include <map>
#include <string>
#include <vector>

#define trace(fmt, ...) __android_log_print(ANDROID_LOG_DEBUG, "JNI", "trace: %s (%i) " fmt, __FUNCTION__, __LINE__, __VA_ARGS__)

#define LOG_TAG "jni.cpp"

#define MAX_CONTROLLERS 4

//axis states
static std::vector< std::map<int, float> > g_axis;

//button states
static std::vector< std::map<int, bool> > g_button;
static std::vector< std::map<int, bool> > g_buttonDown;
static std::vector< std::map<int, bool> > g_buttonUp;

void dispatchGenericMotionEventNative(JNIEnv* env, jobject thiz,
	jint deviceId,
	jint axis,
	jfloat val)
{
	//__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "Device=%d axis=%d val=%f", deviceId, axis, val);
	if (deviceId < 0 ||
		deviceId >= MAX_CONTROLLERS)
	{
		return;
	}
	g_axis[deviceId][axis] = val;
}

void dispatchKeyEventNative(JNIEnv* env, jobject thiz,
	jint deviceId,
	jint keyCode,
	jint action)
{
	//__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "Device=%d KeyCode=%d Action=%d", deviceId, keyCode, action);
	if (deviceId < 0 ||
		deviceId >= MAX_CONTROLLERS)
	{
		return;
	}

	bool buttonDown = action == 0;

	if (g_button[deviceId][keyCode] != buttonDown)
	{
		g_button[deviceId][keyCode] = buttonDown;
		if (buttonDown)
		{
			g_buttonDown[deviceId][keyCode] = true;
		}
		else
		{
			g_buttonUp[deviceId][keyCode] = true;
		}
	}	
}

static JNINativeMethod method_table[] = {
	{ "dispatchGenericMotionEventNative", "(IIF)V", (void *)dispatchGenericMotionEventNative }
};

static int method_table_size = sizeof(method_table) / sizeof(method_table[0]);

static JNINativeMethod method_table2[] = {
	{ "dispatchKeyEventNative", "(III)V", (void *)dispatchKeyEventNative }
};

static int method_table_size2 = sizeof(method_table2) / sizeof(method_table2[0]);

jint JNI_OnLoad(JavaVM* vm, void* reserved)
{
	__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "JNI_OnLoad");

	for (int index = 0; index < MAX_CONTROLLERS; ++index)
	{
		g_axis.push_back(std::map<int, float>());
		g_button.push_back(std::map<int, bool>());
		g_buttonDown.push_back(std::map<int, bool>());
		g_buttonUp.push_back(std::map<int, bool>());
	}

	JNIEnv* env;
	if (vm->GetEnv(reinterpret_cast<void**>(&env), JNI_VERSION_1_6) != JNI_OK)
	{
		return JNI_ERR;
	}

	jclass clazz = env->FindClass("tv/ouya/sdk/OuyaUnityActivity");
	if (clazz)
	{
		jint ret = env->RegisterNatives(clazz, method_table, method_table_size);
		ret = env->RegisterNatives(clazz, method_table2, method_table_size2);
		env->DeleteLocalRef(clazz);
	}
	else
	{
		__android_log_print(ANDROID_LOG_INFO, LOG_TAG, "Failed to find OuyaUnityActivity");
		return JNI_ERR;
	}

	return JNI_VERSION_1_6;
}

extern "C"
{
	// Hello world interface
	char* AndroidGetHelloWorld(long* size)
	{
		const char* cString = "Hello World!\0";
		*size = strlen(cString);
		char* result = new char[*cString];
		strcpy(result, cString);
		return result;
	}

	// Release unmanaged memory
	void AndroidReleaseMemory(char* buffer)
	{
		if (NULL == buffer)
		{
			return;
		}

		delete buffer;
	}

	// Example interface
	void AndroidExampleFunction1(unsigned char* a, int b, int* c)
	{
		(*c) = 3;
	}

	// get axis value
	float getAxis(int deviceId, int axis)
	{
		if (deviceId < 0 ||
			deviceId >= MAX_CONTROLLERS)
		{
			return 0.0f;
		}

		std::map<int, float>::const_iterator search = g_axis[deviceId].find(axis);
		if (search != g_axis[deviceId].end())
		{
			return search->second;
		}
		return 0.0f;
	}

	// check if a button is pressed
	bool isPressed(int deviceId, int keyCode)
	{
		if (deviceId < 0 ||
			deviceId >= MAX_CONTROLLERS)
		{
			return false;
		}

		std::map<int, bool>::const_iterator search = g_button[deviceId].find(keyCode);
		if (search != g_button[deviceId].end())
		{
			return search->second;
		}
		return false;
	}

	// check if a button was down
	bool isPressedDown(int deviceId, int keyCode)
	{
		if (deviceId < 0 ||
			deviceId >= MAX_CONTROLLERS)
		{
			return false;
		}

		std::map<int, bool>::const_iterator search = g_buttonDown[deviceId].find(keyCode);
		if (search != g_buttonDown[deviceId].end())
		{
			return search->second;
		}
		return false;
	}

	// check if a button was up
	bool isPressedUp(int deviceId, int keyCode)
	{
		if (deviceId < 0 ||
			deviceId >= MAX_CONTROLLERS)
		{
			return false;
		}

		std::map<int, bool>::const_iterator search = g_buttonUp[deviceId].find(keyCode);
		if (search != g_buttonUp[deviceId].end())
		{
			return search->second;
		}
		return false;
	}

	// clear the button state for detecting up and down
	void clearButtonStates()
	{
		for (int deviceId = 0; deviceId < MAX_CONTROLLERS; ++deviceId)
		{
			g_buttonDown[deviceId].clear();
			g_buttonUp[deviceId].clear();
		}
	}
}
