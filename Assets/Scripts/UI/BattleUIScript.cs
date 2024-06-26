using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BattleUIScript : MonoBehaviour {

    private static readonly float SKILL_BUTTON_Y_DIFFERENCE = 95f;
    private static readonly float SKILL_BUTTON_STARTING_Y = 300f;

    private static readonly float STATUS_PANEL_X_DIFFERENCE = 300f;
    private static readonly float STATUS_PANEL_STARTING_X = -840f;
    private static readonly float STATUS_PANEL_Y_PARTY = 100f;
    private static readonly float STATUS_PANEL_Y_ENEMY = -100f;

    private static readonly float TARGET_BUTTON_X_DIFFERENCE = 250f;
    private static readonly float TARGET_BUTTON_STARTING_X = -500f;

    private static readonly int MAX_SKILLS = 6;
    private static readonly int MAX_PARTY_MEMBERS = 4;
    private static readonly int MAX_ENEMIES = 5;

    private static readonly Color TURN_COLOR = new Color(255, 210, 0, 175);
    private static readonly Color DEFAULT_COLOR = new Color(0, 0, 0, 255);

    [SerializeField] private GameObject skillPanel;
    [SerializeField] private GameObject exampleSkillButton;
    private static GameObject s_skillPanel;

    [SerializeField] private GameObject statusPanel;
    [SerializeField] private GameObject exampleStatusPanel;

    [SerializeField] private GameObject targetPanel;
    [SerializeField] private GameObject exampleTargetButton;
    private static GameObject s_targetPanel;

    [SerializeField] private GameObject infoPanel;

    private static Unit currentUnit;

    private static GameObject[] skillButtons;
    private static GameObject[] statusPanels;
    private static GameObject[] targetButtons;

    private static TextMeshProUGUI infoText;


    /// <summary>
    /// Sets currentUnit to the Unit's whose turn it is.
    /// </summary>
    /// <param name="unit"></param>
    public static void setUnit(Unit unit) {
        currentUnit = unit;
    }

    // /// <summary>
    // /// Sets the party to the party of the player.
    // /// </summary>
    // /// <param name="party"></param>
    // public static void setParty(PartyMember[] party) {
    //     BattleUIScript.party = party;
    // }

    // /// <summary>
    // /// Sets the enemies to the enemies in the battle.
    // /// </summary>
    // /// <param name="enemies"></param>
    // public static void setEnemies(Enemy[] enemies) {
    //     BattleUIScript.enemies = enemies;
    // }


    /// <summary>
    /// Generates a skill button for SkillPanel UI
    /// </summary>
    /// <param name="previousY"></param>
    /// <returns> Button Cloned from exampleButton </returns>
    private GameObject generateSkillButton(float previousY) {
        if (exampleSkillButton == null) {
            Debug.LogError("exampleSkillButton is null");
            return Instantiate(new GameObject());
        }
        
        GameObject button = Instantiate(exampleSkillButton, skillPanel.transform);
        button.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, previousY - SKILL_BUTTON_Y_DIFFERENCE);
        return button;
    }


    /// <summary>
    /// Generates a status panel for PartyPanel or EnemyPanel UI
    /// </summary>
    /// <param name="examplePanel"></param>
    /// <param name="previousX"></param>
    /// <returns></returns>
    private GameObject generateStatusPanel(float previousX, float y) {
        if (exampleStatusPanel == null) {
            Debug.LogError("examplePanel is null");
            return Instantiate(new GameObject());
        }

        GameObject panel = Instantiate(exampleStatusPanel, statusPanel.transform);
        panel.GetComponent<RectTransform>().anchoredPosition = new Vector2(previousX + STATUS_PANEL_X_DIFFERENCE, y);
        return panel;
    }


    private GameObject generateTargetButton(float previousX) {
        if (exampleTargetButton == null) {
            Debug.LogError("exampleTargetButton is null");
            return Instantiate(new GameObject());
        }

        GameObject button = Instantiate(exampleTargetButton, targetPanel.transform);
        button.GetComponent<RectTransform>().anchoredPosition = new Vector2(previousX + TARGET_BUTTON_X_DIFFERENCE, 0);
        return button;
    }


    // Start is called before the first frame update
    void Start() {
        if (skillPanel == null) {
            Debug.LogError("skillPanel is null");
            return;
        }
        if (exampleSkillButton == null) {
            Debug.LogError("exampleSkillButton is null");
            return;
        }
        if (exampleStatusPanel == null) {
            Debug.LogError("exampleStatusPanel is null");
            return;
        }
        if (statusPanel == null) {
            Debug.LogError("statusPanel is null");
            return;
        }
        if (exampleTargetButton == null) {
            Debug.LogError("exampleTargetButton is null");
            return;
        }
        if (targetPanel == null) {
            Debug.LogError("targetPanel is null");
            return;
        }
        if (infoPanel == null) {
            Debug.LogError("infoPanel is null");
            return;
        }

        skillButtons = new GameObject[MAX_SKILLS];
        for (int i = 0; i < MAX_SKILLS; i++) {
            skillButtons[i] = generateSkillButton((i == 0) ? 
                SKILL_BUTTON_STARTING_Y + SKILL_BUTTON_Y_DIFFERENCE : skillButtons[i - 1].GetComponent<RectTransform>().anchoredPosition.y);
            skillButtons[i].SetActive(false);
            int index = i;
            skillButtons[i].GetComponent<Button>().onClick.AddListener(() => BattleHandlerScript.selectSkill(index));
            skillButtons[i].name = "SkillButton " + i;
        }

        statusPanels = new GameObject[MAX_PARTY_MEMBERS + MAX_ENEMIES];
        for (int i = 0; i < MAX_PARTY_MEMBERS + MAX_ENEMIES; i++) {
            statusPanels[i] = generateStatusPanel(
                (i == 0 || i == MAX_PARTY_MEMBERS) ? 
                    STATUS_PANEL_STARTING_X - STATUS_PANEL_X_DIFFERENCE : statusPanels[i - 1].GetComponent<RectTransform>().anchoredPosition.x,
                (i < MAX_PARTY_MEMBERS) ? STATUS_PANEL_Y_PARTY : STATUS_PANEL_Y_ENEMY);
            statusPanels[i].SetActive(false);
        }

        targetButtons = new GameObject[MAX_ENEMIES];
        for (int i = 0; i < MAX_ENEMIES; i++) {
            targetButtons[i] = generateTargetButton((i == 0) ? 
                TARGET_BUTTON_STARTING_X - TARGET_BUTTON_X_DIFFERENCE : targetButtons[i - 1].GetComponent<RectTransform>().anchoredPosition.x);
            targetButtons[i].SetActive(false);
            int index = i;
            targetButtons[i].GetComponent<Button>().onClick.AddListener(() => BattleHandlerScript.selectTarget(index));
        }

        infoText = infoPanel.GetComponentInChildren<TextMeshProUGUI>();
        s_targetPanel = targetPanel;
        s_skillPanel = skillPanel;
    }


    /// <summary>
    /// Sets up the skill buttons for the current unit
    /// </summary>
    public void openSkillsPanel() {
        if (currentUnit == null) {
            Debug.LogError("currentUnit is null");
            return;
        }
        if (skillPanel == null) {
            Debug.LogError("skillPanel is null");
            return;
        }

        Skill[] skills = currentUnit.getSkills();
        for (int i = 0; i < MAX_SKILLS; i++) {
            if (i < skills.Length) {
                skillButtons[i].SetActive(true);
                skillButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = skills[i].getName();
                    // "Cost: " + skills[i].getCost() + "\n" +
                    // "Type: " + skills[i].getType();
            } else {
                skillButtons[i].SetActive(false);
            }
        }

        skillPanel.transform.parent.gameObject.SetActive(true);
    }


    /// <summary>
    /// Updates the status panel with the current party and enemies.
    /// 
    /// Called by BattleManagerScript
    /// </summary>
    /// <param name="party"></param>
    /// <param name="enemies"></param>
    public static void updateStatusPanel(PartyMember[] party, Enemy[] enemies, Unit currentUnit) {
        if (party == null) {
            Debug.LogError("party is null");
            return;
        }
        if (enemies == null) {
            Debug.LogError("enemies is null");
            return;
        }
        if (statusPanels == null) {
            Debug.LogError("statusPanels is null");
            return;
        }
        
        for (int i = 0; i < party.Length; i++) {
            statusPanels[i].SetActive(true);
            statusPanels[i].GetComponentInChildren<TextMeshProUGUI>().text = party[i].getName() + "\n" +
                "HP: " + party[i].getHp() + "/" + party[i].getMaxHp() + "\n" +
                "SP: " + party[i].getSp() + "/" + party[i].getMaxSp() + "\n";
            if (party[i] == currentUnit) {
                statusPanels[i].GetComponent<Image>().color = TURN_COLOR;
            } else {
                statusPanels[i].GetComponent<Image>().color = DEFAULT_COLOR;
            }
        }

        for (int i = 0; i < enemies.Length; i++) {
            statusPanels[i + MAX_PARTY_MEMBERS].SetActive(true);
            statusPanels[i + MAX_PARTY_MEMBERS].GetComponentInChildren<TextMeshProUGUI>().text = enemies[i].getName() + "\n" +
                "HP: " + enemies[i].getHp() + "/" + enemies[i].getMaxHp() + "\n" +
                "SP: " + enemies[i].getSp() + "/" + enemies[i].getMaxSp() + "\n";
            if (enemies[i] == currentUnit) {
                statusPanels[i + MAX_PARTY_MEMBERS].GetComponent<Image>().color = TURN_COLOR;
            } else {
                statusPanels[i + MAX_PARTY_MEMBERS].GetComponent<Image>().color = DEFAULT_COLOR;
            }
        }
    }


    public static void openTargetPanel(Unit[] units) {
        if (units == null) {
            Debug.LogError("units is null");
            return;
        }
        if (targetButtons == null) {
            Debug.LogError("targetButtons is null");
            return;
        }
        if (s_targetPanel == null) {
            Debug.LogError("s_targetPanel is null");
            return;
        }

        for (int i = 0; i < MAX_ENEMIES; i++) {
            if (i < units.Length) {
                targetButtons[i].SetActive(true);
                targetButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = units[i].getName() + "\n" +
                    "HP: " + units[i].getHp() + "/" + units[i].getMaxHp() + "\n" +
                    "SP: " + units[i].getSp() + "/" + units[i].getMaxSp() + "\n";
            } else {
                targetButtons[i].SetActive(false);
            }
        }

        s_targetPanel.transform.parent.gameObject.SetActive(true);
    }


    public static void setInfoText(string text) {
        if (infoText == null) {
            Debug.LogError("infoText is null");
            return;
        }
        infoText.text = text;
    }


    public static void closeAllPanels() {
        s_targetPanel.transform.parent.gameObject.SetActive(false);
        s_skillPanel.transform.parent.gameObject.SetActive(false);
    }
}
