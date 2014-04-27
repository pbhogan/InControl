using System.Collections.Generic;
using System.IO;
using System.Linq;
using InControl;
using LitJson;
using UnityEngine;

public class DeviceMapper : MonoBehaviour {

	private struct ControlInfo {
		public InputControlType controlType;
		public float startValue;
		public float endValue;
		public bool positive;

		public bool Inverted { get { return endValue < startValue; } }
	}

	private struct MappedControl {
		public InputControlType control;
		public string name;
		public bool positive;

		public MappedControl(InputControlType control, string name, bool positive = true)
			: this() {

			this.control = control;
			this.name = name;
			this.positive = positive;
		}
	}

	private List<MappedControl> mappingOrder = new List<MappedControl>() {
		new MappedControl(InputControlType.LeftStickY, "Left Stick Up", true),
		new MappedControl(InputControlType.LeftStickY, "Left Stick Down", false),
		new MappedControl(InputControlType.LeftStickX,"Left Stick Left",false),
		new MappedControl(InputControlType.LeftStickX,"Left Stick Right",true),
		new MappedControl(InputControlType.RightStickY,"Right Stick Up",true),
		new MappedControl(InputControlType.RightStickY,"Right Stick Down",false),
		new MappedControl(InputControlType.RightStickX,"Right Stick Left",false),
		new MappedControl(InputControlType.RightStickX,"Right Stick Right",true),
		new MappedControl(InputControlType.LeftTrigger,"Left Trigger",true),
		new MappedControl(InputControlType.RightTrigger,"Right Trigger",true),
		new MappedControl(InputControlType.DPadUp,"Digital Pad Up"),
		new MappedControl(InputControlType.DPadDown,"Digital Pad Down"),
		new MappedControl(InputControlType.DPadLeft,"Digital Pad Left"),
		new MappedControl(InputControlType.DPadRight,"Digital Pad Right"),
		new MappedControl(InputControlType.Action1,"Cross/A button"),
		new MappedControl(InputControlType.Action2,"Circle/B button"),
		new MappedControl(InputControlType.Action3,"Square/X button"),
		new MappedControl(InputControlType.Action4,"Triangle/Y button"),
		new MappedControl(InputControlType.LeftBumper,"Left Bumper"),
		new MappedControl(InputControlType.RightBumper,"Right Bumper"),
		new MappedControl(InputControlType.LeftStickButton,"Left Stick Button"),
		new MappedControl(InputControlType.RightStickButton,"Right Stick Button"),
		new MappedControl(InputControlType.Start,"Start Button"),
		new MappedControl(InputControlType.Select,"Select Button"),
		new MappedControl(InputControlType.System,"System Button"),
		new MappedControl(InputControlType.Back,"Back Button"),
		new MappedControl(InputControlType.Pause,"Pause Button"),
		new MappedControl(InputControlType.Menu,"Menu Button"),
		new MappedControl(InputControlType.TiltY,"Tilt forward",true),
		new MappedControl(InputControlType.TiltY,"Tilt backward",false),
		new MappedControl(InputControlType.TiltX,"Tilt left",false),
		new MappedControl(InputControlType.TiltX,"Tilt right",true),
		new MappedControl(InputControlType.TouchPadTap,"Touchpad tap"),
		new MappedControl(InputControlType.TouchPadYAxis,"Touchpad up",true),
		new MappedControl(InputControlType.TouchPadYAxis,"Touchpad down",false),
		new MappedControl(InputControlType.TouchPadXAxis,"Touchpad left",false),
		new MappedControl(InputControlType.TouchPadXAxis,"Touchpad right",true)
	};

	IEnumerator<MappedControl> mappingEnumerator;

	/// <summary>
	/// Keys are target controls, values are the sources for them.
	/// </summary>
	Dictionary<ControlInfo, ControlInfo> controlMappings = new Dictionary<ControlInfo, ControlInfo>();
	private Stack<InputDevice> pendingDevices = new Stack<InputDevice>();
	private InputDevice currentDevice = null;

	private List<InControl.InputControl> pressedButtons;

	void Awake() {
		Debug.Log("Configuring InputManager");
		InputManager.CustomProfilesPath = Application.dataPath + "/Profiles";
		InputManager.Setup();
		foreach (var device in InputManager.Devices) {
			Debug.Log(string.Format("Detected device: {0} ({1} and {2})",
									device.Name,
									device.IsSupportedOnThisPlatform ? "Supported" : "Unsupported",
									device.IsKnown ? "Known" : "Unknown"));
		}
	}

