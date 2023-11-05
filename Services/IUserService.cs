
public interface IUserService
{
     List<UserDto> GetUsers();
     UserDto GetUserByEmail(string email);
     void InsertUser(UserDto user);
      
}