using TMPro;
using UnityEngine;

/**
 * Author: Pantelis Andrianakis
 * Date: January 31st 2019
 */
public class InputManager : MonoBehaviour
{
    public TextMeshProUGUI[] keybindButtons = new TextMeshProUGUI[40];
    // Keyboard Keys.
    [HideInInspector]
    public static readonly KeyCode[] KEY_BINDINGS = new KeyCode[40];
    private static readonly KeyCode KEY_TAB = KeyCode.Tab; // No custom keybindings.
    private static readonly KeyCode KEY_NUMLOCK = KeyCode.Numlock; // No custom keybindings.
    private static readonly KeyCode KEY_RETURN = KeyCode.Return; // No custom keybindings.
    private static readonly KeyCode KEY_KEYPAD_RETURN = KeyCode.KeypadEnter; // No custom keybindings.
    private static readonly KeyCode KEY_ESCAPE = KeyCode.Escape; // No custom keybindings.
    // Mouse buttons.
    public static byte LEFT_MOUSE_ID = 0; // No custom keybindings.
    public static byte RIGHT_MOUSE_ID = 1; // No custom keybindings.
    public static byte SIDE_MOUSE_ID = 3; // No custom keybindings.
    // Axis values.
    private static readonly string AXIS_MOUSE_X_VALUE = "Mouse X";
    private static readonly string AXIS_MOUSE_Y_VALUE = "Mouse Y";
    private static readonly string AXIS_MOUSE_SCROLLWHEEL_VALUE = "Mouse ScrollWheel";
    // Continuously pressed.
    public static bool UP_PRESS = false;
    public static bool LEFT_PRESS = false;
    public static bool DOWN_PRESS = false;
    public static bool RIGHT_PRESS = false;
    public static bool SPACE_PRESS = false;
    public static bool LEFT_MOUSE_PRESS = false;
    public static bool RIGHT_MOUSE_PRESS = false;
    // Key pressed.
    public static bool CHARACTER_DOWN = false;
    public static bool INVENTORY_DOWN = false;
    public static bool SKILLS_DOWN = false;
    public static bool SHORTCUT_1_DOWN = false;
    public static bool SHORTCUT_2_DOWN = false;
    public static bool SHORTCUT_3_DOWN = false;
    public static bool SHORTCUT_4_DOWN = false;
    public static bool SHORTCUT_5_DOWN = false;
    public static bool SHORTCUT_6_DOWN = false;
    public static bool SHORTCUT_7_DOWN = false;
    public static bool SHORTCUT_8_DOWN = false;
    public static bool SHORTCUT_9_DOWN = false;
    public static bool SHORTCUT_10_DOWN = false;
    public static bool SHORTCUT_11_DOWN = false;
    public static bool SHORTCUT_12_DOWN = false;
    public static bool RETURN_DOWN = false;
    public static bool ESCAPE_DOWN = false;
    public static bool TAB_DOWN = false;
    public static bool NUMLOCK_DOWN = false;
    public static bool SIDE_MOUSE_DOWN = false;
    public static bool LEFT_MOUSE_DOWN = false;
    // Key released.
    public static bool LEFT_MOUSE_UP = false;
    public static bool RIGHT_MOUSE_UP = false;
    // Axis related.
    public static float AXIS_MOUSE_X = 0;
    public static float AXIS_MOUSE_Y = 0;
    public static float AXIS_MOUSE_SCROLLWHEEL = 0;

