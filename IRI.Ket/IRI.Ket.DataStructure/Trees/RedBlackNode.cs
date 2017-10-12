using System;
using System.Collections.Generic;
using System.Text;

namespace IRI.Ket.DataStructure.Trees
{
    public class RedBlackNode<T> where T : IComparable
    {
        public static RedBlackNode<T> nilNode = new RedBlackNode<T>();

        public NodeColor Color { get; set; }

        public T Key { get; set; }

        protected RedBlackNode<T> leftChild, rigthChild;

        public RedBlackNode<T> LeftChild
        {
            get { return this.leftChild; }

            set
            {
                if (value != null)
                {
                    this.leftChild = value;

                    value.Parent = this;
                }
            }
        }

        public RedBlackNode<T> RigthChild
        {
            get { return this.rigthChild; }

            set
            {
                if (value != null)
                {
                    this.rigthChild = value;

                    value.Parent = this;
                }
            }
        }

        public RedBlackNode<T> Parent { get; set; }

        private RedBlackNode()
        {
            this.Key = default(T);

            this.Color = NodeColor.Black;
        }

        public RedBlackNode(T key, NodeColor color)
        {
            this.Color = color;

            this.Key = key;

            this.LeftChild = nilNode;

            this.RigthChild = nilNode;
        }

        public override string ToString()
        {
            return string.Format("Key = '{0}', Color = '{3}', Left = '{1}', Rigth = '{2}'",
                Key.ToString(),
                LeftChild == null ? string.Empty : LeftChild.Key.ToString(),
                RigthChild == null ? string.Empty : RigthChild.Key.ToString(),
                this.Color.ToString());
        }
    }
}
