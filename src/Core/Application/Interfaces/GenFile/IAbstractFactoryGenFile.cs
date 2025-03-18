namespace Application.Interfaces;

//абстрактная фабрика
//возвращающая определенный генератор файла
public interface IAbstractFactoryGenFile
{
    IExcelFileGen GetExcelFileGen();
}