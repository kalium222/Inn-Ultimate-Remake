using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using TMPro;

public class GameUIManager : MonoBehaviour
{
    // objects that hold UI elements
    public GameObject Interact;
    public GameObject Use;
    public GameObject Exchange;
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

    public void setDialogBox(string content, bool isContinue = true) {
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

    public void clearDialogBox() {
        BlackBox.SetActive(false);
        Content.SetActive(false);
        Option_Yes.SetActive(false);
        Option_No.SetActive(false);
        Option_Continue.SetActive(false);
    }

    public void setDialogOption(bool isContinuing, bool isYes) {
        if (isContinuing) {
            highlightOption(Option_Continue, true, true);
            highlightOption(Option_Yes, false, false);
            highlightOption(Option_No, false, false);
        } else {
            highlightOption(Option_Continue, false, false);
            highlightOption(Option_Yes, true, isYes);
            highlightOption(Option_No, true, !isYes);
        }
    }

    private void highlightOption(GameObject option, bool isActivated, bool isHighlighted) {
        if (isActivated) {
            option.SetActive(true);
            if (isHighlighted) {
                option.GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Underline;
            } else {
                option.GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Normal;
            }
        } else {
            option.SetActive(false);
        }
    }

    // function for others to show a simple dialog UI and coroutine
    public void ShowDialogue(string content, bool isContinuing = true) {
        StartCoroutine(runDialogCoroutine(content, isContinuing));
    }

    private IEnumerator runDialogCoroutine(string content, bool isContinuing = true) {
        // Start from next frame
        yield return null;
        // First disable all movement and interaction
        HeroController.instance.CanMove = false;
        HeroInteraction.instance.CanInteract = false;

        while (!Input.GetKeyDown(confirmKey)) {
            setDialogBox(content, isContinuing);
            yield return null;
        }

        // Then clear the dialog box
        clearDialogBox();
        // Then re-enable all movement and interaction
        HeroController.instance.CanMove = true;
        HeroInteraction.instance.CanInteract = true;

    }
}
