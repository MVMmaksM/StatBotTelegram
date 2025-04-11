namespace WorkerUpdateEmployees.Interfaces;

public interface IRepository
{
    Task<int> UpdateContactsAsync(string jsonContacts);
}