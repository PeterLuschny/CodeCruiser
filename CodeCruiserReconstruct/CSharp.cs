using System.IO;

namespace Luschny.Utils.CodeCruiser
{
    public class CSharpFormat : CLikeFormat
    {
        protected override string Keywords
        {
            get
            {
                return "abstract as base bool break byte case catch char checked class const continue decimal default delegate do double else enum event explicit extern false finally fixed float for foreach get goto if implicit import in int interface internal is lock long namespace new null object operator out override partial params private protected public readonly ref return sbyte sealed set short sizeof stackalloc static string struct switch this throw true try typeof uint ulong unchecked unsafe ushort using var value virtual void volatile where while yield Xint";
            }
        }

        protected override string Preprocessors
        {
            get
            {
                return "#if #else #elif #endif #define #undef #warning #error #line #region #endregion #pragma";
            }
        }
    }

    public class CSharpConverter : CSharpFormat, IConverter
    {
        public CSharpConverter(bool Alternate, bool LineNumbers, bool EmbedStyleSheet)
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
