using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Main : MonoBehaviour
{

    //对象数据
    public List<GameObject> objList;
    //获取需要管理对象的父对象
    public Transform go;
    //包围盒
    public Bounds mainBound;
    //四叉树
    private Tree tree;
    //记录初始化是否结束
    private bool bInitEnd = false;
    //Role
    private Role role;
    //树的最大深度
    public int maxDepth;
    //初始化
    public void Awake()
    {
        objList = new List<GameObject>(go.transform.childCount);
        //将所有对象插入onactiveDic和objList
        go = GameObject.Find("Objs").transform;
        ResourcesManager.Instance.goM = new Dictionary<GameObject, MeshRenderer>(go.transform.childCount);
        for (int i = 0; i < go.transform.childCount;++i)
        {
            ResourcesManager.Instance.OnActiveObjDic.Add(go.GetChild(i).gameObject);
            go.GetChild(i).gameObject.GetComponent<MeshRenderer>().enabled = false;
            ResourcesManager.Instance.goM.Add(go.GetChild(i).gameObject, go.GetChild(i).gameObject.GetComponent<MeshRenderer>());
            objList.Add(go.GetChild(i).gameObject);
        }

        //新建一棵四叉树
        tree = new Tree(mainBound, maxDepth);
        //将泛型集合中的所有对象数据插入四叉树中
        for(int i = 0; i < objList.Count; ++i)
        {
            tree.InsertObj(objList[i]);
        }
        
        //获取Role下的Role脚本组件
        role = GameObject.Find("Role").GetComponent<Role>();

        //改变初始化结束状态，完成初始化
        bInitEnd = true;
    }

    //如果角色移动，则更新物体
    private void Update()
    {
        if (role.bMove)
        {
            tree.TriggerMove(role.mCamera);
        }
        //tree.TriggerMove(role.mCamera);
        
    }
    //画出包围盒
    private void OnDrawGizmos()
    {
        //如果初始化结束，画出包围盒。开始后初始化才会结束
        if (bInitEnd)
        {
            tree.DrawBound();
        }
        //如果初始化未结束，绘制一个线框盒体
        else
        {
            Gizmos.DrawWireCube(mainBound.center, mainBound.size);
        }
    }
}
