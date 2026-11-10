using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour
{
    public float openDuration = 1;

    public delegate void OnOpened();

    private bool _isOpening = false;
    private bool _isOpened = false;
    public bool IsOpened => _isOpened;
    
    public void Open(OnOpened onOpened)
    {
        // Открывается уже. Делегат вызовется по завершению.
        if (_isOpening)
            return;
        
        // Открыта уже. Делегат не вызовется по завершению. Нужно вызвать.
        if (_isOpened)
        {
            onOpened();
            return;
        }
        
        // Открываем и вызываем делегат.
        StartCoroutine(OpenRoutine(onOpened));
        
        _isOpened = false;
        _isOpening = true;
    }

    private IEnumerator OpenRoutine(OnOpened onOpened)
    {
        Quaternion startRotation = transform.localRotation;
        Quaternion targetRotation = startRotation * Quaternion.Euler(0, 0, -145f); 
    
        float progress = 0;
        
        while (progress < 1f)
        {
            progress += Time.unscaledDeltaTime / openDuration;

            transform.localRotation = Quaternion.Slerp(startRotation, targetRotation, progress);
        
            yield return null;
        }
        
        transform.localRotation = targetRotation;

        _isOpened = true;
        _isOpening = false;
    }
}