using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WebStore.Tests
{
    public static class AssertExt
    {
        public static T IsType<T>(object value)
        {
            Assert.IsInstanceOfType(value, typeof(T));
            return (T)value;
        }
    }
}
