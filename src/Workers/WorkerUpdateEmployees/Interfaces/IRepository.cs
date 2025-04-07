namespace WorkerUpdateEmployees.Interfaces;

public interface IRepository
{
    Task UpdateContactsAsync(string jsonContacts);
}