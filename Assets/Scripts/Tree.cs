using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//四叉树
public class Tree : INode
{
    //树的包围盒
    public Bounds bound { get; set; }
    //根节点
    private Node root;
    //最大深度
    public int maxDepth { get; set; }
    //最大孩子数量
    public int maxChildCount { get; set; }
    //构造函数
    public Tree(Bounds bound,int maxDepth)
    {
        this.bound = bound;
        this.maxDepth = maxDepth;
        this.maxChildCount = 4;
        root = new Node(bound, 0, this, null);
    }
    
    public void InsertObj(GameObject go)
    {
        root.InsertObj(go);
    }

    public void TriggerMove(Camera camera)
    {
        root.TriggerMove(camera);
    }

    public void DrawBound()
    {
        root.DrawBound();
    }
}
