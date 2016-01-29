using FunnyBus.Infrastructure;

namespace FunnyBus.Tests.App
{
    public class TestHandler :
        IHandle<TestMessage, TestMessageResult>
    {
        public TestMessageResult Handle(TestMessage message)
        {
            return new TestMessageResult { Success = true };
        }
    }
}