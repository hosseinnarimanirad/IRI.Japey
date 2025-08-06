using System;
using System.Collections.Generic;
using System.Text;

namespace IRI.Maptor.Sta.DataStructures.Trees;

public class RedBlackTree<T> where T : IComparable
{
    private int count;

    private RedBlackNode<T> root;

    public RedBlackNode<T> Root
    {
        get { return this.root; }
        set
        {
            this.root = value;

            this.root.Parent = null;
        }
    }

    public static RedBlackNode<T> nilNode = RedBlackNode<T>.nilNode;

    public RedBlackTree(T parentValue)
    {
        this.count = 1;

        this.Root = new RedBlackNode<T>(parentValue, NodeColor.Black);

    }

    public RedBlackTree(T[] values)
    {
        if (values == null)
            return;

        this.count = values.Length;

        this.Root = new RedBlackNode<T>(values[0], NodeColor.Black);
        //this.Root.LeftChild = nilNode;
        //this.Root.RigthChild = nilNode;

        for (int i = 1; i < values.Length; i++)
        {
            RedBlackNode<T> node = new RedBlackNode<T>(values[i], NodeColor.Red) { Parent = this.Root };

            //node.LeftChild = nilNode;

            //node.RigthChild = nilNode;

            Add(this.Root, node);

            InsertFixup(node);
        }
    }

    private static void Add(RedBlackNode<T> parent, RedBlackNode<T> node)
    {
        if (parent.Key.CompareTo(node.Key) >= 0)
        {
            if (parent.LeftChild != nilNode)
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
            if (parent.RigthChild != nilNode)
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

        RedBlackNode<T> node = new RedBlackNode<T>(value, NodeColor.Red) { Parent = this.Root };

        Add(this.Root, node);

        InsertFixup(node);
    }

    private void InsertFixup(RedBlackNode<T> node)
    {
        while (node.Parent.Color == NodeColor.Red)
        {
            if (object.ReferenceEquals(node.Parent, node.Parent.Parent.LeftChild))
            {
                RedBlackNode<T> y = node.Parent.Parent.RigthChild;

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
                RedBlackNode<T> y = node.Parent.Parent.LeftChild;

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

    public void LeftRotate(RedBlackNode<T> node)
    {
        if (node.LeftChild == null || node.RigthChild == null)
        {
            throw new NotImplementedException();
        }

        RedBlackNode<T> rigthChild = node.RigthChild;

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
    }

    public void RigthRotate(RedBlackNode<T> node)
    {
        if (node.LeftChild == null || node.RigthChild == null)
        {
            throw new NotImplementedException();
        }

        RedBlackNode<T> leftChild = node.LeftChild;

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
    }
}
