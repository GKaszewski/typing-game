using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class CameraShake : MonoBehaviour {
    private Vector3 _originalPos;

    private void Start() {
        _originalPos = transform.localPosition;
    }

    public IEnumerator Shake(float duration, float magnitude) {
        var originalPos = transform.localPosition;
        var elapsed = 0f;
        while (elapsed < duration) {
            var x = Random.Range(-1f, 1f) * magnitude;
            var y = Random.Range(-1f, 1f) * magnitude;
            transform.localPosition = new Vector3(x, y, originalPos.z);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = originalPos;
    }

    public void ResetPosition() {
        transform.localPosition = _originalPos;
    }
}
