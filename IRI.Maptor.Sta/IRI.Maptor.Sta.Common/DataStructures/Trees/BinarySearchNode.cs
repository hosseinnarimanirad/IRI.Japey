using System;
using System.Collections.Generic;
using System.Text;

namespace IRI.Maptor.Sta.DataStructures.Trees;

public class BinarySearchNode<T> where T : IComparable
{
    public T Key { get; set; }

    private BinarySearchNode<T> _leftChild, _rightChild, _parent;

    public BinarySearchNode<T> LeftChild
    {
        get { return this._leftChild; }

        set
        {
            if (value != null)
            {
                this._leftChild = value;

                value.Parent = this;
            }
        }
    }

    public BinarySearchNode<T> RigthChild
    {
        get { return this._rightChild; }

        set
        {
            if (value != null)
            {
                this._rightChild = value;

                value.Parent = this;
            }
        }
    }

    public  BinarySearchNode<T> Parent
    {
        get { return this._parent; }

        set
        {
            this._parent = value;
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
