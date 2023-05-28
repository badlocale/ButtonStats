using ButtonStats.Shared.Model;

namespace ButtonStats.Client.Api
{
    public interface IApi
    {
        public bool SendInputData(InputData inputData);
    }
}
