using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using LitJson;

namespace InControl {

	public static class CustomProfileReader {

		public static UnityCustomDeviceProfile LoadProfile(string path) {
			if (!File.Exists(path)) {
				throw new FileNotFoundException("No profile found at path '" + path + "'");
			}

			using (var streamReader = new StreamReader(path)) {
				return ReadProfile(streamReader.ReadToEnd());
			}
		}

		#region LitJson wrappers

		private struct JsonList : IEnumerable {
			public List<object> list;
			public JsonType Type { get { return JsonType.Array; } }

			public int Count { get { return list.Count; } }

			public int Int(int index) {
				return Get<int>(index);
			}

			public long Long(int index) {
				return Get<long>(index);
			}

			public float Float(int index) {
				return (float)Get<double>(index);
			}

			public double Double(int index) {
				return Get<double>(index);
			}

			public string String(int index) {
				return Get<string>(index);
			}

			public bool Boolean(int index) {
				return Get<bool>(index);
			}

			public JsonObject Object(int index) {
				return Get<JsonObject>(index);
			}

			public JsonList List(int index) {
				return Get<JsonList>(index);
			}

			private T Get<T>(int index) {
				return (T)list[index];
			}

			public IEnumerator GetEnumerator() {
				foreach (var elem in list) {
					yield return elem;
				}
			}
		}

		private struct JsonObject {
			public Dictionary<string, object> dict;
			public JsonType Type { get { return JsonType.Object; } }

			public bool Contains(string name) {
				return dict.ContainsKey(name);
			}

			public int Int(string name) {
				return Get<int>(name);
			}

			public long Long(string name) {
				return Get<long>(name);
			}

			public float Float(string name) {
				return (float)Get<double>(name);
			}

			public double Double(string name) {
				return Get<double>(name);
			}

			public string String(string name) {
				return Get<string>(name);
			}

			public bool Boolean(string name) {
				return Get<bool>(name);
			}

			public JsonObject Object(string name) {
				return Get<JsonObject>(name);
			}

			public JsonList List(string name) {
				return Get<JsonList>(name);
			}

			private T Get<T>(string name) {
				return (T)dict[name];
			}
		}

		private static JsonObject ReadJson(JsonReader json) {
			if (json.Read()) {
				if (json.Token == JsonToken.ObjectStart) {
					JsonObject result = ReadJsonObject(json);
					return result;
				} else {
					throw new InvalidOperationException("Invalid json data type '" + json.Token + "' when expecting root object");
				}
			}

			throw new InvalidOperationException("Empty json file");
		}

		private static JsonObject ReadJsonObject(JsonReader json) {
			JsonObject result = new JsonObject() { dict = new Dictionary<string, object>() };

			while (json.Read()) {
				if (json.Token == JsonToken.ObjectEnd) {
					return result;
				} else if (json.Token == JsonToken.PropertyName) {
					string name = (string)json.Value;
					json.Read();
					switch (json.Token) {
						case JsonToken.ObjectStart:
							result.dict.Add(name, ReadJsonObject(json));
							break;
						case JsonToken.ArrayStart:
							result.dict.Add(name, ReadJsonList(json));
							break;
						case JsonToken.Int:
							result.dict.Add(name, (int)json.Value);
							break;
						case JsonToken.Long:
							result.dict.Add(name, (long)json.Value);
							break;
						case JsonToken.Single:
							result.dict.Add(name, (float)json.Value);
							break;
						case JsonToken.Double:
							result.dict.Add(name, (double)json.Value);
							break;
						case JsonToken.String:
							result.dict.Add(name, (string)json.Value);
							break;
						case JsonToken.Boolean:
							result.dict.Add(name, (bool)json.Value);
							break;
						default:
							throw new InvalidOperationException("Invalid json data type '" + json.Token + "' when expecting a value");
					}
				} else {
					throw new InvalidOperationException("Invalid json data type '" + json.Token + "' when expecting a property name");
				}
			}

			throw new InvalidOperationException("Json file ended while parsing object");
		}

		private static JsonList ReadJsonList(JsonReader json) {
			JsonList result = new JsonList() { list = new List<object>() };

			while (json.Read()) {
				switch (json.Token) {
					case JsonToken.ArrayEnd:
						return result;
					case JsonToken.ObjectStart:
						result.list.Add(ReadJsonObject(json));
						break;
					case JsonToken.ArrayStart:
						result.list.Add(ReadJsonList(json));
						break;
					case JsonToken.Int:
						result.list.Add((int)json.Value);
						break;
					case JsonToken.Long:
						result.list.Add((long)json.Value);
						break;
					case JsonToken.Single:
						result.list.Add((float)json.Value);
						break;
					case JsonToken.Double:
						result.list.Add((double)json.Value);
						break;
					case JsonToken.String:
						result.list.Add((string)json.Value);
						break;
					case JsonToken.Boolean:
						result.list.Add((bool)json.Value);
						break;
					default:
						throw new InvalidOperationException("Invalid json data type '" + json.Token + "' when expecting a value");
				}
			}

			throw new InvalidOperationException("Json file ended while parsing list");
		}

