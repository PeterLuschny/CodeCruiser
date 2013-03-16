using System.IO;

namespace Luschny.Utils.CodeCruiser
{
    public class GroovyFormat : CLikeFormat
    {
        protected override string Keywords
        {
            get
            {
                return "abstract as assert boolean break byte case catch char class const continue def default do double else enum extends false final finally float for goto if implements import in instanceof int interface long native new null package private protected public return short static strictfp super switch synchronized this threadsafe throw throws transient true try void volatile while";
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

    public class GroovyConverter : GroovyFormat, IConverter
    {
        public GroovyConverter(bool Alternate, bool LineNumbers, bool EmbedStyleSheet)
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
