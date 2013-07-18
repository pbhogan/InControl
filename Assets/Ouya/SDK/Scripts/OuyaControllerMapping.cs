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

using System.Collections.Generic;


public enum AnalogTypes { Left, Right, DPad, LTRT, none }
public enum AxisTypes { Horizontal, Vertical, DPadH, DPadV, none }
public enum JoystickType {ps3,ps2,xbox,ouya,usb,none}


#region Controller Type

// used for when you are getting the axis

public class ControllerType{
	public string name;
	public int leftAnalogH;
	public int leftAnalogV;
	public int rightAnalogH;
	public int rightAnalogV;
	public int dpadH;
	public int dpadV;
	public int triggers = -1;
}

#endregion

#region Button Maps

// how when you push a button

public class ButtonMap{
	public JoystickType type;
	public int ButtonID;
	public OuyaSDK.KeyEnum button;
	
	public ButtonMap (JoystickType type, int buttonID, OuyaSDK.KeyEnum button)
	{
		this.type = type;
		this.ButtonID = buttonID;
		this.button = button;
	}
}

#endregion

public static class OuyaControllerMapping
{
    /// <summary>
    /// Dictionary of controller mappings
    /// </summary>
	private static Dictionary<string, ControllerType> controllers;	
	
    /// <summary>
    /// List of button mappings
    /// </summary>
    private static List<ButtonMap> buttonMap;
	
    /// <summary>
    /// Get a controller by name
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
	public static ControllerType getController(string key){
        if (OuyaControllerMapping.controllers.ContainsKey(key))
        {
            return OuyaControllerMapping.controllers[key];
        }
        else
        {
            return new ControllerType();
        }
	}
	
