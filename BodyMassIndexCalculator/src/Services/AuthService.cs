using BodyMassIndexCalculator.src.Models;

namespace BodyMassIndexCalculator.src.Services
{
    public class AuthService
    {
        public static async Task<AuthResponseResult<string>> SignIn(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || 
                string.IsNullOrEmpty(password))
                return new AuthResponseResult<string>(null, "Ошибка");

            return new AuthResponseResult<string>("Успешно", null);
        }

        public static async Task<AuthResponseResult<string>> SignUp(string firstName, string lastName, string email, string password)
        {
            if (string.IsNullOrEmpty(firstName) || 
                string.IsNullOrEmpty(lastName) || 
                string.IsNullOrEmpty(email) || 
                string.IsNullOrEmpty(password))
                return new AuthResponseResult<string>(null, "Ошибка");

            return new AuthResponseResult<string>("Успешно", null);
        }
    }
}
