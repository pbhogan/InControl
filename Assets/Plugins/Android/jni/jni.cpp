#include <jni.h>
#include <string.h>
#include <android/log.h>
#define trace(fmt, ...) __android_log_print(ANDROID_LOG_DEBUG, "JNI", "trace: %s (%i) " fmt, __FUNCTION__, __LINE__, __VA_ARGS__)

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
}