    private void Update()
    {
        // On all cases check if escape key is pressed.
        ESCAPE_DOWN = Input.GetKeyDown(KEY_ESCAPE);
        // Do not proceed when keybind canvas is active and waits for a new input.
        if (OptionsManager.Instance.GetKeybindMenuCanvas().gameObject.activeSelf)
        {
            return;
        }

        // Continuously pressed.
        UP_PRESS = Input.GetKey(KEY_BINDINGS[0]) || Input.GetKey(KEY_BINDINGS[1]);
        DOWN_PRESS = Input.GetKey(KEY_BINDINGS[2]) || Input.GetKey(KEY_BINDINGS[3]);
        LEFT_PRESS = Input.GetKey(KEY_BINDINGS[4]) || Input.GetKey(KEY_BINDINGS[5]);
        RIGHT_PRESS = Input.GetKey(KEY_BINDINGS[6]) || Input.GetKey(KEY_BINDINGS[7]);
        SPACE_PRESS = Input.GetKey(KEY_BINDINGS[8]) || Input.GetKey(KEY_BINDINGS[9]);
        LEFT_MOUSE_PRESS = Input.GetMouseButton(LEFT_MOUSE_ID);
        RIGHT_MOUSE_PRESS = Input.GetMouseButton(RIGHT_MOUSE_ID);
        // Key pressed.
        CHARACTER_DOWN = Input.GetKeyDown(KEY_BINDINGS[10]) || Input.GetKeyDown(KEY_BINDINGS[11]);
        INVENTORY_DOWN = Input.GetKeyDown(KEY_BINDINGS[12]) || Input.GetKeyDown(KEY_BINDINGS[13]);
        SKILLS_DOWN = Input.GetKeyDown(KEY_BINDINGS[14]) || Input.GetKeyDown(KEY_BINDINGS[15]);
        SHORTCUT_1_DOWN = Input.GetKeyDown(KEY_BINDINGS[16]) || Input.GetKeyDown(KEY_BINDINGS[17]);
        SHORTCUT_2_DOWN = Input.GetKeyDown(KEY_BINDINGS[18]) || Input.GetKeyDown(KEY_BINDINGS[19]);
        SHORTCUT_3_DOWN = Input.GetKeyDown(KEY_BINDINGS[20]) || Input.GetKeyDown(KEY_BINDINGS[21]);
        SHORTCUT_4_DOWN = Input.GetKeyDown(KEY_BINDINGS[22]) || Input.GetKeyDown(KEY_BINDINGS[23]);
        SHORTCUT_5_DOWN = Input.GetKeyDown(KEY_BINDINGS[24]) || Input.GetKeyDown(KEY_BINDINGS[25]);
        SHORTCUT_6_DOWN = Input.GetKeyDown(KEY_BINDINGS[26]) || Input.GetKeyDown(KEY_BINDINGS[27]);
        SHORTCUT_7_DOWN = Input.GetKeyDown(KEY_BINDINGS[28]) || Input.GetKeyDown(KEY_BINDINGS[29]);
        SHORTCUT_8_DOWN = Input.GetKeyDown(KEY_BINDINGS[30]) || Input.GetKeyDown(KEY_BINDINGS[31]);
        SHORTCUT_9_DOWN = Input.GetKeyDown(KEY_BINDINGS[32]) || Input.GetKeyDown(KEY_BINDINGS[33]);
        SHORTCUT_10_DOWN = Input.GetKeyDown(KEY_BINDINGS[34]) || Input.GetKeyDown(KEY_BINDINGS[35]);
        SHORTCUT_11_DOWN = Input.GetKeyDown(KEY_BINDINGS[36]) || Input.GetKeyDown(KEY_BINDINGS[37]);
        SHORTCUT_12_DOWN = Input.GetKeyDown(KEY_BINDINGS[38]) || Input.GetKeyDown(KEY_BINDINGS[39]);
        RETURN_DOWN = Input.GetKeyDown(KEY_RETURN) || Input.GetKeyDown(KEY_KEYPAD_RETURN);
        TAB_DOWN = Input.GetKeyDown(KEY_TAB);
        NUMLOCK_DOWN = Input.GetKeyDown(KEY_NUMLOCK);
        SIDE_MOUSE_DOWN = Input.GetMouseButtonDown(SIDE_MOUSE_ID);
        LEFT_MOUSE_DOWN = Input.GetMouseButtonDown(LEFT_MOUSE_ID);
        // Key released.
        LEFT_MOUSE_UP = Input.GetMouseButtonUp(LEFT_MOUSE_ID);
        RIGHT_MOUSE_UP = Input.GetMouseButtonUp(RIGHT_MOUSE_ID);
        // Axis values.
        AXIS_MOUSE_X = Input.GetAxis(AXIS_MOUSE_X_VALUE);
        AXIS_MOUSE_Y = Input.GetAxis(AXIS_MOUSE_Y_VALUE);
        AXIS_MOUSE_SCROLLWHEEL = Input.GetAxis(AXIS_MOUSE_SCROLLWHEEL_VALUE);

        if (OptionsManager.Instance.GetOptionsCanvas().enabled)
        {
            RefreshButtonTextValues();
        }
    }

