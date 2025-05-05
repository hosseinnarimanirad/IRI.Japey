using System;
using System.Collections.Generic;
using System.Text;

namespace IRI.Sta.DataStructures.Trees;

public class BinarySearchNode<T> where T : IComparable
{
    public T Key { get; set; }

    protected BinarySearchNode<T> leftChild, rightChild, parent;

    public BinarySearchNode<T> LeftChild
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

    public BinarySearchNode<T> RigthChild
    {
        get { return this.rightChild; }

        set
        {
            if (value != null)
            {
                this.rightChild = value;

                value.Parent = this;
            }
        }
    }

    public  BinarySearchNode<T> Parent
    {
        get { return this.parent; }

        set
        {
            this.parent = value;
        }
    }

    public BinarySearchNode(T key)
    {
        this.Key = key;
    }

    public override string ToString()
    {
        return string.Format("Key = '{0}', Left = '{1}', Rigth = '{2}'",
            Key.ToString(),
            LeftChild == null ? string.Empty : LeftChild.Key.ToString(),
            RigthChild == null ? string.Empty : RigthChild.Key.ToString());
    }

}
