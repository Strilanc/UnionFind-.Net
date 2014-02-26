Installation
============

1. Copy the source file `UnionFindNode.cs` into your project.

Algorithm
=========

The algorithm is sourced from [wikipedia's disjoint set data structure article](http://en.wikipedia.org/wiki/Union_find). Operations take amortized nearly constant time.

Usage
=====

In the class that you want to union together, add a field of type `UnionFindNode`. Initialize the node, either eagerly when the class is constructed or lazily just before it is needed, then perform operations on it.

For example, suppose we have a `FancyGraphNode` to which edges can be added but not removed. We want to track if nodes are in the same connected component. We can:

1. Add the field `private readonly UnionFindNode _connectedComponentNode = new UnionFindNode()` to `FancyGraphNode`.
2. When adding an edge, call `edge.Node1._connectedComponentNode.UnionWith(edge.Node2._connectedComponentNode)`.
3. To determine if two nodes are in the same component, evaluate `edge.Node1._connectedComponentNode.IsInSameSetAs(edge.Node2._connectedComponentNode)`.
