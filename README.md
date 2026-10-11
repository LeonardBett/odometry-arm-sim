# Autonomous Drone & Simulated Robotic Arm (Unity 3D)

M2 Computer Science project (embedded systems, robotics), **work in progress**.

The goal is to build a simulated robotic arm in Unity 3D, driven by an autonomous drone that localizes itself through **mathematical odometry** (pose estimation using matrix computation, no computer vision). The simulated scene is a fishing scenario: a boat, a robotic arm, and catchable fish.

## Current status

**Done**
- Multi-joint robotic arm (base, shoulder, elbow, wrist) built with Unity's `ArticulationBody` physics
- Manual keyboard control of each joint (`Controller.cs`)
- Automated joint motion toward a target angle with configurable speed (`TestMove.cs`)
- Object-grabbing mechanism: detects nearby "catchable" objects with a physics overlap check and attaches them to the arm's hook point (`Grappin.cs`)
- Simulation scene with a boat and animated fish assets

**Planned**
- Autonomous drone in the simulation
- Odometry and pose estimation using matrix libraries (transformations, state propagation)
- Kinematics of the arm (forward/inverse) with matrix math
- Coordinating the drone's estimated pose with the arm's movements
- Embedded-systems integration

## Tech stack

- Unity 2022.3 LTS (3D, `ArticulationBody` physics)
- C#
- Linear algebra / matrix library (to be chosen)

## Getting started

1. Install Unity Hub and Unity **2022.3.19f1**
2. Clone this repository
3. Open the project folder from Unity Hub
4. Open `Assets/Scenes/SampleScene.unity` and press Play

**Arm controls:** Left/Right arrows = base, Up/Down arrows = shoulder, W/S = elbow, A/D = wrist.

## Project structure

```
Assets/
├── Controller.cs   # Keyboard control of the arm joints
├── TestMove.cs     # Automated joint movement
├── Grappin.cs      # Object detection and grabbing
└── Scenes/         # Simulation scene
```

## Credits

3D assets: *Quirky Series Ultimate* (animals) and *Low Poly Single Boat* (third-party packs).

---

*This README was written with the help of Claude (Anthropic).*
