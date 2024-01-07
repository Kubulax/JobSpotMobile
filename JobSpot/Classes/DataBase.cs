using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JobSpot.Classes
{
    public class DataBase
    {
        private readonly SQLiteAsyncConnection _connection;

        public DataBase(string dbpath)
        {
            _connection = new SQLiteAsyncConnection(dbpath);
            _connection.CreateTableAsync<User>().Wait();
            _connection.InsertAsync(new User("admin", String.Empty, "admin", 1));
            _connection.CreateTableAsync<UserProfile>().Wait();
            _connection.CreateTableAsync<JobAdvertisement>().Wait();
            _connection.CreateTableAsync<JobApplication>().Wait();
        }


        public Task<int> CreateUserAsync(User user)
        {
            return _connection.InsertAsync(user);
        }

        public Task<User> ReadUserByNameAsync(string nickName)
        {
            List<User> queryResult = _connection.QueryAsync<User>("SELECT * FROM User WHERE Nickname=?", nickName).Result;

            if(queryResult.Count == 0)
            {
                User user = null;
                return Task.FromResult(user);
            }
            else
            {
                return Task.FromResult(queryResult[0]);
            }
        }

        public Task<bool> VerifyUserPasswordAsync(string nickName, string password)
        {
            List<User> queryResult = _connection.QueryAsync<User>("SELECT Id FROM User WHERE Nickname=? AND Password=?", nickName, password).Result;

            if (queryResult.Count == 0)
            {
                return Task.FromResult(false);
            }
            else
            {
                return Task.FromResult(true);
            }
        }


        public Task<int> CreateUserProfileAsync(UserProfile userProfile)
        {
            return _connection.InsertAsync(userProfile);
        }

        public Task<UserProfile> ReadUserProfileByUserIdAsync(int userId)
        {
            List<UserProfile> queryResult = _connection.QueryAsync<UserProfile>("SELECT * FROM UserProfile WHERE UserId=?", userId).Result;

            if (queryResult.Count == 0)
            {
                UserProfile userProfile = null;
                return Task.FromResult(userProfile);
            }
            else
            {
                return Task.FromResult(queryResult[0]);
            }
        }

        public Task<int> UpdateUserProfileAsync(UserProfile userProfile)
        {
            return _connection.ExecuteAsync(@"
                UPDATE UserProfile 
                SET 
                    Name = ?, 
                    Surname = ?, 
                    DateOfBirth = ?,
                    PhoneNumber = ?,
                    PlaceOfResidence = ?, 
                    WorkExperience = ?, 
                    Education = ?,
                    Languages = ?,
                    Skills = ?,
                    Courses = ? 
                WHERE Id=?",
                userProfile.Name, userProfile.Surname, userProfile.DateOfBirth, userProfile.PhoneNumber, 
                userProfile.PlaceOfResidence, userProfile.WorkExperience, userProfile.Education,
                userProfile.Languages, userProfile.Skills, userProfile.Courses,
                userProfile.Id);
        }


        public Task<int> CreateJobAdvertisementAsync(JobAdvertisement jobAdvertisement)
        {
            return _connection.InsertAsync(jobAdvertisement);
        }

        public Task<List<JobAdvertisement>> ReadJobAdvertisementsAsync(List<FilterParam> filterParameters)
        {
            string commandTxt = "SELECT * FROM JobAdvertisement";
            for (int i = 0; i < filterParameters.Count; i++)
            {
                if (i == 0)
                {
                    commandTxt += " WHERE";
                }
                commandTxt += " " + filterParameters[i].ColumnName + " LIKE '%" + filterParameters[i].Value + "%'";
                if (i < filterParameters.Count - 1)
                {
                    commandTxt += " OR ";
                }
            }

            return _connection.QueryAsync<JobAdvertisement>(commandTxt);
        }

        public Task<JobAdvertisement> ReadJobAdvertisementByIdAsync(int jobAdId)
        {
            List<JobAdvertisement> queryResult = _connection.QueryAsync<JobAdvertisement>("SELECT * FROM JobAdvertisement WHERE Id=?", jobAdId).Result;

            if (queryResult.Count == 0)
            {
                JobAdvertisement jobAdvertisement = null;
                return Task.FromResult(jobAdvertisement);
            }
            else
            {
                return Task.FromResult(queryResult[0]);
            }
        }

        public Task<int> UpdateJobAdvertisementAsync(JobAdvertisement jobAdvertisement)
        {
            return _connection.ExecuteAsync(@"
                UPDATE JobAdvertisement
                SET 
                    CompanyName = ?, 
                    PositionName = ?, 
                    Category = ?,
                    Pay = ?,
                    Localization = ?, 
                    PositionLevel = ?, 
                    ContractType = ?,
                    EmploymentType = ?,
                    WorkType = ?,
                    Duties = ?,
                    Requirements = ?,
                    Benefits = ?,
                    RecrutationEnd = ?
                WHERE Id=?",
                jobAdvertisement.CompanyName, jobAdvertisement.PositionName, jobAdvertisement.Category, jobAdvertisement.Pay,
                jobAdvertisement.Localization, jobAdvertisement.PositionLevel, jobAdvertisement.ContractType, jobAdvertisement.EmploymentType,
                jobAdvertisement.WorkType, jobAdvertisement.Duties, jobAdvertisement.Requirements, jobAdvertisement.Benefits, jobAdvertisement.RecrutationEnd,
                jobAdvertisement.Id);
        }

        public Task<int> DeleteJobAdvertisementAsync(JobAdvertisement jobAdvertisement)
        {
            _connection.ExecuteAsync("DELETE FROM JobApplication WHERE JobAdId=?", jobAdvertisement.Id);
            return _connection.DeleteAsync(jobAdvertisement);

        }


        public Task<int> CreateApplicationAsync(JobApplication application)
        {
            return _connection.InsertAsync(application);
        }

        public Task<bool> CheckApplicationAsync(JobApplication application)
        {
            List<JobApplication> queryResult = _connection.QueryAsync<JobApplication>("SELECT Id FROM JobApplication WHERE JobAdId=? AND UserId=?", application.JobAdId, application.UserId).Result;

            if (queryResult.Count == 0)
            {
                return Task.FromResult(false);
            }
            else
            {
                return Task.FromResult(true);
            }
        }

        public Task<List<JobAdvertisement>> ReadApplicationsByUserIdAsync(int userId)
        {
            return _connection.QueryAsync<JobAdvertisement>("SELECT * FROM JobAdvertisement WHERE Id=(SELECT JobAdId FROM JobApplication WHERE UserId=?)", userId);
        }

        public Task<int> DeleteApplicationAsync(JobApplication application)
        {
            return _connection.ExecuteAsync("DELETE FROM JobApplication WHERE JobAdId=? AND UserId=?", application.JobAdId, application.UserId);
        }
    }
}
