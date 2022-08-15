using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INode
{
    Bounds bound { get; set; }
    /// <summary>
    /// 初始化插入一个场景物体
    /// </summary>
    /// <param name="obj"></param>
    void InsertObj(GameObject go);
    /// <summary>
    /// 当触发者（主角）移动时显示/隐藏物体
    /// </summary>
    /// <param name="camera"></param>
    void TriggerMove(Camera camera);
    /// <summary>
    /// 画出节点的包围盒
    /// </summary>
    void DrawBound();
}
