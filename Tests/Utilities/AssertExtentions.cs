using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Tests.Utilities;

public class AssertExtentions : Assert
{
    public static void Length<T>(int expected, IEnumerable<T> enumerable)
    {
        Equal(expected, enumerable.Count());
    }
}