	void Start() {
		InputManager.OnDeviceAttached += InputManager_OnDeviceAttached;

		for (int i = 0; i < InputManager.Devices.Count; ++i) {
			var device = InputManager.Devices[i];
			if (!device.IsKnown) {
				pendingDevices.Push(device);
			} else {
				Debug.Log("Not configuring supported controller " + device.Name);
			}
		}

		pressedButtons = new List<InControl.InputControl>();
	}

	private void InputManager_OnDeviceAttached(InputDevice device) {
		if (!device.IsKnown) {
			pendingDevices.Push(device);
		} else {
			Debug.Log("Not configuring supported controller " + device.Name);
		}
	}

	private bool calibrating = false;
	private float calibrationStartTime = 0;
	[Range(.5f, 5)]
	public float calibrationTime = 5;

	private Dictionary<int, float> deviceDefaultValues = new Dictionary<int, float>();

	private float GetDefaultValue(InputControlType controlType) {
		if (deviceDefaultValues.ContainsKey((int)controlType)) {
			return deviceDefaultValues[(int)controlType];
		} else {
			return 0;
		}
	}

	void Update() {
		InputManager.Update();

		if (currentDevice == null) {
			if (pendingDevices.Count > 0) {
				currentDevice = pendingDevices.Pop();

				calibrating = true;
				calibrationStartTime = Time.realtimeSinceStartup;
			}
		} else {
			if (calibrating) {
				if (calibrationStartTime + calibrationTime < Time.realtimeSinceStartup) {

					calibrating = false;
					deviceDefaultValues.Clear();
					foreach (var control in currentDevice.Controls) {
						if (control != null && control.IsNotNull) {
							deviceDefaultValues.Add((int)control.Target, control.Value);
						}
					}

					pressedButtons.Clear();
					mappingEnumerator = mappingOrder.GetEnumerator();
					mappingEnumerator.MoveNext();
				}
			} else {

				bool deviceFinished = false;
				if (Input.GetKeyDown(KeyCode.Escape)) {
					deviceFinished = !mappingEnumerator.MoveNext();
				} else {
					var control = CatchControl();
					if (control != null) {
						// Key is the target control; value is the source
						var sourceInfo = new ControlInfo() {
							controlType = control.Target,
							startValue = GetDefaultValue(control.Target),
							endValue = control.Value > .8f ? 1 : control.Value < .8f ? -1 : control.Value > -.2f && control.Value < .2f ? 0 : control.Value,
							positive = GetDefaultValue(control.Target) < (control.Value > .8f ? 1 : control.Value < .8f ? -1 : control.Value > -.2f && control.Value < .2f ? 0 : control.Value)
						};

						var targetInfo = new ControlInfo() {
							controlType = mappingEnumerator.Current.control,
							startValue = sourceInfo.startValue,
							endValue = sourceInfo.endValue,
							positive = mappingEnumerator.Current.positive
						};

						controlMappings.Add(targetInfo, sourceInfo);

						deviceFinished = !mappingEnumerator.MoveNext();
						pressedButtons.Clear();
					}
				}

				if (deviceFinished) {
					// Write out device configuration
					WriteMappings(currentDevice.Name, currentDevice.Meta, controlMappings);
					controlMappings.Clear();
					pressedButtons.Clear();
					currentDevice = null;
				}
			}
		}
	}

