using System.IO;

namespace Luschny.Utils.CodeCruiser
{
    public class VisualBasicFormat : CodeFormat
    {
        public override bool CaseSensitive
        {
            get
            {
                return false;
            }
        }

        protected override string CommentRegEx
        {
            get
            {
                return @"(?:'|REM\s).*?(?=\r|\n)";
            }
        }

        protected override string Keywords
        {
            get
            {
                return "AddHandler AddressOf AndAlso Alias And Ansi As Assembly Auto Boolean ByRef Byte ByVal Call Case Catch CBool CByte CChar CDate CDec CDbl Char CInt Class CLng CObj Const CShort CSng CStr CType Date Decimal Declare Default Delegate Dim DirectCast Do Double Each Else ElseIf End EndIf Enum Erase Error Event Exit False Finally For Friend Function Get GetType Global GoSub GoTo Handles If Implements Imports In Inherits Integer Interface Is Let Lib Like Long Loop Me Mod Module MustInherit MustOverride MyBase MyClass Namespace Narrowing New Next Not Nothing NotInheritable NotOverridable Object On Operator Option Optional Or OrElse Overloads Overridable Overrides Partial ParamArray Preserve Private Property Protected Public RaiseEvent ReadOnly ReDim REM RemoveHandler Resume Return Select Set Shadows Shared Short Single Static Step Stop String Structure Sub SyncLock Then Throw To True Try TryCast TypeOf Unicode Until Using Variant Wend When While With Widening WithEvents WriteOnly Xor";
            }
        }

        protected override string Preprocessors
        {
            get
            {
                return @"#\s*Const #\s*If #\s*Else #\s*ElseIf #\s*End\s+If #\s*ExternalSource #\s*End\s+ExternalSource #\s*Region #\s*End\s+Region";
            }
        }

        protected override string StringRegEx
        {
            get
            {
                return "\"\"|\".*?\"";
            }
        }
    }

    public class VisualBasicConverter : VisualBasicFormat, IConverter
    {
        public VisualBasicConverter(bool Alternate, bool LineNumbers, bool EmbedStyleSheet)
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
