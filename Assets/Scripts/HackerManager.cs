using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HackerManager : MonoBehaviour
{
    public TextMeshProUGUI console;
    public TextMeshProUGUI helpInfo;

    [SerializeField] private Image caret;

    private string currentInput = "";

    private string advanceCode = null;

    private Coroutine clearHelpInfoCoroutine;


    public string _testPath;

    void Start()
    {
        console.text = "> ";
        UpdateCaret();
        _testPath = Application.dataPath + "/CSV/testingcsv.csv";
        var testing = CSVReader.ReadCSV(_testPath);
    }

    void Update()
    {
        foreach (char c in Input.inputString)
        {
            if (c == '\b') // has backspace/delete been pressed?
            {
                Backspace();
            }
            else if ((c == '\n') || (c == '\r')) // enter/return
            {
                SubmitInput(currentInput);
            }
            else
            {
                AddInput(c);
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SetAdvanceCode("COPY ME NOW PLEASE");
        }
    }

    private void AddInput(char input, bool consoleOnly = false)
    {
        if (!consoleOnly)
        {
            currentInput += input;
        }
        console.text += input;
        UpdateCaret();
    }

    private void AddInput(string input, bool consoleOnly = false)
    {
        if (!consoleOnly)
        {
            currentInput += input;
        }
        console.text += input;
        UpdateCaret();
    }

    private void Backspace()
    {
        if (currentInput.Length != 0)
        {
            console.text = console.text.Substring(0, console.text.Length - 1);
            currentInput = currentInput.Substring(0, currentInput.Length - 1);
            UpdateCaret();
        }
    }

    private void UpdateCaret()
    {
        StartCoroutine(UpdateCaretDeferred());
    }

    private IEnumerator UpdateCaretDeferred()
    {
        yield return new WaitForEndOfFrame();
        if (!string.IsNullOrEmpty(console.text) && console.textInfo != null && console.textInfo.characterInfo != null) // Will go in here 99.99% of the time
        {
            var lastCharPosition = console.textInfo.characterInfo[console.textInfo.characterCount - 1].bottomRight;
            var bounds = console.textBounds;
            caret.transform.localPosition = new Vector3(lastCharPosition.x + 8, bounds.min.y + (((RectTransform)caret.transform).sizeDelta.y / 2), 1);
        }
        else
        {
            var bounds = console.textBounds;
            caret.transform.localPosition = new Vector3(bounds.max.x + (((RectTransform)caret.transform).sizeDelta.x / 1.5f), bounds.min.y + (((RectTransform)caret.transform).sizeDelta.y/2), 0);
        }
    }

    private void SubmitInput(string input)
    {
        Debug.Log("User entered cheat: " + currentInput);

        AnalyseInput(input);

        AddInput("\n", consoleOnly: true);
        AddInput("> ", consoleOnly: true);
        currentInput = "";
        UpdateCaret();
    }

    private void AnalyseInput(string input)
    {
        input = input.ToLower(); // TODO may not be needed
        if (advanceCode != null && input.Equals(advanceCode, StringComparison.OrdinalIgnoreCase))
        {
            DisplayInfoMessage("Door Opened!!", 5, overrideAdvanceCode: true);
            advanceCode = null;
        }

        if (input == StringConstants.CheatCodes.CheatHealth)
        {
            Debug.LogWarning("Cheat activated: Health");
            DisplayInfoMessage("CHEAT ACTIVATED: HEALTH ADDED", 3);
        }
    }

    private void DisplayInfoMessage(string info, float? secondsToDisplay = null, bool overrideAdvanceCode = false)
    {
        if (advanceCode != null && !overrideAdvanceCode)
        {
            return;
        }

        if (clearHelpInfoCoroutine != null)
        {
            StopCoroutine(clearHelpInfoCoroutine);
        }

        helpInfo.text = info;

        if (secondsToDisplay.HasValue)
        {
            clearHelpInfoCoroutine = StartCoroutine(DeferredInfoMessageClear(secondsToDisplay.Value));
        }
    }

    private IEnumerator DeferredInfoMessageClear(float waitSeconds)
    {
        yield return new WaitForSeconds(waitSeconds);
        helpInfo.text = "";
        clearHelpInfoCoroutine = null;
    }

    private void SetAdvanceCode(string code)
    {
        advanceCode = code;
        DisplayInfoMessage(code, overrideAdvanceCode: true);
    }
}
