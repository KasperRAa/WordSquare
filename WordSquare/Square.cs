using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace WordSquare
{
    internal class Square
    {
        public int Width { get; }
        public int Height { get; }

        public string Words { get; private set; }


        private Graft[] _columns;
        private Graft[] _rows;

        public Square(int width, int height, WordTree tree)
        {
            _columns = new Graft[width];
            for (int i = 0; i < width; i++) _columns[i] = tree.Prune();
            Width = width;

            _rows = new Graft[height];
            for (int i = 0; i < height; i++) _rows[i] = tree.Prune();
            Height = height;

            Words = "";
        }
        public Square(int size, WordTree tree) : this(size, size, tree) { }


        public bool Advance(char c)
        {
            if (!IsValid()) return false;

            int len = Words.Length;
            if (len >= Width * Height) return false;

            if (!_rows[len / Width].Advance(c)) return false;
            if (!_columns[len % Width].Advance(c))
            {
                _rows[len / Width].Back();
                return false;
            }

            Words += c;

            if (!IsValid())
            {
                Back();
                return false;
            }

            return true;
        }

        public void Back()
        {
            int len = Words.Length;
            if (len != 0)
            {
                _rows[(len - 1) / Width].Back();
                _columns[(len - 1) % Width].Back();
                Words = Words.Substring(0, len - 1);
            }
        }

        public bool ColumnDeveloping(int i)
        {
            int len = Words.Length;
            if ((Height - 1) * Width + i < len) return false;
            return _columns[i].IsPartial;
        }
        public bool ColumnComplete(int i)
        {
            return _columns[i].IsValidWord;
        }
        public bool RowDeveloping(int i)
        {
            int len = Words.Length;
            if (Width * (i + 1) + i <= len) return false;
            return _rows[i].IsPartial;
        }
        public bool RowComplete(int i)
        {
            return _rows[i].IsValidWord;
        }

        public bool IsComplete()
        {
            for (int x = 0; x < Width; x++) if (!ColumnComplete(x)) return false;
            for (int y = 0; y < Width; y++) if (!RowComplete(y)) return false;
            return true;
        }
        public bool IsValid()
        {
            for (int x = 0; x < Width; x++) if (!ColumnComplete(x) && !ColumnDeveloping(x)) return false;
            for (int y = 0; y < Width; y++) if (!RowComplete(y) && !RowDeveloping(y)) return false;
            return true;
        }
    }
}
