include $(CLEAR_VARS)

# override strip command to strip all symbols from output library; no need to ship with those..
cmd-strip = $(TOOLCHAIN_PREFIX)strip $1 

LOCAL_ARM_MODE  := arm
LOCAL_PATH      := $(NDK_PROJECT_PATH)/jni
LOCAL_MODULE    := -ouya-ndk
LOCAL_CFLAGS    := -Werror
LOCAL_SRC_FILES := jni.cpp
LOCAL_LDLIBS    := -llog

include $(BUILD_SHARED_LIBRARY)