    /// <summary>
    /// Static constructor
    /// </summary>
    static OuyaControllerMapping()
    {
		controllers = new Dictionary<string, ControllerType>();
		buttonMap = new List<ButtonMap>();
        JoystickType joystickType = JoystickType.none;

        #region PS3 mapping

        ControllerType ps3 = new ControllerType();
		ps3.name = "ps3";
		ps3.leftAnalogH = 1;
		ps3.leftAnalogV = 2;
		ps3.rightAnalogH = 3; //4 was gyro
		ps3.rightAnalogV = 6; //5 was gyro
		ps3.dpadH = 9;
		ps3.dpadV = 10;
        joystickType = JoystickType.ps3;
		
		//Start PS3 ButtonMap ( only has 13 buttons )
        buttonMap.Add(new ButtonMap(joystickType, 350, OuyaSDK.KeyEnum.BUTTON_Y));
        buttonMap.Add(new ButtonMap(joystickType, 351, OuyaSDK.KeyEnum.BUTTON_A));
        buttonMap.Add(new ButtonMap(joystickType, 352, OuyaSDK.KeyEnum.BUTTON_O));
        buttonMap.Add(new ButtonMap(joystickType, 353, OuyaSDK.KeyEnum.BUTTON_U));

        buttonMap.Add(new ButtonMap(joystickType, 354, OuyaSDK.KeyEnum.BUTTON_LB));
        buttonMap.Add(new ButtonMap(joystickType, 355, OuyaSDK.KeyEnum.BUTTON_RB));
        buttonMap.Add(new ButtonMap(joystickType, 356, OuyaSDK.KeyEnum.BUTTON_LT));
        buttonMap.Add(new ButtonMap(joystickType, 357, OuyaSDK.KeyEnum.BUTTON_RT));
        buttonMap.Add(new ButtonMap(joystickType, 358, OuyaSDK.KeyEnum.BUTTON_SELECT));
        buttonMap.Add(new ButtonMap(joystickType, 359, OuyaSDK.KeyEnum.BUTTON_L3));
        buttonMap.Add(new ButtonMap(joystickType, 360, OuyaSDK.KeyEnum.BUTTON_R3));
        buttonMap.Add(new ButtonMap(joystickType, 361, OuyaSDK.KeyEnum.BUTTON_START));
        buttonMap.Add(new ButtonMap(joystickType, 362, OuyaSDK.KeyEnum.BUTTON_SYSTEM));
		
		//4 button dpad mappings
        buttonMap.Add(new ButtonMap(joystickType, 363, OuyaSDK.KeyEnum.BUTTON_DPAD_LEFT));
        buttonMap.Add(new ButtonMap(joystickType, 364, OuyaSDK.KeyEnum.BUTTON_DPAD_RIGHT));
        buttonMap.Add(new ButtonMap(joystickType, 365, OuyaSDK.KeyEnum.BUTTON_DPAD_UP));
        buttonMap.Add(new ButtonMap(joystickType, 366, OuyaSDK.KeyEnum.BUTTON_DPAD_DOWN));

		controllers.Add(ps3.name,ps3);

        #endregion

        #region XBOX mapping

        ControllerType xbox360 = new ControllerType();
		xbox360.name="xbox360";
		xbox360.leftAnalogH = 1;
		xbox360.leftAnalogV = 2;
		xbox360.rightAnalogH = 4;
		xbox360.rightAnalogV = 5;
		xbox360.triggers = 3;
		xbox360.dpadH = 6;
		xbox360.dpadV = 7;
        joystickType = JoystickType.xbox;

        #region Values are different on windows, sometimes they are these

        //Start XBOX 360 ButtonMap ( only has 13 buttons )
        buttonMap.Add(new ButtonMap(joystickType, 350, OuyaSDK.KeyEnum.BUTTON_O));
        buttonMap.Add(new ButtonMap(joystickType, 351, OuyaSDK.KeyEnum.BUTTON_A));
        buttonMap.Add(new ButtonMap(joystickType, 352, OuyaSDK.KeyEnum.BUTTON_U));
        buttonMap.Add(new ButtonMap(joystickType, 353, OuyaSDK.KeyEnum.BUTTON_Y));

        buttonMap.Add(new ButtonMap(joystickType, 354, OuyaSDK.KeyEnum.BUTTON_LB));
        buttonMap.Add(new ButtonMap(joystickType, 355, OuyaSDK.KeyEnum.BUTTON_RB));
        buttonMap.Add(new ButtonMap(joystickType, 356, OuyaSDK.KeyEnum.BUTTON_SELECT));
        buttonMap.Add(new ButtonMap(joystickType, 357, OuyaSDK.KeyEnum.BUTTON_START));
        buttonMap.Add(new ButtonMap(joystickType, 358, OuyaSDK.KeyEnum.BUTTON_L3));
        buttonMap.Add(new ButtonMap(joystickType, 359, OuyaSDK.KeyEnum.BUTTON_R3));
        buttonMap.Add(new ButtonMap(joystickType, 10, OuyaSDK.KeyEnum.BUTTON_LT)); //Doesn't Show up
        buttonMap.Add(new ButtonMap(joystickType, 11, OuyaSDK.KeyEnum.BUTTON_RT)); //Doesn't Show up
        buttonMap.Add(new ButtonMap(joystickType, 12, OuyaSDK.KeyEnum.BUTTON_SYSTEM)); //Dowsn't Show up

        //4 button dpad mappings
        buttonMap.Add(new ButtonMap(joystickType, 13, OuyaSDK.KeyEnum.BUTTON_DPAD_LEFT));
        buttonMap.Add(new ButtonMap(joystickType, 14, OuyaSDK.KeyEnum.BUTTON_DPAD_RIGHT));
        buttonMap.Add(new ButtonMap(joystickType, 15, OuyaSDK.KeyEnum.BUTTON_DPAD_UP));
        buttonMap.Add(new ButtonMap(joystickType, 16, OuyaSDK.KeyEnum.BUTTON_DPAD_DOWN));

        #endregion

        #region And sometimes they are these, just on windows

#if UNITY_EDITOR || UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX

        //Start XBOX 360 ButtonMap ( only has 13 buttons )
        buttonMap.Add(new ButtonMap(joystickType, 370, OuyaSDK.KeyEnum.BUTTON_O));
        buttonMap.Add(new ButtonMap(joystickType, 371, OuyaSDK.KeyEnum.BUTTON_A));
        buttonMap.Add(new ButtonMap(joystickType, 372, OuyaSDK.KeyEnum.BUTTON_U));
        buttonMap.Add(new ButtonMap(joystickType, 373, OuyaSDK.KeyEnum.BUTTON_Y));

        buttonMap.Add(new ButtonMap(joystickType, 374, OuyaSDK.KeyEnum.BUTTON_LB));
        buttonMap.Add(new ButtonMap(joystickType, 375, OuyaSDK.KeyEnum.BUTTON_RB));
        buttonMap.Add(new ButtonMap(joystickType, 376, OuyaSDK.KeyEnum.BUTTON_SELECT));
        buttonMap.Add(new ButtonMap(joystickType, 377, OuyaSDK.KeyEnum.BUTTON_START));
        buttonMap.Add(new ButtonMap(joystickType, 378, OuyaSDK.KeyEnum.BUTTON_L3));
        buttonMap.Add(new ButtonMap(joystickType, 379, OuyaSDK.KeyEnum.BUTTON_R3));
        buttonMap.Add(new ButtonMap(joystickType, 380, OuyaSDK.KeyEnum.BUTTON_LT)); //Doesn't Show up
        buttonMap.Add(new ButtonMap(joystickType, 381, OuyaSDK.KeyEnum.BUTTON_RT)); //Doesn't Show up
        buttonMap.Add(new ButtonMap(joystickType, 382, OuyaSDK.KeyEnum.BUTTON_SYSTEM)); //Dowsn't Show up

        //4 button dpad mappings
        buttonMap.Add(new ButtonMap(joystickType, 383, OuyaSDK.KeyEnum.BUTTON_DPAD_LEFT));
        buttonMap.Add(new ButtonMap(joystickType, 384, OuyaSDK.KeyEnum.BUTTON_DPAD_RIGHT));
        buttonMap.Add(new ButtonMap(joystickType, 385, OuyaSDK.KeyEnum.BUTTON_DPAD_UP));
        buttonMap.Add(new ButtonMap(joystickType, 386, OuyaSDK.KeyEnum.BUTTON_DPAD_DOWN));

#endif

        #endregion

        controllers.Add(xbox360.name,xbox360);

        #endregion

        #region PS2 controller mapping

        ControllerType ps2 = new ControllerType();
        ps2.name = "ps2";
        ps2.leftAnalogH = 1;
        ps2.leftAnalogV = 2;
        ps2.rightAnalogH = 3; //4 was gyro
        ps2.rightAnalogV = 6; //5 was gyro
        ps2.dpadH = 9;
        ps2.dpadV = 10;
        joystickType = JoystickType.ps2;

        //Start PS3 ButtonMap ( only has 13 buttons )
        buttonMap.Add(new ButtonMap(joystickType, 350, OuyaSDK.KeyEnum.BUTTON_Y));
        buttonMap.Add(new ButtonMap(joystickType, 351, OuyaSDK.KeyEnum.BUTTON_A));
        buttonMap.Add(new ButtonMap(joystickType, 352, OuyaSDK.KeyEnum.BUTTON_O));
        buttonMap.Add(new ButtonMap(joystickType, 373, OuyaSDK.KeyEnum.BUTTON_U));

        buttonMap.Add(new ButtonMap(joystickType, 354, OuyaSDK.KeyEnum.BUTTON_LB));
        buttonMap.Add(new ButtonMap(joystickType, 355, OuyaSDK.KeyEnum.BUTTON_RB));
        buttonMap.Add(new ButtonMap(joystickType, 356, OuyaSDK.KeyEnum.BUTTON_LT));
        buttonMap.Add(new ButtonMap(joystickType, 357, OuyaSDK.KeyEnum.BUTTON_RT));
        buttonMap.Add(new ButtonMap(joystickType, 358, OuyaSDK.KeyEnum.BUTTON_SELECT));
        buttonMap.Add(new ButtonMap(joystickType, 359, OuyaSDK.KeyEnum.BUTTON_L3));
        buttonMap.Add(new ButtonMap(joystickType, 360, OuyaSDK.KeyEnum.BUTTON_R3));
        buttonMap.Add(new ButtonMap(joystickType, 361, OuyaSDK.KeyEnum.BUTTON_START));
        buttonMap.Add(new ButtonMap(joystickType, 362, OuyaSDK.KeyEnum.BUTTON_SYSTEM));

        //4 button dpad mappings
        buttonMap.Add(new ButtonMap(joystickType, 363, OuyaSDK.KeyEnum.BUTTON_DPAD_LEFT));
        buttonMap.Add(new ButtonMap(joystickType, 364, OuyaSDK.KeyEnum.BUTTON_DPAD_RIGHT));
        buttonMap.Add(new ButtonMap(joystickType, 365, OuyaSDK.KeyEnum.BUTTON_DPAD_UP));
        buttonMap.Add(new ButtonMap(joystickType, 366, OuyaSDK.KeyEnum.BUTTON_DPAD_DOWN));

        controllers.Add(ps2.name, ps2);

        #endregion

        #region OUYA mapping

        ControllerType ouya = new ControllerType();
        ouya.name = "ouya";
        ouya.leftAnalogH = 1;
        ouya.leftAnalogV = 2;
        ouya.rightAnalogH = 4;
        ouya.rightAnalogV = 5;
        ouya.triggers = 6;
        ouya.dpadH = 9;
        ouya.dpadV = 10;
        joystickType = JoystickType.ouya;

        #region Values are different on windows, sometimes they are these

        //Start OUYA CONTROLLER ButtonMap ( only has 11 buttons )
        buttonMap.Add(new ButtonMap(joystickType, 350, OuyaSDK.KeyEnum.BUTTON_O));
        buttonMap.Add(new ButtonMap(joystickType, 353, OuyaSDK.KeyEnum.BUTTON_A));
        buttonMap.Add(new ButtonMap(joystickType, 351, OuyaSDK.KeyEnum.BUTTON_U));
        buttonMap.Add(new ButtonMap(joystickType, 352, OuyaSDK.KeyEnum.BUTTON_Y));

        buttonMap.Add(new ButtonMap(joystickType, 354, OuyaSDK.KeyEnum.BUTTON_LB));
        buttonMap.Add(new ButtonMap(joystickType, 355, OuyaSDK.KeyEnum.BUTTON_RB));
        buttonMap.Add(new ButtonMap(joystickType, 356, OuyaSDK.KeyEnum.BUTTON_L3));
        buttonMap.Add(new ButtonMap(joystickType, 357, OuyaSDK.KeyEnum.BUTTON_R3));
        buttonMap.Add(new ButtonMap(joystickType, 362, OuyaSDK.KeyEnum.BUTTON_LT)); //Doesn't Show up
        buttonMap.Add(new ButtonMap(joystickType, 363, OuyaSDK.KeyEnum.BUTTON_RT)); //Doesn't Show up
        buttonMap.Add(new ButtonMap(joystickType, 364, OuyaSDK.KeyEnum.BUTTON_SYSTEM)); //Dowsn't Show up

        //4 button dpad mappings
        buttonMap.Add(new ButtonMap(joystickType, 360, OuyaSDK.KeyEnum.BUTTON_DPAD_LEFT));
        buttonMap.Add(new ButtonMap(joystickType, 361, OuyaSDK.KeyEnum.BUTTON_DPAD_RIGHT));
        buttonMap.Add(new ButtonMap(joystickType, 358, OuyaSDK.KeyEnum.BUTTON_DPAD_UP));
        buttonMap.Add(new ButtonMap(joystickType, 359, OuyaSDK.KeyEnum.BUTTON_DPAD_DOWN));

        #endregion


        controllers.Add(ouya.name, ouya);

        //TODO: Complete OUYA Controller Settings

        #endregion
    }


    public static void RegisterCustomControllerMapping(ControllerType controllerType, List<ButtonMap> buttonMaps)
    {
        foreach (ButtonMap bm in buttonMaps)
        {
            buttonMap.Add(bm);
        }
        controllers.Add(controllerType.name, controllerType);
    }


    /// <summary>
    /// Use the button mapping to retrieve the mapped button
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static int findButton(int id, JoystickType joystickType)
    {
        List<ButtonMap> buttons = new List<ButtonMap>();
        buttons = buttonMap.FindAll((e) => { return (e.ButtonID == id && e.type == joystickType); });

        if (buttons.Count > 0)
        {
            return (int) buttons[0].button;
        }
        else
        {
            return 0;
        }
    }
}