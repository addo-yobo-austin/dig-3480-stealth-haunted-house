using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class PetrifyOverlayController : MonoBehaviour
{
    [SerializeField] private CanvasGroup _overlayGroup;
    
    [SerializeField] private TMP_Text _instructionText;
    
    [SerializeField] private GameObject _heart;
    private Vector3 _heartStartingScale;
    [SerializeField] private float _heartbeatTargetScale = 1.25f, _heartbeatDuration = 0.25f;

    private void Start()
    {
        _heartStartingScale = _heart.transform.localScale;
    }

    public void Activate(bool active)
    {
        _instructionText.text = $"Spam [SPACE] for Courage: 0/10";
        _overlayGroup.alpha = active ? 1 : 0;
    }

    public void Heartbeat(int count)
    {
        _instructionText.text = $"Spam [SPACE] for Courage: {count}/10";
        StartCoroutine(HeartbeatCoroutine());
    }

    private IEnumerator HeartbeatCoroutine()
    {
        _heart.transform.localScale = _heartStartingScale * _heartbeatTargetScale;
        float t = 0;
        while (t < _heartbeatDuration)
        {
            // cos 0 = 1
            // cos pi / 2 = 0
            // Lerp between [1, targetScale (i.e. 1.25)] by the cosine of current time normalized between [0, pi / 2],
            // so that the cosine returns [1, 0]
            _heart.transform.localScale = _heartStartingScale * 
                                          Mathf.Lerp(1, _heartbeatTargetScale, Mathf.Cos(Mathf.Lerp(0, Mathf.PI / 2, t / 0.25f)));
            t += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }
}
