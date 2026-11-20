using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour
{
    public float openDuration = 1;

    public delegate void OnOpened();

    private bool _isOpening = false;
    private bool _isOpened = false;
    public bool IsOpened => _isOpened;
    
    private Quaternion _startRotation;
    private Quaternion _endRotation;
    
    public void Open(bool bInstantly, OnOpened onOpened)
    {
        // Открыта уже. Делегат не вызовется по завершению. Нужно вызвать.
        if (_isOpened)
        {
            onOpened();
            return;
        }

        _startRotation = transform.localRotation;
        _endRotation = _startRotation * Quaternion.Euler(0, 0, -145f);
        
        if (bInstantly)
        {
            if (_isOpening)
                StopAllCoroutines();
            
            transform.localRotation = _endRotation;
            
            _isOpened = true;
            _isOpening = false;

            onOpened?.Invoke();
            
            return;
        }
        
        // Открывается уже. Делегат вызовется по завершению.
        if (_isOpening)
            return;
        
        _isOpened = false;
        _isOpening = true;
        
        // Открываем и вызываем делегат.
        StartCoroutine(OpenRoutine(onOpened));
    }

    private IEnumerator OpenRoutine(OnOpened onOpened)
    {
        float progress = 0;
        
        while (progress < 1f)
        {
            progress += Time.unscaledDeltaTime / openDuration;

            transform.localRotation = Quaternion.Slerp(_startRotation, _endRotation, progress);
        
            yield return null;
        }
        
        transform.localRotation = _endRotation;

        _isOpened = true;
        _isOpening = false;
    }
}