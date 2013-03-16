using System.IO;

namespace Luschny.Utils.CodeCruiser
{
    public class JavaFormat : CLikeFormat
    {
        protected override string Keywords
        {
            get
            {
                return "abstract assert boolean byte break case catch char class continue default do double else enum extends false final finally float for if implements import int instanceof interface long native new null object package private protected public return static strictfp short super switch synchronized this throw throws transient true try virtual void volatile while Xint";
            }
        }

        protected override string Preprocessors
        {
            get
            {
                return "@interface @author @beaninfo @docRoot @deprecated @exception @link @param @return @see @serial @serialData @serialField @since @throws @version @linkplain @inheritDoc @value @pre @post @inv";
            }
        }
    }

    public class JavaConverter : JavaFormat, IConverter
    {
        public JavaConverter(bool Alternate, bool LineNumbers, bool EmbedStyleSheet)
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