	private void WriteMappings(string name, string meta, Dictionary<ControlInfo, ControlInfo> mappings) {
		System.Text.StringBuilder builder = new System.Text.StringBuilder();
		var writer = new JsonWriter(builder);
		writer.WriteObjectStart();
		writer.IndentValue = 4;
		writer.PrettyPrint = true;

		string actualName = name.StartsWith("Unknown Device") ? name.Substring(16, name.Length - 16 - 1) : name;
		writer.WritePropertyName("name");
		writer.Write(actualName);

		writer.WritePropertyName("meta");
		writer.Write(meta);

		writer.WritePropertyName("regex");
		writer.Write(actualName);

		writer.WritePropertyName("platforms");
		writer.WriteArrayStart();
		var platform = Application.platform;
		if (platform == RuntimePlatform.WindowsEditor) {
			platform = RuntimePlatform.WindowsPlayer;
		} else if (platform == RuntimePlatform.OSXEditor) {
			platform = RuntimePlatform.OSXPlayer;
		}
		string platformName = platform.ToString().Replace("Player", "").Replace("Web", "");
		writer.Write(platformName);
		writer.WriteArrayEnd();

		writer.WritePropertyName("joystickNames");
		writer.WriteArrayStart();
		writer.Write(actualName);
		writer.WriteArrayEnd();

		writer.WritePropertyName("sensitivity");
		writer.Write(1.0f);

		writer.WritePropertyName("lowerDeadZone");
		writer.Write(.2f);
		writer.WritePropertyName("upperDeadZone");
		writer.Write(1.0f);

		// Separate button and analog mappings!
		var analogMappings = new Dictionary<ControlInfo, ControlInfo>();
		var buttonMappings = new Dictionary<ControlInfo, ControlInfo>();
		foreach (var kvp in mappings) {
			if (IsButton(kvp.Key.controlType)) {
				buttonMappings.Add(kvp.Key, kvp.Value);
			} else {
				analogMappings.Add(kvp.Key, kvp.Value);
			}
		}

		// Maintain single axis (triggers, DPad) in [-1..0]/[0..1] ranges
		analogMappings = CleanIncompleteMappings(analogMappings);
		// Unify composable axis (up/down, left/right) into one mapping (StickY, StickX)
		analogMappings = MergeCompleteMappings(analogMappings);

		writer.WritePropertyName("analogMappings");
		writer.WriteArrayStart();
		foreach (var mapping in analogMappings) {
			WriteMapping(writer, mapping.Value, mapping.Key);
		}
		writer.WriteArrayEnd();

		writer.WritePropertyName("buttonMappings");
		writer.WriteArrayStart();
		foreach (var mapping in buttonMappings) {
			WriteMapping(writer, mapping.Value, mapping.Key);
		}
		writer.WriteArrayEnd();

		writer.WriteObjectEnd();

		string fileName = InputManager.CustomProfilesPath + actualName + " " + platformName + ".profile";
		if (File.Exists(fileName)) {
			File.Delete(fileName);
		}

		var stream = File.Create(fileName);
		using (StreamWriter sw = new StreamWriter(stream)) {
			sw.Write(builder.ToString());
		}
	}

	private void WriteMapping(LitJson.JsonWriter writer, ControlInfo source, ControlInfo target) {
		writer.WriteObjectStart();

		writer.WritePropertyName("handle");
		writer.Write(target.controlType.ToString());

		if (source.Inverted) {
			writer.WritePropertyName("invert");
			writer.Write(source.Inverted);
		}

		writer.WritePropertyName("scale");
		writer.Write(1.0f);

		////float initialValue = deviceDefaultValues[(int)source.controlType];

		// source
		writer.WritePropertyName("source");
		writer.WriteObjectStart();
		writer.WritePropertyName("control");
		writer.Write(source.controlType.ToString());
		writer.WritePropertyName("min");
		writer.Write(Mathf.Min(source.startValue, source.endValue));
		writer.WritePropertyName("max");
		writer.Write(Mathf.Max(source.startValue, source.endValue));
		writer.WriteObjectEnd();

		// target
		writer.WritePropertyName("target");
		writer.WriteObjectStart();
		writer.WritePropertyName("control");
		writer.Write(target.controlType.ToString());
		writer.WritePropertyName("min");
		writer.Write(Mathf.Min(target.startValue, target.endValue));
		writer.WritePropertyName("max");
		writer.Write(Mathf.Max(target.startValue, target.endValue));
		writer.WriteObjectEnd();

		writer.WriteObjectEnd();
	}

	void OnGUI() {
		if (currentDevice != null) {
			GUI.Label(new Rect(10, 0, 1000, 20), string.Format("Device: {0} ({1}): {2} - {3}",
															   currentDevice.Name,
															   currentDevice.Meta,
															   currentDevice.IsKnown ? "Known" : "Unknown",
															   currentDevice.IsSupportedOnThisPlatform ? "Supported" : "Unsupported"));
			var controls = currentDevice.Controls;
			for (int i = 0; i < controls.Length; ++i) {
				if (controls[i] == null) {
					GUI.Label(new Rect(10, 20 + i * 12, 300, 20), "No control in position '" + i + "' (" + (InputControlType)i + ")");
				} else {
					GUI.Label(new Rect(10, 20 + i * 12, 300, 20), string.Format("{0}: {1} - {2:0.00}",
																				controls[i].Handle,
																				controls[i].State,
																				controls[i].Value));
				}
			}

			if (calibrating) {
				GUI.Label(new Rect(500, 20, 500, 20), string.Format("Calibrating ({0:0.0}s). Please leave the gamepad controls free to detect initial values.", calibrationStartTime + calibrationTime - Time.realtimeSinceStartup));
				GUI.Label(new Rect(500, 40, 500, 20), "Wait or press Intro or Escape once the values are stable.");
			} else {
				GUI.Label(new Rect(500, 20, 500, 20), "Setting up mappings");
				GUI.Label(new Rect(500, 40, 500, 20), "Press " + mappingEnumerator.Current.name + " (or Escape if the pad has no equivalent control)");

				int i = 0;
				foreach (var pair in controlMappings) {
					GUI.Label(new Rect(500, 70 + i * 20, 500, 20),
							  string.Format("Mapped {0} [{1:0.00} : {2:0.00}] to {3} [{4:0.00} : {5:0.00}]",
											pair.Value.controlType, pair.Value.startValue, pair.Value.endValue,
											pair.Key.controlType, pair.Key.startValue, pair.Key.endValue));
					++i;
				}
			}
		} else {
			GUI.Label(new Rect(10, 0, 1000, 20), "No device available. Please, connect a new controller.");
		}

	}

