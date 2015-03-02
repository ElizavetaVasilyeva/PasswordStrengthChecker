using System;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Remoting.Messaging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PasswordStrengthChecker.BLL;
using PasswordStrengthChecker.ORM;
using Rhino.Mocks;

namespace PasswordStrengthCheckerTests
{
    [TestClass]
    public class VerifyTest
    {
        private IUserRepository repository;
        private PasswordChecker checker;

        [TestInitialize]
        public void Initialize()
        {
            repository= MockRepository.GenerateStrictMock<IUserRepository>();
            checker= new PasswordChecker(repository);
        }

        [TestMethod]
        public void VerifyTest_If_Password_Length_More_Then7_Characters_ReturnTrue()
        {
            //Arrange
            var testPwd = "qwerty123";
            var name = "Kate";
            bool expendedPas = true;
            User user=new User{Id=new Random().Next(100),Name = name,Password =testPwd, Role = false};

            repository.Expect(x => x.Create(user)).IgnoreArguments().Return(user.Id).Repeat.Once();

            //Act
            bool actual = checker.Verify(testPwd, name).Item1;
            //Assert
            Assert.AreEqual(expendedPas, actual);
            repository.VerifyAllExpectations();
        }
     

        [TestMethod]
        public void VerifyTest_If_Password_Length_Less_Then7_Characters_ReturnFalse()
        {
            //Arrange
            var testPwd = "fdsf";
            var name = "Valya";
            bool expendedPas = false;
            //Act
            bool actual = checker.Verify(testPwd, name).Item1;
            //Assert
            Assert.AreEqual(expendedPas,actual);
        }

        [TestMethod]
        public void VerifyTest_If_Password_Do_Not_Contain__Alphabetical_Character_ReturnFalse()
        {
            //Arrange
            var testPwd = "5785459)_";
            var name = "Kolya";
            bool expendedPas = false;
            //Act
            bool actual = checker.Verify(testPwd, name).Item1;
            //Assert
            Assert.AreEqual(expendedPas,actual);
        }

        [TestMethod]
        public void VerifyTest_If_Password_Contain_Alphabetical_Character_ReturnTrue()
        {
            //Arrange
            var testPwd = "5785459r)";
            var name = "Kolya";
            bool expendedPas = true;
            User user = new User { Id = new Random().Next(100), Name = name, Password =testPwd, Role = false };

            repository.Expect(x => x.Create(user)).IgnoreArguments().Return(user.Id).Repeat.Once();

            //Act
            bool actual = checker.Verify(testPwd, name).Item1;
            //Assert
            Assert.AreEqual(expendedPas,actual);
            repository.VerifyAllExpectations();
        }

        [TestMethod]
        public void VerifyTest_If_Password_Contain_Digit_Character_ReturnTrue()
        {
            //Arrange
            var testPwd = "5785459r)_";
            var name = "Petia";
            bool expendedPas = true;
            User user = new User { Id = new Random().Next(100), Name = name, Password = testPwd, Role = false };

            repository.Expect(x => x.Create(user)).IgnoreArguments().Return(user.Id).Repeat.Once();

            //Act
            bool actual = checker.Verify(testPwd, name).Item1;
            //Assert
            Assert.AreEqual(expendedPas,actual);
            repository.VerifyAllExpectations();
        }
        [TestMethod]
        public void VerifyTest_If_Password_Do_Not_Contain_Digit_Character_ReturnFalse()
        {
            //Arrange
            var testPwd = "jirkItxh";
            var name = "Olga";
            bool expendedPas = false;
            //Act
            bool actual = checker.Verify(testPwd, name).Item1;
            //Assert
            Assert.AreEqual(expendedPas,actual);
        }

        [TestMethod]
        public void VerifyTest_If_Password_Length_Less_Then10_Characters_ForAdmins_ReturnFalse()
        {
            //Arrange
            var testPwd = "jirkItxh";
            var name = "Olga";
            bool expendedPas = false;
            
            //Act
            bool actual = checker.Verify(testPwd, "Olga", true).Item1;
            //Assert
            Assert.AreEqual(expendedPas,actual);
        }

        [TestMethod]
        public void VerifyTest_If_Password_Length_More_Then10_Characters()
        {
            //Arrange
            var testPwd = "ji8kItx*klo";
            var name = "Olga";
            bool expendedPas = true;
            User user = new User { Id = new Random().Next(100), Name = name, Password = testPwd, Role = true };

            repository.Expect(x => x.Create(user)).IgnoreArguments().Return(user.Id).Repeat.Once();

            //Act
            bool actual = checker.Verify(testPwd, name, true).Item1;
            //Assert
            Assert.AreEqual(expendedPas,actual);
            repository.VerifyAllExpectations();
        }

        [TestMethod]
        public void VerifyTest_If_Password_Contain_Special_Character_ForAdmins_ReturnTrue()
        {
            //Arrange
            var testPwd = "ji8kItx@hjkgfh";
            var name = "Olga";
            bool expendedPas = true;
            User user = new User { Id = new Random().Next(100), Name = name, Password = testPwd, Role = true };

            repository.Expect(x => x.Create(user)).IgnoreArguments().Return(user.Id).Repeat.Once();

            //Act
            bool actual = checker.Verify(testPwd, name, true).Item1;
            //Assert
            Assert.AreEqual(expendedPas, actual);
            repository.VerifyAllExpectations();
        }

        [TestMethod]
        public void VerifyTest_If_Password_Length_Less_Then10_Characters()
        {
            //Arrange
            var testPwd = "98kI";
            var name = "Nastya";
            bool expendedPas = false;
            //Act
            bool actual = checker.Verify(testPwd, name, true).Item1;
            //Assert
            Assert.AreEqual(expendedPas,actual);
        }

        [TestMethod]
        public void VerifyTest_If_Password_Do_Not_Contain_Special_Character_ForAdmins_ReturnFalse()
        {
            //Arrange
            var testPwd = "j768kI";
            var name = "OlgaV";
            bool expendedPas = false;
            //Act
            bool actual = checker.Verify(testPwd, name, true).Item1;
            //Assert
            Assert.AreEqual(expendedPas, actual);
        }

        [TestMethod]
        public void VerifyTest_If_Password_Is_Empty_ReturnFalse()
        {
            var testPwd = "";
            var name = "Olga";
            bool expendedPas = false;
            //Act
            bool actual = checker.Verify(testPwd, name).Item1;
            //Assert
            Assert.AreEqual(expendedPas,actual);
        }

        [TestMethod]
        public void VerifyTest_If_Password_Is_Null_ReturnFalse()
        {
            bool expendedPas = false;
            var name = "Olga";
            //Act
            bool actual = checker.Verify(default(string), name).Item1;
            //Assert
            Assert.AreEqual(expendedPas, actual);
        }
    }
}
