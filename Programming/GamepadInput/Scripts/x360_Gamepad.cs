﻿using UnityEngine;
using XInputDotNetPure;
using System.Collections.Generic;

//https://lcmccauley.wordpress.com/2015/04/20/x360-input-tutorial-unity-p1/

// Stores states of a single gamepad button
public struct xButton
{
    public ButtonState prev_state;
    public ButtonState state;
}

// Stores state of a single gamepad trigger
public struct TriggerState
{
    public float prev_value;
    public float current_value;
}

// Rumble (vibration) event
class xRumble
{
    public float timer;    // Rumble timer
    public float duration; // Fade-out (in seconds)
    public Vector2 power;  // Intensity of rumble

    // Decrease timer
    public void Update()
    {
        this.timer -= Time.deltaTime;
    }
}

public class x360_Gamepad
{
    private GamePadState prev_state; // Previous gamepad state
    private GamePadState state;      // Current gamepad state

    private int gamepadIndex;        // Numeric index (1,2,3 or 4
    private PlayerIndex playerIndex;    // XInput 'Player' index
    private List<xRumble> rumbleEvents; // Stores rumble events

    // Button input map (explained soon!)
    private Dictionary<string, xButton> inputMap;

    // States for all buttons/inputs supported
    private xButton A, B, X, Y; // Action (face) buttons
    private xButton DPad_Up, DPad_Down, DPad_Left, DPad_Right;

    private xButton Guide;       // Xbox logo button}
    private xButton Back, Start;
    private xButton L3, R3;      // Thumbstick buttons
    private xButton LB, RB;      // 'Bumper' (shoulder) buttons
    private TriggerState LT, RT; // Triggers

    // Return numeric gamepad index
    public int Index { get { return gamepadIndex; } }

    // Return gamepad connection state
    public bool IsConnected { get { return state.IsConnected; } }


    // Constructor
    public x360_Gamepad(int index)
    {
        // Set gamepad index
        gamepadIndex = index - 1;
        playerIndex = (PlayerIndex)gamepadIndex;

        // Create rumble container and input map
        rumbleEvents = new List<xRumble>();
        inputMap = new Dictionary<string, xButton>();
        CreateDictionary();
    }

    // Update gamepad state
    public void Update()
    {
        // Get current state
        state = GamePad.GetState(playerIndex);

        // Check gamepad is connected
        if (state.IsConnected)
        {
            UpdateButtonStates();
            UpdateInputMap();
            HandleRumble();
        }
    }

    // Refresh previous gamepad state
    public void Refresh()
    {
        // This 'saves' the current state for next update
        prev_state = state;

        // Check gamepad is connected
        if (state.IsConnected)
        {
            UpdateButtonStates();
            UpdateInputMap();
        }
    }

    //Updates the button states
    private void UpdateButtonStates()
    {
        A.state = state.Buttons.A;
        B.state = state.Buttons.B;
        X.state = state.Buttons.X;
        Y.state = state.Buttons.Y;

        DPad_Up.state = state.DPad.Up;
        DPad_Down.state = state.DPad.Down;
        DPad_Left.state = state.DPad.Left;
        DPad_Right.state = state.DPad.Right;

        Guide.state = state.Buttons.Guide;
        Back.state = state.Buttons.Back;
        Start.state = state.Buttons.Start;
        L3.state = state.Buttons.LeftStick;
        R3.state = state.Buttons.RightStick;
        LB.state = state.Buttons.LeftShoulder;
        RB.state = state.Buttons.RightShoulder;

        // Read trigger values into trigger states
        LT.current_value = state.Triggers.Left;
        RT.current_value = state.Triggers.Right;
    }

    // Return button state
    public bool GetButton(string button)
    {
        return inputMap[button].state
              == ButtonState.Pressed ? true : false;
    }

    // Return button state - on CURRENT frame -- checks previous and current states
    public bool GetButtonDown(string button)
    {
        return (inputMap[button].prev_state == ButtonState.Released &&
                inputMap[button].state == ButtonState.Pressed) ? true : false;
    }

