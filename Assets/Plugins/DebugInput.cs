#if UNITY_ANDROID && !UNITY_EDITOR

using System;
using System.Collections.Generic;
using tv.ouya.console.api;

namespace tv.ouya.console.api
{
    public class DebugInput
    {
        public static string DebugGetAxisName(int axis)
        {
            Dictionary<int, string> names = new Dictionary<int, string>();
            names[OuyaController.AXIS_LS_X] = "AXIS_LS_X";
            names[OuyaController.AXIS_LS_Y] = "AXIS_LS_Y";
            names[OuyaController.AXIS_RS_X] = "AXIS_RS_X";
            names[OuyaController.AXIS_RS_Y] = "AXIS_RS_Y";
            names[OuyaController.AXIS_L2] = "AXIS_L2";
            names[OuyaController.AXIS_R2] = "AXIS_R2";

            if (names.ContainsKey(axis))
            {
                return names[axis];
            }
            else
            {
                return string.Empty;
            }
        }

        public static String DebugGetButtonName(int button)
        {
            Dictionary<int, string> names = new Dictionary<int, string>();
            names[OuyaController.BUTTON_O] = "BUTTON_O";
            names[OuyaController.BUTTON_U] = "BUTTON_U";
            names[OuyaController.BUTTON_Y] = "BUTTON_Y";
            names[OuyaController.BUTTON_A] = "BUTTON_A";
            names[OuyaController.BUTTON_L1] = "BUTTON_L1";
            names[OuyaController.BUTTON_R1] = "BUTTON_R1";
            names[OuyaController.BUTTON_L3] = "BUTTON_L3";
            names[OuyaController.BUTTON_R3] = "BUTTON_R3";
            names[OuyaController.BUTTON_DPAD_UP] = "BUTTON_DPAD_UP";
            names[OuyaController.BUTTON_DPAD_DOWN] = "BUTTON_DPAD_DOWN";
            names[OuyaController.BUTTON_DPAD_RIGHT] = "BUTTON_DPAD_RIGHT";
            names[OuyaController.BUTTON_DPAD_LEFT] = "BUTTON_DPAD_LEFT";
            names[OuyaController.BUTTON_MENU] = "BUTTON_MENU";

            if (names.ContainsKey(button))
            {
                return names[button];
            }
            else
            {
                return string.Empty;
            }
        }
    }
}

#endif