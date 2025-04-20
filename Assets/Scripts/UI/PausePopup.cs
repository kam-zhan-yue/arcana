using Kuroneko.UIDelivery;
using UnityEngine;

public class PausePopup : Popup
{
    private bool _paused;
    
    protected override void InitPopup()
    {
        
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            TogglePause();
        }
    }

    private void TogglePause()
    {
        _paused = !_paused;
        Time.timeScale = _paused ? 0f : 1f;
        if (_paused)
            ShowPopup();
        else
            HidePopup();
    }
}
