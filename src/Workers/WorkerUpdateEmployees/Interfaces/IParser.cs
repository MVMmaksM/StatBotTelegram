using Domain.Entities;

namespace WorkerUpdateEmployees.Interfaces;

public interface IParser
{
    void ParseFormEmployee(string content);
}