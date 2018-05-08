using UnityEngine;
using System.Collections;

public class RefreshGamepads : MonoBehaviour
{
    void LateUpdate ()
    {
        GamepadManager.Instance.Refresh();
    }
}
