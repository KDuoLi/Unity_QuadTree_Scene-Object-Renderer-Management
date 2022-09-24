# QuadTree
- Unity版本：2021.3（LTS）
- 使用四叉树对场景物体的静态物体和动态物体进行渲染管理
- 几个问题：
1. 如何计算包围盒是否在摄像机的视野范围内
- 渲染管线是局部坐标系=》世界坐标系=》摄像机坐标系=》裁剪坐标系=》屏幕坐标系，其中在后三个坐标系中可以很便捷的得到某个点是否处于摄像机可视范围内。先将包围盒的八个点变换到裁剪坐标系下，再对其进行可视判断，一般情况下，如果八个点都不在摄像机视野范围内，则该包围盒不在摄像机视野范围内，否则在摄像机视野范围内。但是由于包围盒的y轴高度可变，如果y轴过大，则会出现包围盒的一部分在视锥体上，而包围盒的八个点不在视锥体上，导致不准确，所以这里，对Bound来说，它有8个点，当它的8个点同时处于摄像机裁剪块上方/下方/前方/后方/左方/右方，那么该bound不与摄像机可视范围交叉，即包围盒视锥体范围内。具体代码在Expand.cs
2. 如何对物体进行渲染管理
- 当玩家移动时，将刷新场景对象状态。即先将存在activeObjDic的对象放入inActiveObjDic和存在inActiveObjDic的对象放入onActiveObjDic，然后从根节点出发，刷新节点下的对象，其次对每个子节点的包围盒进行摄像机视野范围判断，如果在，刷新子节点下的物体状态并将这些物体放入activeObjDic中，直至到达最大深度或者该节点下没有子节点（如果该节点下没有物体，则不会继续创建子节点）。activeObjDic是存放当前状态渲染的对象，inActiveObjDic是存放下一帧不渲染的对象，onActiveObjDic是存放当前状态不渲染的对象
![](https://github.com/KDuoLi/myBlog-Picture/blob/main/QuadTree/%E6%B8%B2%E6%9F%93%E7%AE%A1%E7%90%86.png?raw=true)
3. 物体如何存放节点下
- Awake时，将所有物体放入四叉树中，如果物体同时被两个及以上包围盒的范围内，则该物体属于它们的父物体，否则属于唯一的子节点
![](https://github.com/KDuoLi/myBlog-Picture/blob/main/QuadTree/%E6%8F%92%E5%85%A5%E7%89%A9%E4%BD%93.png?raw=true)
4. 动态物体管理
![](https://github.com/KDuoLi/myBlog-Picture/blob/main/QuadTree/%E5%8A%A8%E6%80%81%E7%89%A9%E4%BD%93%E7%AE%A1%E7%90%86.png?raw=true)
