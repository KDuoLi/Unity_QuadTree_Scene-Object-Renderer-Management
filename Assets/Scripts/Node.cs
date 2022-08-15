using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Node : INode
{
    //包围盒
    public Bounds bound { get; set; }
    //当前深度
    private int depth;
    //该节点所属的树
    private Tree belongTree;
    //孩子节点数组
    private Node[] childList;
    //该节点下对象数据的泛型集合
    public List<GameObject> objList;
    //父节点
    public Node parentNode;
    //构造函数
    public Node(Bounds bound, int depth, Tree belongTree, Node parentN)
    {
        this.parentNode = parentN;
        this.belongTree = belongTree;
        this.bound = bound;
        this.depth = depth;
        //childList = new Node[belongTree.maxChildCount];
        objList = new List<GameObject>();
    }
    
    public void InsertObj(GameObject go)
    {
        Node node = null;
        bool bChild = false;
        //如果还没到叶子节点，可以拥有儿子且儿子未创建，则创建儿子（受该变量的访问属性会影响！！！）
        if(depth < belongTree.maxDepth && childList == null)
        {
            CerateChild();
        }
        //如果已经创建孩子节点数组
        if(childList != null)
        {
            for (int i = 0; i < childList.Length; ++i)
            {
                //按索引i获取孩子节点
                Node item = childList[i];
                //如果孩子节点为空，则跳出循环
                if (item == null)
                {
                    break;
                }
                //如果该对象的位置包含在包围盒中
                if (item.bound.Contains(go.transform.position))
                {
                    //如果局部变量node不为空
                    if (node != null)
                    {
                        //跳出循环，不属于孩子节点，属于该节点
                        bChild = false;
                        break;
                    }
                    //将局部变量item赋值给局部变量node
                    node = item;
                    bChild = true;
                }
            }
        }
        
        if (bChild)
        {
            node.InsertObj(go);
        }
        else
        {
            go.GetComponent<SceneObjData>().nodeNow = this;
            objList.Add(go);
        }
    }

    public void TriggerMove(Camera camera)
    {
        //角色移动刷新场景对象状态
        if(depth == 0)
        {
            ResourcesManager.Instance.RefreshStatus();
        }

        //刷新当前节点下的对象
        for(int i = 0; i < objList.Count; ++i)
        {
            //进入该节点中意味着该节点在摄像机内，把该节点保存的物体全部创建出来
            ResourcesManager.Instance.Load(objList[i]);
        }


        //刷新孩子节点
        if (childList != null)
        {
            //对所有孩子节点
            for(int i = 0; i < childList.Length; ++i)
            {
                //如果该孩子节点的包围盒在摄像机视野范围内
                if (childList[i].bound.CheckBoundIsInCamera(camera))
                {
                    childList[i].TriggerMove(camera);
                }
            }
        }
    }

    //创建孩子节点数组并加入孩子节点
    private void CerateChild()
    {
        //根据所属树的最大孩子数量来创建孩子节点的泛型集合
        childList = new Node[belongTree.maxChildCount];
        //索引
        int index = 0;
        for(int i = -1; i <= 1; i+=2)
        {
            for(int j = -1; j <= 1; j+=2)
            {
                //获取孩子节点的中心偏移
                Vector3 centerOffset = new Vector3(bound.size.x / 4 * i, 0, bound.size.z / 4 * j);
                //获取孩子节点的大小
                Vector3 cSize = new Vector3(bound.size.x / 2, bound.size.y, bound.size.z / 2);
                //根据以上创建一个包围盒
                Bounds cBound = new Bounds(bound.center + centerOffset, cSize);
                //根据包围盒创建孩子节点，按索引index赋值
                childList[index++] = new Node(cBound, depth + 1, belongTree, this);
            }
        }
    }
    
    //画出该节点的包围盒
    public void DrawBound()
    {
        //该节点的对象数据的泛型集合的数量不为0
        if(objList.Count != 0)
        {
            //画出一个蓝色线框盒体
            Gizmos.color = Color.blue;
            //第一个参数是中心，第二个参数是大小
            Gizmos.DrawWireCube(bound.center, bound.size - Vector3.one * 0.1f);
        }
        //否则（该节点下没有对象数据）
        else
        {
            //画出一个绿色线框盒体
            Gizmos.color = Color.green;
            //第一个参数是中心，第二个参数是大小
            Gizmos.DrawWireCube(bound.center, bound.size - Vector3.one * 0.1f);
        }
        //如果孩子节点数组不为空，则画出所有孩子节点数组的包围盒
        if(childList != null)
        {
            for(int i = 0; i < childList.Length; ++i)
            {
                childList[i].DrawBound();
            }
        }
        
    }

    
}
