using System;

namespace Shkadun_TheBank
{
    class NewRandom
    {
        static Random random;
        public static string CreateNumberAccount()  //Генерация номера счёта
        {
            random = new Random();
            //Разрешённые символы
            char[] charArray = {'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r',
                                's', 't', 'u', 'v', 'w', 'x', 'y', 'z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9'};
            string number = "";

            for (int i = 0; i < 20; i++)
            {
                number += charArray[random.Next(0, charArray.Length - 1)];
            }

            Console.WriteLine(number);

            return number;
        }

        public static long RandomCardNumber()   //Генерация номера карты
        {
            string number = "";
            //Разрешённые символы
            char[] charArray = { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' };

            for (int i = 0; i < 16; i++)
            {
                number += charArray[random.Next(0, charArray.Length - 1)];
            }

            return long.Parse(number);
        }
    }
}
