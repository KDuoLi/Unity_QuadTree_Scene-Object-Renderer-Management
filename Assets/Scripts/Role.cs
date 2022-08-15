using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[ExecuteAlways] 
public class Role : MonoBehaviour
{
    //是否移动
    public bool bMove = false;
    //用于判断角色是否移动
    public Vector3 lastPos = Vector3.zero;
    //移动速度
    public float moveSpeed = 5;
    //旋转速度
    public float rotSpeed = 3;
    //摄像机
    public Camera mCamera;
    //初始化
    private void Awake()
    {
        //获取摄像机组件
        mCamera = transform.Find("Camera").GetComponent<Camera>();
    }

    //WASD移动，并检测角色是否移动
    private void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.back * Time.deltaTime * moveSpeed);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.left * Time.deltaTime * moveSpeed);
            transform.localEulerAngles -= new Vector3(0, rotSpeed, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right * Time.deltaTime * moveSpeed);
            transform.localEulerAngles += new Vector3(0, rotSpeed, 0);
        }
        
        //检测人物是否移动。如果移动，则置bMove为true，更新lastPos
        if (lastPos != transform.position)
        {
            bMove = true;
            lastPos = transform.position;
        }
        else
        {
            bMove = false;
        }
    }
    
    //画出摄像机的视野范围
    private void OnDrawGizmos()
    {
        //包围盒的颜色为黄
        Gizmos.color = Color.yellow;
        //Gizmos.matrix存储 Gizmos的位置、旋转和缩放
        Matrix4x4 temp = Gizmos.matrix;
        //修改矩阵。Matrix4x4.TRS返回一个矩阵，第一个参数是位置，第二个参数是旋转定向，第三个参数是缩放
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
        //如果该摄像机是正交摄像机
        if (mCamera.orthographic)
        {
            //获取摄像机远、近裁剪平面的距离差值
            float spread = mCamera.farClipPlane - mCamera.nearClipPlane;
            //获取摄像机远、近裁剪平面的距离中值
            float center = (mCamera.farClipPlane + mCamera.nearClipPlane) * 0.5f;
            //画一个线框盒体，第一个参数是中心，第二个参数是大小
            Gizmos.DrawWireCube(new Vector3(0, 0, center), new Vector3(mCamera.orthographicSize * 2 * mCamera.aspect, mCamera.orthographicSize * 2, spread));
        }
        //如果该摄像机是透视摄像机
        else
        {
            //绘制截锥体。第一个参数是顶点，第二个参数是视野，第三个参数是该视椎体远平面的距离，第四个参数是该视椎体近平面的距离，第五个参数是宽度/高度比
            Gizmos.DrawFrustum(Vector3.zero, mCamera.fieldOfView, mCamera.farClipPlane, mCamera.nearClipPlane, mCamera.aspect);
        }
        //赋值,修改Gizmos的位置、旋转和缩放
        Gizmos.matrix = temp;
    }
}
