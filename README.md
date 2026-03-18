# Haptic

Unity + Arduino haptics prototype built around an Arduino Uno, a small servo motor, and serial communication via Ardity.

## Project Goal

This project explores a simple haptic feedback pipeline:

1. Arduino drives a servo motor.
2. Unity sends commands to Arduino through the serial port.
3. A virtual scene triggers physical feedback on the servo.

The current repository includes:

- A Unity 2022 project
- Arduino sketches for standalone servo testing and Unity-driven control
- Scene scripts for keyboard movement, collision feedback, proximity feedback, and a Level ++ soft-shape feedback mode

## Hardware

- Arduino Uno
- Micro servo motor
- USB cable
- Jumper wires

## Wiring

Typical servo wiring:

- Servo `GND` (brown/black) -> Arduino `GND`
- Servo `5V` (red) -> Arduino `5V`
- Servo `Signal` (orange/yellow) -> Arduino `D9`

The Arduino sketches in this repository expect the servo signal on pin `9`.

## Arduino Files

### `SimpleServo.ino`

Use this first to verify that:

- the servo is wired correctly
- the board is detected by Arduino IDE
- the servo can move to a target angle

This is the hardware sanity check sketch.

### `ServoToUnity_Student.ino`

Use this for Unity integration.

It:

- reads serial commands from Unity
- expects commands in the form `bXXXe`
- parses the integer angle
- constrains it to `0..180`
- writes the angle to the servo

Examples:

- `b0e`
- `b45e`
- `b90e`
- `b180e`

## Unity Scripts

Relevant scripts in `Assets/Ardity/Scripts/`:

- `SendToArduino.cs`: sends servo angles over serial
- `SimpleKeyboardMover.cs`: moves the avatar with keyboard input
- `CollisionHapticFeedback.cs`: basic collision-triggered feedback
- `ProximityServoSweep.cs`: proximity-based oscillation
- `SoftShapeHapticFeedback.cs`: Level ++ continuous soft-object + shape simulation

## Setup

### 1. Arduino IDE

1. Connect Arduino Uno by USB.
2. Open Arduino IDE.
3. Select board: `Arduino Uno`
4. Select the correct serial port in `Tools -> Port`

### 2. Hardware Test

1. Open `SimpleServo.ino`
2. Upload it
3. Change the target angle if needed
4. Confirm that the servo moves

If this fails, do not continue to Unity until wiring and power are stable.

### 3. Unity-Controlled Servo

1. Open `ServoToUnity_Student.ino`
2. Upload it to the Arduino
3. Open the Serial Monitor at `9600`
4. Send `b45e`
5. Confirm that the servo moves to about 45 degrees
6. Close the Serial Monitor

The Serial Monitor must be closed before Unity tries to open the same COM port.

### 4. Unity Project

1. Open the Unity project
2. Open `Assets/Scenes/SampleScene.unity`
3. Find the object with `SerialController`
4. Set:
   - `Port Name` to the actual Arduino COM port
   - `Baud Rate` to `9600`

### 5. Basic Demo

For the basic collision version:

1. Put `SendToArduino`, `SimpleKeyboardMover`, `Rigidbody`, and a `Collider` on the avatar
2. Put a `Collider` on the cube
3. Add `CollisionHapticFeedback` to the avatar
4. Set the cube tag to `HapticTarget`
5. Enter Play mode
6. Move the avatar with arrow keys or `WASD`

Result:

- touching the cube rotates the servo
- leaving the cube resets it

## Level ++ Mode

The repository also includes a more advanced haptic mode: `SoftShapeHapticFeedback.cs`

This mode is intended to simulate:

- approach feedback inside an activation range
- a soft object that becomes stronger as you press further in
- a shaped surface where the center feels stronger than the edges

Recommended setup:

1. Add `SendToArduino` and `SimpleKeyboardMover` to the avatar
2. Disable other haptic scripts on the avatar
3. Add `SoftShapeHapticFeedback` to the avatar
4. Assign the target cube transform and collider
5. Enter Play mode

The script assumes a box-like target and computes:

- proximity feedback outside the object
- press depth once the avatar enters the object volume
- an additional shape factor based on where the avatar touches the object

## Important Limitation

This project uses a standard positional servo, not a continuous rotation servo.

That means:

- it can move to angles
- it can oscillate between angles
- it cannot behave like a DC motor that spins forever at arbitrary speed

So when the project talks about stronger or faster feedback, it usually means:

- larger angle changes
- faster oscillation
- stronger positional resistance impression

## Repository Notes

Ignored from version control:

- `Library/`
- `Temp/`
- `Logs/`
- `obj/`
- Unity build outputs
- Arduino IDE installer executables

## Suggested Next Work

- Tune the haptic mapping values for your physical servo
- Add a UI text overlay showing current angle / distance / pressure
- Build a dedicated Level ++ scene for soft-object and shape exploration
