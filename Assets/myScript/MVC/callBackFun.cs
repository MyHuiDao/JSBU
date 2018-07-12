using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;


/// <summary>
/// 回调函数，保存Contrall的方法。
/// </summary>
class CallBackFun
{
   
    private static CallBackFun md = null;
    public  Dictionary<int, MethodInfo> methods = null;
    private object doObject = new object();
    public  object[] pars = new object[1];
    public object obj;

   
    private CallBackFun()
    {
        methods = new Dictionary<int, MethodInfo>();
    }
    public static CallBackFun shareCallBack//实例化callBackFun对象，创建出一个字典，存放contrall控制类的方法
    {
        get
        {
            if (md == null) md = new CallBackFun();
            return md;
        }
    }
    /// <summary>
    /// 添加方法
    /// </summary>
    /// <param name="o">反射对象</param>
    public void addMethod(object o)
    {
        Type t = o.GetType();
        //obj= Activator.CreateInstance(t);//创建对象的实例
        //object[] mParam = new object[] { 5, 10 };//构造一个object数组作为参数
        obj = o;
        MethodInfo[] mf = t.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static);//获得对象的所有方法
        for (int i = 0; i < mf.Length; i++)
        {
          
            if (mf[i].Name.Substring(0,2)=="do")
            {
             
                int methodID = int.Parse(mf[i].Name.Substring(2,5));
            
                //addMethod(o, methodID, mf[i]);//在此处又用了重载addMethod,没必要，直接添加即可。用DO来做判断，后面的数字作为键值，方法本身作为内容存储。
                methods.Add(methodID, mf[i]); 
              
                //MessageBox.Show("sss");
            }
        }
        /*

        MethodInfo mm = t.GetMethod(methodName);
        int type = int.Parse(mm.Name.Substring(2));
        addMethod(o, type, mm);*/
    }


    /// <summary>
    /// 删除某一方法
    /// </summary>
    /// <param name="type"></param>
    public void removeMethod(int type)
    {
        if (this.methods.ContainsKey(type))
        {
            this.methods.Remove(type);
        }
    }
   
    /// <summary>
    /// 判断字典里面是否有此方法
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public bool isMethod(int type)
    {
        if (!this.methods.ContainsKey(type)) return false;
        return true;
    }
    /// <summary>
    /// 删除字典里面的所有方法
    /// </summary>
    public void removeAllMethods()
    {
        this.methods.Clear();
        //formRun = null;
    }
    /// <summary>
    /// 删除某个对象监听的所有方法
    /// </summary>
    /// <param name="o">对象</param>
    public void remove(object o)
    {
        Type t = o.GetType();
        MethodInfo[] mf = t.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static);
        for (int i = 0; i < mf.Length; i++)
        {
            if (mf[i].Name.StartsWith("do"))
            {
                int methodID = int.Parse(mf[i].Name.Substring(2));
                removeMethod(methodID);
            }
        }
    }
}





