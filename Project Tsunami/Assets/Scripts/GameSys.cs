using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSys : MonoBehaviour
{
    [Header("Debug")]
    public Text velocityDebug;
    [Header("UI Elements")]
    public GameObject pauseMenu;
    public Slider mouseSensitivtySlider;
    public static bool isPaused;
    [Header("Player Settings")]
    public Rigidbody player;
    Player playerCode;
    public static float mouseSensitivity;

    // Start is called before the first frame update
    void Start()
    {
        mouseSensitivity = mouseSensitivtySlider.value;
        playerCode = player.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        pauseMenu.SetActive(isPaused);
        mouseSensitivity = mouseSensitivtySlider.value;
        velocityDebug.text = "Player velocity: " +  player.velocity.magnitude.ToString() + "\n" +
                             "Ramp Acceleration: " + playerCode.rampAcceleration + "\n" +
                             "Player Inclination: " + playerCode.playerViewRotation + "\n" +
                             "Ramp Exit Impulse: " + playerCode.rampExitImpulse + "\n" +
                             "Inclination Multiplier " + playerCode.inclinationMultiplier + "\n" +
                             "Ramp Strafe: " + playerCode.rampStrafe;
    }

    public static bool Pause()
    {
        isPaused = !isPaused;
        switch (isPaused)
        {
            case true:
            Time.timeScale = 0;
            break;

            case false:
            Time.timeScale = 1;
            break;

        }
        return isPaused;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
