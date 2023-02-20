using ButtonsStats.Shared.Model;

namespace ButtonsStats.Client.Api
{
    public interface IApi
    {
        public bool SendInputData(InputData inputData);
    }
}
