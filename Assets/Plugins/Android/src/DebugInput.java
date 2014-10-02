package tv.ouya.console.api;

import tv.ouya.console.api.OuyaController;
import android.util.Log;
import android.util.SparseArray;
import android.view.MotionEvent;

public class DebugInput {
	private static final String TAG = DebugInput.class.getSimpleName();
	
	public static String debugGetAxisName(int axis) {
		SparseArray<String> names = new SparseArray<String>();
        names.append(MotionEvent.AXIS_X, "AXIS_X");
        names.append(MotionEvent.AXIS_Y, "AXIS_Y");
        names.append(MotionEvent.AXIS_PRESSURE, "AXIS_PRESSURE");
        names.append(MotionEvent.AXIS_SIZE, "AXIS_SIZE");
        names.append(MotionEvent.AXIS_TOUCH_MAJOR, "AXIS_TOUCH_MAJOR");
        names.append(MotionEvent.AXIS_TOUCH_MINOR, "AXIS_TOUCH_MINOR");
        names.append(MotionEvent.AXIS_TOOL_MAJOR, "AXIS_TOOL_MAJOR");
        names.append(MotionEvent.AXIS_TOOL_MINOR, "AXIS_TOOL_MINOR");
        names.append(MotionEvent.AXIS_ORIENTATION, "AXIS_ORIENTATION");
        names.append(MotionEvent.AXIS_VSCROLL, "AXIS_VSCROLL");
        names.append(MotionEvent.AXIS_HSCROLL, "AXIS_HSCROLL");
        names.append(MotionEvent.AXIS_Z, "AXIS_Z");
        names.append(MotionEvent.AXIS_RX, "AXIS_RX");
        names.append(MotionEvent.AXIS_RY, "AXIS_RY");
        names.append(MotionEvent.AXIS_RZ, "AXIS_RZ");
        names.append(MotionEvent.AXIS_HAT_X, "AXIS_HAT_X");
        names.append(MotionEvent.AXIS_HAT_Y, "AXIS_HAT_Y");
        names.append(MotionEvent.AXIS_LTRIGGER, "AXIS_LTRIGGER");
        names.append(MotionEvent.AXIS_RTRIGGER, "AXIS_RTRIGGER");
        names.append(MotionEvent.AXIS_THROTTLE, "AXIS_THROTTLE");
        names.append(MotionEvent.AXIS_RUDDER, "AXIS_RUDDER");
        names.append(MotionEvent.AXIS_WHEEL, "AXIS_WHEEL");
        names.append(MotionEvent.AXIS_GAS, "AXIS_GAS");
        names.append(MotionEvent.AXIS_BRAKE, "AXIS_BRAKE");
        names.append(MotionEvent.AXIS_DISTANCE, "AXIS_DISTANCE");
        names.append(MotionEvent.AXIS_TILT, "AXIS_TILT");
        names.append(MotionEvent.AXIS_GENERIC_1, "AXIS_GENERIC_1");
        names.append(MotionEvent.AXIS_GENERIC_2, "AXIS_GENERIC_2");
        names.append(MotionEvent.AXIS_GENERIC_3, "AXIS_GENERIC_3");
        names.append(MotionEvent.AXIS_GENERIC_4, "AXIS_GENERIC_4");
        names.append(MotionEvent.AXIS_GENERIC_5, "AXIS_GENERIC_5");
        names.append(MotionEvent.AXIS_GENERIC_6, "AXIS_GENERIC_6");
        names.append(MotionEvent.AXIS_GENERIC_7, "AXIS_GENERIC_7");
        names.append(MotionEvent.AXIS_GENERIC_8, "AXIS_GENERIC_8");
        names.append(MotionEvent.AXIS_GENERIC_9, "AXIS_GENERIC_9");
        names.append(MotionEvent.AXIS_GENERIC_10, "AXIS_GENERIC_10");
        names.append(MotionEvent.AXIS_GENERIC_11, "AXIS_GENERIC_11");
        names.append(MotionEvent.AXIS_GENERIC_12, "AXIS_GENERIC_12");
        names.append(MotionEvent.AXIS_GENERIC_13, "AXIS_GENERIC_13");
        names.append(MotionEvent.AXIS_GENERIC_14, "AXIS_GENERIC_14");
        names.append(MotionEvent.AXIS_GENERIC_15, "AXIS_GENERIC_15");
        names.append(MotionEvent.AXIS_GENERIC_16, "AXIS_GENERIC_16");
        
        String axisName = names.get(axis);
        if (null == axisName) {
        	return "";
        } else {
        	return axisName;
        }
	}
	
