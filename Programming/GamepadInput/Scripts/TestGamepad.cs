//-----------------------------------------------------------
// Xbox 360 Gamepad 'Testing' script
// Bonus Testing code by Lawrence M
//
// Disclaimer:
//-----------------------------------------------------------
// * Code in this script is provided AS IS and is intended
//   to demonstrate using the GamepadManager system and
//   reading input from a gamepad.
//
// * This script is NOT guaranteed to work for 3rd-Party
//   (non Microsoft official) controllers.
//
// * Script tested in Unity 5.0.0f4 'Personal' on a Windows
//   7 PC, using a Microsoft Xbox 360 controller (wired USB).
//
// USE: Attach this script to the main camera or any active
//      gameObject in the scene to use it.
//-----------------------------------------------------------

using UnityEngine;
using System.Collections;

// Sample testing script
public class TestGamepad : MonoBehaviour
{
    public int playerIndex = 1;
    private x360_Gamepad gamepad; // Gamepad instance

    // Use this for initialization
    void Start()
    {
        // Obtain the desired gamepad from GamepadManager
        gamepad = GamepadManager.Instance.GetGamepad(playerIndex);
        //print(playerIndex + " " + gamepad.GetHashCode());
    }

    // Update is called once per frame
    void Update()
    {
        // Sample code to test button input
        if (gamepad.GetStick_L().X > 0)
        {
            print("button pushed");
        }
        // Sample code to test button input
        if (gamepad.GetStick_L().X < 0)
        {
            print("button pushed");
        }
    }

    // Send some rumble events to the gamepad
    void TestRumble()
    {
        //                timer            power         fade
        //gamepad.AddRumble(0.1f, new Vector2(0.9f, 0.9f), 0.5f);
        //gamepad.AddRumble(0.2f, new Vector2(0.5f, 0.5f), 0.2f);
    }
}
