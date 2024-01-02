using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PureSnap.Service
{
    public static class DirExplorer
    {
        public static void InitApp()
        {

            var DocumentPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (Directory.Exists(DocumentPath))
            {
                if (!Directory.Exists(DocumentPath+"/PureSnap") || !Directory.Exists(DocumentPath + "/PureSnap/Templates/") || !Directory.Exists(DocumentPath + "/PureSnap/Templates/Screens"))
                {
                    Directory.CreateDirectory(DocumentPath + "/PureSnap" + "/Templates/Screens");

                    File.Copy("Templates/BlankPage.html", DocumentPath + "/PureSnap" + "/Templates/BlankPage.html");
                    File.Copy("Templates/BlankPage.html", DocumentPath + "/PureSnap" + "/Templates/CurrentPage.html");
                    var files = Directory.GetFiles("Templates/Screens/");
                    foreach (var item in files)
                    {
                        
                        Debug.WriteLine(Path.GetFullPath(item));
                        //File.Copy(item, DocumentPath + "/PureSnap" + "/Templates/Screens/" + Path.GetFileName());
                    }
                }
                
            }


        }
    }
}
