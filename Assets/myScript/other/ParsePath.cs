using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ParsePath {

    List<Vector2[]> list = new List<Vector2[]>();
    
    public List<Vector2[]> JieXiPath()
    {
		TextAsset data = Resources.Load("PathRes/path") as TextAsset;
        if (data == null)
            return null;
		System.IO.MemoryStream ms = new MemoryStream(data.bytes);
        System.IO.BinaryReader br = new BinaryReader(ms);
        int count = br.ReadInt32();
        int count1 = br.ReadInt32();
        for (int j = 0; j < count; ++j)
        {
            Vector2[] points = new Vector2[count1];
            for (int i = 0; i < count1; ++i)
            {
                points[i].x = float.Parse(br.ReadString());
                points[i].y = float.Parse(br.ReadString());
            }
            list.Add(points);
            if (j == count - 1)
                break;
            count1 = br.ReadInt32();
        }
        ms.Close();
        br.Close();
        return list;
    }
    

}
