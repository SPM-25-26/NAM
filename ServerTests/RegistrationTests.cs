namespace ServerTests
{
    [TestClass]
    public sealed class RegistrationTests
    {
        [TestMethod]
        public void RegisterUser_ReturnsOk_WhenRegistrationIsSuccessful()
        {
            return;
        }

        [TestMethod]
        public void RegisterUser_ReturnsValidationProblem_WhenEmailOrPasswordAreIncorrect()
        {
            return;
        }

        [TestMethod]
        public void RegisterUser_ReturnsBadRequest_WhenEmailAlreadyExists()
        {
            return;
        }
    }
}
