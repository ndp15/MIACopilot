using ApprenticeManagement.Repositories;
using ApprenticeManagement.Services;
using ApprenticeManagement.UI;

var repository = new ApprenticeRepository();
var service = new ApprenticeService(repository);
var menu = new ConsoleMenu(service);

menu.Run();
