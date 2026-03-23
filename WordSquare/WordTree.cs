using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordSquare
{
    internal class WordTree
    {
        private Node _root;

        public WordTree(string[] dict)
        {
            _root = new();
            foreach (var word in dict)
            {
                var node = _root;
                foreach (var c in word)
                {
                    node = node.Add(c);
                }
                node.FinaliseWord();
            }
            _root.Finalise();
        }

        public Graft Prune()
        {
            return new(_root);
        }
    }
}
