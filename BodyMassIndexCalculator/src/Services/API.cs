using BodyMassIndexCalculator.src.Models;
using BodyMassIndexCalculator.src.Services.Interfaces;
using Supabase.Gotrue.Exceptions;

namespace BodyMassIndexCalculator.src.Services
{
    public class API : IAPI
    {
        public async Task<ResponseResult<BodyMassIndexCalculation?>> CreateCalculation(Guid userId, int height, int weight, double bodyMassIndex, string recommendation)
        {
            try
            {
                var newCalculation = new BodyMassIndexCalculation
                {
                    CreatedAt = DateTime.Now,
                    UserId = userId,
                    Height = height,
                    Weight = weight,
                    BodyMassIndex = bodyMassIndex,
                    Recommendation = recommendation
                };

                var response = await SupabaseService.Client
                    .From<BodyMassIndexCalculation>()
                    .Insert(newCalculation);

                return new(response.Model, null);
            }
            catch (GotrueException gex)
            {
                string userFriendlyError = GetUserFriendlyError(gex);
                return new(null, userFriendlyError);
            }
            catch (Exception)
            {
                return new(null, "Произошла непредвиденная ошибка");
            }
        }

        public async Task<IEnumerable<BodyMassIndexCalculation>> GetCalculationsByUserId(Guid userId)
        {
            var response = await SupabaseService.Client
                .From<BodyMassIndexCalculation>()
                .Select("created_at, height, weight, body_mass_index, recommendation")
                .Where(bmic => bmic.UserId == userId)
                .Get();

            return response.Models.AsEnumerable();
        }

        private static string GetUserFriendlyError(GotrueException gex) => gex.StatusCode == 0
            ? "Произошла непредвиденная ошибка"
            : gex.StatusCode switch
            {
                400 => "Некорректные данные",
                401 => "Ошибка авторизации",
                404 => "Ресурс не найден",
                422 => "Данные не могут быть обработаны",
                500 => "Ошибка на сервере",
                _ => $"Произошла непредвиденная ошибка (код: {gex.StatusCode})"
            };
    }
}
