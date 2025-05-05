using System;
using System.Collections.Generic;
using System.Text;

namespace IRI.Sta.DataStructures.Trees;

public class OrderStatisticNode<T> where T : IComparable
{
    int size;

    public int Size
    {
        get { return size; }

        internal set { size = value; }
    }

    public static OrderStatisticNode<T> nilNode = new OrderStatisticNode<T>();

    public NodeColor Color { get; set; }

    public T Key { get; set; }

    protected OrderStatisticNode<T> leftChild, rigthChild;

    public OrderStatisticNode<T> LeftChild
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

    public OrderStatisticNode<T> RigthChild
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

    public OrderStatisticNode<T> Parent { get; set; }

    private OrderStatisticNode()
    {
        this.Key = default(T);

        this.Color = NodeColor.Black;
    }

    public OrderStatisticNode(T key, NodeColor color)
    {
        this.Color = color;

        this.Key = key;

        this.Size = 1;

        this.LeftChild = nilNode;

        this.RigthChild = nilNode;
    }

    public override string ToString()
    {
        return string.Format("Key = '{0}', Color = '{3}', Left = '{1}', Rigth = '{2}', Size = '{4}'",
            Key.ToString(),
            LeftChild == null ? string.Empty : LeftChild.Key.ToString(),
            RigthChild == null ? string.Empty : RigthChild.Key.ToString(),
            this.Color.ToString(),
            Size);
    }
}
