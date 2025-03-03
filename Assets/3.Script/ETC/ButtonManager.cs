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

    

    // 특정 UI 오브젝트 위를 클릭했는지 확인하는 함수
    private bool IsPointerOverUIObject(string uiObjectName)
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition // 마우스 클릭 위치 가져오기
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (RaycastResult result in results)
        {
            if (result.gameObject.name.Contains(uiObjectName)) // 특정 이름이 포함된 UI 감지
            {
                return true; // 특정 UI 위를 클릭했음
            }
        }
        return false; // 특정 UI 위가 아님
    }

    public void OnRespawnButtonClick()
    {
        StartCoroutine(RespawnMonstersWithInterval(monsterSpawner.spawnDelay));
    }
    
    private IEnumerator RespawnMonstersWithInterval(float delay)
    {
        List<GameObject> respawnList = new List<GameObject>(monsterSpawner.alivemonster);
        monsterSpawner. alivemonster.Clear(); // 기존 리스트 초기화
    
        foreach (GameObject monster in respawnList)
        {
            if (monster != null)
            {
                monster.GetComponent<MonsterManager>().Respawn(); // 몬스터 다시 활성화
                yield return new WaitForSeconds(delay); // 일정 시간 간격을 두고 하나씩 소환
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
        // 클릭한 위치가 Panel이 아닐 때 실행
        objectdetector.userSpawner.UserSpawn(objectdetector.Tileposition);
        CanvasObject.instance.SetCheckBox.SetActive(false);
        objectdetec.SetActive(true);
    }
    public void OnButtonUpgrade()
    {
        usermanager = userInformation.Check_User.GetComponent<UserManager>();
        usermanager.Upgrade();
        CanvasObject.instance.User_Upgrade.text = "강화 레벨 : " + usermanager.upgrade.ToString();
        CanvasObject.instance.User_Attack.text = "공격력 : " + usermanager.attack.ToString();

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
        //기존 캐릭터의 정보 저장
        int oldLevel = userInformation.Check_User_inform.level;
        int oldAttackPower = userInformation.Check_User_inform.attack;
        float oldmax_Exp = userInformation.Check_User_inform.max_experience;
        float oldcur_Exp = userInformation.Check_User_inform.cur_experience;
        int oldupgrade = userInformation.Check_User_inform.upgrade;

        // 현재 유저의 위치 저장
        Vector3 spawnPosition = userInformation.Check_User.transform.position;

        // 새로운 유저 생성
        GameObject newUser = Instantiate(userdata.UserList[id], spawnPosition, Quaternion.identity);

        // 기존 유저 삭제
        Destroy(userInformation.Check_User);

        // 새로운 유저 적용
        userInformation.Check_User = newUser;
        userInformation.Check_User_inform = newUser.GetComponent<UserManager>();

        //기존 캐릭터의 정보를 새로운 캐릭터에 적용
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

        // UI 업데이트
        userInformation.SetInfoPanel();
    }




}
