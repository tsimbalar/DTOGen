using System.Linq;
using DTOGen.Json;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DTOGen.Tests.Json
{
    [TestClass]
    public class JsonTypeParserTest
    {
        

        [TestMethod]
        public void LoadTypes_with_empty_Json_object_must_return_empty()
        {
            // Arrange		
            var json = "{}";
            var sut = MakeSut();
            // Act
            var actual = sut.LoadTypes(json).ToList();

            // Assert		
            Assert.IsFalse(actual.Any(), "should be empty");
        }

        [TestMethod]
        public void LoadTypes_with_one_type_definition_with_no_props_must_return_one_type()
        {
            // Arrange		
            var json = @"{
Type1:{}
}";
            var sut = MakeSut();

            // Act
            var res = sut.LoadTypes(json).ToList();

            // Assert		
            Assert.AreEqual(1, res.Count);
            Assert.AreEqual("Type1", res[0].TypeName);
            Assert.AreEqual(0, res[0].Properties.Count());
        }

        [TestMethod]
        public void LoadTypes_with_one_type_definition_with_one_simple_property_must_return_type()
        {
            // Arrange		
            var json = @"{
Type1:{
        prop1 : 'String'
        }
}";
            var sut = MakeSut();
            // Act
            var res = sut.LoadTypes(json).ToList();

            // Assert		
            Assert.AreEqual(1, res[0].Properties.Count());
            Assert.AreEqual("prop1", res[0].Properties[0].Name);
            Assert.AreEqual("String", res[0].Properties[0].Type);
            Assert.AreEqual(false, res[0].Properties[0].IsCollection);
        }

        [TestMethod]
        public void LoadTypes_with_nested_type_must_return_type_for_containing_property()
        {
            // Arrange		
            var json = @"{
Type1:{
        complexProp : {
    subProp: 'nothing'
}
        }
}";
            var sut = MakeSut();

            // Act
            var res = sut.LoadTypes(json).ToList();

            // Assert		
            Assert.AreEqual(2, res.Count);
            Assert.AreEqual("Type1_ComplexProp", res[0].Properties[0].Type);
        }

        [TestMethod]
        public void LoadTypes_with_nested_type_must_return_2_type_definitions()
        {
            // Arrange		
            var json = @"{
Type1:{
        complexProp : {
    subProp: 'nothing'
}
        }
}";
            var sut = MakeSut();

            // Act
            var res = sut.LoadTypes(json).ToList();

            // Assert		
            Assert.AreEqual(2, res.Count);
            Assert.AreEqual("Type1_ComplexProp", res[1].TypeName);
            Assert.AreEqual("subProp", res[1].Properties[0].Name);
            Assert.AreEqual("nothing", res[1].Properties[0].Type);
            Assert.AreEqual(false, res[1].Properties[0].IsCollection);
        }

        [TestMethod]
        public void LoadTypes_with_nested_array_must_create_property_with_collection_type()
        {
            // Arrange		
            var json = @"{
Type1:{
        Children : [{
    SubProp: 'nothing'
}]
        }
}";
            var sut = MakeSut();

            // Act
            var res = sut.LoadTypes(json).ToList();

            // Assert		
            Assert.AreEqual(2, res.Count);
            Assert.AreEqual("Type1_Children_Item", res[0].Properties[0].Type);
            Assert.AreEqual(true, res[0].Properties[0].IsCollection);

        }

        [TestMethod]
        public void LoadTypes_with_nested_array_must_create_listItem_type()
        {
            // Arrange		
            var json = @"{
Type1:{
        Children : [{
    SubProp: 'nothing'
}]
        }
}";
            var sut = MakeSut();

            // Act
            var res = sut.LoadTypes(json).ToList();

            // Assert		
            Assert.AreEqual(2, res.Count);
            Assert.AreEqual("Type1_Children_Item", res[1].TypeName);
            Assert.AreEqual("SubProp", res[1].Properties[0].Name);
            Assert.AreEqual("nothing", res[1].Properties[0].Type);

        }

        [TestMethod]
        public void LoadTypes_with_array_of_simple_type_must_add_a_collection_property()
        {
            // Arrange		
            var json = @"{
Type1:{
        Children : ['string']
        }
}";
            var sut = MakeSut();

            // Act
            var res = sut.LoadTypes(json).ToList();

            // Assert		
            Assert.AreEqual(1, res.Count);
            Assert.AreEqual("Children", res[0].Properties[0].Name);
            Assert.AreEqual("string", res[0].Properties[0].Type);
            Assert.AreEqual(true, res[0].Properties[0].IsCollection);
        }

        #region Test Helper Methods
        private static JsonTypeParser MakeSut()
        {
            return new JsonTypeParser();
        }
        #endregion
    }
}
