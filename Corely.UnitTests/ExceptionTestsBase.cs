namespace Corely.UnitTests
{
    public abstract class ExceptionTestsBase<TException> where TException : Exception, new()
    {
        protected virtual bool HasDefaultConstructor => true;
        protected virtual bool HasMessageConstructor => true;
        protected virtual bool HasInnerExceptionConstructor => true;

        [Fact]
        public void DefaultConstructor_ShouldWork()
        {
            if (HasDefaultConstructor) { Activator.CreateInstance<TException>(); }
        }

        [Fact]
        public void MessageConstructor_ShouldWork()
        {
            if (HasMessageConstructor) { Activator.CreateInstance(typeof(TException), "message"); }
        }

        [Fact]
        public void MessageInnerExceptionConstructor_ShouldWork()
        {
            if (HasInnerExceptionConstructor) { Activator.CreateInstance(typeof(TException), "message", new Exception()); }
        }
    }
}
