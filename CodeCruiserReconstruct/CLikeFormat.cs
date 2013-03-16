namespace Luschny.Utils.CodeCruiser
{
    public abstract class CLikeFormat : CodeFormat
    {
        protected CLikeFormat()
        {
        }

        protected override string CommentRegEx
        {
            get
            {
                return @"/\*.*?\*/|//.*?(?=\r|\n)";
            }
        }

        protected override string StringRegEx
        {
            get
            {
                return "@?\"\"|@?\".*?(?!\\\\).\"|''|'.*?(?!\\\\).'";
            }
        }
    }
} 
