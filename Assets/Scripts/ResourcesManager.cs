using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//管理类
public class ResourcesManager : MonoBehaviour
{
    //单例模式，静态变量
    public static ResourcesManager Instance;
    //删除时间

    //当前活动对象的泛型集合
    public List<GameObject> activeObjDic;
    //当前即将非活动对象的泛型集合
    public List<GameObject> inActiveObjDic;
    //当前非活动对象的泛型集合
    public List<GameObject> onActiveObjDic;
    #region get set
    public List<GameObject> ActiveObjDic
    {
        get
        {
            if (activeObjDic == null)
            {
                activeObjDic = new List<GameObject>();
            }
            return activeObjDic;
        }
        set
        {
            activeObjDic = value;
        }
    }

    public List<GameObject> InActiveObjDic 
    {
        get 
        {
            if(inActiveObjDic == null)
            {
                inActiveObjDic = new List<GameObject>();
            }
            return inActiveObjDic;
        }
        set 
        {
            inActiveObjDic = value;
        }
    }

    public List<GameObject> OnActiveObjDic
    {
        get
        {
            if(onActiveObjDic == null)
            {
                onActiveObjDic = new List<GameObject>();
            }
            return onActiveObjDic;
        }
        set
        {
            onActiveObjDic = value;
        }
    }
    #endregion

    //单例模式初始化
    private void Awake()
    {
        Instance = this;
    }


    //检测该对象是否在活动的泛型集合
    public GameObject CheckIsActive(GameObject go)
    {
        if(ActiveObjDic.Contains(go))
        {
            go.GetComponent<Renderer>().enabled = true;
            return go;
        }
        return null;
    }
    //检测该对象是否在即将非活动对象的泛型集合
    public GameObject CheckIsInActive(GameObject go)
    {
        if(InActiveObjDic.Contains(go))
        {
            go.GetComponent<Renderer>().enabled = true;
            ActiveObjDic.Add(go);
            InActiveObjDic.Remove(go);
            return go;
        }
        return null;
    }
    //检测该对象是否在非活动对象的泛型集合
    public GameObject CheckIsOnActive(GameObject go)
    {
        if(OnActiveObjDic.Contains(go))
        {
            go.GetComponent<Renderer>().enabled = true;
            ActiveObjDic.Add(go);
            OnActiveObjDic.Remove(go);
            return go;
        }
        return null;
    }

    //加载对象
    public void Load(GameObject go)
    {
        //如果该对象处于活动状态，则只修改该对象数据的状态即可
        if (CheckIsActive(go) != null)
        {
            return;
        }
        //如果该对象不在非活跃字典中
        if (CheckIsInActive(go) != null)
        {
            return;
        }

        if (CheckIsOnActive(go) == null)
        {
            Debug.Log("错误");
        }
    }
    
    //角色移动创建物体后刷新场景对象状态，将此次未进入节点（status = old）的物体从显示字典中移到隐藏字典中，并将此次进入节点（status = new）的物体标记为old为下次创建做准备
    public void RefreshStatus()
    {
        //将即将非活动对象的泛型集合的所有对象禁用并移入非活动对象的泛型集合
        for (int i = 0; i < InActiveObjDic.Count; ++i)
        {
            InActiveObjDic[i].GetComponent<Renderer>().enabled = false;
            OnActiveObjDic.Add(InActiveObjDic[i]);
            InActiveObjDic.Remove(InActiveObjDic[i]);
        }

        //将活动对象的泛型集合的所有对象移入即将非活动对象的泛型集合
        for (int i = 0; i < ActiveObjDic.Count; ++i)
        {
            InActiveObjDic.Add(ActiveObjDic[i]);
            ActiveObjDic.Remove(ActiveObjDic[i]);
        }
    }
}
