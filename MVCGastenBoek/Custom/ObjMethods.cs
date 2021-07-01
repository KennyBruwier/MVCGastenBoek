using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace MVCGastenBoek.Custom
{
    public static class ObjMethods
    {
        /// <summary>
        /// copies properties values form source to target where target is tracked by entity framework (ex. update)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TU"></typeparam>
        /// <param name="source"></param>
        /// <param name="target"></param>
        public static void EditProperties<T,TU>(T source, TU target)
        {
            var sourceProps = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(x => x.CanRead).ToList();
            var targetProps = typeof(TU).GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(x => x.CanWrite).ToList();
            foreach (PropertyInfo sourceProp in sourceProps)
            {
                bool bContinue = true;
                PropertyInfo p = targetProps.FirstOrDefault(x => x.Name == sourceProp.Name);
                if (p != null)
                {
                    if (!(sourceProp.PropertyType.IsClass &&
                        (sourceProp.PropertyType.Assembly.FullName == typeof(T).Assembly.FullName)))
                    {
                        var sourcePropValue = sourceProp.GetValue(source, null);
                        if (sourceProp.Name.Substring(sourceProp.Name.Trim().Length - 2).ToLower() == "id")
                        {
                            if ((sourcePropValue is int @int && @int == 0) ||
                                (sourcePropValue is double @double && @double == 0))
                                bContinue = false;
                        }
                        if (bContinue)
                        {
                            if (p.CanWrite)
                            {
                                var targetValue = p.GetValue(target, null);
                                if ((p.PropertyType == sourceProp.PropertyType) &&
                                    (p.GetValue(target, null) != sourceProp.GetValue(source, null)))
                                    p.SetValue(target, sourceProp.GetValue(source, null), null);
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// copies properties values form source to target and creates a new instance of target 
        /// not suitable for EF updates, use EditProperties instead
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TU"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static TU CopyProperties<T, TU>(T source) where TU : new()
        {
            var sourceProps = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(x => x.CanRead).ToList();
            var destProps = typeof(TU).GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(x => x.CanWrite).ToList();
            TU dest = new TU();
            foreach (PropertyInfo sourceProp in sourceProps)
            {
                bool bContinue = true;
                PropertyInfo p = destProps.FirstOrDefault(x => x.Name == sourceProp.Name);
                if (p != null)
                {
                    if (!(sourceProp.PropertyType.IsClass && 
                        (sourceProp.PropertyType.Assembly.FullName == typeof(T).Assembly.FullName)))
                    {
                        var sourcePropValue = sourceProp.GetValue(source, null);
                        if (sourceProp.Name.Substring(sourceProp.Name.Trim().Length - 2).ToLower() == "id")
                        {
                            if ((sourcePropValue is int @int && @int == 0) ||
                                (sourcePropValue is double @double && @double == 0))
                                bContinue = false;
                            //var keyAttribute = Attribute.GetCustomAttribute(p, typeof(KeyAttribute)) as KeyAttribute;
                            //if (keyAttribute != null) // don't copy key
                            //    bContinue = false;
                        }
                        if (bContinue)
                        {
                            if (p.CanWrite)
                            {
                                var destValue = p.GetValue(dest, null);
                                if ((p.PropertyType == sourceProp.PropertyType)&&
                                    (p.GetValue(dest,null) != sourceProp.GetValue(source,null)))
                                    p.SetValue(dest, sourceProp.GetValue(source, null), null);
                            }
                        }
                    }
                }
            }
            return dest;
        }
    }
}
