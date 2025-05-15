﻿using System.Security.Principal;

namespace IRI.Sta.Common.Abstrations;

public interface ISecurityRepository
{
    void AddUser(string userName, string passwordHash);
    void DeleteUser(string userName);
    bool Authenticate(string userName, string passwordHash);
    GenericPrincipal GetPrincipal(string userName, string passwordHash);
    void Grant(string userName, string role);
    void UpdateUser(string oldUserName, string oldPasswordHash, string newUserName, string newPasswordHash);
    bool UserExists(string userName);
}
