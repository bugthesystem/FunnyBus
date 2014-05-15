using FunnyBus.Infrastructure;

namespace FunnyBus.Tests
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