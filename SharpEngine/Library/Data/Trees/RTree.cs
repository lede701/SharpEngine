using SharpEngine.Library.Math;
using SharpEngine.Library.Math.Physics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEngine.Library.Data.Trees
{
	public class RTree : IEnumerable<ICollider>
	{
		public IEnumerator<ICollider> GetEnumerator()
		{
			throw new NotImplementedException();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			throw new NotImplementedException();
		}

		private Dictionary<ICollider, RTreeNode> mappings = new Dictionary<ICollider, RTreeNode>();

		private readonly int maxKeysPerNode;
		private readonly int minKeysPerNode;

		internal RTreeNode Root;

		public RTree()
		{
			maxKeysPerNode = 30;
			minKeysPerNode = (int)maxKeysPerNode / 2;
		}

		public RTree(int keysPerNode)
		{
			if(keysPerNode < 3)
			{
				throw new Exception("Max keys per node should be at least 3.");
			}

			maxKeysPerNode = keysPerNode;
			minKeysPerNode = (int)keysPerNode / 2;
		}

		public void Add(ICollider collider)
		{
			RTreeNode node = new RTreeNode(maxKeysPerNode, null)
			{
				Rectangle = collider.NodeRectangle,
				Collider = collider
			};
			mappings[collider] = node;

		}

		private void InsertToLeaf(RTreeNode node)
		{
			if(Root == null)
			{
				Root = new RTreeNode(maxKeysPerNode, null);
				Root.Add(node);
			}else
			{
				RTreeNode leaf = FindInsertionLeaf(Root, node);
				InsertAndSplit(leaf, node);
			}
		}

		private void InsertAndSplit(RTreeNode node, RTreeNode value)
		{
			if(node.KeyCount < maxKeysPerNode)
			{
				node.Add(value);
				ExpandAncestors(node);
			}else
			{
				List<RTreeNode> e = new List<RTreeNode>(new RTreeNode[] { value });
				e.AddRange(node.Children);
			}
		}

		private void ExpandAncestors(RTreeNode node)
		{
			while(node.Parent != null)
			{
				node.Parent.Rectangle.Merge(node.Rectangle);
				node.Parent.height += 1;
				node = node.Parent;
			}
		}

		private Tuple<RTreeNode, RTreeNode> GetDistantPairs(List<RTreeNode> enteries)
		{
			Tuple<RTreeNode, RTreeNode> results = null;

			double maxArea = double.MinValue;
			for (int i = 0; i < enteries.Count; ++i)
			{
				for (int j = i + 1; j < enteries.Count; ++j)
				{
					double currentArea = enteries[i].Rectangle.GetEnlargementArea(enteries[j].Rectangle);
					if(currentArea > maxArea)
					{
						results = new Tuple<RTreeNode, RTreeNode>(enteries[i], enteries[j]);
						maxArea = currentArea;
					}
				}
			}

			return results;
		}

		internal RTreeNode FindInsertionLeaf(RTreeNode node, RTreeNode child)
		{
			if(node.IsLeaf)
			{
				return node;
			}
			return FindInsertionLeaf(node.GetMinimumEnlargementArea(child.Rectangle), child);
		}

		internal class RTreeNode
		{
			internal int index;
			internal int height;
			internal TRectangle Rectangle { get; set; }
			internal int KeyCount {
				get
				{
					return Children.Count;
				}
			}

			public ICollider Collider { get; set; }

			internal RTreeNode Parent { get; set; }
			internal List<RTreeNode> Children { get; set; }

			internal bool IsLeaf => Rectangle.Collider != null 
				|| (Children[0] != null && Children[0].Rectangle.Collider != null);

			internal RTreeNode(int maxKeysPerNode, RTreeNode parent)
			{
				Parent = parent;
				Children = new List<RTreeNode>();
			}

			internal void Add(RTreeNode node)
			{
				node.Parent = this;
				node.index = Children.Count;
				Children.Add(node);

				height = node.height + 1;
			}
			internal RTreeNode GetMinimumEnlargementArea(TRectangle rect)
			{
				return Children[Children.Take(Children.Count).Select((node, index) => new { node, index })
					.OrderBy(x => x.node.Rectangle.GetEnlargementArea(rect))
					.ThenBy(x => x.node.Rectangle.Area).First().index];
			}
		}
	}
}
