using System;
using System.Collections.Generic;
using System.Text;

namespace IRI.Maptor.Sta.DataStructures.Trees;

internal static class TreeOperation
{
    public static BinarySearchNode<T> GetMinimum<T>(BinarySearchNode<T> node) where T : IComparable
    {
        BinarySearchNode<T> result = node;

        while (result.LeftChild != null)
        {
            result = result.LeftChild;
        }

        return result;
    }

    public static BinarySearchNode<T> GetMaximum<T>(BinarySearchNode<T> node) where T : IComparable
    {
        BinarySearchNode<T> result = node;

        while (result.RigthChild != null)
        {
            result = result.RigthChild;
        }

        return result;
    }

    public static BinarySearchNode<T> GetSuccessor<T>(BinarySearchNode<T> node) where T : IComparable
    {
        if (node.RigthChild != null)
        {
            return TreeOperation.GetMinimum(node.RigthChild);
        }

        BinarySearchNode<T> result = node.Parent;

        while (result != null && result.RigthChild == node)
        {
            node = result;

            result = node.Parent;
        }

        return result;
    }

    public static BinarySearchNode<T> GetPredecessor<T>(BinarySearchNode<T> node) where T : IComparable
    {
        if (node.LeftChild != null)
        {
            return TreeOperation.GetMaximum(node.LeftChild);
        }

        BinarySearchNode<T> result = node.Parent;

        while (result != null && result.LeftChild == node)
        {
            node = result;

            result = node.Parent;
        }

        return result;
    }

    public static BinarySearchNode<T> LookFor<T>(BinarySearchNode<T> rootNode, T value) where T : IComparable
    {
        if (rootNode.Key.Equals(value))
        {
            return rootNode;
        }
        else if (rootNode.Key.CompareTo(value) > 0 && rootNode.LeftChild != null)
        {
            return LookFor(rootNode.LeftChild, value);
        }
        else if (rootNode.Key.CompareTo(value) < 0 && rootNode.RigthChild != null)
        {
            return LookFor(rootNode.RigthChild, value);
        }

        return null;
    }

    public static BinarySearchNode<T> LookFor<T>(BinarySearchNode<T> rootNode, BinarySearchNode<T> node) where T : IComparable
    {
        //Check the difference with rootNode.Equal(node)
        if (object.ReferenceEquals(rootNode, node))
        {
            return rootNode;
        }
        else if (rootNode.Key.CompareTo(node.Key) > 0 && rootNode.LeftChild != null)
        {
            return LookFor(rootNode.LeftChild, node);
        }
        else if (rootNode.Key.CompareTo(node.Key) < 0 && rootNode.RigthChild != null)
        {
            return LookFor(rootNode.RigthChild, node);
        }

        return null;
    }

}
