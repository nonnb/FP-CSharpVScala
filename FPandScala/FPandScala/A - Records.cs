using System;
using NUnit.Framework;

namespace FPandScala
{
    // Classes are reference types, and by default, have the default 'reference equality' comparison.
    // Unless two variables point to the same object instance allocated on the heap, then they are not regarded as equal
    public class MyPointClass
    {
        public MyPointClass(decimal x, decimal y, decimal z)
        {
            X = x;
            Y = y;
            Z = z;
        }
        public decimal X { get; set; }
        public decimal Y { get; set; }
        public decimal Z { get; set; }
    }

    [TestFixture]
    public class PointTests
    {
        [Test]
        public void ClassReferenceEquality()
        {
            var point1 = new MyPointClass(1, 2, 3);
            var point2 = new MyPointClass(1, 2, 3);

            Assert.AreEqual(point1, point2, "Reference Types aren't equal, by default");
        }

        [Test]
        public void RecordValueEquality()
        {
            var point1 = new MyPointRecord(1, 2, 3);
            var point2 = new MyPointRecord(1, 2, 3);

            Assert.AreEqual(point1, point2, "Records are equal, by default");
        }

        [Test]
        public void ClassValueEquality()
        {
            var point1 = new MyPointClassWithValueEquality(1, 2, 3);
            var point2 = new MyPointClassWithValueEquality(1, 2, 3);

            Assert.AreEqual(point1, point2, "Lots of work needed to get the same behaviour as records");
        }
    }

    /// <summary>
    /// To make reference types 'equal', we need to add a lot of additional boilerplate code ...
    /// </summary>
    public class MyPointClassWithValueEquality : IEquatable<MyPointClassWithValueEquality>
    {
        public MyPointClassWithValueEquality(decimal x, decimal y, decimal z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public void Deconstruct(out decimal x, out decimal y, out decimal z)
            => (x, y, z) = (X, Y, Z);

        public decimal X { get;  }
        public decimal Y { get;  }
        public decimal Z { get;  }

        public bool Equals(MyPointClassWithValueEquality other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return X == other.X && Y == other.Y && Z == other.Z;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((MyPointClassWithValueEquality) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y, Z);
        }
    }

    // But now, with C# records, we can get ALL of the benefit of IEquatable, Equals, HashCode, 3 Immutable Properties AND a Deconstruct in one line!
    public record MyPointRecord(decimal X, decimal Y, decimal Z);

    // NB - RECORDS ARE NOT STRUCTS - records are still heap allocated. Structs are value types, and should be avoided unless you know what you're doing.
}
