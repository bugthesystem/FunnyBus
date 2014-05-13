using System;
using System.Threading.Tasks;
using NUnit.Framework;

namespace FunnyBus.Tests.Extensions
{
    public static class AssertExtensions
    {
        public static async Task ThrowsAsync<TException>(Func<Task> actionToExecute) where TException : Exception
        {
            var expected = typeof(TException);
            Type actual = null;

            try
            {
                await actionToExecute();
            }
            catch (TException exception)
            {
                actual = exception.GetType();
            }

            Assert.AreEqual(expected, actual);
        }
    }
}