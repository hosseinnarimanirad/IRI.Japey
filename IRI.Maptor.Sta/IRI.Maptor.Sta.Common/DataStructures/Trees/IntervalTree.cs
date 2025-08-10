﻿using System;
using System.Collections.Generic;
using System.Text;

namespace IRI.Maptor.Sta.DataStructures.Trees;

public class IntervalTree<T> where T : IComparable
{
    private int count;

    private IntervalTreeNode<T> root;

    public IntervalTreeNode<T> Root
    {
        get { return this.root; }
        set { this.root = value; this.root.Parent = null; }
    }

    public static IntervalTreeNode<T> nilNode = IntervalTreeNode<T>.nilNode;

    public IntervalTree(T parentLowValue, T parentHighValue)
    {
        this.count = 1;

        this.Root = new IntervalTreeNode<T>(parentLowValue, parentHighValue, NodeColor.Black);
    }

    private static void Add(IntervalTreeNode<T> parent, IntervalTreeNode<T> node)
    {
        if (parent.Low.CompareTo(node.Low) >= 0)
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

    public void Insert(T low, T high)
    {
        this.count++;

        IntervalTreeNode<T> node = new IntervalTreeNode<T>(low, high, NodeColor.Red) { Parent = this.Root };

        Add(this.Root, node);

        InsertFixup(node);
    }

    private void InsertFixup(IntervalTreeNode<T> node)
    {
        while (node.Parent.Color == NodeColor.Red)
        {
            if (object.ReferenceEquals(node.Parent, node.Parent.Parent.LeftChild))
            {
                IntervalTreeNode<T> y = node.Parent.Parent.RigthChild;

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
                IntervalTreeNode<T> y = node.Parent.Parent.LeftChild;

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

    public void LeftRotate(IntervalTreeNode<T> node)
    {
        if (node.LeftChild == null || node.RigthChild == null)
        {
            throw new NotImplementedException();
        }

        IntervalTreeNode<T> rigthChild = node.RigthChild;

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

        //
        //rigthChild.Max = this.Max(rigthChild.High, rigthChild.LeftChild.Max, rigthChild.RigthChild.Max);

        //node.Max = this.Max(node.High, node.LeftChild.High, node.RigthChild.High);
    }

    public void RigthRotate(IntervalTreeNode<T> node)
    {
        if (node.LeftChild == null || node.RigthChild == null)
        {
            throw new NotImplementedException();
        }

        IntervalTreeNode<T> leftChild = node.LeftChild;

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

        ////
        //leftChild.Max = this.Max(leftChild.High, leftChild.LeftChild.Max, leftChild.RigthChild.Max);

        //node.Max = this.Max(node.High, node.LeftChild.High, node.RigthChild.High);
    }

    private T Max(T first, T second, T third)
    {
        if (first.CompareTo(second) > 0)
        {
            return first.CompareTo(third) > 0 ? first : second;
        }
        else
        {
            return second.CompareTo(third) > 0 ? second : third;
        }
    }

    public IntervalTreeNode<T> Search(T lowValue, T highValue)
    {
        return Search(this.Root, lowValue, highValue);
    }

    private IntervalTreeNode<T> Search(IntervalTreeNode<T> parentNode, T lowValue, T highValue)
    {
        while (!object.Equals(parentNode, nilNode) && !AreOverlaping(parentNode, lowValue, highValue))
        {
            if (!object.ReferenceEquals(nilNode, parentNode.LeftChild) && parentNode.LeftChild.Max.CompareTo(lowValue) >= 0)
            {
                parentNode = parentNode.LeftChild;
            }
            else
            {
                parentNode = parentNode.RigthChild;
            }
        }

        return object.ReferenceEquals(parentNode, nilNode) ? null : parentNode;
    }

    private bool AreOverlaping(IntervalTreeNode<T> interval, T low, T high)
    {
        return interval.Low.CompareTo(high) < 0 && low.CompareTo(interval.High) < 0;
    }

}
