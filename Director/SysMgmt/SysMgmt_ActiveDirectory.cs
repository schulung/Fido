/*
 *
 *  Copyright 2015 Netflix, Inc.
 *
 *     Licensed under the Apache License, Version 2.0 (the "License");
 *     you may not use this file except in compliance with the License.
 *     You may obtain a copy of the License at
 *
 *         http://www.apache.org/licenses/LICENSE-2.0
 *
 *     Unless required by applicable law or agreed to in writing, software
 *     distributed under the License is distributed on an "AS IS" BASIS,
 *     WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *     See the License for the specific language governing permissions and
 *     limitations under the License.
 *
 */

using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using Fido_Main.Fido_Support.ErrorHandling;
using Fido_Main.Fido_Support.Objects.Fido;

namespace Fido_Main.Director.SysMgmt
{
  static class SysMgmt_ActiveDirectory
  {
    public static UserReturnValues Getuserinfo(string userIdAsString)
    {
      try
      {

        var search = CreateDirectorySearcher(userIdAsString); 
        var userInfo = CreateEmptyUserReturnValues();

        var resultCol = search.FindAll();

        if ((resultCol == null) || (!resultCol.PropertiesLoaded.Any()))
        {
            return userInfo;
        }

        for (var counter = 0; counter < resultCol.Count; counter++)
        {
          var result = resultCol[counter];

          if (result.Properties.Contains("samaccountname") && result.Properties.Contains("mail") && result.Properties.Contains("displayname"))
          {
              TryInitializeUserInfoProperty("mail", ref userInfo.UserEmail, result);
              TryInitializeUserInfoProperty("mail", ref userInfo.UserEmail, result);
             
          }

          if (string.IsNullOrEmpty(lUserInfo.ManagerName)) continue;
          var lManagerValues = Getmanagerinfo(lUserInfo.ManagerName);
          for (var i = 0; i < lManagerValues.Count; i++)
          {
            if (!lManagerValues[i].Any()) continue;
            switch (i)
            {
              case 0:
                lUserInfo.ManagerMail = lManagerValues[0];
                break;
              case 1:
                lUserInfo.ManagerID = lManagerValues[1];
                break;
              case 2:
                lUserInfo.ManagerName = lManagerValues[2];
                break;
              case 3:
                lUserInfo.ManagerTitle = lManagerValues[3];
                break;
              case 4:
                lUserInfo.ManagerMobile = lManagerValues[4];
                break;
            }
          }
        }

        return lUserInfo;
      }
      catch (Exception e)
      {
        Fido_EventHandler.SendEmail("Fido Error", "Fido Failed: {0} Exception caught in Active Directory grab user info area:" + e);
      }
      return null;
    }


    private static void TryInitializeUserInfoProperty(string propertyName, ref string userInfoPropertyToInit, IEnumerable<string> foundProperties)
    {

        if (foundProperties.Properties[propertyName].Count > 0)
        {
            userInfoPropertyToInit = (String)result.Properties[propertyName].First() ?? string.Empty;

        }

    }

    private static UserReturnValues CreateEmptyUserReturnValues()
    {
        var result = new UserReturnValues()
        {
            UserEmail = string.Empty,
            UserID = string.Empty,
            Username = string.Empty,
            Department = string.Empty,
            Title = string.Empty,
            EmployeeType = string.Empty,
            CubeLocation = string.Empty,
            City = string.Empty,
            State = string.Empty,
            StreetAddress = string.Empty,
            MobileNumber = string.Empty,
            ManagerID = string.Empty,
            ManagerMail = string.Empty,
            ManagerMobile = string.Empty,
            ManagerTitle = string.Empty,
            ManagerName = string.Empty
        };
        return result;
    }

    private static DirectorySearcher CreateDirectorySearcher(string userId)
    {

        var domainPath = Object_Fido_Configs.GetAsString("fido.ldap.basedn", string.Empty);
        var user = Object_Fido_Configs.GetAsString("fido.ldap.userid", string.Empty);
        var pwd = Object_Fido_Configs.GetAsString("fido.ldap.pwd", string.Empty);
        var searchRoot = new DirectoryEntry(domainPath, user, pwd);

        var search = new DirectorySearcher(searchRoot)
        {
            Filter = "(&(objectClass=user)(objectCategory=person)(sAMAccountName=" + userId + "))"
        };
        
        ConfigureSearchProperties(search);
        
        return search;
    }


    private static void ConfigureSearchProperties(DirectorySearcher search)
    {
        search.PropertiesToLoad.Add("samaccountname");
        search.PropertiesToLoad.Add("mail");
        search.PropertiesToLoad.Add("displayname");
        search.PropertiesToLoad.Add("department");
        search.PropertiesToLoad.Add("title");
        search.PropertiesToLoad.Add("employeeType");
        search.PropertiesToLoad.Add("manager");
        search.PropertiesToLoad.Add("info");
        search.PropertiesToLoad.Add("l");
        search.PropertiesToLoad.Add("st");
        search.PropertiesToLoad.Add("streetAddress");
        search.PropertiesToLoad.Add("mobile");
    }

    private static List<string> Getmanagerinfo(string sUserDN)
    {
      try
      {
        var lManagerValues = new List<string>();
        string domainPath = Object_Fido_Configs.GetAsString("fido.ldap.basedn", string.Empty);
        string user = Object_Fido_Configs.GetAsString("fido.ldap.userid", string.Empty);
        string pwd = Object_Fido_Configs.GetAsString("fido.ldap.pwd", string.Empty);
        var searchRoot = new DirectoryEntry(domainPath, user, pwd);
        var search = new DirectorySearcher(searchRoot)
        {
          Filter = "(&(objectClass=user)(objectCategory=person)(distinguishedName=" + sUserDN + "))"
        };
        search.PropertiesToLoad.Add("mail");
        search.PropertiesToLoad.Add("samaccountname");
        search.PropertiesToLoad.Add("displayname");
        search.PropertiesToLoad.Add("title");
        search.PropertiesToLoad.Add("mobile");

        SearchResultCollection resultCol = search.FindAll();
        for (var counter = 0; counter < resultCol.Count; counter++)
        {
          //var UserNameEmailString = string.Empty;
          var result = resultCol[counter];
          if (result.Properties["mail"].Count > 0) lManagerValues.Add((String)result.Properties["mail"][0]);
          if (result.Properties["samaccountname"].Count > 0) lManagerValues.Add((String)result.Properties["samaccountname"][0]); 
          if (result.Properties["displayname"].Count > 0) lManagerValues.Add((String)result.Properties["displayname"][0]); 
          if (result.Properties["title"].Count > 0) lManagerValues.Add((String)result.Properties["title"][0]); 
          if (result.Properties["mobile"].Count > 0) lManagerValues.Add((String)result.Properties["mobile"][0]); 
          
        }
        return lManagerValues;
      }
      catch (Exception error)
      {
        Fido_EventHandler.SendEmail("Fido Error", "Fido Failed: {0} Exception caught in Active Directory grab manager info area:" + error);
      }
      return null;
    }
  }
}
