using System.Collections;
using System.IO;
using System.Collections.Generic;
using System;

namespace UnityEditor.iOS.Xcode.Custom
{
    public class XcodeSetting
    {
        private static string pluginPath = "";
        public static void OnPostprocessBuild(string xcodePath, string pluginPath)
        {
            XcodeSetting.pluginPath = pluginPath;
            string projPath = PBXProject.GetPBXProjectPath(xcodePath);
            PBXProject proj = new PBXProject();

            proj.ReadFromString(File.ReadAllText(projPath));
            //读取配置文件
            string configPath = Path.Combine(pluginPath, "module/XcodeSetting.json");
            string json = File.ReadAllText(configPath);
            Hashtable table = json.hashtableFromJson();

            //lib
            SetLibs(proj, table.SGet<Hashtable>("libs"));
            //framework
            SetFrameworks(proj, table.SGet<Hashtable>("frameworks"));
            //building setting
            SetBuildProperties(proj, table.SGet<Hashtable>("properties"));
            //复制文件
            CopyFiles(proj, xcodePath, table.SGet<Hashtable>("files"));
            //复制文件夹
            CopyFolders(proj, xcodePath, table.SGet<Hashtable>("folders"));
            //文件编译符号
            SetFilesCompileFlag(proj, table.SGet<Hashtable>("filesCompileFlag"));
            //写入
            File.WriteAllText(projPath, proj.WriteToString());
            //plist
            string plistPath = xcodePath + "/Info.plist";
            PlistDocument plist = new PlistDocument();
            plist.ReadFromString(File.ReadAllText(plistPath));
            PlistElementDict rootDict = plist.root;

            SetPlist(proj, rootDict, table.SGet<Hashtable>("plist"));
            //写入
            plist.WriteToFile(plistPath);
        }

        private static void AddLibToProject(PBXProject inst, string targetGuid, string lib)
        {
            string fileGuid = inst.AddFile("usr/lib/" + lib, "Frameworks/" + lib, PBXSourceTree.Sdk);
            inst.AddFileToBuild(targetGuid, fileGuid);
        }

        private static void RemoveLibFromProject(PBXProject inst, string targetGuid, string lib)
        {
            string fileGuid = inst.AddFile("usr/lib/" + lib, "Frameworks/" + lib, PBXSourceTree.Sdk);
            inst.RemoveFileFromBuild(targetGuid, fileGuid);
        }

        //设置frameworks
        private static void SetFrameworks(PBXProject proj, Hashtable table)
        {
            if (table != null)
            {
                string target = proj.TargetGuidByName(PBXProject.GetUnityTargetName());
                ArrayList addList = table["+"] as ArrayList;
                if (addList != null)
                {
                    foreach (string i in addList)
                    {
                        proj.AddFrameworkToProject(target, i, false);
                    }
                }
                ArrayList removeList = table["-"] as ArrayList;
                if (removeList != null)
                {
                    foreach (string i in removeList)
                    {
                        proj.RemoveFrameworkFromProject(target, i);
                    }
                }
            }
        }

        //设置libs
        private static void SetLibs(PBXProject proj, Hashtable table)
        {
            if (table != null)
            {
                string target = proj.TargetGuidByName(PBXProject.GetUnityTargetName());
                ArrayList addList = table["+"] as ArrayList;
                if (addList != null)
                {
                    foreach (string i in addList)
                    {
                        AddLibToProject(proj, target, i);
                    }
                }
                ArrayList removeList = table["-"] as ArrayList;
                if (removeList != null)
                {
                    foreach (string i in removeList)
                    {
                        RemoveLibFromProject(proj, target, i);
                    }
                }
            }
        }

        //设置编译属性
        private static void SetBuildProperties(PBXProject proj, Hashtable table)
        {
            if (table != null)
            {
                string target = proj.TargetGuidByName(PBXProject.GetUnityTargetName());
                Hashtable setTable = table.SGet<Hashtable>("=");
                foreach (DictionaryEntry i in setTable)
                {
                    proj.SetBuildProperty(target, i.Key.ToString(), i.Value.ToString());
                }
                Hashtable addTable = table.SGet<Hashtable>("+");
                foreach (DictionaryEntry i in addTable)
                {
                    ArrayList array = i.Value as ArrayList;
                    List<string> list = new List<string>();
                    foreach (var flag in array)
                    {
                        list.Add(flag.ToString());
                    }
                    proj.UpdateBuildProperty(target, i.Key.ToString(), list, null);
                }
                Hashtable removeTable = table.SGet<Hashtable>("-");
                foreach (DictionaryEntry i in removeTable)
                {
                    ArrayList array = i.Value as ArrayList;
                    List<string> list = new List<string>();
                    foreach (var flag in array)
                    {
                        list.Add(flag.ToString());
                    }
                    proj.UpdateBuildProperty(target, i.Key.ToString(), null, list);
                }
            }
        }

