using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//挂载于对象预制体上
public class SceneObjData : MonoBehaviour
{
    //此时所属节点
    public Node nodeNow;
    //位置
    public Vector3 pos;
    
    public Camera cameraM;
    private void Start() 
    {
        cameraM = GameObject.Find("Camera").GetComponent<Camera>();
        pos = transform.position;
    }

    private void Update() 
    {
        if(pos != transform.position)
        {
            pos = transform.position;
            if(!nodeNow.bound.Contains(pos))
            {
                nodeNow.objList.Remove(gameObject);
                OutNode(nodeNow.parentNode, pos);
                Updateobj();
            }
        }
    }

    private void OutNode(Node node, Vector3 pos)
    {
        if(node == null)
        {
            return;
        }

        if(node.bound.Contains(pos))
        {
            node.InsertObj(gameObject);
            return;
        }
        else
        {
            OutNode(node.parentNode, pos);
        }
    }


    private void Updateobj()
    {
        //判断该节点的包围盒在不在摄像机视野范围内
        if(nodeNow.bound.CheckBoundIsInCamera(cameraM))
        {
            //判断该对象是否被渲染
            if(gameObject.GetComponent<Renderer>().enabled == false)
            {
                ResourcesManager.Instance.Load(gameObject);
                // gameObject.GetComponent<Renderer>().enabled = true;
                // //移除该对象在OnActiveObjDic、InActiveObjDic
                // if(ResourcesManager.Instance.OnActiveObjDic.Contains(gameObject))
                // {
                //     ResourcesManager.Instance.OnActiveObjDic.Remove(gameObject);
                // }
                // if(ResourcesManager.Instance.InActiveObjDic.Contains(gameObject))
                // {
                //     ResourcesManager.Instance.InActiveObjDic.Remove(gameObject);
                // }
                // //添加该对象在ActiveObjDic
                // if(!ResourcesManager.Instance.ActiveObjDic.Contains(gameObject))
                // {
                //     ResourcesManager.Instance.ActiveObjDic.Add(gameObject);
                // }
            } 
        }
        //不在
        else if(!nodeNow.bound.CheckBoundIsInCamera(cameraM))
        {
            if(gameObject.GetComponent<Renderer>().enabled == true)
            {
                gameObject.GetComponent<Renderer>().enabled = false;
                //移除该对象在ActiveObjDic
                if(ResourcesManager.Instance.ActiveObjDic.Contains(gameObject))
                {
                    ResourcesManager.Instance.ActiveObjDic.Add(gameObject);
                }
                //添加该对象在OnActiveObjDic
                if(ResourcesManager.Instance.OnActiveObjDic.Contains(gameObject))
                {
                    ResourcesManager.Instance.OnActiveObjDic.Remove(gameObject);
                }
            }
        }
    }
}
