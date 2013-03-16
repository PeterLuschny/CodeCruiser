using System.IO;

namespace Luschny.Utils.CodeCruiser
{
    public interface IConverter
    {
        string FormatCode(FileStream reader);
        string FormatCode(string source);
        string FormatSubCode(string source);

        bool Alternate { get; set; }
        bool EmbedStyleSheet { get; set; }
        bool LineNumbers { get; set; }
        byte TabSpaces { get; set; }
    }
}