    // Add a rumble event to the gamepad
    public void AddRumble(float timer, Vector2 power, float fadeTime)
    {
        xRumble rumble = new xRumble();

        rumble.timer = timer;
        rumble.power = power;
        rumble.duration = fadeTime;

        rumbleEvents.Add(rumble);
    }

    // Update and apply rumble events
    private void HandleRumble()
    {
        // Ignore if there are no events
        if (rumbleEvents.Count > 0)
        {
            Vector2 currentPower = new Vector2(0, 0);

            for (int i = 0; i < rumbleEvents.Count; ++i)
            {
                // Update current event
                rumbleEvents[i].Update();

                if (rumbleEvents[i].timer > 0)
                {
                    // Calculate current power
                    float timeLeft = Mathf.Clamp(rumbleEvents[i].timer / rumbleEvents[i].duration, 0f, 1f);
                    currentPower = new Vector2(Mathf.Max(rumbleEvents[i].power.x * timeLeft, currentPower.x),
                                               Mathf.Max(rumbleEvents[i].power.y * timeLeft, currentPower.y));

                    // Apply vibration to gamepad motors
                    GamePad.SetVibration(playerIndex, currentPower.x, currentPower.y);
                }
                else
                {
                    // Remove expired event
                    rumbleEvents.Remove(rumbleEvents[i]);
                }
            }
        }
    }

    public bool ThumbsticksPressed()
    {
        if(GetStick_L().X > 0 || GetStick_L().X < 0 || GetStick_L().Y > 0 || GetStick_L().Y < 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // Return axes of left thumbstick
    public GamePadThumbSticks.StickValue GetStick_L()
    {
        return state.ThumbSticks.Left;
    }

    // Return axes of right thumbstick
    public GamePadThumbSticks.StickValue GetStick_R()
    {
        return state.ThumbSticks.Right;
    }

    // Return axis of left trigger
    public float GetTrigger_L() { return state.Triggers.Left; }

    // Return axis of right trigger
    public float GetTrigger_R() { return state.Triggers.Right; }

    // Check if left trigger was tapped - on CURRENT frame
    public bool GetTriggerTap_L()
    {
        return (LT.prev_value == 0f && LT.current_value >= 0.1f) ? true : false;
    }

    // Check if right trigger was tapped - on CURRENT frame
    public bool GetTriggerTap_R()
    {
        return (RT.prev_value == 0f && RT.current_value >= 0.1f) ? true : false;
    }

    #region Input Map
    // Update input map
    private void UpdateInputMap()
    {
        inputMap["A"] = A;
        inputMap["B"] = B;
        inputMap["X"] = X;
        inputMap["Y"] = Y;

        inputMap["DPad_Up"] = DPad_Up;
        inputMap["DPad_Down"] = DPad_Down;
        inputMap["DPad_Left"] = DPad_Left;
        inputMap["DPad_Right"] = DPad_Right;

        inputMap["Guide"] = Guide;
        inputMap["Back"] = Back;
        inputMap["Start"] = Start;

        // Thumbstick buttons
        inputMap["L3"] = L3;
        inputMap["R3"] = R3;

        // Shoulder ('bumper') buttons
        inputMap["LB"] = LB;
        inputMap["RB"] = RB;
    }
    
    private void CreateDictionary()
    {
        inputMap.Add("A", A);
        inputMap.Add("B", B);
        inputMap.Add("X", X);
        inputMap.Add("Y", Y);

        inputMap.Add("DPad_Up", DPad_Up);
        inputMap.Add("DPad_Down", DPad_Down);
        inputMap.Add("DPad_Left", DPad_Left);
        inputMap.Add("DPad_Right", DPad_Right);

        inputMap.Add("Guide", Guide);
        inputMap.Add("Back", Back);
        inputMap.Add("Start", Start);

        inputMap.Add("L3", L3);
        inputMap.Add("R3", R3);
        inputMap.Add("LB", LB);
        inputMap.Add("RB", RB);
    }
    #endregion
}
