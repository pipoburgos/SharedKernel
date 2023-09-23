using SharedKernel.Application.Reflection;

namespace SharedKernel.Application.Tests.Reflection
{
    public class TestObjectPrivateSettersTests
    {
        [Fact]
        public void CreateObject()
        {
            var obj = ReflectionHelper.CreateInstance<TestObjectPrivateSetters>();

            Assert.NotNull(obj);
        }

        [Fact]
        public void SetPropertyValueGuidDefault()
        {
            Guid id = default;

            var obj = ReflectionHelper.CreateInstance<TestObjectPrivateSetters>();
            ReflectionHelper.SetProperty(obj, nameof(TestObjectPrivateSetters.Id), id);

            Assert.NotNull(obj);
            Assert.Equal(id, obj.Id);
        }

        [Fact]
        public void SetPropertyValueGuidEmpty()
        {
            var id = Guid.Empty;

            var obj = ReflectionHelper.CreateInstance<TestObjectPrivateSetters>();
            ReflectionHelper.SetProperty(obj, nameof(TestObjectPrivateSetters.Id), id);

            Assert.NotNull(obj);
            Assert.Equal(id, obj.Id);
        }

        [Fact]
        public void SetPropertyValueGuidNewGuid()
        {
            var id = Guid.NewGuid();

            var obj = ReflectionHelper.CreateInstance<TestObjectPrivateSetters>();
            ReflectionHelper.SetProperty(obj, nameof(TestObjectPrivateSetters.Id), id);

            Assert.NotNull(obj);
            Assert.Equal(id, obj.Id);
        }

        [Fact]
        public void SetPropertyValueNullableGuidNull()
        {
            var obj = ReflectionHelper.CreateInstance<TestObjectPrivateSetters>();
            ReflectionHelper.SetProperty(obj, nameof(TestObjectPrivateSetters.AdminId), null);

            Assert.NotNull(obj);
            Assert.Null(obj.AdminId);
        }

        [Fact]
        public void SetPropertyValueNullableGuidEmpty()
        {
            Guid? adminId = Guid.Empty;

            var obj = ReflectionHelper.CreateInstance<TestObjectPrivateSetters>();
            ReflectionHelper.SetProperty(obj, nameof(TestObjectPrivateSetters.AdminId), adminId);

            Assert.NotNull(obj);
            Assert.Equal(adminId, obj.AdminId);
        }

        [Fact]
        public void SetPropertyValueNullableGuidNewGuid()
        {
            Guid? adminId = Guid.NewGuid();

            var obj = ReflectionHelper.CreateInstance<TestObjectPrivateSetters>();
            ReflectionHelper.SetProperty(obj, nameof(TestObjectPrivateSetters.AdminId), adminId);

            Assert.NotNull(obj);
            Assert.Equal(adminId, obj.AdminId);
        }

        [Fact]
        public void SetPropertyValueAge()
        {
            const int age = 3;

            var obj = ReflectionHelper.CreateInstance<TestObjectPrivateSetters>();
            ReflectionHelper.SetProperty(obj, nameof(TestObjectPrivateSetters.Age), age);

            Assert.NotNull(obj);
            Assert.Equal(age, obj.Age);
        }

        [Fact]
        public void SetPropertyValueDateTimeNow()
        {
            var created = DateTime.Now;

            var obj = ReflectionHelper.CreateInstance<TestObjectPrivateSetters>();
            ReflectionHelper.SetProperty(obj, nameof(TestObjectPrivateSetters.Created), created);

            Assert.NotNull(obj);
            Assert.Equal(created, obj.Created);
        }

        [Fact]
        public void SetPropertyValueNullableDateTimeNow()
        {
            var birthday = DateTime.Now;

            var obj = ReflectionHelper.CreateInstance<TestObjectPrivateSetters>();
            ReflectionHelper.SetProperty(obj, nameof(TestObjectPrivateSetters.Birthday), birthday);

            Assert.NotNull(obj);
            Assert.Equal(birthday, obj.Birthday);
        }

        [Fact]
        public void SetPropertyValueStringNull()
        {
            var obj = ReflectionHelper.CreateInstance<TestObjectPrivateSetters>();
            ReflectionHelper.SetProperty(obj, nameof(TestObjectPrivateSetters.Name), null);

            Assert.NotNull(obj);
            Assert.Null(obj.Name);
        }

        [Fact]
        public void SetPropertyValueStringEmpty()
        {
            var name = string.Empty;

            var obj = ReflectionHelper.CreateInstance<TestObjectPrivateSetters>();
            ReflectionHelper.SetProperty(obj, nameof(TestObjectPrivateSetters.Name), name);

            Assert.NotNull(obj);
            Assert.Equal(name, obj.Name);
        }

        [Fact]
        public void SetPropertyValueStringWhitespace()
        {
            const string name = "   ";

            var obj = ReflectionHelper.CreateInstance<TestObjectPrivateSetters>();
            ReflectionHelper.SetProperty(obj, nameof(TestObjectPrivateSetters.Name), name);

            Assert.NotNull(obj);
            Assert.Equal(name, obj.Name);
        }

        [Fact]
        public void SetPropertyValueNameNotNullNotEmpty()
        {
            const string name = "Roberto";

            var obj = ReflectionHelper.CreateInstance<TestObjectPrivateSetters>();
            ReflectionHelper.SetProperty(obj, nameof(TestObjectPrivateSetters.Name), name);

            Assert.NotNull(obj);
            Assert.Equal(name, obj.Name);
        }

        [Fact]
        public void SetPropertyValueNullableDouble()
        {
            double? latitude = 2.3;

            var obj = ReflectionHelper.CreateInstance<TestObjectPrivateSetters>();
            ReflectionHelper.SetProperty(obj, nameof(TestObjectPrivateSetters.Latitude), latitude);

            Assert.NotNull(obj);
            Assert.Equal(latitude, obj.Latitude);
        }
    }
}