        //设置plist
        private static void SetPlist(PBXProject proj, PlistElementDict node, Hashtable arg)
        {
            if (arg != null)
            {
                foreach (DictionaryEntry i in arg)
                {
                    string key = i.Key.ToString();
                    object val = i.Value;
                    var vType = i.Value.GetType();
                    if (vType == typeof(string))
                    {
                        node.SetString(key, (string)val);
                    }
                    else if (vType == typeof(bool))
                    {
                        node.SetBoolean(key, (bool)val);
                    }
                    else if (vType == typeof(double))
                    {
                        int v = int.Parse(val.ToString());
                        node.SetInteger(key, v);
                    }
                    else if (vType == typeof(ArrayList))
                    {
                        var t = node.CreateArray(key);
                        var array = val as ArrayList;
                        SetPlist(proj, t, array);
                    }
                    else if (vType == typeof(Hashtable))
                    {
                        var t = node.CreateDict(key);
                        var table = val as Hashtable;
                        SetPlist(proj, t, table);
                    }
                }
            }
        }

        private static void SetPlist(PBXProject proj, PlistElementArray node, ArrayList arg)
        {
            if (arg != null)
            {
                foreach (object i in arg)
                {
                    object val = i;
                    var vType = i.GetType();
                    if (vType == typeof(string))
                    {
                        node.AddString((string)val);
                    }
                    else if (vType == typeof(bool))
                    {
                        node.AddBoolean((bool)val);
                    }
                    else if (vType == typeof(double))
                    {
                        int v = int.Parse(val.ToString());
                        node.AddInteger(v);
                    }
                    else if (vType == typeof(ArrayList))
                    {
                        var t = node.AddArray();
                        var array = val as ArrayList;
                        SetPlist(proj, t, array);
                    }
                    else if (vType == typeof(Hashtable))
                    {
                        var t = node.AddDict();
                        var table = val as Hashtable;
                        SetPlist(proj, t, table);
                    }
                }
            }
        }

        //复制文件
        private static void CopyFiles(PBXProject proj, string xcodePath, Hashtable arg)
        {
            foreach (DictionaryEntry i in arg)
            {
                //string src = Path.Combine(System.Environment.CurrentDirectory, i.Key.ToString());
                string src = Path.Combine(pluginPath, i.Key.ToString());
                string des = Path.Combine(xcodePath, i.Value.ToString());
                CopyFile(proj, xcodePath, src, des);
            }
        }

        //复制文件夹
        private static void CopyFolders(PBXProject proj, string xcodePath, Hashtable arg)
        {
            foreach (DictionaryEntry i in arg)
            {
                //string src = Path.Combine(System.Environment.CurrentDirectory, i.Key.ToString());
                string src = Path.Combine(pluginPath, i.Key.ToString());
                string des = Path.Combine(xcodePath, i.Value.ToString());
                CopyFolder(src, des);
                AddFolderBuild(proj, xcodePath, i.Value.ToString());
            }
        }

        private static void CopyFile(PBXProject proj, string xcodePath, string src, string des)
        {
            bool needCopy = NeedCopy(src);
            if (needCopy)
            {
                File.Copy(src, des);
                string target = proj.TargetGuidByName(PBXProject.GetUnityTargetName());
                proj.AddFileToBuild(target, proj.AddFile(des, des.Replace(xcodePath + "/", ""), PBXSourceTree.Absolute));
                AutoAddSearchPath(proj, xcodePath, des);
                Console.WriteLine("copy file " + src + " -> " + des);
            }
        }

        private static void CopyFolder(string srcPath, string dstPath)
        {
            if (Directory.Exists(dstPath))
                Directory.Delete(dstPath);
            if (File.Exists(dstPath))
                File.Delete(dstPath);

            Directory.CreateDirectory(dstPath);

            foreach (var file in Directory.GetFiles(srcPath))
            {
                if (NeedCopy(Path.GetFileName(file)))
                {
                    File.Copy(file, Path.Combine(dstPath, Path.GetFileName(file)));
                }
            }

            foreach (var dir in Directory.GetDirectories(srcPath))
                CopyFolder(dir, Path.Combine(dstPath, Path.GetFileName(dir)));
        }

