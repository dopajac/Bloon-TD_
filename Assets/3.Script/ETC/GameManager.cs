using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int meso; // 皋家
    
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
        CanvasObject.instance.User_life.text = "格见 : " + life;
        CanvasObject.instance.User_meso.text = "皋家 : " + meso;
        //Stageexperience += 
    }
}
