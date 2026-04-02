using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordSquare
{
    internal class Graft
    {
        public string Word
        {
            get
            {
                return _node.Word;
            }
        }
        public bool IsValidWord
        {
            get
            {
                return _node.IsValidWord;
            }
        }
        public bool IsPartial
        {
            get
            {
                return _node.IsPartial;
            }
        }

        private Node _node;

        public Graft(Node node)
        {
            _node = node;
            //node = node.Advance('a');
            //node = node.Advance('a');
            //for (int i = 0; i < 5; i++) Console.WriteLine($"{i}|{node.HasRemainder(i)}");
            //Console.ReadKey();
        }

        public bool Advance(char c)
        {
            Node? node = _node.Advance(c);
            if (node != null)
            {
                _node = node;
                return true;
            }
            return false;
        }
        public void Back()
        {
            if (_node.Parent != null) _node = _node.Parent;
        }
        public bool HasRemainder(int n) => _node.HasRemainder(n);
    }
}
