using UnityEngine;
using DG.Tweening;

public class ParticleCamera : MonoBehaviour
{
    private Transform confetti, money;
    public static ParticleCamera Instance;
    private Transform fakeGold;
    private Canvas canvas;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        money = transform.Find("Money");
        confetti = transform.Find("Confetti");
        fakeGold = Resources.Load<Transform>("FakeGold");

        canvas = FindObjectOfType<Canvas>();
    }

    private void OnEnable()
    {
        EventManager.FinishGame += FinishParticle;
    }

    private void OnDisable()
    {
        EventManager.FinishGame -= FinishParticle;
    }

    public void PlayParticle(CameraParticle particle)
    {
        switch (particle)
        {
            case CameraParticle.Confetti:
                for (int i = 0; i < confetti.childCount; i++)
                {
                    confetti.GetChild(i).GetComponent<ParticleSystem>().Play();
                }

                break;
            case CameraParticle.Money:
                for (int i = 0; i < money.childCount; i++)
                {
                    money.GetChild(i).GetComponent<ParticleSystem>().Play();
                }

                break;
            default:
                break;
        }
    }

    public void CoinEffect(Vector3 pos, int Amount = 1)
    {
        if (!canvas) canvas = FindObjectOfType<Canvas>();

        for (int i = 0; i < Amount; i++)
        {
            var _fakeGold = Instantiate(fakeGold, canvas.transform);
            pos = Camera.main.WorldToScreenPoint(
                pos + new Vector3(Random.Range(-.5f, .5f), Random.Range(-.5f, .5f), 0));
            _fakeGold.position = pos;

            _fakeGold.DOMove(GameBase.Instance.MenuManager.PlayTimeMenu.GoldImage.transform.position, 0.5f)
                .SetDelay(i * 0.15f)
                .OnComplete(() => { Destroy(_fakeGold.gameObject); });
        }
    }


    private void FinishParticle(GameStat stat)
    {
        if (stat == GameStat.Win)
            PlayParticle(CameraParticle.Confetti);
    }
}

public static partial class ParticleExtension
{
    public static void Play(this CameraParticle particle)
    {
        ParticleCamera.Instance.PlayParticle(particle);
    }

    public static void PlayCoinEffect(Vector3 pos, int Amount = 1)
    {
        for (int i = 0; i < Amount; i++)
        {
            ParticleCamera.Instance.CoinEffect(pos);
        }
    }
}