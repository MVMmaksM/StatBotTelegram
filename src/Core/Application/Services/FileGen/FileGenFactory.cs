using Application.Interfaces;

namespace Application.Services.FileGen;

//конкретная реализация фабрики
public class FileGenFactory : IAbstractFactoryGenFile
{
    public IExcelFileGen GetExcelFileGen() => new ExcelFileGen();
}