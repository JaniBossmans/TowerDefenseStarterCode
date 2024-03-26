using UnityEngine;
using UnityEngine.UIElements;



public class TowerMenu : MonoBehaviour
{
    private Button archerButton;
    private Button swordButton;
    private Button wizardButton;
    private Button updateButton;
    private Button destroyButton;

    private VisualElement root;

    private ConstructionSite selectedSite;

    void Awake()
    {
        // Check if an instance of TowerMenu already exists
        if (Instance == null)
        {
            // This is the first instance - make it the Singleton
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (Instance != this)
        {
            // This means a different instance is already the Singleton - destroy this one
            Destroy(gameObject);
        }
    }

    public static TowerMenu Instance { get; private set; }

    void Start()
    {
        root = GetComponent<UIDocument>().rootVisualElement;

        archerButton = root.Q<Button>("archer-button");
        swordButton = root.Q<Button>("sword-button");
        wizardButton = root.Q<Button>("wizard-button");
        updateButton = root.Q<Button>("button-upgrade");
        destroyButton = root.Q<Button>("button-destroy");

        if (archerButton != null)
        {
            archerButton.clicked += OnArcherButtonClicked;
        }

        if (swordButton != null)
        {
            swordButton.clicked += OnSwordButtonClicked;
        }

        if (wizardButton != null)
        {
            wizardButton.clicked += OnWizardButtonClicked;
        }

        if (updateButton != null)
        {
            updateButton.clicked += OnUpdateButtonClicked;
        }

        if (destroyButton != null)
        {
            destroyButton.clicked += OnDestroyButtonClicked;
        }

        root.visible = false;

        DontDestroyOnLoad(this.gameObject);
    }

    private void OnArcherButtonClicked()
    {
        SoundManager.Instance.PlayUISound();
        GameManager.Instance.Build(Enums.TowerType.Archer, Enums.SiteLevel.Level1);
    }

    private void OnSwordButtonClicked()
    {
        SoundManager.Instance.PlayUISound();
        GameManager.Instance.Build(Enums.TowerType.Sword, Enums.SiteLevel.Level1);
    }

    private void OnWizardButtonClicked()
    {
        SoundManager.Instance.PlayUISound();
        GameManager.Instance.Build(Enums.TowerType.Wizard, Enums.SiteLevel.Level1);
    }

    private void OnUpdateButtonClicked()
    {
        SoundManager.Instance.PlayUISound();

        if (selectedSite == null) return;

        Enums.SiteLevel nextLevel = selectedSite.Level + 1; // Verhoog het level met één.
        GameManager.Instance.Build(selectedSite.TowerType, nextLevel);
    }

    private void OnDestroyButtonClicked()
    {
        SoundManager.Instance.PlayUISound();

        if (selectedSite == null) return;

        // Roep de nieuwe DestroyTower methode aan
        GameManager.Instance.DestroyTower();
    }

    private void OnDestroy()
    {
        if (archerButton != null)
        {
            archerButton.clicked -= OnArcherButtonClicked;
        }

        if (swordButton != null)
        {
            swordButton.clicked -= OnSwordButtonClicked;
        }

        if (wizardButton != null)
        {
            wizardButton.clicked -= OnWizardButtonClicked;
        }

        if (updateButton != null)
        {
            updateButton.clicked -= OnUpdateButtonClicked;
        }

        if (destroyButton != null)
        {
            destroyButton.clicked -= OnArcherButtonClicked;
        }
    }

    public void SetSite(ConstructionSite site)
    {
        selectedSite = site;

        if (selectedSite == null)
        {
            root.visible = false;
            return;
        }
        else
        {
            root.visible = true;
            EvaluateMenu();
        }
    }

    public void EvaluateMenu()
    {
        if (selectedSite == null)
        {
            return; // Vroegtijdig terugkeren als er geen site geselecteerd is.
        }

        // Haal de beschikbare credits op
        int availableCredits = GameManager.Instance.GetCredits();

        // Standaard alle knoppen uitschakelen
        archerButton.SetEnabled(false);
        swordButton.SetEnabled(false);
        wizardButton.SetEnabled(false);
        updateButton.SetEnabled(false);
        destroyButton.SetEnabled(false);

        switch (selectedSite.Level)
        {
            case Enums.SiteLevel.Onbebouwd:
                // Als de Level Onbebouwd is, controleer de kosten en schakel de bouwknoppen in indien mogelijk
                int archerCost = GameManager.Instance.GetCost(Enums.TowerType.Archer, Enums.SiteLevel.Level1);
                int swordCost = GameManager.Instance.GetCost(Enums.TowerType.Sword, Enums.SiteLevel.Level1);
                int wizardCost = GameManager.Instance.GetCost(Enums.TowerType.Wizard, Enums.SiteLevel.Level1);
                archerButton.SetEnabled(availableCredits >= archerCost);
                swordButton.SetEnabled(availableCredits >= swordCost);
                wizardButton.SetEnabled(availableCredits >= wizardCost);
                break;

            case Enums.SiteLevel.Level1:
            case Enums.SiteLevel.Level2:
                // Als de Level 1 of 2 is, controleer de kosten voor upgraden en schakel update en vernietig knoppen in indien mogelijk
                Enums.SiteLevel nextLevel = selectedSite.Level + 1;
                int updateCost = GameManager.Instance.GetCost(selectedSite.TowerType, nextLevel);
                updateButton.SetEnabled(availableCredits >= updateCost);
                destroyButton.SetEnabled(true);
                break;

            case Enums.SiteLevel.Level3:
                // Als de Level 3 is, alleen de vernietigknop inschakelen
                destroyButton.SetEnabled(true);
                break;
        }
    }
}
