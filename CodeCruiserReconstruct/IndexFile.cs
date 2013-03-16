using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Xml.Linq;

namespace Luschny.Utils.CodeCruiser
{
    internal class IndexFile
    {
        private XElement body;
        private XDocument index;
        private ArrayList list;
        private XNamespace ns;

        public IndexFile()
        {
            string content = "Index created by CodeCruiser.";
            this.ns = "http://www.w3.org/1999/xhtml";
            XDeclaration declaration = new XDeclaration("1.0", "utf-8", "no");
            XDocumentType type = new XDocumentType("html", "-//W3C//DTD XHTML 1.1//EN", "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd", null);
            XElement element = new XElement((XName)(this.ns + "title"), content);
            XElement element2 = new XElement((XName)(this.ns + "style"), new object[] { new XAttribute("type", "text/css"), "body {font-size: 11pt; font-family: Verdana, 'Trebuchet MS', Sans-Serif, Arial, Helvetica;}" });
            XElement element3 = new XElement((XName)(this.ns + "base"), new XAttribute("target", "code"));
            XElement element4 = new XElement((XName)(this.ns + "head"), new object[] { element, element2, element3 });
            this.body = new XElement((XName)(this.ns + "body"), new XElement((XName)(this.ns + "p"), content));
            XElement element5 = new XElement((XName)(this.ns + "html"), new object[] { element4, this.body });
            this.list = new ArrayList();
            this.index = new XDocument(declaration, new object[] { type, element5 });
        }

        public void Add(string src, string dst)
        {
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(src);
            this.list.Add(this.MakeElemet(dst, fileNameWithoutExtension, src));
        }

        private XElement MakeElemet(string locName, string srcName, string orgName)
        {
            return new XElement((XName)(this.ns + "p"), new object[] { new XElement((XName)(this.ns + "a"), new object[] { new XAttribute("href", new Uri(locName)), srcName }), new XElement((XName)(this.ns + "span"), new object[] { new XAttribute("style", "font-size:x-small"), "   " + orgName }) });
        }

        public string Save(string destDir)
        {
            this.list.Sort(new XComparer());
            foreach (object obj2 in this.list)
            {
                this.body.Add(obj2);
            }
            this.index.Save(destDir);
            return ((XElement)((XElement)this.list[0]).FirstNode).FirstAttribute.Value;
        }

        public class XComparer : IComparer
        {
            private CaseInsensitiveComparer cic = new CaseInsensitiveComparer(CultureInfo.InvariantCulture);

            int IComparer.Compare(object x, object y)
            {
                string a = ((XElement)x).Value;
                string b = ((XElement)y).Value;
                return this.cic.Compare(a, b);
            }
        }
    }
}
