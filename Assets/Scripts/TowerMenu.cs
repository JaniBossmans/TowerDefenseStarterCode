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
    }

    private void OnArcherButtonClicked()
    {
        GameManager.Instance.Build(Enums.TowerType.Archer, Enums.SiteLevel.Level1);
    }

    private void OnSwordButtonClicked()
    {
        GameManager.Instance.Build(Enums.TowerType.Sword, Enums.SiteLevel.Level1);
    }

    private void OnWizardButtonClicked()
    {
        GameManager.Instance.Build(Enums.TowerType.Wizard, Enums.SiteLevel.Level1);
    }

    private void OnUpdateButtonClicked()
    {
        if (selectedSite == null) return;

        Enums.SiteLevel nextLevel = selectedSite.Level + 1; // Verhoog het level met één.
        GameManager.Instance.Build(selectedSite.TowerType, nextLevel);
    }

    private void OnDestroyButtonClicked()
    {
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

        // Standaard alle knoppen uitschakelen
        archerButton.SetEnabled(false);
        swordButton.SetEnabled(false);
        wizardButton.SetEnabled(false);
        updateButton.SetEnabled(false);
        destroyButton.SetEnabled(false);

        switch (selectedSite.Level)
        {
            case Enums.SiteLevel.Onbebouwd:
                // Als de Level Onbebouwd is, schakel de bouwknoppen in
                archerButton.SetEnabled(true);
                swordButton.SetEnabled(true);
                wizardButton.SetEnabled(true);
                break;

            case Enums.SiteLevel.Level1:
            case Enums.SiteLevel.Level2:
                // Als de Level 1 of 2 is, schakel alleen update en vernietig knoppen in
                updateButton.SetEnabled(true);
                destroyButton.SetEnabled(true);
                break;

            case Enums.SiteLevel.Level3:
                // Als de Level 3 is, alleen de vernietigknop inschakelen
                destroyButton.SetEnabled(true);
                break;

                // Geen default case nodig, tenzij je onvoorziene Levels verwacht
        }
    }
}