	public InControl.InputControl CatchControl() {
		foreach (var control in currentDevice.Controls) {
			if (control != null) {
				float initial = deviceDefaultValues[(int)control.Target];

				bool pressed = false;
				bool released = false;
				if (initial == 0) {
					pressed = control.WasPressed;
					released = control.WasReleased;
				} else {
					pressed = Mathf.Abs(initial - control.Value) > .01f
							  && Mathf.Abs(initial - control.LastValue) < .01f;
					released = Mathf.Abs(initial - control.Value) < .01f
							   && Mathf.Abs(initial - control.LastValue) > .01f;
				}

				if (pressed) {
					this.pressedButtons.Add(control);
				} else if (released) {
					if (this.pressedButtons.Contains(control)) {
						this.pressedButtons.Remove(control);
					}
				}
			}
		}

		foreach (var control in this.pressedButtons) {
			float initial = deviceDefaultValues[(int)control.Target];

			if (initial == 0) {
				if (Mathf.Abs(control.Value) > .8f) {
					return control;
				}
			} else {
				if (initial > .9f) {
					// Range: 1-0
					if (control.Value < .2f) {
						return control;
					}
				} else {
					if (initial < .2f && initial > -.2f) {
						// Range: -1-initial-1
						if (Mathf.Abs(control.Value) > .8f) {
							return control;
						}
					} else if (initial > 0) {
						// Range: 0-initial-1
						if ((control.Value - initial > (1 - initial) * .8f)
							|| (initial - control.Value < initial * .8f)) {

							return control;
						}
					} else if (initial < 0) {
						// Range: -1-initial-0
						if ((control.Value > initial * .8f)
							|| (control.Value - initial < (-1 - initial) * .8f)) {

							return control;
						}
					}
				}
			}
		}

		return null;
	}

	private bool IsButton(InputControlType type) {
		return type == InputControlType.Action1
			|| type == InputControlType.Action2
			|| type == InputControlType.Action3
			|| type == InputControlType.Action4
			|| type == InputControlType.LeftBumper
			|| type == InputControlType.RightBumper
			|| type == InputControlType.LeftStickButton
			|| type == InputControlType.RightStickButton
			|| type == InputControlType.Start
			|| type == InputControlType.Select
			|| type == InputControlType.Back
			|| type == InputControlType.Menu
			|| type == InputControlType.Pause
			|| type == InputControlType.System;
	}

	/// <summary>
	/// Ensure all mappings of standalone axes has the target range in [0..-1] or [0..1].
	/// Some pads report axes in the [-1..1] or [1..-1] ranges, but the result must have a rest point in 0.
	/// </summary>
	/// <param name="mappings">Keys are the final control; values the sources.</param>
	/// <returns></returns>
	private Dictionary<ControlInfo, ControlInfo> CleanIncompleteMappings(Dictionary<ControlInfo, ControlInfo> mappings) {
		var cleanableMappings = from mapping in mappings
								where (mapping.Key.controlType == InputControlType.LeftTrigger
									   || mapping.Key.controlType == InputControlType.RightTrigger
									   || mapping.Key.controlType == InputControlType.DPadUp
									   || mapping.Key.controlType == InputControlType.DPadDown
									   || mapping.Key.controlType == InputControlType.DPadLeft
									   || mapping.Key.controlType == InputControlType.DPadRight)
								   && ((mapping.Key.startValue == 1 && mapping.Key.endValue == -1)
									   || (mapping.Key.startValue == -1 && mapping.Key.endValue == 1))
								select mapping;

		var newMappings = new Dictionary<ControlInfo, ControlInfo>(mappings);

		foreach (var mapping in cleanableMappings) {
			newMappings.Remove(mapping.Key);
			var source = mapping.Value;
			var target = mapping.Key;

			if (source.Inverted) {
				var newTarget = new ControlInfo() {
					controlType = target.controlType,
					positive = target.positive,
					startValue = -1,
					endValue = 0
				};

				newMappings.Add(newTarget, source);
			} else {
				var newTarget = new ControlInfo() {
					controlType = target.controlType,
					positive = target.positive,
					startValue = 1,
					endValue = 0
				};

				newMappings.Add(newTarget, source);
			}
		}

		return newMappings;
	}

