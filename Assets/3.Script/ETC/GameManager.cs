using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int meso; // �޼�
    
    [SerializeField]
    public int life;
    [SerializeField]
    public int StageLevel;
    [SerializeField]
    public int Stageexperience;
    private void Awake()
    {
        if (instance != null)
        {
            return;
        }
        instance = this;
    }

    private void Start()
    {
        meso = 100;
        life = 100;
    }

    private void Update()
    {
        CanvasObject.instance.User_life.text = "��� : " + life;
        CanvasObject.instance.User_meso.text = "�޼� : " + meso;
        //Stageexperience += 
    }
}
