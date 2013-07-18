File.open "InputManager.asset", "w" do |file|
  file.write "%YAML 1.1\n%TAG !u! tag:unity3d.com,2011:\n--- !u!13 &1\nInputManager:\n  m_ObjectHideFlags: 0\n  m_Axes:\n"
  (1..10).each do |joystick|
    (0..9).each do |analog|
      file.write <<-eos
  - serializedVersion: 1
    m_Name: joystick #{joystick} analog #{analog}
    descriptiveName:
    descriptiveNegativeName:
    negativeButton:
    positiveButton:
    altNegativeButton:
    altPositiveButton:
    gravity: 10
    dead: .001
    sensitivity: 1
    snap: 0
    invert: 0
    type: 2
    axis: #{analog}
    joyNum: #{joystick}
      eos
    end
  end
end