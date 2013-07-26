require "rubygems"
require "json"
require "erubis"


def fix_mapping_attribute(k, v)
  v = "\"#{v}\"" unless ["target", "button", "invert", "ranged"].include? k
  v = "InputControlType.#{v}" if k == "target"
  v = "InputControlMapping.Range.Positive" if k == "ranged" && v == "+"
  v = "InputControlMapping.Range.Negative" if k == "ranged" && v == "-"
  v
end


output_path = "../Assets/InControl/Unity/DeviceProfiles"
template = DATA.read
Dir["profiles/*.json"].each do |path|
  name = File.basename(path, ".json")
  puts path

  json = File.read(path)
  data = JSON.parse(json)
  data["meta"] ||= ""
  data["class_name"] = name + "Profile"
  data["supportedPlatforms"] ||= []
  data["joystickNames"] ||= []

  data["buttonMappings"].each do |m|
    m.each do |k, v|
      m[k] = fix_mapping_attribute(k, v)
    end
  end

  data["analogMappings"].each do |m|
    m.each do |k, v|
      m[k] = fix_mapping_attribute(k, v)
    end
  end

  eruby = Erubis::Eruby.new(template)
  File.open File.join(output_path, data["class_name"] + ".cs"), "w" do |file|
    file.write eruby.result(data)
  end
end


__END__
using System;
using System.Collections;
using System.Collections.Generic;


namespace InControl
{
	[AutoDiscover]
	public class <%= class_name %> : UnityInputDeviceProfile
	{
		public <%= class_name %>()
		{
			Name = "<%= name %>";
			Meta = "<%= meta %>";

			<% unless supportedPlatforms.empty? %>
			SupportedPlatforms = new[]
			{
				<%= '"' + supportedPlatforms.join("\",\n\t\t\t\t\"") + '"' %>
			};

			<% end %>
			<% unless joystickNames.empty? %>
			JoystickNames = new[]
			{
				<%= '"' + joystickNames.join("\",\n\t\t\t\t\"") + '"' %>
			};

			<% end %>
			Sensitivity = <%= sensitivity %>f;
			DeadZone = <%= deadZone %>f;

			ButtonMappings = new[]
			{
				<% buttonMappings.each_with_index do |m, i| %>
				new InputControlButtonMapping()
				{
					<% j = 0; m.each do |k, v| %>
					<%= k.capitalize %> = <%= v %><% if j < m.size - 1 %>,<% end; j += 1 %>
					<% end %>
				}<% if i < buttonMappings.size - 1 %>,<% end %>
				<% end %>
			};

			AnalogMappings = new InputControlAnalogMapping[]
			{
				<% analogMappings.each_with_index do |m, i| %>
				new InputControlAnalogMapping()
				{
					<% j = 0; m.each do |k, v| %>
					<%= k.capitalize %> = <%= v %><% if j < m.size - 1 %>,<% end; j += 1 %>
					<% end %>
				}<% if i < analogMappings.size - 1 %>,<% end %>
				<% end %>
			};
		}
	}
}

