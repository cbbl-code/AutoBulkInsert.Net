
using NUnit.Framework;
using Repository;
using System;

namespace RepositoryUnitTests
{

    [TestFixture]
    public class TypePropertyLoaderTests
    {

        private class TestClass<T>
        {
            public T PropertyA { get; set; }
        }


        [TestCase("System.String", "Testing")]
        [TestCase("System.Char", 'A')]
        [TestCase("System.Int32", 10000)]
        [TestCase("System.Int16", 10)]
        [TestCase("System.Int64", 1000000)]
        [TestCase("System.Boolean", true)]
        [TestCase("System.Byte", 64)]
        [TestCase("System.Nullable`1[System.DateTime]", null)]
        [TestCase("System.DateTime", "2014-10-02T00:00Z")]
        public void GetPropertyValue_WhenGivenObjectWithPropertyAsType_ReturnsCorrectType(
            string testClassPropertyTypeName,
            dynamic testClassPropertyValue)
        {
            //Arrange
            Type testClassPropertyType = Type.GetType(testClassPropertyTypeName);
            Type testUboundGenericType = typeof(TestClass<>);
            Type testClosedGenericType = testUboundGenericType.MakeGenericType(testClassPropertyType);
            var testInstance = Activator.CreateInstance(testClosedGenericType);

             Type t = testClosedGenericType.GetGenericArguments()[0];
            var propertyAValue = (testClassPropertyValue == null) ? null : Convert.ChangeType(testClassPropertyValue, t);

            var closedProperty = testClosedGenericType.GetProperty("PropertyA");
            closedProperty.SetValue(testInstance, propertyAValue);

            Type UnBoundPropertyLoaderType = typeof(TypePropertyLoader<>);
            Type ClosedPropertyLoaderGenericType = UnBoundPropertyLoaderType.MakeGenericType(testClosedGenericType);
            dynamic sut = Activator.CreateInstance(ClosedPropertyLoaderGenericType);
            

            //Act
            var result = sut.GetPropertyValue("PropertyA", testInstance);

            //Assert
            Assert.AreEqual(propertyAValue, result);

        }

 
        [TestCase("System.Nullable`1[System.DateTime]", null)]
        public void GetPropertyValue_WhenGivenObjectWithPropertyAsNullableType_ReturnsCorrectType(
            string testClassPropertyTypeName,
            dynamic testClassPropertyValue)
        {
            //Arrange
            Type testClassPropertyType = Type.GetType(testClassPropertyTypeName);
            Type testUboundGenericType = typeof(TestClass<>);
            Type testClosedGenericType = testUboundGenericType.MakeGenericType(testClassPropertyType);
            var testInstance = Activator.CreateInstance(testClosedGenericType);

            Type t = testClosedGenericType.GetGenericArguments()[0];
            var propertyAValue = (testClassPropertyValue == null) ? null : Convert.ChangeType(testClassPropertyValue, t);

            var closedProperty = testClosedGenericType.GetProperty("PropertyA");
            closedProperty.SetValue(testInstance, propertyAValue);

            Type UnBoundPropertyLoaderType = typeof(TypePropertyLoader<>);
            Type ClosedPropertyLoaderGenericType = UnBoundPropertyLoaderType.MakeGenericType(testClosedGenericType);
            dynamic sut = Activator.CreateInstance(ClosedPropertyLoaderGenericType);


            //Act
            var result = sut.GetPropertyValue("PropertyA", testInstance);

            //Assert
            Assert.IsNull(result);
        }

    }
}

