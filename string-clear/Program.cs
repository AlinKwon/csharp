// See https://aka.ms/new-console-template for more information

using AlinKwon.Helpers.StringUtil ;

Console.WriteLine("Hello, Clear!");

ISafeStringManager safeStringManager = new SafeStringManager();

string securityNumber = "1234567890123456" ;

Console.WriteLine($"{securityNumber}");
safeStringManager.ClearStringToX(securityNumber);
Console.WriteLine($"{securityNumber}");


string test1 = "abcdefghijklmnopqrstuvwxyz" ;
string test2 = "AlinKwon" ;
string test3 = "ASD#FEASGSSG" ;
string test4 = "youremail@abc.com" ;

safeStringManager.Register(test1);
safeStringManager.Register(test2);
safeStringManager.Register(test3);
safeStringManager.Register(test4);
Console.WriteLine($"{test1}");
Console.WriteLine($"{test2}");
Console.WriteLine($"{test3}");
Console.WriteLine($"{test4}");

safeStringManager.ClearAllRegisterString();
Console.WriteLine($"{test1}");
Console.WriteLine($"{test2}");
Console.WriteLine($"{test3}");
Console.WriteLine($"{test4}");