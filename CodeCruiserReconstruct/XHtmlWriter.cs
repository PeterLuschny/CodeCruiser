using System.IO;
using System.Xml;

namespace Luschny.Utils.CodeCruiser
{
    internal sealed class XHtmlWriter
    {
        private IConverter Converter;

        public XHtmlWriter(string type, bool paper, bool line, bool embed)
        {
            this.Converter = ConverterFactory.CreateConverter(type, paper, line, embed);
        }

        private static void CodeTransform(string srcFileName, string dstFileName, IConverter Converter)
        {
            if (!File.Exists(srcFileName))
            {
                throw new FileNotFoundException("FILE DOES NOT EXIST: " + srcFileName);
            }
            FileStream reader = new FileStream(srcFileName, FileMode.Open, FileAccess.Read, FileShare.None);
            FileStream stream = new FileStream(dstFileName, FileMode.Create, FileAccess.Write, FileShare.None);
            StreamWriter writer = new StreamWriter(stream);
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(srcFileName);
            CodeFrame.WriteHeader(writer, fileNameWithoutExtension, Converter.EmbedStyleSheet);
            writer.Write(Converter.FormatCode(reader));
            CodeFrame.WriteFooter(writer);
            writer.Flush();
            stream.Close();
            reader.Close();
        }

        public void Transform(string taskFile, IndexFile index, CruiserForm.CallBack fcb)
        {
            XmlReaderSettings settings = new XmlReaderSettings
            {
                IgnoreComments = true,
                IgnoreWhitespace = true
            };
            using (XmlReader reader = XmlReader.Create(taskFile, settings))
            {
                string dstFileName = null;
                reader.MoveToContent();
                reader.Read();
                while (reader.Read())
                {
                    string srcFileName = reader.ReadElementContentAsString("src", "");
                    dstFileName = reader.ReadElementContentAsString("dst", "");
                    CodeTransform(srcFileName, dstFileName, this.Converter);
                    index.Add(srcFileName, dstFileName);
                    fcb.Call();
                    CodeFrame.WriteCss(Path.GetDirectoryName(dstFileName) + @"\CodeCruiser.css");
                    reader.ReadEndElement();
                }
            }
        }
    }
} 
