using BodyMassIndexCalculator.src.Models;

namespace BodyMassIndexCalculator.src.Services.Interfaces
{
    public interface IAPI
    {
        public Task<IEnumerable<BodyMassIndexCalculation>> GetCalculationsByUserId(Guid userId);
        public Task<ResponseResult<BodyMassIndexCalculation?>> CreateCalculation(Guid userId, int height, int weight, double bodyMassIndex, string recommendation);
    }
}
