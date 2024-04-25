using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BattleUIScript : MonoBehaviour {

    private static readonly float SKILL_BUTTON_Y_DIFFERENCE = 95f;
    private static readonly int MAX_SKILLS = 6;

    private static Unit currentUnit;
    private static GameObject[] skillButtons;

    [SerializeField] private GameObject skillPanel;
    [SerializeField] private GameObject exampleSkillButton;

    public static void setUnit(Unit unit) {
        currentUnit = unit;
    }


    private GameObject generateButton(float previousY) {
        if (exampleSkillButton == null) {
            Debug.LogError("exampleSkillButton is null");
            return null;
        }
        Vector3 position = exampleSkillButton.transform.position;
        return Instantiate(exampleSkillButton, new Vector3(position.x, previousY - SKILL_BUTTON_Y_DIFFERENCE, position.z), Quaternion.identity);
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

        skillButtons = new GameObject[MAX_SKILLS];
        for (int i = 0; i < MAX_SKILLS; i++) {
            skillButtons[i] = generateButton(
                (i == 0) ? exampleSkillButton.transform.position.y : skillButtons[i - 1].transform.position.y);
            skillButtons[i].transform.SetParent(skillPanel.transform, false);
            skillButtons[i].SetActive(false);
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


}
