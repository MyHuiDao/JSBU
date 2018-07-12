using System;
using System.Text;
/// <summary>
/// 物品
/// </summary>
[Serializable]
public class Item
{

    public float x { get; set; }
    public float y { get; set; }   // 最大可持有数量

    public Item(float x, float y)
    {
        this.x = x;
        this.y = y;
    }

}