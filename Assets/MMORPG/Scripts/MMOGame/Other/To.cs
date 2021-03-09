using System.Collections.Generic;
using ETModel;
using Google.Protobuf.Collections;
using System.Net;
using System.Threading.Tasks;
using System.Linq;

namespace MMOGame
{
    /// <summary>
    /// 容器转换辅助方法
    /// </summary>
    public static class To
    {
        //数组-RepeatedField
        public static RepeatedField<T> RepeatedField<T>(T[] arr)
        {
            RepeatedField<T> a = new RepeatedField<T>();
            foreach (var b in arr)
            {
                a.Add(b);
            }
            return a;
        }

        //列表-RepeatedField
        public static RepeatedField<T> RepeatedField<T>(List<T> list)
        {
            RepeatedField<T> a = new RepeatedField<T>();
            foreach (var b in list)
            {
                a.Add(b);
            }
            return a;
        }

        //重复字段-RepeatedField
        public static T[] Array<T>(RepeatedField<T> fields)
        {
            T[] a = new T[fields.Count];
            for (int i = 0; i < fields.Count; i++)
            {
                a[i] = fields[i];
            }
            return a;
        }

        //重复字段-列表
        public static List<T> List<T>(RepeatedField<T> fields)
        {
            List<T> a = new List<T>();
            foreach (var b in fields)
            {
                a.Add(b);
            }
            return a;
        }

        // 数组末尾添加
        public static T[] ArrayAdd<T>(T[] arr,T a)where T : new(){
            List<T> list = new List<T>();
            list.Add(a);
            return list.ToArray();

        }
    }
}