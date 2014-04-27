using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InControl {

    /// <summary>
    /// Class representing devices created from custom configuration files.
    /// </summary>
    public class UnityCustomDeviceProfile : UnityInputDeviceProfile {

        ////public class CustomControlMapping {
        ////    public string Handle;
        ////    public bool Raw;
        ////    public bool Invert;
        ////    public float Scale;
        ////    public InputControlType Source;
        ////    public float SourceMin;
        ////    public float SourceMax;
        ////    public InputControlType Target;
        ////    public float TargetMin;
        ////    public float TargetMax;
        ////}

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
            ////this.AnalogMappings = new InputControlMapping[analogMappings.Length];
            ////for (int i = 0; i < analogMappings.Length; ++i) {
            ////    this.AnalogMappings[i] = ConvertMapping(analogMappings[i]);
            ////}

            this.ButtonMappings = buttonMappings;
            ////this.ButtonMappings = new InputControlMapping[buttonMappings.Length];
            ////for (int i = 0; i < buttonMappings.Length; ++i) {
            ////    this.ButtonMappings[i] = ConvertMapping(buttonMappings[i]);
            ////}
        }

        public override bool IsKnown {
            get { return true; }
        }

        ////private static InputControlMapping ConvertMapping(CustomControlMapping mapping) {
        ////    InputControlMapping newMapping = new InputControlMapping();
        ////    newMapping.Handle = mapping.Handle;
        ////    newMapping.Raw = mapping.Raw;
        ////    newMapping.Invert = mapping.Invert;
        ////    newMapping.Scale = mapping.Scale;
        ////    newMapping.Source = ControlTypeToSource(mapping.Source);
        ////    newMapping.SourceRange = new InputControlMapping.Range();
        ////    newMapping.SourceRange.Minimum = mapping.SourceMin;
        ////    newMapping.SourceRange.Maximum = mapping.SourceMax;
        ////    newMapping.Target = mapping.Target;
        ////    newMapping.TargetRange = new InputControlMapping.Range();
        ////    newMapping.TargetRange.Minimum = mapping.TargetMin;
        ////    newMapping.TargetRange.Maximum = mapping.TargetMax;

        ////    return newMapping;
        ////}

        ////private static InputControlSource ControlTypeToSource(InputControlType type) {
        ////    switch (type) {
        ////        case InputControlType.Analog0:
        ////            return Analog0;
        ////        case InputControlType.Analog1:
        ////            return Analog1;
        ////        case InputControlType.Analog2:
        ////            return Analog2;
        ////        case InputControlType.Analog3:
        ////            return Analog3;
        ////        case InputControlType.Analog4:
        ////            return Analog4;
        ////        case InputControlType.Analog5:
        ////            return Analog5;
        ////        case InputControlType.Analog6:
        ////            return Analog6;
        ////        case InputControlType.Analog7:
        ////            return Analog7;
        ////        case InputControlType.Analog8:
        ////            return Analog8;
        ////        case InputControlType.Analog9:
        ////            return Analog9;
        ////        case InputControlType.Analog10:
        ////            return Analog10;
        ////        case InputControlType.Analog11:
        ////            return Analog11;
        ////        case InputControlType.Analog12:
        ////            return Analog12;
        ////        case InputControlType.Analog13:
        ////            return Analog13;
        ////        case InputControlType.Analog14:
        ////            return Analog14;
        ////        case InputControlType.Analog15:
        ////            return Analog15;
        ////        case InputControlType.Analog16:
        ////            return Analog16;
        ////        case InputControlType.Analog17:
        ////            return Analog17;
        ////        case InputControlType.Analog18:
        ////            return Analog18;
        ////        case InputControlType.Analog19:
        ////            return Analog19;
        ////        case InputControlType.Button0:
        ////            return Button0;
        ////        case InputControlType.Button1:
        ////            return Button1;
        ////        case InputControlType.Button2:
        ////            return Button2;
        ////        case InputControlType.Button3:
        ////            return Button3;
        ////        case InputControlType.Button4:
        ////            return Button4;
        ////        case InputControlType.Button5:
        ////            return Button5;
        ////        case InputControlType.Button6:
        ////            return Button6;
        ////        case InputControlType.Button7:
        ////            return Button7;
        ////        case InputControlType.Button8:
        ////            return Button8;
        ////        case InputControlType.Button9:
        ////            return Button9;
        ////        case InputControlType.Button10:
        ////            return Button10;
        ////        case InputControlType.Button11:
        ////            return Button11;
        ////        case InputControlType.Button12:
        ////            return Button12;
        ////        case InputControlType.Button13:
        ////            return Button13;
        ////        case InputControlType.Button14:
        ////            return Button14;
        ////        case InputControlType.Button15:
        ////            return Button15;
        ////        case InputControlType.Button16:
        ////            return Button16;
        ////        case InputControlType.Button17:
        ////            return Button17;
        ////        case InputControlType.Button18:
        ////            return Button18;
        ////        case InputControlType.Button19:
        ////            return Button19;
        ////        default:
        ////            throw new InvalidOperationException("Cannot use control type '" + type + "' as a source value. Please, use only Analog[0,19] or Button[0,19].");
        ////    }
        ////}
    }
}
