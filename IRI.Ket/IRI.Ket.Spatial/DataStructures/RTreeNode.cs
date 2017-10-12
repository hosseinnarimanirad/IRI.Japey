using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRI.Ket.Spatial.DataStructures
{
    public class RTreeNode
    {
        private Rectangle boundary;

        public int NumberOfKeys
        {
            get
            {
                if (IsLeaf)
                {
                    return this.keys.Count;
                }
                else
                {
                    return this.pointers.Count;
                }
            }
        }

        private bool isLeaf;

        public bool IsLeaf
        {
            get { return this.isLeaf; }

            set
            {
                this.isLeaf = value;

                if (isLeaf && this.pointers.Count > 0)
                {
                    throw new NotImplementedException();
                }
                else if (!IsLeaf && this.keys.Count > 0)
                {
                    throw new NotImplementedException();
                }
            }
        }

        public Rectangle Boundary
        {
            get { return this.boundary; }
        }

        private List<Rectangle> keys;

        private List<RTreeNode> pointers;

        public RTreeNode()
        {
            this.boundary = Rectangle.nilValue;

            this.keys = new List<Rectangle>();

            this.pointers = new List<RTreeNode>();

            this.IsLeaf = true;
        }

        public void FixBoundary()
        {
            Rectangle result;

            if (IsLeaf)
            {
                if (this.keys.Count < 1)
                    return;

                result = this.keys[0];

                foreach (Rectangle item in this.keys)
                {
                    result = result.Add(item);
                }
            }
            else
            {
                if (this.pointers.Count < 1)
                    return;

                result = this.pointers[0].Boundary;

                foreach (RTreeNode item in this.pointers)
                {
                    result = result.Add(item.Boundary);
                }
            }

            this.boundary = result;
        }

        public void FindTheMostDistantRectangles(out int first, out int second)
        {
            if (IsLeaf)
            {
                if (keys.Count < 1)
                {
                    throw new NotImplementedException();
                }

                first = 0; second = 1; double tempValue = -1;

                for (int i = 0; i < keys.Count; i++)
                {
                    for (int j = i + 1; j < keys.Count; j++)
                    {
                        double distance = keys[i].CalculateCenterDistance(keys[i]);

                        if (tempValue < distance)
                        {
                            tempValue = distance;

                            first = i; second = j;
                        }
                    }
                }

            }
            else
            {
                if (pointers.Count < 1)
                {
                    throw new NotImplementedException();
                }

                first = 0; second = 1; double tempValue = -1;

                for (int i = 0; i < pointers.Count; i++)
                {
                    for (int j = i + 1; j < pointers.Count; j++)
                    {
                        double distance = pointers[i].Boundary.CalculateCenterDistance(pointers[i].Boundary);

                        if (tempValue < distance)
                        {
                            tempValue = distance;

                            first = i; second = j;
                        }
                    }
                }
            }
        }


        public void AddKey(Rectangle rectangle)
        {
            if (!IsLeaf)
            {
                throw new NotImplementedException();
            }

            this.keys.Add(rectangle);

            FixBoundary();
        }

        public void RemoveKey(Rectangle rectangle)
        {
            if (!IsLeaf)
            {
                throw new NotImplementedException();
            }

            this.keys.Remove(rectangle);

            FixBoundary();
        }

        public void RemoveKeyAt(int index)
        {
            if (!IsLeaf)
            {
                throw new NotImplementedException();
            }

            this.keys.RemoveAt(index);

            FixBoundary();
        }

        public void InsertKey(int index, Rectangle rectangle)
        {
            if (!IsLeaf)
            {
                throw new NotImplementedException();
            }

            this.keys.Insert(index, rectangle);

            FixBoundary();
        }


        public void AddPointer(RTreeNode node)
        {
            if (IsLeaf)
            {
                throw new NotImplementedException();
            }

            this.pointers.Add(node);

            FixBoundary();
        }

        public void RemovePointer(RTreeNode node)
        {
            if (IsLeaf)
            {
                throw new NotImplementedException();
            }

            this.pointers.Remove(node);

            FixBoundary();
        }

        public void RemovePointerAt(int index)
        {
            if (IsLeaf)
            {
                throw new NotImplementedException();
            }

            this.pointers.RemoveAt(index);

            FixBoundary();
        }

        public void InsertPointer(int index, RTreeNode node)
        {
            if (IsLeaf)
            {
                throw new NotImplementedException();
            }

            this.pointers.Insert(index, node);

            FixBoundary();
        }


        public Rectangle GetKey(int index)
        {
            return this.keys[index];
        }

        public RTreeNode GetPointer(int index)
        {
            return this.pointers[index];
        }

        public IEnumerable<Rectangle> SkipKeys(int count)
        {
            return this.keys.Skip(count);
        }

        public void AddRangeKeys(IEnumerable<Rectangle> collection)
        {
            if (!IsLeaf)
            {
                throw new NotImplementedException();
            }

            this.keys.AddRange(collection);

            FixBoundary();
        }

        public void RemoveRangeKeys(int index, int count)
        {
            if (!IsLeaf)
            {
                throw new NotImplementedException();
            }

            this.keys.RemoveRange(index, count);

            FixBoundary();
        }

        public IEnumerable<RTreeNode> SkipPointers(int count)
        {
            return this.pointers.Skip(count);
        }

        public void AddRangePointers(IEnumerable<RTreeNode> collection)
        {
            if (IsLeaf)
            {
                throw new NotImplementedException();
            }

            this.pointers.AddRange(collection);

            FixBoundary();
        }

        public void RemoveRangePointers(int index, int count)
        {
            if (IsLeaf)
            {
                throw new NotImplementedException();
            }

            this.pointers.RemoveRange(index, count);

            FixBoundary();
        }

        public override string ToString()
        {
            string result = string.Empty;

            foreach (Rectangle item in keys)
            {
                result += string.Format("({0}), ", item.ToString());
            }

            if (result.Length > 0)
            {
                result.Remove(result.Length - 2, 2);
            }

            return result;
        }

        public IEnumerable<Rectangle> GetSubRectangles()
        {
            if (isLeaf)
            {
                foreach (Rectangle item in this.keys)
                {
                    yield return item;
                }
            }
            else
            {
                foreach (RTreeNode item in this.pointers)
                {
                    yield return item.Boundary;
                }
            }
        }

    }
}