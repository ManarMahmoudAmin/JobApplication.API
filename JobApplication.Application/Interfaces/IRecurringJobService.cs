namespace JobApplication.Application.Interfaces
{
    public interface IRecurringJobService
    {
        Task AutoCloseOldJobsAsync();
    }   
}
