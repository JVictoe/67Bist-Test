using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class PlayerStatusManager : MonoBehaviour
{
    public static PlayerStatusManager Instance;

    [SerializeField] private TextMeshProUGUI playerLevel = default;
    [SerializeField] private TextMeshProUGUI playerMoney = default;
    [SerializeField] private TextMeshProUGUI maxPeopleToLoadText = default;
    [SerializeField] private TextMeshProUGUI moneyToUpText = default;

    [SerializeField] private Button buttonUp = default;

    [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer = default;
    [SerializeField] private PlayerCombat playerCombat = default;

    [SerializeField] private Color[] colors = default;

    [SerializeField] private Camera mainCamera = default;

    private int maxPeopleToLoad;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        if (!PlayerPrefs.HasKey("FirstPlay"))
        {
            PlayerLevel = 1;
            PlayerMoney = 0;

            PlayerPrefs.SetInt("FirstPlay", 1);
        }

        buttonUp.onClick.AddListener(UpLevel);

        GetPlayerData();
        SetMaxPlayer(false);
    }

    private void UpLevel()
    {
        if (PlayerMoney >= MaxMoneyToUp)
        {
            PlayerMoney = PlayerMoney - MaxMoneyToUp;

            PlayerLevel += 1;

            GetPlayerData();
            SetMaxPlayer(false);
        }
    }

    private void GetPlayerData()
    {
        playerLevel.text = "LEVEL: " + PlayerLevel.ToString();
        playerMoney.text = PlayerMoney.ToString();

        skinnedMeshRenderer.material.DOColor(colors[PlayerLevel > 18 ? 17 : PlayerLevel - 1], 1f);

        moneyToUpText.text = MaxMoneyToUp.ToString();
    }

    public void SetMaxPlayer(bool remove)
    {
        maxPeopleToLoad = PlayerLevel == 1 ? 1 : PlayerLevel + 2;

        maxPeopleToLoadText.text = "Peoples: " + playerCombat.Childs.Count + "/" + maxPeopleToLoad;

        if (playerCombat.Childs.Count > 0 && !remove) mainCamera.DOFieldOfView(Mathf.Clamp(mainCamera.fieldOfView, 60, 85) + 5, 0.3f);
        else mainCamera.DOFieldOfView(Mathf.Clamp(mainCamera.fieldOfView, 60, 85) - 5, 0.3f);
    }

    public void AddMoney(int value)
    {
        PlayerMoney = PlayerMoney + value;

        GetPlayerData();
    }

    private int PlayerLevel { get { return PlayerPrefs.GetInt("PlayerLevel"); } set { PlayerPrefs.SetInt("PlayerLevel", value); } }
    private int PlayerMoney { get { return PlayerPrefs.GetInt("PlayerMoney"); } set { PlayerPrefs.SetInt("PlayerMoney", value); } }
    private int MaxMoneyToUp { get { return PlayerPrefs.GetInt("PlayerLevel") * 10; } }
    public int MaxPeopleToLoad => maxPeopleToLoad;
}
