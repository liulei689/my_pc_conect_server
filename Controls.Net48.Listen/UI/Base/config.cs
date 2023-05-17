using SharpConfig;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UI.Properties;

namespace UI.Base
{
    public static class IniConfig
    {
        public enum IniType
        {
            Read,
            Write
        }
        public static Encoding _encoding = null;
        public static string _path = "";
        public static Configuration Path(string path, IniType iniType = IniType.Read, Encoding encoding = null)
        {
            _encoding = encoding;
            _path = path;
            if (iniType == IniType.Write)
                return new Configuration();
            if (!Directory.Exists(System.IO.Path.GetDirectoryName(path))) Directory.CreateDirectory((System.IO.Path.GetDirectoryName(path)));
            return Configuration.LoadFromFile(path, encoding);
        }
        public static Configuration CreateSectionsFromOneObject(this Configuration cfg, object p)
        {
            cfg.Add(Section.FromObject(p.GetType().Name, p));
            return cfg;
        }
        public static bool SaveCacheToFile(this Configuration cfg)
        {
            try
            {
                cfg.SaveToFile(_path, _encoding);
            }
            catch { return false; }
            return true;
        }
        public static async Task<bool> SaveCacheToFileAsync(this Configuration cfg)
        {
            return await Task.Run(() => {
                return cfg.SaveCacheToFile();
            });
        }
        public static T LoadFromIFile<T>(this Configuration cfg) where T : new()
        {
            T val = new T();
            cfg[val.GetType().Name].SetValuesTo(val);
            return val;
        }
        private static void PrintConfig(Configuration cfg)
        {
            foreach (Section section in cfg)
            {
                Console.WriteLine("[{0}]", section.Name);

                foreach (Setting setting in section)
                {
                    Console.Write("  ");

                    if (setting.IsArray)
                        Console.Write("[Array, {0} elements] ", setting.ArraySize);

                    Console.WriteLine(setting.ToString());
                }
                Console.WriteLine();
            }
        }
    }
}