	/// <summary>
	/// Merges mappings for two directions of the same axis into one.
	/// Used to collapse sticks and accelerometers positive and negative directions into one single mapping.
	/// e.g.: LeftStickY up [0..1] and LeftStickY down [0..-1] become LeftStickY [-1..1]
	/// </summary>
	/// <param name="mappings">Keys are the final control; values the sources.</param>
	private Dictionary<ControlInfo, ControlInfo> MergeCompleteMappings(Dictionary<ControlInfo, ControlInfo> mappings) {
		var mergerableMappings = new HashSet<IEnumerable<KeyValuePair<ControlInfo, ControlInfo>>>();

		var leftStickYMaps = from mapping in mappings
							 where mapping.Key.controlType == InputControlType.LeftStickY
							 select mapping;
		var leftStickXMaps = from mapping in mappings
							 where mapping.Key.controlType == InputControlType.LeftStickX
							 select mapping;
		var rightStickYMaps = from mapping in mappings
							  where mapping.Key.controlType == InputControlType.RightStickY
							  select mapping;
		var rightSickXMaps = from mapping in mappings
							 where mapping.Key.controlType == InputControlType.RightStickX
							 select mapping;
		var tiltYMaps = from mapping in mappings
						where mapping.Key.controlType == InputControlType.TiltY
						select mapping;
		var tiltXMaps = from mapping in mappings
						where mapping.Key.controlType == InputControlType.TiltX
						select mapping;
		var tiltZMaps = from mapping in mappings
						where mapping.Key.controlType == InputControlType.TiltZ
						select mapping;
		var touchPadYMaps = from mapping in mappings
							where mapping.Key.controlType == InputControlType.TouchPadYAxis
							select mapping;
		var touchPadXMaps = from mapping in mappings
							where mapping.Key.controlType == InputControlType.TouchPadXAxis
							select mapping;

		mergerableMappings.Add(leftStickYMaps);
		mergerableMappings.Add(leftStickXMaps);
		mergerableMappings.Add(rightStickYMaps);
		mergerableMappings.Add(rightSickXMaps);
		mergerableMappings.Add(tiltYMaps);
		mergerableMappings.Add(tiltXMaps);
		mergerableMappings.Add(tiltZMaps);
		mergerableMappings.Add(touchPadYMaps);
		mergerableMappings.Add(touchPadXMaps);

		var newMappings = new Dictionary<ControlInfo, ControlInfo>(mappings);

		foreach(var mergeableMapping in mergerableMappings){
			var maps = mergeableMapping.ToArray();

			// Both ends mapped to the same analog
			if (maps.Length == 2 && maps[0].Value.controlType == maps[1].Value.controlType) {
				newMappings.Remove(maps[0].Key);
				newMappings.Remove(maps[1].Key);

				var newMapping = MergeMapping(maps[0], maps[1]);

				newMappings.Add(newMapping.Key, newMapping.Value);
			}
		}

		return newMappings;
	}

	private KeyValuePair<ControlInfo, ControlInfo> MergeMapping(KeyValuePair<ControlInfo, ControlInfo> map0,
																KeyValuePair<ControlInfo, ControlInfo> map1) {

		var positive = map0.Key.positive ? map0 : map1;
		var negative = map0.Key.positive ? map1 : map0;

		ControlInfo newSource = new ControlInfo() {
			controlType = positive.Value.controlType,
			startValue = negative.Value.endValue,
			endValue = positive.Value.endValue,
			positive = true
		};

		ControlInfo newTarget = new ControlInfo() {
			controlType = positive.Key.controlType,
			startValue = negative.Key.endValue,
			endValue = positive.Key.endValue,
			positive = true
		};

		return new KeyValuePair<ControlInfo, ControlInfo>(newTarget, newSource);
	}
}
