using System;
using System.Runtime.CompilerServices;

namespace Luschny.Utils.CodeCruiser
{
    internal class CruiserState
    {
        public COD cod { get; set; }

        public string destinationPath { get; set; }

        public DST dst { get; set; }

        public string sourcePath { get; set; }

        public SRC src { get; set; }

        public STY sty { get; set; }

        public enum COD
        {
            java,
            cs,
            vb,
            js,
            groovy,
            asy,
            ps1
        }

        public enum DST
        {
            same,
            pool
        }

        public enum SRC
        {
            file,
            dir,
            tree
        }

        [Flags]
        public enum STY
        {
            embed = 4,
            line = 1,
            paper = 2
        }
    }
} 
