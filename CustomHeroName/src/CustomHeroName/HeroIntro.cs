using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace CustomHeroName
{
    [Serializable]
    public class HeroIntro
    {
        [JsonIgnore, XmlIgnore]
        public HeroType type;
        public string name;
        public string subtitle1;
        public string subtitle2;

        public HeroIntro(HeroType type, string name = "",  string subtitle1 = "", string subtitle2 = "")
        {
            this.type = type;
            this.name = name;
            this.subtitle1 = subtitle1;
            this.subtitle2 = subtitle2;
        }

        public string GetName()
        {
            if (string.IsNullOrEmpty(name))
                return null;
            return name;
        }

        public string GetSubtitle1()
        {
            if (string.IsNullOrEmpty(subtitle1))
                return null;
            return subtitle1;
        }

        public string GetSubtitle2()
        {
            if (string.IsNullOrEmpty(subtitle2))
                return null;
            return subtitle2;
        }
    }
}
