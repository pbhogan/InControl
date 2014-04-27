using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InControl {

    /// <summary>
    /// Class representing devices created from custom configuration files.
    /// </summary>
    public class UnityCustomDeviceProfile : UnityInputDeviceProfile {

        public UnityCustomDeviceProfile(string name,
                                        string meta,
                                        string regex,
                                        string[] platforms,
                                        string[] names,
                                        float sensitivity,
                                        float lowerDeadZone,
                                        float upperDeadZone,
                                        InputControlMapping[] analogMappings,
                                        InputControlMapping[] buttonMappings) {

            this.Name = name;
            this.Meta = meta;
            this.RegexName = regex;
            this.SupportedPlatforms = platforms;
            this.JoystickNames = names;
            this.Sensitivity = sensitivity;
            this.LowerDeadZone = lowerDeadZone;
            this.UpperDeadZone = upperDeadZone;

            this.AnalogMappings = analogMappings;

            this.ButtonMappings = buttonMappings;
        }

        public override bool IsKnown {
            get { return true; }
        }

    }
}
