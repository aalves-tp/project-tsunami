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
    public GameObject cheatBox;
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
                             "Player Inclination: " + playerCode.playerViewRotation + "\n" +
                             "Ramp Exit Impulse: " + playerCode.rampExitImpulse + "\n" +
                             "Inclination Multiplier " + playerCode.inclinationMultiplier + "\n" +
                             "Air Strafe " + playerCode.airStrafe + "\n" +
                             "Ramp Strafe: " + playerCode.rampStrafe;

        if(Input.GetButtonDown("Cheat Menu") && isPaused)
        {
            cheatBox.SetActive(!cheatBox.activeSelf);
        }

        if(Input.GetButtonDown("Submit"))
        {
            ReadCheat(cheatBox.GetComponentsInChildren<Text>()[1].text);
        }
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

    public void ReadCheat(string cheat)
    {
        if(cheat.ToLower() == "crick")
        {
            playerCode.playerViewRotation = 0;
            cheatBox.SetActive(!cheatBox.activeSelf);
        }else if(cheat.ToLower() == "homesick")
        {
            playerCode.transform.position = playerCode.startPosition;
            playerCode.GetComponent<Rigidbody>().velocity = Vector3.zero;
            cheatBox.GetComponent<InputField>().text = "";
            cheatBox.SetActive(!cheatBox.activeSelf);
        }

        else
        {
            cheatBox.GetComponent<InputField>().text = "INVALID CHEAT CODE, TRY AGAIN";
        }
    }
}
