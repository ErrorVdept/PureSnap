using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PureSnap
{
    public class Templater
    {
        public Templater() { }
        public string ProduceScreenTemplate(string template,string path, int width, int height) 
        {
            var currentTemplate = template;
            currentTemplate = currentTemplate.Replace("TEMPLATE_HEIGHT", height.ToString()+"px");
            currentTemplate = currentTemplate.Replace("TEMPLATE_WIDTH", width.ToString() + "px");
            currentTemplate = currentTemplate.Replace("IMAGE_HEIGHT", (height-100).ToString() + "px");
            currentTemplate = currentTemplate.Replace("IMAGE_WIDTH", (width-100).ToString() + "px");
            currentTemplate = currentTemplate.Replace("IMAGE_PATH", path);
            currentTemplate = currentTemplate.Replace("BG_COLOR", "black"); 
            return currentTemplate;
        }
    }
}
