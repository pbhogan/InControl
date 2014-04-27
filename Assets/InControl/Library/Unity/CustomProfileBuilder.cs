using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InControl  {
    public class CustomProfileBuilder {

        private List<string> SupportedPlatforms { get; set; }
        private List<string> JoystickNames { get; set; }

        private float Sensitivity { get; set; }
        private float LowerDeadZone { get; set; }
        private float UpperDeadZone { get; set; }

        private List<InputControlMapping> AnalogMappings { get; set; }
        private List<InputControlMapping> ButtonMappings { get; set; }

        /// <summary>
        /// Start the process of configuring a new custom input profile.
        /// </summary>
        /// <returns></returns>
        public static CustomProfileBuilder CreateProfile() {
            return new CustomProfileBuilder();
        }

        private CustomProfileBuilder() {
            this.SupportedPlatforms = new List<string>();
            this.JoystickNames = new List<string>();
            this.AnalogMappings = new List<InputControlMapping>();
            this.ButtonMappings = new List<InputControlMapping>();
        }

        /// <summary>
        /// Copy constructor.
        /// </summary>
        /// <param name="other">Instance to copy the values from.</param>
        private CustomProfileBuilder(CustomProfileBuilder other) {
            this.SupportedPlatforms = new List<string>(other.SupportedPlatforms);
            this.JoystickNames = new List<string>(other.JoystickNames);

            this.Sensitivity = other.Sensitivity;
            this.LowerDeadZone = other.LowerDeadZone;
            this.UpperDeadZone = other.UpperDeadZone;

            this.AnalogMappings = new List<InputControlMapping>(other.AnalogMappings);
            this.ButtonMappings = new List<InputControlMapping>(other.ButtonMappings);
        }

        /// <summary>
        /// Create the already configured custom input profile.
        /// </summary>
        /// <param name="name">Name of the profile..</param>
        /// <param name="meta">Meta information regarding the profile.</param>
        /// <param name="regex">Regular expression to use to identify compatible devices. Might be missing.</param>
        /// <returns></returns>
        public UnityCustomDeviceProfile Instantiate(string name, string meta, string regex) {
            var profile = new UnityCustomDeviceProfile(name,
                                                       meta,
                                                       regex,
                                                       this.SupportedPlatforms.ToArray(),
                                                       this.JoystickNames.ToArray(),
                                                       this.Sensitivity,
                                                       this.LowerDeadZone,
                                                       this.UpperDeadZone,
                                                       this.AnalogMappings.ToArray(),
                                                       this.ButtonMappings.ToArray());

            return profile;
        }

        public CustomProfileBuilder AddSupportedPlatform(string platform) {
            if (string.IsNullOrEmpty(platform)) {
                return this;
            }

            var newProfile = new CustomProfileBuilder(this);
            newProfile.SupportedPlatforms.Add(platform);

            return newProfile;
        }

        public CustomProfileBuilder AddSupportedPlatforms(IEnumerable<string> platforms) {
            if (platforms == null) {
                return null;
            }

            var newProfile = new CustomProfileBuilder(this);
            newProfile.SupportedPlatforms.AddRange(platforms);

            return newProfile;
        }

        public CustomProfileBuilder AddJoystickName(string name) {
            if (string.IsNullOrEmpty(name)) {
                return this;
            }

            var newProfile = new CustomProfileBuilder(this);
            newProfile.JoystickNames.Add(name);

            return newProfile;
        }

        public CustomProfileBuilder AddJoystickNames(IEnumerable<string> names) {
            if (names == null) {
                return null;
            }

            var newProfile = new CustomProfileBuilder(this);
            newProfile.JoystickNames.AddRange(names);

            return newProfile;
        }

        public CustomProfileBuilder AddAnalogMapping(InputControlMapping mapping) {
            if (mapping == null) {
                return this;
            }

            var newProfile = new CustomProfileBuilder(this);
            for (int i = 0; i < newProfile.AnalogMappings.Count; ++i) {
                if (newProfile.AnalogMappings[i].Target == mapping.Target) {
                    newProfile.AnalogMappings.RemoveAt(i);
                }
            }

            newProfile.AnalogMappings.Add(mapping);

            return newProfile;
        }

        public CustomProfileBuilder AddAnalogMappings(IEnumerable<InputControlMapping> mappings) {
            if (mappings == null) {
                return null;
            }

            var newProfile = new CustomProfileBuilder(this);
            newProfile.AnalogMappings.AddRange(mappings);

            return newProfile;
        }

        public CustomProfileBuilder AddButtonMapping(InputControlMapping mapping) {
            if (mapping == null) {
                return this;
            }

            var newProfile = new CustomProfileBuilder(this);
            for (int i = 0; i < newProfile.ButtonMappings.Count; ++i) {
                if (newProfile.ButtonMappings[i].Target == mapping.Target) {
                    newProfile.ButtonMappings.RemoveAt(i);
                }
            }

            newProfile.ButtonMappings.Add(mapping);

            return newProfile;
        }

        public CustomProfileBuilder AddButtonMappings(IEnumerable<InputControlMapping> mappings) {
            if (mappings == null) {
                return null;
            }

            var newProfile = new CustomProfileBuilder(this);
            newProfile.ButtonMappings.AddRange(mappings);

            return newProfile;
        }

        public CustomProfileBuilder SetSensitivity(float sensitivity, float lowerDeadZone, float upperDeadZone) {
            var newProfile = new CustomProfileBuilder(this);
            newProfile.Sensitivity = sensitivity;
            newProfile.LowerDeadZone = lowerDeadZone;
            newProfile.UpperDeadZone = upperDeadZone;

            return newProfile;
        }
    }
}
