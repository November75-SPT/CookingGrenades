# Cooking Grenades

This mod adds grenade cooking mechanics and realistic fuse time randomization.

## ✨ Features

### Cooking
- Allows **cooking of grenades** (holding before throwing).
- Cooking **only starts after** the safety pull ring removal animation is complete.
- Prevents storing the grenade back into inventory while cooking.

### Cooking Method

- **Hold** the mouse button to prepare to throw the grenade.
- **Press the opposite mouse button** to start cooking.
- **Release** the originally held button to throw the cooked grenade.
- (note) Originally planned to spawn a Unity object that explodes when the timer expires, but due to complexity, the system was simplified:
  - Grenade is **automatically thrown** when the fuse time expires.

### Cooking Notification (Optional)
- Optionally displays a **notification** when cooking begins.

### Realistic Fuse Time Randomization

- Fuse times follow a **normal distribution**.
- Adjustable using the `FuseTimeSpreadFactor` parameter.

### Inventory UI Toggle

- Toggle between showing the **default fuse time** or the **randomized fuse time** in the inventory UI.

### Fuse Time Tester

- tool to **test and visualize** fuse time distributions for debugging and balance.

## Installation

- Unzip `CookingGrenades.zip` into your SPT folder.

## Configuration

Settings are managed in `BepInEx/config/CookingGrenades.cfg`.

### Options

#### 0. Cooking Grenades
- **`EnableCookingNotification`** (Default: `false`):
  - Shows a notification when grenade cooking begins.
- **`ShowDefaultFuseTimeInInventoryUI`** (Default: `true`):
  - If `true`, displays the default fuse time in the inventory UI. If `false`, shows the randomized value.

#### 1. Realistic Fuse Time
- **`RealisticFuseTimeEnable`** (Default: `true`):
  - Enables or disables fuse time randomization.
- **`FuseTimeSpreadFactor`** (Default: `0.085`, Range: `0.001 ~ 0.6`):
  - Controls the variability of fuse times. Higher values increase randomness.

#### 2. Realistic Fuse Time Tester
- **`TimeSimulationValue`** (Default: `5.0`, Range: `1.0 ~ 10.0`):
  - The base fuse time to simulate in the tester.
- **`FuseTimeTestCount`** (Default: `10000`, Range: `1 ~ 100000`):
  - Number of iterations for the fuse time distribution test.
- **`TimeSimulationOutput`** (Default: `false`):
  - Set to `true` to run the simulation once. Results are logged to `BepInEx/LogOutput.log`.

#### 3. Debug
- **`DebugGUI`** (Default: `false`):
  - Enables a GUI to display cooking time (for debugging).
- **`UserWarningConfirmed`** (Default: `false`, Advanced):
  - Confirms user acknowledgment of warnings (hidden in advanced settings).

### Example video
[![IMAGE ALT TEXT HERE](https://img.youtube.com/vi/qB4tEmT6GlQ/0.jpg)](https://www.youtube.com/watch?v=qB4tEmT6GlQ)

## Grenade Cooking Guide

This section outlines real-world safety procedures for "cooking off" grenades, adapted from military training manuals.

### 🍳 Cooking Off

#### Definition
"Cooking off" is the practice of using part of the grenade's delay time **before throwing**, causing it to detonate **aboveground** or **shortly after impact**.

#### Steps for Cooking Off
1. Remove the **safety clip**.
2. Disengage **pull ring from confidence clip** (if equipped).
3. Remove the **pull ring and safety pin**.
4. Release the **safety lever**.
5. Count: **"one thousand one, one thousand two"** (about 2 seconds).
6. Throw the grenade in a **high arc**.
7. Seek **cover** until detonation.

#### Purpose
- Shortens time between impact and explosion.
- Useful for aboveground or near-impact detonation.

#### ⚠️ Warnings on Cooking Off
- **Do not cook off live fragmentation grenades in training.**
- **Use the cook-off procedure only with grenades that have a fuze setting of 4 seconds or greater in a combat environment.**
- **Never cook off** the following grenades:
  - M84 (stun grenade / flashbang)
  - M18, M83, M14 (smoke or special-purpose grenades)
- These often have **1.0 to 2.3 second fuzes** — extremely dangerous.
- Cooking off **exploding, bursting, or burning-type grenades** can cause **death or serious injury**.

#### ✅ Summary Table
| Action        | Description                                             | Risk Level          | Use When?                     |
|---------------|---------------------------------------------------------|----------------------|-------------------------------|
| **Cooking Off** | Delaying throw after pulling pin for quicker detonation | High (unless trained) | **Combat only** with long fuze |

**Reference:** [TC 3-23.30, C1 – *Hand Grenades* (01 Feb 2023)](https://armypubs.army.mil/epubs/DR_pubs/DR_a/ARN37340-TC_3-23.30-001-WEB-3.pdf)

## Credits

- **Provided .csproj**: Michael P. Starkweather, CJ
- **SPT Modding Community**: I referenced the source code of many mods for learning purposes while creating this mod. Sincere thanks to the community for making their mods open-source and helping me learn.
- **Grok AI**: An all-knowing friend who answers any question I have.