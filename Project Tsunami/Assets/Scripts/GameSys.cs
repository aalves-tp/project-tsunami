using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSys : MonoBehaviour
{
    [Header("Debug")]
    public GameObject statisticsBox;
    public Text statisticsText;
    [Header("UI Elements")]
    public GameObject pauseMenu;
    public GameObject cheatBox;
    public Slider mouseSensitivtySlider;
    public static bool isPaused;
    [Header("Player Settings")]
    public Rigidbody player;
    Player playerCode;
    public static float mouseSensitivity;
    [Header("Physics")]
    public PhysicMaterial rampPhysicsMaterial;
    public static PhysicMaterial getRampPhysicsMaterial;


    // Start is called before the first frame update
    void Start()
    {
        mouseSensitivity = mouseSensitivtySlider.value;
        playerCode = player.GetComponent<Player>();
        Cursor.lockState = CursorLockMode.Locked;
        getRampPhysicsMaterial = rampPhysicsMaterial;
    }

    // Update is called once per frame
    void Update()
    {
        pauseMenu.SetActive(isPaused);
        mouseSensitivity = mouseSensitivtySlider.value;

        statisticsText.text = 
        "Current Velocity: " + playerCode.playerPhysics.velocity.magnitude.ToString("F2") + "\n" +
        "Air Strafe: " + playerCode.airStrafe + "\n" +
        "Air Acceleration: " + playerCode.airAcceleration + "\n" +
        "Player View Orientation: " + playerCode.playerViewOrientation + "\n" +
        "Ramp Friction: " + rampPhysicsMaterial.dynamicFriction + "\n";

        if(Input.GetButtonDown("Fire2"))
        {
            Pause();
            if(!isPaused)
            {
                statisticsBox.SetActive(true);
            }else{
                statisticsBox.SetActive(false);
            }
        }

        if(Input.GetButtonDown("Cheat Menu") && isPaused)
        {
            cheatBox.SetActive(!cheatBox.activeSelf);
        }else if(Input.GetButtonDown("Cheat Menu") && !isPaused)
        {
            statisticsBox.SetActive(!statisticsBox.activeSelf);
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
            Cursor.lockState = CursorLockMode.None;
            break;

            case false:
            Cursor.lockState = CursorLockMode.Locked;
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
        if(cheat.ToLower() == "homesick")
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