		#endregion LitJson wrappers

		private static UnityCustomDeviceProfile ReadProfile(string jsonText) {
			JsonReader json = new JsonReader(jsonText);
			JsonObject jsonObj = ReadJson(json);

			string name = jsonObj.String("name");
			string meta = jsonObj.String("meta");
			string regex = jsonObj.Contains("regex") ? jsonObj.String("regex") : null;

			var jsonPlatforms = jsonObj.List("platforms");
			var platforms = from object platform in jsonPlatforms
							select (string)platform;
			var jsonNames = jsonObj.List("joystickNames");
			var joystickNames = from object joystickName in jsonNames
								select (string)joystickName;

			float sensitivity = jsonObj.Contains("sensitivity") ? jsonObj.Float("sensitivity") : 1;
			float lowerDeadZone = jsonObj.Contains("lowerDeadZone") ? jsonObj.Float("lowerDeadZone") : .1f;
			float upperDeadZone = jsonObj.Contains("upperDeadZone") ? jsonObj.Float("upperDeadZone") : 0;

			var jsonAnalogs = jsonObj.List("analogMappings");
			var analogs = new List<InputControlMapping>(jsonAnalogs.Count);
			for (int i = 0; i < jsonAnalogs.Count; ++i) {
				var dictionary = jsonAnalogs.Object(i);
				var mapping = ReadMapping(dictionary);
				analogs.Add(mapping);
			}

			var jsonButtons = jsonObj.List("buttonMappings");
			var buttons = new List<InputControlMapping>(jsonButtons.Count);
			for (int i = 0; i < jsonButtons.Count; ++i) {
				var dictionary = jsonButtons.Object(i);
				var mapping = ReadMapping(dictionary);
				buttons.Add(mapping);
			}

			var profile = CustomProfileBuilder.CreateProfile()
											  .SetSensitivity(sensitivity, lowerDeadZone, upperDeadZone)
											  .AddSupportedPlatforms(platforms)
											  .AddJoystickNames(joystickNames)
											  .AddAnalogMappings(analogs)
											  .AddButtonMappings(buttons)
											  .Instantiate(name, meta, regex);

			return profile;
		}

		private static InputControlMapping ReadMapping(JsonObject json) {
			var mapping = new InputControlMapping();

			mapping.Handle = json.String("handle");

			if (json.Contains("raw")) {
				mapping.Raw = json.Boolean("raw");
			}

			if (json.Contains("invert")) {
				mapping.Invert = json.Boolean("invert");
			}

			if (json.Contains("scale")) {
				mapping.Scale = json.Float("scale");
			}

			var jsonSource = json.Object("source");
			string sourceString = jsonSource.String("control");
			bool analog = sourceString.StartsWith("Analog");
			bool button = sourceString.StartsWith("Button");
			if (analog) {
				int number = int.Parse(sourceString.Replace("Analog", ""));
				mapping.Source = new UnityAnalogSource(number);
			} else if (button) {
				int number = int.Parse(sourceString.Replace("Button", ""));
				mapping.Source = new UnityButtonSource(number);
			} else {
				throw new InvalidOperationException("Invalid source type '" + sourceString + "'. Source type must be either 'Button' or 'Analog', followed by a number.");
			}

			if (jsonSource.Contains("min") || jsonSource.Contains("max")) {
				mapping.SourceRange = new InputControlMapping.Range();
				mapping.SourceRange.Minimum = jsonSource.Contains("min") ? jsonSource.Float("min") : 0;
				mapping.SourceRange.Maximum = jsonSource.Contains("max") ? jsonSource.Float("max") : 1;
			}

			var jsonTarget = json.Object("target");
			string targetString = jsonTarget.String("control");
			mapping.Target = (InputControlType)Enum.Parse(typeof(InputControlType), targetString);

			if (jsonTarget.Contains("min") || jsonTarget.Contains("max")) {
				mapping.TargetRange = new InputControlMapping.Range();
				mapping.TargetRange.Minimum = jsonTarget.Contains("min") ? jsonTarget.Float("min") : 0;
				mapping.TargetRange.Maximum = jsonTarget.Contains("max") ? jsonTarget.Float("max") : 1;
			}

			return mapping;
		}
	}
}
