using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    [SerializeField] private GameObject objectdetec;
    ObjectDetector objectdetector;

    public UserInformation userInformation;
    UserManager usermanager;
    public CanvasObject canvasObject;

    private void Start()
    {
        objectdetector = objectdetec.GetComponent<ObjectDetector>();
        TryGetComponent(out userInformation);
        TryGetComponent(out canvasObject);
    }

    public void OnButtonSetUser()
    {
        objectdetector.userSpawner.UserSpawn(objectdetector.Tileposition);
        canvasObject.SetCheckBox.SetActive(false);
        objectdetec.SetActive(true);
    }

    public void OnButtonNOSetUser()
    {
        canvasObject.SetCheckBox.SetActive(false);
        objectdetec.SetActive(true);
    }
    public void OnButtonUpgrade()
    {

        usermanager = userInformation.Check_User.GetComponent<UserManager>();
        usermanager.attack++;
        canvasObject.User_Attack.text = "���ݷ� : " + usermanager.attack.ToString();

    }
    public void OnButtonCloseinfoPanel()
    {
        canvasObject.Information_Panel.SetActive(false);
    }

    public void OnbuttonJobUpgrade()
    {
        canvasObject.JobUpgrade_Panel.SetActive(true);
        if (userInformation.Check_User.layer == LayerMask.NameToLayer("Adventurer"))
        {
            canvasObject.adventurerUpgrade_Panel.SetActive(true);
        }
        else if (userInformation.Check_User.layer == LayerMask.NameToLayer("Warrior"))
        {
            canvasObject.WarriorUpgrade_Panel.SetActive(true);
        }
        else if (userInformation.Check_User.layer == LayerMask.NameToLayer("Wizard"))
        {
            canvasObject.WizardUpgrade_Panel.SetActive(true);
        }
        else if (userInformation.Check_User.layer == LayerMask.NameToLayer("Archer"))
        {
            canvasObject.ArcherUpgrade_Panel.SetActive(true);
        }
        else if (userInformation.Check_User.layer == LayerMask.NameToLayer("Thief"))
        {
            canvasObject.ThiefUpgrade_Panel.SetActive(true);
        }
        else if (userInformation.Check_User.layer == LayerMask.NameToLayer("Pirate"))
        {
            canvasObject.PirateUpgrade_Panel.SetActive(true);
        }
    }

    public void OnbuttonChooseJob(int id)
    {

        // ���� ������ ��ġ ����
        Vector3 spawnPosition = userInformation.Check_User.transform.position;

        // ���� ���� ����
        Destroy(userInformation.Check_User);

        // ���ο� ���� ����
        GameObject newUser = Instantiate(canvasObject.userdata.UserList[id], spawnPosition, Quaternion.identity);
        Debug.Log(canvasObject.userdata.UserList[id].name);
        // Check_User ������Ʈ (���ο� ������ ����)
        userInformation.Check_User = newUser;
    }


}
