using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class json_toggle
{
    public string name { get; set; }
    public string key { get; set; }
    public json_toggle(string name, string key)
    {
        this.name = name;
        this.key = key;
    }
}
