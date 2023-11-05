
using MongoDB.Driver;

public class UserService : IUserService
{
    private readonly MongoDbDatabase _db;

    public UserService(MongoDbDatabase db)
    {
      _db = db;        
    }

    public UserDto GetUserByEmail(string email)
    {
        return  _db._users.Find(x=>x.Email==email).FirstOrDefault();
    }

    public List<UserDto> GetUsers()
    {
        return _db._users.Find(x=>true).ToList();
    }

    public void InsertUser(UserDto user)
    {
       _db._users.InsertOne(user);
    }
}