using PhoneApp.Domain.DTO;

namespace ImportDummyJsonPlugin
{
    public class UserDto : EmployeesDTO
    {
        public UserDto(string firstName, string lastName, string phone)
        {
            Name = $"{firstName} {lastName}";
            Phone = phone;
        }
    }
}