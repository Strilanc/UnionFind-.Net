using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public class UnionFindNodeTest {
    private static void CompareAgainstReference(int count, IEnumerable<dynamic> operations) {
        var sets = Enumerable.Range(0, count).Select(e => new HashSet<int> { e }).ToArray();
        var nodes = Enumerable.Range(0, count).Select(e => new UnionFindNode()).ToArray();

        foreach (var op in operations) {
            var i1 = (int)op.i1;
            var i2 = (int)op.i2;
            if (op.compareElseUnion) {
                var isSame = nodes[i1].Find() == nodes[i2].Find();
                var isSame2 = nodes[i1].IsUnionedWith(nodes[i2]);
                var isSameRef = sets[i1] == sets[i2];
                Assert.AreEqual(isSame, isSameRef);
                Assert.AreEqual(isSame2, isSameRef);
            } else {
                var b1 = nodes[i1].Union(nodes[i2]);
                var b2 = sets[i1] != sets[i2];
                Assert.IsTrue(b1 == b2);
                if (b2) {
                    foreach (var e in sets[i1]) {
                        Assert.IsTrue(sets[i2].Add(e));
                        sets[e] = sets[i2];
                    }
                }
            }
        }   
    }

    [TestMethod]
    public void TestTrivial() {
        var r1 = new UnionFindNode();
        var r2 = new UnionFindNode();
        
        Assert.IsTrue(r1.IsUnionedWith(r1));
        Assert.IsTrue(r2.IsUnionedWith(r2));
        Assert.IsTrue(!r1.IsUnionedWith(r2));

        Assert.IsTrue(r1.Union(r2));
        Assert.IsTrue(r1.IsUnionedWith(r1));
        Assert.IsTrue(r2.IsUnionedWith(r2));
        Assert.IsTrue(r1.IsUnionedWith(r2));
    }
    [TestMethod]
    public void TestTrivial2() {
        var r1 = new UnionFindNode();
        var r2 = new UnionFindNode();
        var r3 = new UnionFindNode();
        Assert.IsTrue(!r1.IsUnionedWith(r2));
        Assert.IsTrue(!r1.IsUnionedWith(r3));
        Assert.IsTrue(!r2.IsUnionedWith(r3));

        Assert.IsTrue(r1.Union(r3));
        Assert.IsTrue(!r1.IsUnionedWith(r2));
        Assert.IsTrue(r1.IsUnionedWith(r3));
        Assert.IsTrue(!r2.IsUnionedWith(r3));

        Assert.IsTrue(r1.Union(r2));
        Assert.IsTrue(r1.IsUnionedWith(r2));
        Assert.IsTrue(r1.IsUnionedWith(r3));
        Assert.IsTrue(r2.IsUnionedWith(r3));

        Assert.IsTrue(!r1.Union(r3));
    }
    [TestMethod]
    public void TestChain() {
        var r = new List<UnionFindNode>();
        r.Add(new UnionFindNode());
        foreach (var repeat in Enumerable.Range(0, 100)) {
            r.Add(new UnionFindNode());
            Assert.IsTrue(r.Last().Union(r.First()));
            foreach (var e1 in r) {
                foreach (var e2 in r) {
                    Assert.IsTrue(e1.IsUnionedWith(e2));
                }
            }
        }
    }

    [TestMethod]
    public void TestRandomized() {
        var rng = new Random(2357);
        const int n = 1000;
        const int uni = 200;
        const int cmpPerUni = 20;
        foreach (var repeat in Enumerable.Range(0, 10)) {
            var ops = Enumerable.Range(0, uni * cmpPerUni)
                                .Select(i => new {
                                    compareElseUnion = i % cmpPerUni == 0,
                                    i1 = rng.Next(n),
                                    i2 = rng.Next(n)
                                }).ToArray();
            CompareAgainstReference(n, ops);
        }
    }
}
