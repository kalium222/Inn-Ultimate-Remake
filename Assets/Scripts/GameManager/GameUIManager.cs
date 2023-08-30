using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
using TMPro;
using Dialoues;

public class GameUIManager : MonoBehaviour
{

    // objects that hold UI elements
    // UI element that shows up when player can interact, use, or exchange
    [HideInInspector]
    public GameObject Interact;
    [HideInInspector]
    public GameObject Use;
    [HideInInspector]
    public GameObject Exchange;
    // When changing scene, the curtain will fade in and out
    [HideInInspector]
    public GameObject CurtainCanvas;
    // The duration of the fade, set in inspector
    public float FadeDuration = 0.5f;
    // BlackBox is for dialogue
    GameObject BlackBox;
    GameObject Option_Yes;
    GameObject Option_No;
    GameObject Option_Continue;
    GameObject Content;

    // Keycodes for DialogBox, get from HeroInteraction
    private KeyCode leftKey;
    private KeyCode rightKey;
    private KeyCode confirmKey;

    public static GameUIManager instance;
    private void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
        // Find all the UI elements in the son objects
        // Find the curtain and set the alpha to 0
        CurtainCanvas = transform.Find("CurtainCanvas").gameObject;
        CurtainCanvas.GetComponent<CanvasGroup>().alpha = 0;
        // For dialog box
        GameObject DialogBox = transform.Find("DialogBox").gameObject;
        BlackBox = DialogBox.transform.Find("BlackBox").gameObject;
        Option_Yes = BlackBox.transform.Find("Yes").gameObject;
        Option_No = BlackBox.transform.Find("No").gameObject;
        Option_Continue = BlackBox.transform.Find("Continue").gameObject;
        Content = BlackBox.transform.Find("Content").gameObject;
        BlackBox.SetActive(false);
        Option_Yes.SetActive(false);
        Option_No.SetActive(false);
        Option_Continue.SetActive(false);
        Content.SetActive(false);
        // For HeroUI
        GameObject HeroUI = transform.Find("HeroUI").gameObject;
        Interact = HeroUI.transform.Find("Interact").gameObject;
        Use = HeroUI.transform.Find("Use").gameObject;
        Exchange = HeroUI.transform.Find("Exchange").gameObject;
        Interact.SetActive(false);
        Use.SetActive(false);
        Exchange.SetActive(false);
    }

    private void Start() {
        leftKey = HeroInteraction.instance.previousItemKey;
        rightKey = HeroInteraction.instance.nextItemKey;
        confirmKey = HeroInteraction.instance.interactKey;
    }

    /// <summary>
    /// Set the dialog box with content
    /// </summary>
    /// <param name="content">the content of one dialog displayed in the blackbox</param>
    /// <param name="isContinue">whether this dialogue has options</param>
    public void SetDialogBox(string content, bool isContinue = true) {
        Interact.SetActive(false);
        Use.SetActive(false);
        Exchange.SetActive(false);

        BlackBox.SetActive(true);
        Content.SetActive(true);
        Content.GetComponent<TextMeshProUGUI>().text = content;
        Option_Yes.SetActive(!isContinue);
        Option_No.SetActive(!isContinue);
        Option_Continue.SetActive(isContinue);
    }

    /// <summary>
    /// Clear the dialog box
    /// </summary>
    public void ClearDialogBox() {
        BlackBox.SetActive(false);
        Content.SetActive(false);
        Option_Yes.SetActive(false);
        Option_No.SetActive(false);
        Option_Continue.SetActive(false);
    }

    /// <summary>
    /// Set the dialog box with options
    /// </summary>
    /// <param name="isContinuing">whether the dialog is not optioned</param>
    /// <param name="isYes">parameter for current option, in order to hight the current option</param>
    public void SetDialogOption(bool isContinuing, bool isYes) {
        if (isContinuing) {
            HighlightOption(Option_Continue, true, true);
            HighlightOption(Option_Yes, false, false);
            HighlightOption(Option_No, false, false);
        } else {
            HighlightOption(Option_Continue, false, false);
            HighlightOption(Option_Yes, true, isYes);
            HighlightOption(Option_No, true, !isYes);
        }
    }

    private void HighlightOption(GameObject option, bool Activate, bool isHighlighted) {
        if (isHighlighted) {
            option.GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Underline;
        } else {
            option.GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Normal;
        }
        option.SetActive(Activate);
    }

    /// <summary>
    /// Public method for others to show one dialog coroutine
    /// </summary>
    /// <param name="content">The content of the dialog</param>
    /// <param name="isContinuing">Whether the dialog is not optioned</param>
    public void ShowDialogue(string content, bool isContinuing = true) {
        StartCoroutine(DialogCoroutine(content, isContinuing));
    }
    // The event for others to subscribe, to convey the decision of the dialog
    // Invoked when one dialog is ended
    [HideInInspector]
    public UnityEvent<bool> OnDecisionMade;
    /// <summary>
    /// A public coroutine for showing dialog
    /// </summary>
    /// <param name="content">The content of the dialog</param>
    /// <param name="isContinuing">Whether the dialog is not optioned</param>
    /// <returns>A IEnumerator for c# to run coroutine</returns>
    public IEnumerator DialogCoroutine(string content, bool isContinuing = true) {
        // Start from next frame
        yield return null;
        // First disable all movement and interaction
        HeroController.instance.CanMove = false;
        HeroInteraction.instance.CanInteract = false;
        // A state parameter for the current option
        bool isYes = true;
        // set the dialog box
        SetDialogBox(content, isContinuing);
        // framely update the dialog
        while (!Input.GetKeyDown(confirmKey)) {
            // handle the option
            if (Input.GetKeyDown(leftKey) || (Input.GetAxis("Horizontal") < -0.1f)) {
                isYes = true;
            } else if (Input.GetKeyDown(rightKey) || (Input.GetAxis("Horizontal") > 0.1f)) {
                isYes = false;
            }
            SetDialogOption(isContinuing, isYes);
            yield return null;
        }

        // Sent the decision to the subscribers
        OnDecisionMade.Invoke(isYes);
        // Then clear the dialog box
        ClearDialogBox();
        // Then re-enable all movement and interaction
        HeroController.instance.CanMove = true;
        HeroInteraction.instance.CanInteract = true;
    }
    // Similar, but for Conversation
    public IEnumerator ConversationCoroutine(Conversation con) {
        if (con == null) yield break;
        foreach (Dialogue dialogue in con) {
            yield return DialogCoroutine(dialogue.text, dialogue.isContinuing);
        }
    }

    // public coroutines for fading in and out the curtain
    public IEnumerator CurtainFadingIn() {
        float timer = 0f;
        while (timer < FadeDuration) {
            timer += Time.deltaTime;
            CurtainCanvas.GetComponent<CanvasGroup>().alpha = timer / FadeDuration;
            yield return null;
        }
        CurtainCanvas.GetComponent<CanvasGroup>().alpha = 1;
    }
    public IEnumerator CurtainFadingOut() {
        float timer = 0f;
        while (timer < FadeDuration) {
            timer += Time.deltaTime;
            CurtainCanvas.GetComponent<CanvasGroup>().alpha = 1 - timer / FadeDuration;
            yield return null;
        }
        CurtainCanvas.GetComponent<CanvasGroup>().alpha = 0;
    }
}
