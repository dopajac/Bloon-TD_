using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasObject : MonoBehaviour
{
    [SerializeField]
    public GameObject Information_Panel;
    
    [SerializeField]
    public Image User_image;
    [SerializeField]
    public Text User_name;
    [SerializeField]
    public Text User_Upgrade;
    [SerializeField]
    public Text User_Attack;
    [SerializeField]
    public Text User_AttackSpeed;
    [SerializeField]
    public GameObject SetCheckBox;
    [SerializeField]
    public GameObject JobUpgrade_Panel;
    [SerializeField]
    public GameObject adventurerUpgrade_Panel;
    [SerializeField]
    public GameObject WarriorUpgrade_Panel;
    [SerializeField]
    public GameObject WizardUpgrade_Panel;
    [SerializeField]
    public GameObject ThiefUpgrade_Panel;
    [SerializeField]
    public GameObject ArcherUpgrade_Panel;
    [SerializeField]
    public GameObject PirateUpgrade_Panel;

    [SerializeField]
    public UserData userdata;
}
