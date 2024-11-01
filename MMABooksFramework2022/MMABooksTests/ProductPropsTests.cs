using NUnit.Framework;

using MMABooksProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMABooksTests
{
    [TestFixture]
    public class ProductPropsTests
    {
        // A public ProductProps object that will be used
        // in most of the ProductProps tests.
        ProductProps props;
        [SetUp]
        // This runs before each test and sets all the fields
        // in the public ProductProps props object to the one specified
        // in the SetUp method.
        public void Setup()
        {
            props = new ProductProps();
            props.ProductCode = "TYHN";
            props.Description = "Description About Coding Book";
            props.UnitPrice = 10.00m;
            props.OnHandQuantity = 100;
        }

        [Test]
        // This is used to the test the GetState method
        // from the ProductProps class. By checking the
        // JSON-formatted string for each field that was 
        // int the serialized object, where we check if it
        // it contains all the correct fields by using
        // Contains and the field values we are looking
        // for.
        public void TestGetState()
        {
            string jsonString = props.GetState();
            Console.WriteLine(jsonString);
            Assert.IsTrue(jsonString.Contains(props.ProductCode));
            Assert.IsTrue(jsonString.Contains(props.Description));
            Assert.IsTrue(jsonString.Contains(props.UnitPrice.ToString()));
            Assert.IsTrue(jsonString.Contains(props.OnHandQuantity.ToString()));
        }

        [Test]
        // This is used to the test the SetState method
        // from the ProductProps class. Where it checks
        // to see if it's able to covert the JSON-formatted
        // string back into a ProductProps object. Using
        // AreEqual tests to see if the fields of the
        // ProductProps object are what they are
        // supposed to be given the SON-formatted string.
        public void TestSetState()
        {
            string jsonString = props.GetState();
            ProductProps newProps = new ProductProps();
            newProps.SetState(jsonString);
            Assert.AreEqual(props.ProductCode, newProps.ProductCode);
            Assert.AreEqual(props.Description, newProps.Description);
            Assert.AreEqual(props.UnitPrice, newProps.UnitPrice);
            Assert.AreEqual(props.OnHandQuantity, newProps.OnHandQuantity);
        }

        [Test]
        // This is used to test the Clone method from the 
        // ProductProps class, to see if it's able to
        // copy field of one ProductProps object to
        // another ProductProps object. Using AreEqual
        // tests to see if the field values with correctly
        // copied over.
        public void TestClone()
        {
            ProductProps newProps = (ProductProps)props.Clone();
            Assert.AreEqual(props.ProductCode, newProps.ProductCode);
            Assert.AreEqual(props.Description, newProps.Description);
            Assert.AreEqual(props.UnitPrice, newProps.UnitPrice);
            Assert.AreEqual(props.OnHandQuantity, newProps.OnHandQuantity);
        }
    }
}
