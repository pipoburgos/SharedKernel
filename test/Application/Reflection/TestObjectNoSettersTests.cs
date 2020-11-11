using System;
using SharedKernel.Application.Reflection;
using Xunit;

namespace SharedKernel.Application.Tests.Reflection
{
    public class TestObjectNoSettersTests
    {
        [Fact]
        public void CreateObject()
        {
            var obj = ReflectionHelper.CreateInstance<TestObjectNoSetters>();

            Assert.NotNull(obj);
        }

        [Fact]
        public void SetPropertyValueGuidDefault()
        {
            Guid id = default;

            var obj = ReflectionHelper.CreateInstance<TestObjectNoSetters>();

            Assert.Throws<ArgumentNullException>(() =>
                ReflectionHelper.SetProperty(obj, nameof(TestObjectNoSetters.Id), id));
        }

        [Fact]
        public void SetPropertyValueGuidEmpty()
        {
            var id = Guid.Empty;

            var obj = ReflectionHelper.CreateInstance<TestObjectNoSetters>();

            Assert.Throws<ArgumentNullException>(() =>
                ReflectionHelper.SetProperty(obj, nameof(TestObjectNoSetters.Id), id));
        }

        [Fact]
        public void SetPropertyValueGuidNewGuid()
        {
            var id = Guid.NewGuid();

            var obj = ReflectionHelper.CreateInstance<TestObjectNoSetters>();

            Assert.Throws<ArgumentNullException>(() =>
                ReflectionHelper.SetProperty(obj, nameof(TestObjectNoSetters.Id), id));
        }

        [Fact]
        public void SetPropertyValueNullableGuidNull()
        {
            Guid? adminId = null;

            var obj = ReflectionHelper.CreateInstance<TestObjectNoSetters>();

            // ReSharper disable once ExpressionIsAlwaysNull
            Assert.Throws<ArgumentNullException>(() =>
                ReflectionHelper.SetProperty(obj, nameof(TestObjectNoSetters.AdminId), adminId));
        }

        [Fact]
        public void SetPropertyValueNullableGuidEmpty()
        {
            Guid? adminId = Guid.Empty;

            var obj = ReflectionHelper.CreateInstance<TestObjectNoSetters>();

            Assert.Throws<ArgumentNullException>(() =>
                ReflectionHelper.SetProperty(obj, nameof(TestObjectNoSetters.AdminId), adminId));
        }

        [Fact]
        public void SetPropertyValueNullableGuidNewGuid()
        {
            Guid? adminId = Guid.NewGuid();

            var obj = ReflectionHelper.CreateInstance<TestObjectNoSetters>();

            Assert.Throws<ArgumentNullException>(() =>
                ReflectionHelper.SetProperty(obj, nameof(TestObjectNoSetters.AdminId), adminId));
        }

        [Fact]
        public void SetPropertyValueAge()
        {
            const int age = 3;

            var obj = ReflectionHelper.CreateInstance<TestObjectNoSetters>();

            Assert.Throws<ArgumentNullException>(() =>
                ReflectionHelper.SetProperty(obj, nameof(TestObjectNoSetters.Age), age));
        }

        [Fact]
        public void SetPropertyValueDateTimeNow()
        {
            var created = DateTime.Now;

            var obj = ReflectionHelper.CreateInstance<TestObjectNoSetters>();

            Assert.Throws<ArgumentNullException>(() =>
                ReflectionHelper.SetProperty(obj, nameof(TestObjectNoSetters.Created), created));
        }

        [Fact]
        public void SetPropertyValueNullableDateTimeNow()
        {
            var birthday = DateTime.Now;

            var obj = ReflectionHelper.CreateInstance<TestObjectNoSetters>();

            Assert.Throws<ArgumentNullException>(() =>
                ReflectionHelper.SetProperty(obj, nameof(TestObjectNoSetters.Birthday), birthday));
        }

        [Fact]
        public void SetPropertyValueStringNull()
        {
            const string name = null;

            var obj = ReflectionHelper.CreateInstance<TestObjectNoSetters>();

            Assert.Throws<ArgumentNullException>(() =>
                ReflectionHelper.SetProperty(obj, nameof(TestObjectNoSetters.Name), name));
        }

        [Fact]
        public void SetPropertyValueStringEmpty()
        {
            var name = string.Empty;

            var obj = ReflectionHelper.CreateInstance<TestObjectNoSetters>();

            Assert.Throws<ArgumentNullException>(() =>
                ReflectionHelper.SetProperty(obj, nameof(TestObjectNoSetters.Name), name));
        }

        [Fact]
        public void SetPropertyValueStringWhitespace()
        {
            const string name = "   ";

            var obj = ReflectionHelper.CreateInstance<TestObjectNoSetters>();
            Assert.Throws<ArgumentNullException>(() =>
                ReflectionHelper.SetProperty(obj, nameof(TestObjectNoSetters.Name), name));
        }

        [Fact]
        public void SetPropertyValueNameNotNullNotEmpty()
        {
            const string name = "Roberto";

            var obj = ReflectionHelper.CreateInstance<TestObjectNoSetters>();

            Assert.Throws<ArgumentNullException>(() =>
                ReflectionHelper.SetProperty(obj, nameof(TestObjectNoSetters.Name), name));
        }

        [Fact]
        public void SetPropertyValueNullableDouble()
        {
            double? latitude = 2.3;

            var obj = ReflectionHelper.CreateInstance<TestObjectNoSetters>();

            Assert.Throws<ArgumentNullException>(() =>
                ReflectionHelper.SetProperty(obj, nameof(TestObjectNoSetters.Latitude), latitude));
        }
    }
}
