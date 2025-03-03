using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class ButtonManager : MonoBehaviour
{
    [SerializeField] private GameObject objectdetec;
    ObjectDetector objectdetector;

    public UserInformation userInformation;
    private UserManager usermanager;
    private MonsterSpawner monsterSpawner;
    [SerializeField]
    public UserData userdata;

    private void Start()
    {
        objectdetector = objectdetec.GetComponent<ObjectDetector>();
        TryGetComponent(out userInformation);
        monsterSpawner = FindObjectOfType<MonsterSpawner>();
    }

    

    // Ư�� UI ������Ʈ ���� Ŭ���ߴ��� Ȯ���ϴ� �Լ�
    private bool IsPointerOverUIObject(string uiObjectName)
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition // ���콺 Ŭ�� ��ġ ��������
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (RaycastResult result in results)
        {
            if (result.gameObject.name.Contains(uiObjectName)) // Ư�� �̸��� ���Ե� UI ����
            {
                return true; // Ư�� UI ���� Ŭ������
            }
        }
        return false; // Ư�� UI ���� �ƴ�
    }

    public void OnRespawnButtonClick()
    {
        StartCoroutine(RespawnMonstersWithInterval(monsterSpawner.spawnDelay));
    }
    
    private IEnumerator RespawnMonstersWithInterval(float delay)
    {
        List<GameObject> respawnList = new List<GameObject>(monsterSpawner.alivemonster);
        monsterSpawner. alivemonster.Clear(); // ���� ����Ʈ �ʱ�ȭ
    
        foreach (GameObject monster in respawnList)
        {
            if (monster != null)
            {
                monster.GetComponent<MonsterManager>().Respawn(); // ���� �ٽ� Ȱ��ȭ
                yield return new WaitForSeconds(delay); // ���� �ð� ������ �ΰ� �ϳ��� ��ȯ
            }
        }
    }



    public void OnButtonNOSetUser()
    {
        CanvasObject.instance.SetCheckBox.SetActive(false);
        objectdetec.SetActive(true);
    }
    public void OnButtonSetUser()
    {
        // Ŭ���� ��ġ�� Panel�� �ƴ� �� ����
        objectdetector.userSpawner.UserSpawn(objectdetector.Tileposition);
        CanvasObject.instance.SetCheckBox.SetActive(false);
        objectdetec.SetActive(true);
    }
    public void OnButtonUpgrade()
    {
        usermanager = userInformation.Check_User.GetComponent<UserManager>();
        usermanager.Upgrade();
        CanvasObject.instance.User_Upgrade.text = "��ȭ ���� : " + usermanager.upgrade.ToString();
        CanvasObject.instance.User_Attack.text = "���ݷ� : " + usermanager.attack.ToString();

    }
    public void OnButtonCloseinfoPanel()
    {
        CanvasObject.instance.Information_Panel.SetActive(false);
    }

    public void OnbuttonJobUpgrade()
    {
        CanvasObject.instance.JobUpgrade_Panel.SetActive(true);
        if (userInformation.Check_User.layer == LayerMask.NameToLayer("Adventurer"))
        {
            CanvasObject.instance.adventurerUpgrade_Panel.SetActive(true);
        }
        else if (userInformation.Check_User.layer == LayerMask.NameToLayer("Warrior"))
        {
            CanvasObject.instance.WarriorUpgrade_Panel.SetActive(true);
        }
        else if (userInformation.Check_User.layer == LayerMask.NameToLayer("Wizard"))
        {
            CanvasObject.instance.WizardUpgrade_Panel.SetActive(true);
        }
        else if (userInformation.Check_User.layer == LayerMask.NameToLayer("Archer"))
        {
            CanvasObject.instance.ArcherUpgrade_Panel.SetActive(true);
        }
        else if (userInformation.Check_User.layer == LayerMask.NameToLayer("Thief"))
        {
            CanvasObject.instance.ThiefUpgrade_Panel.SetActive(true);
        }
        else if (userInformation.Check_User.layer == LayerMask.NameToLayer("Pirate"))
        {
            CanvasObject.instance.PirateUpgrade_Panel.SetActive(true);
        }
    }

    public void OnbuttonChooseJob(int id)
    {
        //���� ĳ������ ���� ����
        int oldLevel = userInformation.Check_User_inform.level;
        int oldAttackPower = userInformation.Check_User_inform.attack;
        float oldmax_Exp = userInformation.Check_User_inform.max_experience;
        float oldcur_Exp = userInformation.Check_User_inform.cur_experience;
        int oldupgrade = userInformation.Check_User_inform.upgrade;

        // ���� ������ ��ġ ����
        Vector3 spawnPosition = userInformation.Check_User.transform.position;

        // ���ο� ���� ����
        GameObject newUser = Instantiate(userdata.UserList[id], spawnPosition, Quaternion.identity);

        // ���� ���� ����
        Destroy(userInformation.Check_User);

        // ���ο� ���� ����
        userInformation.Check_User = newUser;
        userInformation.Check_User_inform = newUser.GetComponent<UserManager>();

        //���� ĳ������ ������ ���ο� ĳ���Ϳ� ����
        userInformation.Check_User_inform.level = oldLevel;
        userInformation.Check_User_inform.attack = oldAttackPower;
        userInformation.Check_User_inform.max_experience = oldmax_Exp;
        userInformation.Check_User_inform.cur_experience = oldcur_Exp;
        userInformation.Check_User_inform.upgrade = oldupgrade;

        CanvasObject.instance.JobUpgrade_Panel.SetActive(false);
        foreach (Transform child in CanvasObject.instance.JobUpgrade_Panel.transform)
        {
            child.gameObject.SetActive(false);
        }

        // UI ������Ʈ
        userInformation.SetInfoPanel();
    }




}
