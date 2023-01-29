using System.Collections.Generic;
using System.Linq;
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
    public class CompaniesControllerTests
    {
        private Mock<IMapper> _mockMapper;
        private CompaniesController _controller;
        private Mock<ICompanyManager> _mockManager;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockMapper = new Mock<IMapper>();
            _mockManager = new Mock<ICompanyManager>();
            _controller = new CompaniesController(_mockMapper.Object, _mockManager.Object);
        }

        [TestClass]
        public class PostCompanyTests : CompaniesControllerTests
        {
            [TestMethod]
            public async Task PostCompany_ShouldReturnBadRequest_GivenInvalidModelState()
            {
                // Arrange
                _controller.ModelState.AddModelError("key", "error message");
                var company = new Company();

                // Act
                var result = await _controller.PostCompany(company);

                // Assert
                Assert.IsInstanceOfType(result, typeof(InvalidModelStateResult));
            }

            [TestMethod]
            public async Task PostCompany_ShouldReturnBadRequest_GivenCompanyNameAlreadyInUse()
            {
                // Arrange
                var companies = new List<Company>
                {
                    new Company { CompanyName = "existing company" }
                }.AsQueryable();


                _mockManager.Setup(m => m.GetQuery()).Returns(companies);


                var company = new Company { CompanyName = "existing company" };

                // Act
                var result = await _controller.PostCompany(company);

                // Assert
                Assert.IsInstanceOfType(result, typeof(BadRequestErrorMessageResult));
                Assert.AreEqual("Company Name is already in use", ((BadRequestErrorMessageResult)result).Message);
            }

            [TestMethod]
            public async Task PostCompany_ShouldReturnOk_GivenValidCompany()
            {
                var company = new Company { CompanyName = "Test Company" };

                _mockManager.Setup(x => x.GetQuery()).Returns(new List<Company>() { }.AsQueryable);
                _mockManager.Setup(x => x.Save(company)).ReturnsAsync(1);

                var mockMapper = new Mock<IMapper>();
                mockMapper.Setup(x => x.Map<Company, DisplayCompanyDto>(company)).Returns(new DisplayCompanyDto()
                { CompanyName = "Test Company", Id = 1 });


                // Act
                var result = await _controller.PostCompany(company);

                // Assert
                Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<DisplayCompanyDto>));
                _mockManager.Verify(x => x.Save(It.IsAny<Company>()), Times.Once);
                _mockManager.Verify(x => x.Save(It.Is<Company>(x => x.CompanyName == "Test Company")), Times.Once);
            }
        }

        [TestClass]
        public class GetAllTests : CompaniesControllerTests
        {
            [TestMethod]
            public void GetAll_ShouldReturnOk_GivenValidData()
            {
                // Arrange
                var companyList = new List<Company>
                {
                    new Company
                    {
                        CompanyName = "Company 1", Persons = new List<Person>()
                        {
                            new Person { FullName = "Test McTester" }
                        }
                    },
                    new Company { CompanyName = "Company 2", Persons = new List<Person>() }
                };

                _mockManager.Setup(x => x.GetQuery()).Returns(companyList.ToList().AsQueryable());

                _mockMapper.Setup(x => x.Map<List<Company>, List<DisplayCompanyDto>>(companyList))
                    .Returns(new List<DisplayCompanyDto>
                    {
                        new DisplayCompanyDto { CompanyName = "Company 1", NumberOfPeople = 1},
                        new DisplayCompanyDto { CompanyName = "Company 2", NumberOfPeople = 0}
                    });

                // Act
                var result = _controller.GetAll() as OkNegotiatedContentResult<List<DisplayCompanyDto>>;

                // Assert
                Assert.IsNotNull(result);
                Assert.IsNotNull(result.Content);
                Assert.AreEqual(2, result.Content.Count);
                Assert.AreEqual("Company 1", result.Content[0].CompanyName);
                Assert.AreEqual("Company 2", result.Content[1].CompanyName);
                Assert.AreEqual(1, result.Content[0].NumberOfPeople);
                Assert.AreEqual(0, result.Content[1].NumberOfPeople);
            }
        }
    }
}