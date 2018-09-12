using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData : SingletonMonoBehaviour<SaveData>
{
    //ゲットしたか？
    public void setNewAtom(int atomicNumber)
    {
        string key = GlobalData.Instance.atomName[atomicNumber];
        PlayerPrefs.SetInt(key, 1);
    }
    //コレクション画面で解放したか？
    public void releaseAtom(int atomicNumber)
    {
        string key = GlobalData.Instance.atomName[atomicNumber] + "release";
        PlayerPrefs.SetInt(key, 1);
    }

    public bool hasTheAtom(int atomicNumber)
    {
        string key = GlobalData.Instance.atomName[atomicNumber];
        if (PlayerPrefs.GetInt(key,0) == 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool releaseTheAtom(int atomicNumber)
    {
        string key = GlobalData.Instance.atomName[atomicNumber] + "release";
        if (PlayerPrefs.GetInt(key, 0) == 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public int highScore(int stageNo)
    {
        string key = ""+stageNo;
        return PlayerPrefs.GetInt(key,0);
    }
    public void setHighScore(int stageNo,int highScore)
    {
        string key = "" + stageNo;
        int prevHighScore = PlayerPrefs.GetInt(key, 0);
        if (prevHighScore < highScore)
        {
            PlayerPrefs.SetInt(key, highScore);
        }
    }

    //1:そのステージをノーダメージでクリアした
    public void setMaxHPclear(int stageNo)
    {
        string key = "maxHP" + stageNo;
        int prev = PlayerPrefs.GetInt(key, 0);
        if (prev == 1)
        {
            return;
        }
        PlayerPrefs.SetInt(key, 1);
    }
    public bool isClearMaxHP(int stageNo)
    {
        string key = "maxHP" + stageNo;
        if (PlayerPrefs.GetInt(key,0) == 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //そのステージをクリアしたか？
    public void setClear(int stageNo)
    {
        string key = "clear" + stageNo;
        int prev = PlayerPrefs.GetInt(key, 0);
        if (prev == 1)
        {
            return;
        }
        PlayerPrefs.SetInt(key, 1);
    }
    public bool isClear(int stageNo)
    {
        string key = "clear" + stageNo;
        if (PlayerPrefs.GetInt(key, 0) == 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void delete()
    {
        PlayerPrefs.DeleteAll();
    }
}