	public int debugGetKeyCode(String name) {
		if (name.equals("BUTTON_O")) {
			return OuyaController.BUTTON_O;
		} else if (name.equals("BUTTON_U")) {
			return OuyaController.BUTTON_U;
		} else if (name.equals("BUTTON_Y")) {
			return OuyaController.BUTTON_Y;
		} else if (name.equals("BUTTON_A")) {
			return OuyaController.BUTTON_A;
		} else if (name.equals("BUTTON_L1")) {
			return OuyaController.BUTTON_L1;
		} else if (name.equals("BUTTON_R1")) {
			return OuyaController.BUTTON_R1;
		} else if (name.equals("BUTTON_L3")) {
			return OuyaController.BUTTON_L3;
		} else if (name.equals("BUTTON_R3")) {
			return OuyaController.BUTTON_R3;
		} else if (name.equals("BUTTON_DPAD_UP")) {
			return OuyaController.BUTTON_DPAD_UP;
		} else if (name.equals("BUTTON_DPAD_DOWN")) {
			return OuyaController.BUTTON_DPAD_DOWN;
		} else if (name.equals("BUTTON_DPAD_RIGHT")) {
			return OuyaController.BUTTON_DPAD_RIGHT;
		} else if (name.equals("BUTTON_DPAD_LEFT")) {
			return OuyaController.BUTTON_DPAD_LEFT;
		} else if (name.equals("BUTTON_MENU")) {
			return OuyaController.BUTTON_MENU;
		} else {
			return 0;
		}
	}
	
	public static String debugGetButtonName(int button) {
		SparseArray<String> names = new SparseArray<String>();
        names.append(OuyaController.BUTTON_O, "BUTTON_O");
        names.append(OuyaController.BUTTON_U, "BUTTON_U");
        names.append(OuyaController.BUTTON_Y, "BUTTON_Y");
        names.append(OuyaController.BUTTON_A, "BUTTON_A");
        names.append(OuyaController.BUTTON_L1, "BUTTON_L1");
        names.append(OuyaController.BUTTON_R1, "BUTTON_R1");
        names.append(OuyaController.BUTTON_L3, "BUTTON_L3");
        names.append(OuyaController.BUTTON_R3, "BUTTON_R3");
        names.append(OuyaController.BUTTON_DPAD_UP, "BUTTON_DPAD_UP");
        names.append(OuyaController.BUTTON_DPAD_DOWN, "BUTTON_DPAD_DOWN");
        names.append(OuyaController.BUTTON_DPAD_RIGHT, "BUTTON_DPAD_RIGHT");
        names.append(OuyaController.BUTTON_DPAD_LEFT, "BUTTON_DPAD_LEFT");
        names.append(OuyaController.BUTTON_MENU, "BUTTON_MENU");
        
        String buttonName = names.get(button);
        if (null == buttonName) {
        	return "";
        } else {
        	return buttonName;
        }
	}

	public static void debugOuyaMotionEvent(MotionEvent motionEvent) {
		Log.i(TAG, "("+OuyaController.AXIS_LS_X + ") OuyaController.AXIS_LS_X value="+motionEvent.getAxisValue(OuyaController.AXIS_LS_X));
		Log.i(TAG, "("+OuyaController.AXIS_LS_Y + ") OuyaController.AXIS_LS_Y value="+motionEvent.getAxisValue(OuyaController.AXIS_LS_Y));
		Log.i(TAG, "("+OuyaController.AXIS_RS_X + ") OuyaController.AXIS_RS_X value="+motionEvent.getAxisValue(OuyaController.AXIS_RS_X));
		Log.i(TAG, "("+OuyaController.AXIS_RS_Y + ") OuyaController.AXIS_RS_Y value="+motionEvent.getAxisValue(OuyaController.AXIS_RS_Y));
		Log.i(TAG, "("+OuyaController.AXIS_L2 + ") OuyaController.AXIS_L2 value="+motionEvent.getAxisValue(OuyaController.AXIS_L2));
		Log.i(TAG, "("+OuyaController.AXIS_R2 + ") OuyaController.AXIS_R2 value="+motionEvent.getAxisValue(OuyaController.AXIS_R2));
	}
	
