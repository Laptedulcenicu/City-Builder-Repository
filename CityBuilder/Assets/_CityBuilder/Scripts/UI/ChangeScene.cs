using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _CityBuilder.Scripts.UI
{
    public class ChangeScene : MonoBehaviour
    {
        private void Start()
        {
            transform.DOScale(Vector3.one * 1.05f, 2f).SetLoops(-1, LoopType.Yoyo);
            GetComponent<Button>().onClick.AddListener(() =>
            {
                SceneManager.LoadScene("GamePlay");
            });
        }

        private void OnDestroy()
        {
            transform.DOKill();
        }
    }
}
