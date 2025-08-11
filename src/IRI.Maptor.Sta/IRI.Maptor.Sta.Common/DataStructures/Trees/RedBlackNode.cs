using System;
using System.Collections.Generic;
using System.Text;

namespace IRI.Maptor.Sta.DataStructures.Trees;

public class RedBlackNode<T> where T : IComparable
{
    public static RedBlackNode<T> nilNode = new RedBlackNode<T>();

    public NodeColor Color { get; set; }

    public T Key { get; set; }

    private RedBlackNode<T> _leftChild, _rigthChild;

    public RedBlackNode<T> LeftChild
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

    public RedBlackNode<T> RigthChild
    {
        get { return this._rigthChild; }

        set
        {
            if (value != null)
            {
                this._rigthChild = value;

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
