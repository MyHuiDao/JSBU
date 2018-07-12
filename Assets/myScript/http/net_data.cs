using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class net_data  {

    public string data { get; set; }
    public string show { get; set; }
    public net_data(string data,string show)
    {
        this.data = data;
        this.show = show;
    }
}