        private static void AddFolderBuild(PBXProject proj, string xcodePath, string root)
        {
            //获得源文件下所有目录文件
            string currDir = Path.Combine(xcodePath, root);
            if (root.EndsWith(".framework") || root.EndsWith(".bundle"))
            {
                string target = proj.TargetGuidByName(PBXProject.GetUnityTargetName());
                Console.WriteLine(string.Format("add framework or bundle to build:{0}->{1}", currDir, root));
                proj.AddFileToBuild(target, proj.AddFile(currDir, root, PBXSourceTree.Source));
                return;
            }
            List<string> folders = new List<string>(Directory.GetDirectories(currDir));
            foreach (string folder in folders)
            {
                string name = Path.GetFileName(folder);
                string t_path = Path.Combine(currDir, name);
                string t_projPath = Path.Combine(root, name);
                if (folder.EndsWith(".framework") || folder.EndsWith(".bundle"))
                {
                    string target = proj.TargetGuidByName(PBXProject.GetUnityTargetName());
                    Console.WriteLine(string.Format("add framework or bundle to build:{0}->{1}", t_path, t_projPath));
                    proj.AddFileToBuild(target, proj.AddFile(t_path, t_projPath, PBXSourceTree.Source));
                    AutoAddSearchPath(proj, xcodePath, t_path);
                }
                else
                {
                    AddFolderBuild(proj, xcodePath, t_projPath);
                }
            }
            List<string> files = new List<string>(Directory.GetFiles(currDir));
            foreach (string file in files)
            {
                if (NeedCopy(file))
                {
                    string name = Path.GetFileName(file);
                    string t_path = Path.Combine(currDir, name);
                    string t_projPath = Path.Combine(root, name);
                    string target = proj.TargetGuidByName(PBXProject.GetUnityTargetName());
                    proj.AddFileToBuild(target, proj.AddFile(t_path, t_projPath, PBXSourceTree.Source));
                    AutoAddSearchPath(proj, xcodePath, t_path);
                    Console.WriteLine("add file to build:" + Path.Combine(root, file));
                }
            }
        }

        //在复制文件加入工程时，当文件中有framework、h、a文件时，自动添加相应的搜索路径
        private static void AutoAddSearchPath(PBXProject proj, string xcodePath, string filePath)
        {
            if (filePath.EndsWith(".framework"))
            {//添加框架搜索路径
                string addStr = "$PROJECT_DIR" + Path.GetDirectoryName(filePath.Replace(xcodePath, ""));
                Hashtable arg = new Hashtable();
                Hashtable add = new Hashtable();
                arg.Add("+", add);
                arg.Add("=", new Hashtable());
                arg.Add("-", new Hashtable());
                var array = new ArrayList();
                array.Add(addStr);
                add.Add("FRAMEWORK_SEARCH_PATHS", array);
                SetBuildProperties(proj, arg);
            }
            else if (filePath.EndsWith(".h"))
            {//添加头文件搜索路径
                string addStr = "$PROJECT_DIR" + Path.GetDirectoryName(filePath.Replace(xcodePath, ""));
                Hashtable arg = new Hashtable();
                Hashtable add = new Hashtable();
                arg.Add("+", add);
                arg.Add("=", new Hashtable());
                arg.Add("-", new Hashtable());
                var array = new ArrayList();
                array.Add(addStr);
                add.Add("HEADER_SEARCH_PATHS", array);
                SetBuildProperties(proj, arg);
            }
            else if (filePath.EndsWith(".a"))
            {//添加静态库搜索路径
                string addStr = "$PROJECT_DIR" + Path.GetDirectoryName(filePath.Replace(xcodePath, ""));
                Hashtable arg = new Hashtable();
                Hashtable add = new Hashtable();
                arg.Add("+", add);
                arg.Add("=", new Hashtable());
                arg.Add("-", new Hashtable());
                var array = new ArrayList();
                array.Add(addStr);
                add.Add("LIBRARY_SEARCH_PATHS", array);
                SetBuildProperties(proj, arg);
            }
        }

        private static void SetFilesCompileFlag(PBXProject proj, Hashtable arg)
        {
            string target = proj.TargetGuidByName(PBXProject.GetUnityTargetName());
            foreach (DictionaryEntry i in arg)
            {
                string fileProjPath = i.Key.ToString();
                string fguid = proj.FindFileGuidByProjectPath(fileProjPath);
                if (fguid == null)
                    continue;
                ArrayList des = i.Value as ArrayList;
                List<string> list = new List<string>();
                foreach (var flag in des)
                {
                    list.Add(flag.ToString());
                }
                proj.SetCompileFlagsForFile(target, fguid, list);
            }
        }

        private static bool NeedCopy(string file)
        {
            string fileName = Path.GetFileNameWithoutExtension(file);
            string fileEx = Path.GetExtension(file);
            if (fileName.StartsWith(".") || file.EndsWith(".gitkeep") || file.EndsWith(".DS_Store"))
            {
                return false;
            }
            return true;
        }
    }
}