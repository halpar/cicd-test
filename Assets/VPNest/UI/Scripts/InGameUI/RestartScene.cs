using UnityEngine;
using UnityEngine.UI;
using VP.Nest.SceneManagement;

namespace VP.Nest.UI.InGame
{
    public class RestartScene : MonoBehaviour
    {
        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(() =>
            {
                GameManager.Instance.LoadCurrentLevelScene();
            });
        }
    }
}