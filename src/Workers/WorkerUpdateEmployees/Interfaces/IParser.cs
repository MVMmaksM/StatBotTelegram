using Domain.Entities;
using WorkerUpdateEmployees.Model;

namespace WorkerUpdateEmployees.Interfaces;

public interface IParser
{
    List<Contact> ParseContact(string content);
}