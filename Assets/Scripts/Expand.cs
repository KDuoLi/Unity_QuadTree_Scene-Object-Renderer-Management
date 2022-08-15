using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Expand 
{
    //检测包围盒是否在摄像机范围内。如果该Bound的八个点都不在摄像机可视范围内，返回false，否则返回true

    //渲染管线是局部坐标系=》世界坐标系=》摄像机坐标系=》裁剪坐标系=》ndc-》屏幕坐标系，其中在后三个坐标系中可以很便捷的得到某个点是否处于摄像机可视范围内。

    //在此用裁剪坐标系来判断，省了几次坐标转换，判断某个点在摄像机可视范围内方法如下：

    //1.将该点转换到裁剪空间，得到裁剪空间中的坐标为vec(x,y,z,w)，那么如果-w<x<w&&-w<y<w&&-w<z<w，那么该点在摄像机可视范围内。

    //2.对Bound来说，它有8个点，当它的8个点同时处于摄像机裁剪块上方/下方/前方/后方/左方/右方，那么该bound不与摄像机可视范围交叉
    public static bool CheckBoundIsInCamera(this Bounds bound, Camera camera)
    {
        //传入裁剪空间中的Vector4坐标，如果该点在摄像机可视范围内，返回0
        System.Func<Vector4, int> ComputeOutCode = (projectionPos) =>
        {
            int _code = 0;
            if (projectionPos.x < -projectionPos.w) _code |= 1;
            if (projectionPos.x > projectionPos.w) _code |= 2;
            if (projectionPos.y < -projectionPos.w) _code |= 4;
            if (projectionPos.y > projectionPos.w) _code |= 8;
            if (projectionPos.z < -projectionPos.w) _code |= 16;
            if (projectionPos.z > projectionPos.w) _code |= 32;
            return _code;
        };

        Vector4 worldPos = Vector4.one;
        int code = 63;
        //对Bound的八个点
        for (int i = -1; i <= 1; i += 2)
        {
            for (int j = -1; j <= 1; j += 2)
            {
                for (int k = -1; k <= 1; k += 2)
                {
                    worldPos.x = bound.center.x + i * bound.extents.x;
                    worldPos.y = bound.center.y + j * bound.extents.y;
                    worldPos.z = bound.center.z + k * bound.extents.z;
                    //projectionMatrix为投影矩阵，是从摄像机空间转换到裁剪空间的矩阵；worldToCameraMatrix为从世界空间变换为摄像机空间的矩阵
                    code &= ComputeOutCode(camera.projectionMatrix * camera.worldToCameraMatrix * worldPos);
                }
            }
        }
        return code == 0 ? true : false;
    }
}
