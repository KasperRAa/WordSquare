using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordSquare
{
    internal class Node
    {
        public bool IsPartial 
        {
            get
            {
                return _remainders.Count > 1 || _remainders.Count == 1 && !_remainders.Contains(0);
            }
        }
        public bool IsValidWord
        {
            get
            {
                return _remainders.Contains(0);
            }
        }
        public readonly string Word;
        public Node? Parent;

        private Dictionary<char, Node> _nodes;
        private HashSet<int> _remainders;

        public Node() : this ("", null)
        { }
        public Node(string word, Node? parent)
        {
            Word = word;
            _nodes = new();
            Parent = parent;
            _remainders = new();
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
            _remainders.Add(0);
        }
        public void Finalise()
        {
            if (_nodes.Count != 0)
            {
                foreach (var node in _nodes.Values)
                {
                    node.Finalise();
                    foreach (var n in node._remainders) _remainders.Add(n + 1);
                }
            }
        }
        public bool HasRemainder(int n) => _remainders.Contains(n);
    }
}
