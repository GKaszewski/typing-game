using TMPro;
using UnityEngine;

public class TabInputField : MonoBehaviour {
    [SerializeField] private TMP_InputField originalWordInput;
    [SerializeField] private TMP_InputField translatedWordInput;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Tab)) {
            if (originalWordInput.isFocused) {
                translatedWordInput.Select();
            }
            else if (translatedWordInput.isFocused) {
                originalWordInput.Select();
            }
        }
    }
}