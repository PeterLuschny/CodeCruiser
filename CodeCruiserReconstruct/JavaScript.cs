using System.IO;

namespace Luschny.Utils.CodeCruiser
{
    public class JavaScriptFormat : CLikeFormat
    {
        protected override string Keywords
        {
            get
            {
                return "var function abstract as base bool break byte case catch char checked class const continue decimal default delegate do double else enum event explicit extern false finally fixed float for foreach goto if implicit in int interface internal is lock long namespace new null object operator out override params private protected public readonly ref return sbyte sealed short sizeof stackalloc static string struct switch this throw true try typeof uint ulong unchecked unsafe ushort using virtual void while";
            }
        }

        protected override string Preprocessors
        {
            get
            {
                return @"@\w*";
            }
        }
    }

    public class JavaScriptConverter : JavaScriptFormat, IConverter
    {
        public JavaScriptConverter(bool Alternate, bool LineNumbers, bool EmbedStyleSheet)
        {
            base.Alternate = Alternate;
            base.LineNumbers = LineNumbers;
            base.EmbedStyleSheet = EmbedStyleSheet;
        }

        public string FormatCode(FileStream reader)
        {
            return base.FormatCode((Stream)reader);
        }
    }
} 