    public void ResetKeyCodes()
    {
        SetKeybind(0, KeyCode.W); // KEY_UP_1
        SetKeybind(1, KeyCode.UpArrow); // KEY_UP_2
        SetKeybind(2, KeyCode.S); // KEY_DOWN_1
        SetKeybind(3, KeyCode.DownArrow); // KEY_DOWN_2
        SetKeybind(4, KeyCode.A); // KEY_LEFT_1
        SetKeybind(5, KeyCode.LeftArrow); // KEY_LEFT_2
        SetKeybind(6, KeyCode.D); // KEY_RIGHT_1
        SetKeybind(7, KeyCode.RightArrow); // KEY_RIGHT_2
        SetKeybind(8, KeyCode.Space); // KEY_JUMP_1
        SetKeybind(9, KeyCode.None); // KEY_JUMP_2
        SetKeybind(10, KeyCode.C); // KEY_CHARACTER_1
        SetKeybind(11, KeyCode.None); // KEY_CHARACTER_2
        SetKeybind(12, KeyCode.I); // KEY_INVENTORY_1
        SetKeybind(13, KeyCode.None); // KEY_INVENTORY_2
        SetKeybind(14, KeyCode.K); // KEY_SKILLS_1
        SetKeybind(15, KeyCode.None); // KEY_SKILLS_2
        SetKeybind(16, KeyCode.Alpha1); // KEY_SHORTCUT_1_1
        SetKeybind(17, KeyCode.None); // KEY_SHORTCUT_1_2
        SetKeybind(18, KeyCode.Alpha2); // KEY_SHORTCUT_2_1
        SetKeybind(19, KeyCode.None); // KEY_SHORTCUT_2_2
        SetKeybind(20, KeyCode.Alpha3); // KEY_SHORTCUT_3_1
        SetKeybind(21, KeyCode.None); // KEY_SHORTCUT_3_2
        SetKeybind(22, KeyCode.Alpha4); // KEY_SHORTCUT_4_1
        SetKeybind(23, KeyCode.None); // KEY_SHORTCUT_4_2
        SetKeybind(24, KeyCode.Alpha5); // KEY_SHORTCUT_5_1
        SetKeybind(25, KeyCode.None); // KEY_SHORTCUT_5_2
        SetKeybind(26, KeyCode.Alpha6); // KEY_SHORTCUT_6_1
        SetKeybind(27, KeyCode.None); // KEY_SHORTCUT_6_2
        SetKeybind(28, KeyCode.Alpha7); // KEY_SHORTCUT_7_1
        SetKeybind(29, KeyCode.None); // KEY_SHORTCUT_7_2
        SetKeybind(30, KeyCode.Alpha8); // KEY_SHORTCUT_8_1
        SetKeybind(31, KeyCode.None); // KEY_SHORTCUT_8_2
        SetKeybind(32, KeyCode.Alpha9); // KEY_SHORTCUT_9_1
        SetKeybind(33, KeyCode.None); // KEY_SHORTCUT_9_2
        SetKeybind(34, KeyCode.Alpha0); // KEY_SHORTCUT_10_1
        SetKeybind(35, KeyCode.None); // KEY_SHORTCUT_10_2
        SetKeybind(36, KeyCode.Minus); // KEY_SHORTCUT_11_1
        SetKeybind(37, KeyCode.None); // KEY_SHORTCUT_11_2
        SetKeybind(38, KeyCode.Equals); // KEY_SHORTCUT_12_1
        SetKeybind(39, KeyCode.None); // KEY_SHORTCUT_12_2

        // Update player options.
        NetworkManager.SendPacket(new PlayerOptionsUpdate());
    }

    public void RefreshButtonTextValues()
    {
        for (int i = 0; i < keybindButtons.Length; i++)
        {
            if (!keybindButtons[i].text.Equals(KEY_BINDINGS[i].ToString()))
            {
                keybindButtons[i].text = KEY_BINDINGS[i].ToString();
            }
        }
    }

    public static int SetKeybind(int buttonId, KeyCode keycode)
    {
        // Check for restricted keys.
        if (keycode == KeyCode.Escape //
            || keycode == KeyCode.Return //
            || keycode == KeyCode.KeypadEnter //
            || keycode == KeyCode.Numlock //
            || keycode == KeyCode.Tab //
            || keycode == KeyCode.Mouse0 //
            || keycode == KeyCode.Mouse1 //
            || keycode == KeyCode.Mouse2 //
            || keycode == KeyCode.Mouse3 //
            || keycode == KeyCode.Mouse4 //
            || keycode == KeyCode.Mouse5 //
            || keycode == KeyCode.Mouse6)
        {
            return 0; // This key cannot be bound.
        }

        // Check for already bound keys.
        for (int i = 0; i < KEY_BINDINGS.Length; i++)
        {
            if (KEY_BINDINGS[i] == keycode && keycode != KeyCode.None)
            {
                return 1; // Key already bound.
            }
        }

        // Set keybind.
        KEY_BINDINGS[buttonId] = keycode;
        return 2; // Success.
    }
}
