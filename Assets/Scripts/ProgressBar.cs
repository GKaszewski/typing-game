using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode()]
public class ProgressBar : MonoBehaviour {
    
    #if UNITY_EDITOR
    [MenuItem("GameObject/UI/Progress Bar")]
    public static void AddProgressBar() {
        var obj = Instantiate(Resources.Load<GameObject>("UI/ProgressBar"));
        obj.transform.SetParent(Selection.activeGameObject.transform, false);
    }
    #endif
    
    [SerializeField] private int minimum;
    [SerializeField] private int maximum;
    [SerializeField] private float current;
    [SerializeField] private Image mask;
    [SerializeField] private Image fill;
    [SerializeField] private Color color;

    private void Update() {
        GetCurrentFill();
    }

    private void GetCurrentFill() {
        float currentOffset = current - minimum;
        float maximumOffset = maximum - minimum;
        var fillAmount = currentOffset / maximumOffset;
        mask.fillAmount = fillAmount;

        fill.color = color;
    }
    
    public void SetProgress(float progress) {
        current = progress;
    }
    
    public void SetColor(Color color) {
        this.color = color;
    }
    
    public void SetMinimum(int minimum) {
        this.minimum = minimum;
    }
    
    public void SetMaximum(int maximum) {
        this.maximum = maximum;
    }
}