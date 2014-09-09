namespace InControl {
  /**
   * Profile for: 
   * 
   * * Android TV Remote app (which runs on your phone/tablet).
   * * Actual TV remote (hopefully)
   * 
   * Tested with ADT-1.
   * 
   * Profile by Artūras 'arturaz' Šlajus <arturas@tinylabproductions.com>.
   **/
  [AutoDiscover]
  public class AndroidTVTouchInputProfile : UnityInputDeviceProfile {
    public AndroidTVTouchInputProfile() {
      Name = "Android TV Touch Input";
      SupportedPlatforms = new[] { "ANDROID" };
      JoystickNames = new[] { "touch-input", "navigation-input" };

      Sensitivity = 1.0f;
      LowerDeadZone = 0.2f;

      ButtonMappings = new[] {
        new InputControlMapping {
          Handle = "A",
          Target = InputControlType.Action1,
          Source = Button0
        }
      };

      AnalogMappings = new[] {
        new InputControlMapping {
          Handle = "DPad Left",
          Target = InputControlType.DPadLeft,
          Source = Analog4,
          SourceRange = InputControlMapping.Range.Negative,
          TargetRange = InputControlMapping.Range.Negative,
          Invert = true
        },
        new InputControlMapping {
          Handle = "DPad Right",
          Target = InputControlType.DPadRight,
          Source = Analog4,
          SourceRange = InputControlMapping.Range.Positive,
          TargetRange = InputControlMapping.Range.Positive
        },
        new InputControlMapping {
          Handle = "DPad Up",
          Target = InputControlType.DPadUp,
          Source = Analog5,
          SourceRange = InputControlMapping.Range.Negative,
          TargetRange = InputControlMapping.Range.Negative
        },
        new InputControlMapping {
          Handle = "DPad Down",
          Target = InputControlType.DPadDown,
          Source = Analog5,
          SourceRange = InputControlMapping.Range.Positive,
          TargetRange = InputControlMapping.Range.Positive,
          Invert = true
        },
      };
    }
  }
}
