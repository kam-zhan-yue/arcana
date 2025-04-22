using Kuroneko.UIDelivery;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameCompletePopup : Popup
{
    [SerializeField] private GameSettings settings;
    private bool _restarted = false;
    protected override void InitPopup()
    {
    }

    private void Update()
    {
        if (!_restarted && Input.GetMouseButtonDown(0))
        {
            Restart();
        }
    }

    private void Restart()
    {
        _restarted = true;
        SceneManager.LoadScene(settings.levels[0].buildIndex);
    }
}