	public static void debugMotionEvent(MotionEvent motionEvent) {
		SparseArray<String> names = new SparseArray<String>();
        names.append(MotionEvent.AXIS_X, "AXIS_X");
        names.append(MotionEvent.AXIS_Y, "AXIS_Y");
        names.append(MotionEvent.AXIS_PRESSURE, "AXIS_PRESSURE");
        names.append(MotionEvent.AXIS_SIZE, "AXIS_SIZE");
        names.append(MotionEvent.AXIS_TOUCH_MAJOR, "AXIS_TOUCH_MAJOR");
        names.append(MotionEvent.AXIS_TOUCH_MINOR, "AXIS_TOUCH_MINOR");
        names.append(MotionEvent.AXIS_TOOL_MAJOR, "AXIS_TOOL_MAJOR");
        names.append(MotionEvent.AXIS_TOOL_MINOR, "AXIS_TOOL_MINOR");
        names.append(MotionEvent.AXIS_ORIENTATION, "AXIS_ORIENTATION");
        names.append(MotionEvent.AXIS_VSCROLL, "AXIS_VSCROLL");
        names.append(MotionEvent.AXIS_HSCROLL, "AXIS_HSCROLL");
        names.append(MotionEvent.AXIS_Z, "AXIS_Z");
        names.append(MotionEvent.AXIS_RX, "AXIS_RX");
        names.append(MotionEvent.AXIS_RY, "AXIS_RY");
        names.append(MotionEvent.AXIS_RZ, "AXIS_RZ");
        names.append(MotionEvent.AXIS_HAT_X, "AXIS_HAT_X");
        names.append(MotionEvent.AXIS_HAT_Y, "AXIS_HAT_Y");
        names.append(MotionEvent.AXIS_LTRIGGER, "AXIS_LTRIGGER");
        names.append(MotionEvent.AXIS_RTRIGGER, "AXIS_RTRIGGER");
        names.append(MotionEvent.AXIS_THROTTLE, "AXIS_THROTTLE");
        names.append(MotionEvent.AXIS_RUDDER, "AXIS_RUDDER");
        names.append(MotionEvent.AXIS_WHEEL, "AXIS_WHEEL");
        names.append(MotionEvent.AXIS_GAS, "AXIS_GAS");
        names.append(MotionEvent.AXIS_BRAKE, "AXIS_BRAKE");
        names.append(MotionEvent.AXIS_DISTANCE, "AXIS_DISTANCE");
        names.append(MotionEvent.AXIS_TILT, "AXIS_TILT");
        names.append(MotionEvent.AXIS_GENERIC_1, "AXIS_GENERIC_1");
        names.append(MotionEvent.AXIS_GENERIC_2, "AXIS_GENERIC_2");
        names.append(MotionEvent.AXIS_GENERIC_3, "AXIS_GENERIC_3");
        names.append(MotionEvent.AXIS_GENERIC_4, "AXIS_GENERIC_4");
        names.append(MotionEvent.AXIS_GENERIC_5, "AXIS_GENERIC_5");
        names.append(MotionEvent.AXIS_GENERIC_6, "AXIS_GENERIC_6");
        names.append(MotionEvent.AXIS_GENERIC_7, "AXIS_GENERIC_7");
        names.append(MotionEvent.AXIS_GENERIC_8, "AXIS_GENERIC_8");
        names.append(MotionEvent.AXIS_GENERIC_9, "AXIS_GENERIC_9");
        names.append(MotionEvent.AXIS_GENERIC_10, "AXIS_GENERIC_10");
        names.append(MotionEvent.AXIS_GENERIC_11, "AXIS_GENERIC_11");
        names.append(MotionEvent.AXIS_GENERIC_12, "AXIS_GENERIC_12");
        names.append(MotionEvent.AXIS_GENERIC_13, "AXIS_GENERIC_13");
        names.append(MotionEvent.AXIS_GENERIC_14, "AXIS_GENERIC_14");
        names.append(MotionEvent.AXIS_GENERIC_15, "AXIS_GENERIC_15");
        names.append(MotionEvent.AXIS_GENERIC_16, "AXIS_GENERIC_16");
        
        int source = motionEvent.getSource();
        
        final int count = names.size();
        for (int i = 0; i < count; i++) {
        	float val = motionEvent.getAxisValue(names.keyAt(i));
            Log.i(TAG, "(" + names.keyAt(i) + ") "+ names.valueAt(i) + "=" + val + " source="+source);
        }
	}
}
