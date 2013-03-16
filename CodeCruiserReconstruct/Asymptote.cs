using System.IO;

namespace Luschny.Utils.CodeCruiser
{
    public class AsymptoteFormat : CLikeFormat
    {
        protected override string Keywords
        {
            get
            {
                return "abstract access and arrowhead as atleast base bbox3 binarytree binarytreeNode block bool bounds break byte case catch char checked class const continue controls coord cputime curl cycle decimal default delegate do double else enum event explicit extern false file finally fixed float for foreach frame from get goto grid3 guide horner hsv if implicit import in include indexedTransform int interface internal key light line linefit lock long marginT marker namespace new newframe node null object operator out override pair params partial path path3 pen picture position postscript private projection protected public quote readonly real ref restricted return revolution sbyte scaleT scientific sealed segment set short side sizeof slice splitface stackalloc static string struct surface switch tension tensionSpecifier this throw ticklocate ticksgridT tickvalues transform tree triple true try typedef typeof uint ulong unchecked unravel ushort using value var vertex virtual void volatile where while ";
            }
        }

        protected override string Preprocessors
        {
            get
            {
                return "Braid Legend TreeNode AND Arc ArcArrow ArcArrows Arrow Arrows Automatic AvantGarde BBox BWRainbow BWRainbow2 Bar Bars BeginArcArrow BeginArrow BeginBar BeginDotMargin BeginMargin BeginPenMargin Blank Bookman Bottom BottomTop Break Broken BrokenLog Ceil Circle CircleBarIntervalMarker Cos Courier CrossIntervalMarker DefaultFormat DefaultLogFormat Degrees DotMargin DotMargins Dotted Draw EndArcArrow EndArrow EndBar EndDotMargin EndMargin EndPenMargin Fill FillDraw Floor Format Full Gaussian Gaussrand Gaussrandpair Grayscale Helvetica Hermite HookHead Label Landscape Left LeftRight LeftTicks Linear Log LogFormat Margin Margins Mark MidArcArrow MidArrow NOT NewCenturySchoolBook NoBox NoFill NoMargin NoTicks NoZero NoZeroFormat None OR OmitFormat OmitTick Palatino PaletteTicks Pen PenMargin PenMargins Portrait RadialShade Rainbow Range Relative Right RightTicks Rotate Round Scale ScaleX ScaleY Seascape Shift Sin Slant Spline StickIntervalMarker Straight Symbol Tan TeXify Ticks TildeIntervalMarker TimesRoman Top TrueMargin UnFill UpsideDown VERSION XEquals XOR XYgrid XZero XZgrid Y YEquals YXgrid YZero YZgrid ZXgrid ZYgrid ZapfChancery ZapfDingbats ";
            }
        }
    }

    public class AsymptoteConverter : AsymptoteFormat, IConverter
    {
        public AsymptoteConverter(bool Alternate, bool LineNumbers, bool EmbedStyleSheet)
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
