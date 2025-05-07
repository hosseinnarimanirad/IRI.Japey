using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRI.Sta.Spatial.AdvancedStructures;

public class KdTreeNode<T>
{
    public T Point { get; set; }

    //public Func<T, T, int> comparer;
    
    protected KdTreeNode<T> leftChild, rightChild, parent;

    public KdTreeNode<T> LeftChild
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

    public KdTreeNode<T> RigthChild
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

    public KdTreeNode<T> Parent
    {
        get { return this.parent; }

        set
        {
            this.parent = value;
        }
    }

    public KdTreeNode(T point)
    {
        this.Point = point;

        //this.comparer = comparer;
    }

    public override string ToString()
    {
        return string.Format("Key = '{0}', Left = '{1}', Rigth = '{2}'",
            Point.ToString(),
            LeftChild == null ? string.Empty : LeftChild.Point.ToString(),
            RigthChild == null ? string.Empty : RigthChild.Point.ToString());
    }
}
