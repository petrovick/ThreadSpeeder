using System;
using System.Collections.Generic;
using System.Text;
using ThreadSpeeder.Example.Business;
using ThreadSpeeder.Example.DTO;

namespace ThreadSpeeder.Example.Application
{
    public class UserApplication
    {
        public List<UserAddressAgeDTO> ProcessWithoutThreadSpeeder(List<User> users)
        {
            List<UserAddressAgeDTO> returnedList = new List<UserAddressAgeDTO>();
            foreach (var user in users)
            {
                returnedList.Add(MakeYourFuncHere(user));
            }
            return returnedList;
        }

        public List<UserAddressAgeDTO> ProcessWithThreadSpeeder(List<User> users)
        {
            List<UserAddressAgeDTO> returnedList = new List<UserAddressAgeDTO>();
            //new ThreadSpeeder.Core.ThreadHelper<List<User>, User, List<UserAddressAgeDTO>, UserAddressAgeDTO>().PreProcess(users, returnedList, 300, this.MakeYourFuncHere);
            new ThreadSpeeder.ThreadHelper<List<User>, User, List<UserAddressAgeDTO>, UserAddressAgeDTO>().PreProcess(users, returnedList, 100, this.MakeYourFuncHere);
            return returnedList;
        }

        private UserAddressAgeDTO MakeYourFuncHere(User user)
        {
            System.Threading.Thread.Sleep(50);//Processign of any kind such as, Making API request, Making Database Transactions, etc...
            UserAddressAgeDTO dto = new UserAddressAgeDTO();
            dto.Age = user.Age;
            dto.City = user.Address.City;
            dto.IdUser = user.IdUser;
            return dto;
        }
    }
}
