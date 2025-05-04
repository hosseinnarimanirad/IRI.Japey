using System;
using System.Collections.Generic;
using System.Text;

namespace IRI.Msh.DataStructure.Trees
{
    public class OrderStatisticTree<T> where T : IComparable
    {
        private int count;

        private OrderStatisticNode<T> root;

        public OrderStatisticNode<T> Root
        {
            get { return this.root; }
            set { this.root = value; this.root.Parent = null; }
        }

        public OrderStatisticTree()
        {

        }

        public OrderStatisticNode<T> Select(OrderStatisticNode<T> subtreeRoot, int i)
        {
            int r = subtreeRoot.LeftChild.Size + 1;

            if (r == i)
            {
                return subtreeRoot;
            }
            else if (i < r)
            {
                return Select(subtreeRoot.LeftChild, i);
            }
            else
            {
                return Select(subtreeRoot.RigthChild, i - r);
            }
        }

        public int Rank(OrderStatisticNode<T> node)
        {
            int r = node.LeftChild.Size + 1;

            OrderStatisticNode<T> temp = node;

            while (!object.ReferenceEquals(this.Root, temp))
            {
                if (object.ReferenceEquals(temp, temp.Parent.RigthChild))
                {
                    r += temp.Parent.LeftChild.Size + 1;
                }

                temp = temp.Parent;
            }

            return r;
        }


        public OrderStatisticTree(T[] values)
        {
            if (values == null)
                return;

            this.count = values.Length;

            this.Root = new OrderStatisticNode<T>(values[0], NodeColor.Black);
            //this.Parent.LeftChild = nilNode;
            //this.Parent.RigthChild = nilNode;

            for (int i = 1; i < values.Length; i++)
            {
                OrderStatisticNode<T> node = new OrderStatisticNode<T>(values[i], NodeColor.Red) { Parent = this.Root };

                //node.LeftChild = nilNode;

                //node.RigthChild = nilNode;

                Add(this.Root, node);

                InsertFixup(node);
            }
        }

        private static void Add(OrderStatisticNode<T> parent, OrderStatisticNode<T> node)
        {
            parent.Size++;

            if (parent.Key.CompareTo(node.Key) >= 0)
            {
                if (parent.LeftChild != OrderStatisticNode<T>.nilNode)
                {
                    Add(parent.LeftChild, node);
                }
                else
                {
                    parent.LeftChild = node;
                }
            }
            else
            {
                if (parent.RigthChild != OrderStatisticNode<T>.nilNode)
                {
                    Add(parent.RigthChild, node);
                }
                else
                {
                    parent.RigthChild = node;
                }
            }
        }

        public void Insert(T value)
        {
            this.count++;

            OrderStatisticNode<T> node = new OrderStatisticNode<T>(value, NodeColor.Red) { Parent = this.Root };

            //node.LeftChild = nilNode;

            //node.RigthChild = nilNode;

            Add(this.Root, node);

            InsertFixup(node);
        }

        private void InsertFixup(OrderStatisticNode<T> node)
        {
            while (node.Parent.Color == NodeColor.Red)
            {
                if (object.ReferenceEquals(node.Parent, node.Parent.Parent.LeftChild))
                {
                    OrderStatisticNode<T> y = node.Parent.Parent.RigthChild;

                    if (y.Color == NodeColor.Red)
                    {
                        node.Parent.Color = NodeColor.Black;

                        y.Color = NodeColor.Black;

                        node.Parent.Parent.Color = NodeColor.Red;

                        node = node.Parent.Parent;
                    }
                    else if (object.ReferenceEquals(node, node.Parent.RigthChild))
                    {
                        node = node.Parent;

                        LeftRotate(node);
                    }
                    else
                    {
                        node.Parent.Color = NodeColor.Black;

                        node.Parent.Parent.Color = NodeColor.Red;

                        RigthRotate(node.Parent.Parent);
                    }
                }
                else if (object.ReferenceEquals(node.Parent, node.Parent.Parent.RigthChild))
                {
                    OrderStatisticNode<T> y = node.Parent.Parent.LeftChild;

                    if (y.Color == NodeColor.Red)
                    {
                        node.Parent.Color = NodeColor.Black;

                        y.Color = NodeColor.Black;

                        node.Parent.Parent.Color = NodeColor.Red;

                        node = node.Parent.Parent;
                    }
                    else if (object.ReferenceEquals(node, node.Parent.LeftChild))
                    {
                        node = node.Parent;

                        RigthRotate(node);
                    }
                    else
                    {
                        node.Parent.Color = NodeColor.Black;

                        node.Parent.Parent.Color = NodeColor.Red;

                        LeftRotate(node.Parent.Parent);
                    }
                }
                if (node.Parent == null)
                {
                    break;
                }
            }

            this.Root.Color = NodeColor.Black;
        }

        public void LeftRotate(OrderStatisticNode<T> node)
        {
            if (node.LeftChild == null || node.RigthChild == null)
            {
                throw new NotImplementedException();
            }

            OrderStatisticNode<T> rigthChild = node.RigthChild;

            node.RigthChild = rigthChild.LeftChild;

            if (node.Parent == null)
            {
                this.Root = rigthChild;
            }
            else if (object.ReferenceEquals(node, node.Parent.LeftChild))
            {
                node.Parent.LeftChild = rigthChild;
            }
            else
            {
                node.Parent.RigthChild = rigthChild;
            }

            rigthChild.LeftChild = node;

            rigthChild.Size = node.Size;

            node.Size = node.LeftChild.Size + node.RigthChild.Size + 1;
        }

        public void RigthRotate(OrderStatisticNode<T> node)
        {
            if (node.LeftChild == null || node.RigthChild == null)
            {
                throw new NotImplementedException();
            }

            OrderStatisticNode<T> leftChild = node.LeftChild;

            node.LeftChild = leftChild.RigthChild;

            if (node.Parent == null)
            {
                this.Root = leftChild;
            }
            else if (object.ReferenceEquals(node, node.Parent.LeftChild))
            {
                node.Parent.LeftChild = leftChild;
            }
            else
            {
                node.Parent.RigthChild = leftChild;
            }

            leftChild.RigthChild = node;

            leftChild.Size = node.Size;

            node.Size = node.LeftChild.Size + node.RigthChild.Size + 1;
        }
    }
}
