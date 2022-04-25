using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Tests.Utilities;

public static class AssertX
{
    public static void Length<T>(int expected, IEnumerable<T> enumerable)
    {
        Assert.Equal(expected, enumerable.Count());
    }
}