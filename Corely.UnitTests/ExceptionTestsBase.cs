namespace Corely.UnitTests
{
    public abstract class ExceptionTestsBase<T> where T : Exception, new()
    {
        protected virtual bool HasDefaultConstructor => true;
        protected virtual bool HasMessageConstructor => true;
        protected virtual bool HasInnerExceptionConstructor => true;

        [Fact]
        public void DefaultConstructor_ShouldWork()
        {
            if (HasDefaultConstructor) { Activator.CreateInstance<T>(); }
        }

        [Fact]
        public void MessageConstructor_ShouldWork()
        {
            if (HasMessageConstructor) { Activator.CreateInstance(typeof(T), "message"); }
        }

        [Fact]
        public void MessageInnerExceptionConstructor_ShouldWork()
        {
            if (HasInnerExceptionConstructor) { Activator.CreateInstance(typeof(T), "message", new Exception()); }
        }
    }
}
