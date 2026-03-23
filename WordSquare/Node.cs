using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordSquare
{
    internal class Node
    {
        public bool IsPartial { get; private set; }
        public bool IsValidWord { get; private set; }
        public readonly string Word;
        public Node? Parent;

        private Dictionary<char, Node> _nodes;

        public Node()
        {
            Word = "";
            _nodes = new();
            Parent = null;
        }
        public Node(string word, Node parent)
        {
            Word = word;
            _nodes = new();
            Parent = parent;
        }

        public Node Add(char c)
        {
            if (!_nodes.ContainsKey(c)) _nodes.Add(c, new(Word + c, this));
            return _nodes[c];
        }

        public Node? Advance(char c)
        {
            if (_nodes.ContainsKey(c)) return _nodes[c];
            else return null;
        }

        public void FinaliseWord()
        {
            IsValidWord = true;
        }
        public void Finalise()
        {
            if (_nodes.Count != 0)
            {
                IsPartial = true;
                foreach (var node in _nodes.Values) node.Finalise();
            }
        }
    }
}
