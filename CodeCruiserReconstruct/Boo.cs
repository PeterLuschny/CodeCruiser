using System.IO;

namespace Luschny.Utils.CodeCruiser
{
    public class BooFormat : CLikeFormat
    {
        protected override string Keywords
        {
            get
            {
                return "abstract as array base bool break byte case cast catch char checked class const continue constructor decimal def default delegate do double else elif enum event explicit extern false finally fixed float for foreach get goto if implicit import in int interface internal is lock long namespace new null not object operator out override partial params private protected public readonly ref return sbyte sealed self set short sizeof static string struct switch this throw true try typeof uint ulong unless ushort using value virtual void volatile where while yield Xint";
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

    public class BooConverter : BooFormat, IConverter
    {
        public BooConverter(bool Alternate, bool LineNumbers, bool EmbedStyleSheet)
        {
            base.Alternate = Alternate;
            base.LineNumbers = LineNumbers;
            base.EmbedStyleSheet = EmbedStyleSheet;
        }

        public string FormatCode(FileStream reader)
        {
            return base.FormatCode(reader);
        }
    }
}