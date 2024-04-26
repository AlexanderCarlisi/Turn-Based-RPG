using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleUIScript : MonoBehaviour {

    private static readonly float SKILL_BUTTON_Y_DIFFERENCE = 95f;
    private static readonly float STATUS_PANEL_X_DIFFERENCE = 200f;
    private static readonly int MAX_SKILLS = 6;
    private static readonly int MAX_PARTY_MEMBERS = 4;
    private static readonly int MAX_ENEMIES = 5;

    [SerializeField] private GameObject skillPanel;
    [SerializeField] private GameObject exampleSkillButton;

    [SerializeField] private GameObject statusPanel;
    [SerializeField] private GameObject examplePartyPanel;
    [SerializeField] private GameObject exampleEnemyPanel;

    private static Unit currentUnit;

    private static GameObject[] skillButtons;
    private static GameObject[] statusPanels;


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
            return null;
        }
        Vector3 position = exampleSkillButton.transform.position;
        return Instantiate(exampleSkillButton, new Vector3(position.x, previousY - SKILL_BUTTON_Y_DIFFERENCE, position.z), Quaternion.identity);
    }


    /// <summary>
    /// Generates a status panel for PartyPanel or EnemyPanel UI
    /// </summary>
    /// <param name="examplePanel"></param>
    /// <param name="previousX"></param>
    /// <returns></returns>
    private GameObject generateStatusPanel(GameObject examplePanel, float previousX) {
        if (examplePanel == null) {
            Debug.LogError("examplePanel is null");
            return null;
        }
        // RectTransform rectTransform = examplePanel.rectTransform;
        // GameObject newPanel = Instantiate(examplePanel, new Vector3(previousX + STATUS_PANEL_X_DIFFERENCE, rectTransform.position.y, rectTransform.position.z), Quaternion.identity);
        Vector3 position = examplePanel.transform.position;
        return Instantiate(examplePanel, new Vector3(previousX + STATUS_PANEL_X_DIFFERENCE, position.y, position.z), Quaternion.identity);
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
        if (examplePartyPanel == null) {
            Debug.LogError("examplePartyPanel is null");
            return;
        }
        if (exampleEnemyPanel == null) {
            Debug.LogError("exampleEnemyPanel is null");
            return;
        }

        skillButtons = new GameObject[MAX_SKILLS];
        for (int i = 0; i < MAX_SKILLS; i++) {
            skillButtons[i] = generateSkillButton(
                (i == 0) ? exampleSkillButton.transform.position.y : skillButtons[i - 1].transform.position.y);
            skillButtons[i].transform.SetParent(skillPanel.transform, false);
            skillButtons[i].SetActive(false);
        }

        statusPanels = new GameObject[MAX_PARTY_MEMBERS + MAX_ENEMIES];
        for (int i = 0; i < MAX_PARTY_MEMBERS + MAX_ENEMIES; i++) {
            GameObject examplePanel = (i < MAX_PARTY_MEMBERS) ? examplePartyPanel : exampleEnemyPanel;
            statusPanels[i] = generateStatusPanel(
                examplePanel,
                (i == 0 || i == MAX_PARTY_MEMBERS) ? examplePanel.transform.position.x : statusPanels[i - 1].transform.position.x);
            statusPanels[i].transform.SetParent(statusPanel.transform, false);
            statusPanels[i].gameObject.SetActive(false);
        }
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
            } else {
                skillButtons[i].SetActive(false);
            }
        }

        skillPanel.SetActive(true);
    }


    /// <summary>
    /// Updates the status panel with the current party and enemies.
    /// 
    /// Called by BattleManagerScript
    /// </summary>
    /// <param name="party"></param>
    /// <param name="enemies"></param>
    public void updateStatusPanel(PartyMember[] party, Enemy[] enemies) {
        if (party == null) {
            Debug.LogError("party is null");
            return;
        }
        if (enemies == null) {
            Debug.LogError("enemies is null");
            return;
        }

        // int[] stats = currentUnit.getStatsAsArray();
        for (int i = 0; i < MAX_PARTY_MEMBERS + MAX_ENEMIES; i++) {
            if (i < MAX_PARTY_MEMBERS) {
                statusPanels[i].GetComponentInChildren<TextMeshProUGUI>().text = party[i].getName() + "\n" +
                    "HP: " + party[i].getHp() + "/" + party[i].getMaxHp() + "\n" +
                    "SP: " + party[i].getSp() + "/" + party[i].getMaxSp() + "\n";
                    // "Level: " + party[i].getLevel() + "\n" +
                    // "Strength: " + party[i].getStrength() + "\n" +
                    // "Intelligence: " + party[i].getIntelligence() + "\n" +
                    // "Endurence: " + party[i].getEndurence() + "\n" +
                    // "Defense: " + party[i].getDefense() + "\n" +
                    // "Agility: " + party[i].getAgility() + "\n" +
                    // "Spirit: " + party[i].getSpirit() + "\n" +
                    // "Luck: " + party[i].getLuck();
            } else {
                statusPanels[i].GetComponentInChildren<TextMeshProUGUI>().text = enemies[i - MAX_PARTY_MEMBERS].getName() + "\n" +
                    "HP: " + enemies[i - MAX_PARTY_MEMBERS].getHp() + "/" + enemies[i - MAX_PARTY_MEMBERS].getMaxHp() + "\n" +
                    "SP: " + enemies[i - MAX_PARTY_MEMBERS].getSp() + "/" + enemies[i - MAX_PARTY_MEMBERS].getMaxSp() + "\n";
                    // "Level: " + enemies[i - MAX_PARTY_MEMBERS].getLevel() + "\n" +
                    // "Strength: " + enemies[i - MAX_PARTY_MEMBERS].getStrength() + "\n" +
                    // "Intelligence: " + enemies[i - MAX_PARTY_MEMBERS].getIntelligence() + "\n" +
                    // "Endurence: " + enemies
            }
        }
    }
}
