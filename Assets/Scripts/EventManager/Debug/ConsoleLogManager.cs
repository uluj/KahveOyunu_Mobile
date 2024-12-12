using UnityEngine.UI;
using UnityEngine;

public class ConsoleLogManager : MonoBehaviour
{
    public GameObject consoleCanvas; // Reference to the canvas with console logs
    public Text consoleLogText; // Reference to the text component for logs

    //
    private bool isConsoleVisible =false; // Flag to track console visibility
    private void OnEnable()
    {
        CustomEventManager.OnLogMessage += LogToConsole;
    }
    private void OnDisable()
    {
        CustomEventManager.OnLogMessage -= LogToConsole;    
    }
    private void Start()
    {
        if (consoleCanvas != null)
        {
            consoleCanvas.SetActive(false); // Hide the console initially
        }
    }

    public void ToggleConsole()
    {
        isConsoleVisible = !isConsoleVisible;
        if (consoleLogText != null)
        {
            consoleCanvas.SetActive(isConsoleVisible);
        }
    }
    public void LogToConsole(string message)
    {
        if (consoleLogText != null)
        {
            consoleLogText.text += message + "\n"; // Append the message to the log
        }
        Debug.Log(message); // Log the message to the Unity console
    }

}
