using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GUIManager : MonoBehaviour
{
    public Text gameTime;
    public Text p1Score;
    public Text p2Score;
    public GameObject afterActionMenu;

    void Start()
    {
        afterActionMenu.SetActive(false);
    }
}
