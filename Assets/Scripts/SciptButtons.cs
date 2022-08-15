using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SciptButtons : MonoBehaviour
{
    
    public void VitessGenerater(lvl state)
    {
        switch (state)
        {
            case lvl.Lvl_1:
                GameManager.vitess = 100;
                break;
            case lvl.Lvl_2:
                GameManager.vitess = 80;
                break;
            case lvl.Lvl_3:
                GameManager.vitess = 30;
                break;
            default:
                GameManager.vitess = 90;
                break;
        }
    }
    public void SelectLvl1()
    {
        VitessGenerater(lvl.Lvl_1);
        SceneManager.LoadScene("main");
    }
    public void SelectLvl2()
    {
        VitessGenerater(lvl.Lvl_2);
        SceneManager.LoadScene("main");
    }
    public void SelectLvl3()
    {
        VitessGenerater(lvl.Lvl_3);
        SceneManager.LoadScene("main");
    }
}
public enum lvl
{
    Lvl_1,
    Lvl_2,
    Lvl_3
}