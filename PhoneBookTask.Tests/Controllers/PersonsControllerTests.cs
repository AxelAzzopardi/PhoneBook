using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http.Results;
using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PhoneBookTask.Controllers;
using PhoneBookTask.Dtos;
using PhoneBookTask.Managers.Interfaces;
using PhoneBookTask.Models;

namespace PhoneBookTask.Tests.Controllers
{
    [TestClass]
    public class PersonsControllerTests
    {
        private Mock<IMapper> _mockMapper;
        private PersonsController _controller;
        private Mock<IPersonManager> _mockManager;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockMapper = new Mock<IMapper>();
            _mockManager = new Mock<IPersonManager>();
            _controller = new PersonsController(_mockMapper.Object, _mockManager.Object);
        }

        [TestMethod]
        public async Task TestAddEditDeletePerson()
        {
            // Arrange
            var mockPerson = new Person
            {
                Id = 1,
                FullName = "John Doe",
                PhoneNumber = "79797979",
                Address = "123 Main St",
                CompanyId = 1
            };
            var displayPerson = new DisplayPersonDto
            {
                Id = 1,
                FullName = "John Doe",
                PhoneNumber = "79797979",
                Address = "123 Main St"
            };

            _mockManager.Setup(x => x.GetQuery()).Returns(new List<Person> { mockPerson }.AsQueryable());
            _mockMapper.Setup(x => x.Map<Person, DisplayPersonDto>(It.IsAny<Person>()))
                .Returns(displayPerson);

            // Act
            var result = await _controller.PostPerson(mockPerson);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<DisplayPersonDto>));
            var contentResult = result as OkNegotiatedContentResult<DisplayPersonDto>;
            Assert.AreEqual(displayPerson, contentResult.Content);

            // Act
            mockPerson.FullName = "Jane Doe";
            mockPerson.PhoneNumber = "99999999";
            displayPerson.FullName = "Jane Doe";
            displayPerson.PhoneNumber = "99999999";

            result = await _controller.PutPerson(1, mockPerson);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(StatusCodeResult));
            var statusResult = result as StatusCodeResult;
            Assert.AreEqual(HttpStatusCode.NoContent, statusResult.StatusCode);

            // Act
            result = await _controller.DeletePerson(1);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<Person>));
            var deleteResult = result as OkNegotiatedContentResult<Person>;
            Assert.AreEqual(mockPerson, deleteResult.Content);
        }

        [TestClass]
        public class GetAllTests : PersonsControllerTests
        {
            [TestMethod]
            public void GetAll_WithSearchQuery_ReturnsCorrectResults()
            {
                // Arrange
                const string searchQuery = "test";
                var people = new List<Person>
                {
                    new Person { Id = 1, Address = "test address", FullName = "test name", PhoneNumber = "111", Company = new Company { CompanyName = "test company" } },
                    new Person { Id = 2, Address = "address 2", FullName = "name 2", PhoneNumber = "222", Company = new Company { CompanyName = "company 2" } }
                };

                _mockManager.Setup(x => x.GetQuery()).Returns(people.AsQueryable());
                _mockMapper
                    .Setup(x => x.Map<List<Person>, List<DisplayPersonDto>>(It.Is<List<Person>>(x => x[0].Address == "test address")))
                    .Returns(new List<DisplayPersonDto>() { new DisplayPersonDto() { Id = 1 } });

                var result = _controller.GetAll(searchQuery) as OkNegotiatedContentResult<List<DisplayPersonDto>>;

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(1, result.Content.Count);
                _mockManager.Verify(x => x.GetQuery(), Times.Once);
                _mockMapper.Verify(x => x.Map<List<Person>, List<DisplayPersonDto>>(It.IsAny<List<Person>>()), Times.Once);
            }

            [TestMethod]
            public void GetAll_SearchQueryEmpty_ReturnsAllPeople()
            {
                // Arrange
                var people = new List<Person>
                {
                    new Person
                    {
                        Id = 1, FullName = "John Doe", PhoneNumber = "123456789", Address = "Address 1",
                        Company = new Company { Id = 1, CompanyName = "Company 1" }
                    },
                    new Person
                    {
                        Id = 2, FullName = "Jane Doe", PhoneNumber = "987654321", Address = "Address 2",
                        Company = new Company { Id = 2, CompanyName = "Company 2" }
                    },
                    new Person
                    {
                        Id = 3, FullName = "Jim Doe", PhoneNumber = "123789456", Address = "Address 3",
                        Company = new Company { Id = 3, CompanyName = "Company 3" }
                    }
                };

                _mockManager.Setup(x => x.GetQuery()).Returns(people.AsQueryable());

                var expected = new List<DisplayPersonDto>
                {
                    new DisplayPersonDto
                    {
                        Id = 1, FullName = "John Doe", PhoneNumber = "123456789", Address = "Address 1",
                        CompanyName = "Company 1"
                    },
                    new DisplayPersonDto
                    {
                        Id = 2, FullName = "Jane Doe", PhoneNumber = "987654321", Address = "Address 2",
                        CompanyName = "Company 2"
                    },
                    new DisplayPersonDto
                    {
                        Id = 3, FullName = "Jim Doe", PhoneNumber = "123789456", Address = "Address 3",
                        CompanyName = "Company 3"
                    }
                };

                _mockMapper.Setup(x => x.Map<List<Person>, List<DisplayPersonDto>>(people)).Returns(expected);

                // Act
                var result = _controller.GetAll(string.Empty) as OkNegotiatedContentResult<List<DisplayPersonDto>>;

                // Assert
                Assert.IsNotNull(result);
                CollectionAssert.AreEqual(expected, result.Content);
            }
        }


        [TestClass]
        public class PostPerson : PersonsControllerTests
        {
            [TestMethod]
            public async Task PostPerson_ReturnsBadRequest_WhenModelStateIsInvalid()
            {
                // Arrange
                _controller.ModelState.AddModelError("Key", "Error message");

                // Act
                var result = await _controller.PostPerson(new Person());

                // Assert
                Assert.IsInstanceOfType(result, typeof(InvalidModelStateResult));
            }

            [TestMethod]
            public async Task PostPerson_CallsSaveMethod_WhenModelStateIsValid()
            {
                // Arrange
                var person = new Person
                { FullName = "John Doe", PhoneNumber = "123456789", Address = "Address", CompanyId = 1 };

                // Act
                var result = await _controller.PostPerson(person);

                // Assert
                _mockManager.Verify(x => x.Save(person), Times.Once());
                Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<DisplayPersonDto>));
            }
        }

        [TestClass]
        public class Random : PersonsControllerTests
        {
            [TestMethod]
            public void Random_Person_Returns_Random_Person_From_List()
            {
                // Arrange
                var people = new List<Person>
                {
                    new Person { Id = 1, FullName = "Person 1", Address = "Address 1", PhoneNumber = "Phone 1", Company = new Company { CompanyName = "Company 1" } },
                    new Person { Id = 2, FullName = "Person 2", Address = "Address 2", PhoneNumber = "Phone 2", Company = new Company { CompanyName = "Company 2" } },
                    new Person { Id = 3, FullName = "Person 3", Address = "Address 3", PhoneNumber = "Phone 3", Company = new Company { CompanyName = "Company 3" } },
                    new Person { Id = 4, FullName = "Person 4", Address = "Address 4", PhoneNumber = "Phone 4", Company = new Company { CompanyName = "Company 4" } },
                    new Person { Id = 5, FullName = "Person 5", Address = "Address 5", PhoneNumber = "Phone 5", Company = new Company { CompanyName = "Company 5" } },
                    new Person { Id = 6, FullName = "Person 6", Address = "Address 6", PhoneNumber = "Phone 6", Company = new Company { CompanyName = "Company 6" } },
                    new Person { Id = 7, FullName = "Person 7", Address = "Address 7", PhoneNumber = "Phone 7", Company = new Company { CompanyName = "Company 7" } },
                    new Person { Id = 8, FullName = "Person 8", Address = "Address 8", PhoneNumber = "Phone 8", Company = new Company { CompanyName = "Company 8" } },
                    new Person { Id = 9, FullName = "Person 9", Address = "Address 9", PhoneNumber = "Phone 9", Company = new Company { CompanyName = "Company 9" } },
                    new Person { Id = 10, FullName = "Person 10", Address = "Address 10", PhoneNumber = "Phone 10", Company = new Company { CompanyName = "Company 10" } }
                };

                _mockManager.Setup(p => p.GetQuery()).Returns(people.AsQueryable());
                var mapperMock = new Mock<IMapper>();

                _mockMapper.Setup(x => x.Map<Person, DisplayPersonDto>(It.IsAny<Person>()))
                    .Returns((Person p) => new DisplayPersonDto
                    {
                        Address = p.Address,
                        CompanyName = p.Company.CompanyName,
                        FullName = p.FullName,
                        Id = p.Id,
                        PhoneNumber = p.PhoneNumber
                    });

                // Act
                var result = _controller.Random() as OkNegotiatedContentResult<DisplayPersonDto>;

                // Assert
                Assert.IsNotNull(result);
                Assert.IsNotNull(result.Content);
                Assert.IsTrue(people.Any(p => p.Id == result.Content.Id));
            }
        }
    }
}
