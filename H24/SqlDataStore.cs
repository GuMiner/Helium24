using H24.Definitions.Maps;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using H24.Definitions;
using H24.Definitions.DataStore;
using Microsoft.Extensions.Logging;

namespace H24
{
    class SqlDataStore : INotesDataStore, ISystemStatsDataStore, IUserDataStore
    {
        private readonly ILogger logger;
        public SqlDataStore(ILogger logger)
        {
            this.logger = logger;
        }

        public Note GetNote(string id)
        {
            return PerformQuery<Note>((conn) =>
            {
                List<Poi> poi = new List<Poi>();
                NpgsqlCommand command = new NpgsqlCommand(
                    $"SELECT Id, CurrentNotes, LastNotes, LastEdit FROM Notes WHERE Id = '{id}'", conn);
                NpgsqlDataReader dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    return new Note()
                    {
                        Id = (String)dataReader[0],
                        Notes = (String)dataReader[1],
                        LastNotes = (String)dataReader[2],
                        LastEdit = (DateTime)dataReader[3]
                    };
                }

                return null;
            },
            (ex) => $"Could not read the notes from {id}: {ex.Message}");
        }

        public void SaveNote(Note note)
        {
            PerformQuery((conn) =>
            {
                NpgsqlCommand command = FormInsertCommand(conn, "Notes", new List<Tuple<string, object>>
                {
                    Tuple.Create<string, object>("Id", note.Id),
                    Tuple.Create<string, object>("CurrentNotes", note.Notes),
                    Tuple.Create<string, object>("LastNotes", note.LastNotes),
                    Tuple.Create<string, object>("LastEdit", note.LastEdit),
                });
                command.ExecuteNonQuery();
            },
            (ex) => $"Could not add note with ID {note.Id}");
        }

        public void UpdateNote(Note note)
        {
            PerformQuery((conn) =>
            {
                NpgsqlCommand command = FormUpdateCommand(conn, "Notes", new List<Tuple<string, object>>
                {
                    Tuple.Create<string, object>("CurrentNotes", note.Notes),
                    Tuple.Create<string, object>("LastNotes", note.LastNotes),
                    Tuple.Create<string, object>("LastEdit", note.LastEdit),
                }, Tuple.Create<string, string>("Id", note.Id));
                command.ExecuteNonQuery();
            },
            (ex) => $"Could not update note with ID {note.Id}");
        }

        public User GetUser(string userName)
        {
            return PerformQuery<User>((conn) =>
            {
                List<Poi> poi = new List<Poi>();
                NpgsqlCommand command = new NpgsqlCommand(
                    $"SELECT UserName, PasswordHash, Name, LastLoginDate, LoginCount FROM Users WHERE UserName = :userName", conn);
                command.Parameters.Add(new NpgsqlParameter("userName", userName));
                NpgsqlDataReader dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    return new User()
                    {
                        UserName = (String)dataReader[0],
                        PasswordHash = (String)dataReader[1],
                        Name = (String)dataReader[2],
                        LastLoginDate = (DateTime)dataReader[3],
                        LoginCount = (int)dataReader[4],
                    };
                }

                return null;
            },
            (ex) => $"Could not read the {userName} user: {ex.Message}");
        }

        public void SaveUser(User user)
        {
            PerformQuery((conn) =>
            {
                NpgsqlCommand command = FormInsertCommand(conn, "Users", new List<Tuple<string, object>>
                {
                    Tuple.Create<string, object>("UserName", user.UserName),
                    Tuple.Create<string, object>("PasswordHash", user.PasswordHash),
                    Tuple.Create<string, object>("Name", user.Name),
                    Tuple.Create<string, object>("LastLoginDate", user.LastLoginDate),
                    Tuple.Create<string, object>("LoginCount", user.LoginCount),
                });
                command.ExecuteNonQuery();
            },
            (ex) => $"Could not add user {user.UserName}");
        }

        public void UpdateUserLoginData(string userName, DateTime lastLoginDate, int loginCount)
        {
            PerformQuery((conn) =>
            {
                NpgsqlCommand command = FormUpdateCommand(conn, "Users", new List<Tuple<string, object>>
                {
                    Tuple.Create<string, object>("LastLoginDate", lastLoginDate),
                    Tuple.Create<string, object>("LoginCount", loginCount),
                }, Tuple.Create<string, string>("UserName", userName));
                command.ExecuteNonQuery();
            },
            (ex) => $"Could not update user enabled features for {userName}");
        }

        public void SaveSystemStat(SystemStat stat)
        {
            PerformQuery((conn) =>
            {
                NpgsqlCommand command = FormInsertCommand(conn, "SystemStats", new List<Tuple<string, object>>
                {
                    Tuple.Create<string, object>("Id", stat.Id),
                    Tuple.Create<string, object>("UserProcessorPercent", stat.UserProcessorPercent),
                    Tuple.Create<string, object>("DiskFreePercentage", stat.DiskFreePercentage),
                });
                command.ExecuteNonQuery();
            },
            (ex) => $"Could not add a new system stat with ID {stat.Id}: {ex.Message}");
        }

        public ICollection<SystemStat> GetSystemStats(DateTime afterDate)
        {
            return PerformQuery<ICollection<SystemStat>>((conn) =>
            {
                List<SystemStat> stats = new List<SystemStat>();
                NpgsqlCommand command = new NpgsqlCommand(
                    $"SELECT Id, UserProcessorPercent, DiskFreePercentage FROM SystemStats WHERE Id > '{afterDate:yyyy-MM-dd-hh-mm}' ORDER BY Id ASC", conn);
                NpgsqlDataReader dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    stats.Add(new SystemStat()
                    {
                        Id = (String)dataReader[0],
                        UserProcessorPercent = (float)dataReader[1],
                        DiskFreePercentage = (float)dataReader[2]
                    });
                }

                return stats;
            },
            (ex) => $"Could not read the system stats after at {afterDate}: {ex.Message}");
        }

        public string GetSqlDbStatus()
        {
            string dbStatus = "Up!";
            PerformQuery((conn) =>
            {
                NpgsqlCommand command = new NpgsqlCommand("SELECT UserName FROM Users LIMIT 1", conn);
                command.ExecuteNonQuery();
            },
            (ex) =>
            {
                dbStatus = "Down: " + ex.Message;
                return "SQL Status:" + dbStatus;
            });

            return dbStatus;
        }

        private NpgsqlCommand FormInsertCommand(NpgsqlConnection conn, string dbName, List<Tuple<string, object>> columnNamesAndParameters)
        {
            NpgsqlCommand command = new NpgsqlCommand(
                $"INSERT INTO {dbName} ({string.Join(", ", columnNamesAndParameters.Select(cap => cap.Item1))}) VALUES ({string.Join(", ", columnNamesAndParameters.Select(cap => $":{cap.Item1}"))})", conn);
            foreach (Tuple<string, object> parameter in columnNamesAndParameters)
            {
                command.Parameters.Add(new NpgsqlParameter(parameter.Item1, parameter.Item2));
            }

            return command;
        }

        private NpgsqlCommand FormUpdateCommand(NpgsqlConnection conn, string dbName, List<Tuple<string, object>> columnNamesAndParameters, Tuple<string, string> whereClause)
        {
            NpgsqlCommand command = new NpgsqlCommand(
                $"UPDATE {dbName} SET {string.Join(", ", columnNamesAndParameters.Select(cap => $"{cap.Item1} = :{cap.Item1}"))} WHERE {whereClause.Item1} = :{whereClause.Item1}", conn);
            foreach (Tuple<string, object> parameter in columnNamesAndParameters)
            {
                command.Parameters.Add(new NpgsqlParameter(parameter.Item1, parameter.Item2));
            }

            command.Parameters.Add(new NpgsqlParameter(whereClause.Item1, whereClause.Item2));
            return command;
        }

        private void PerformQuery(Action<NpgsqlConnection> queryAction, Func<Exception, string> errorMessageConverter)
        {
            PerformQuery((conn) =>
                {
                    queryAction(conn);
                    return 0;
                },
                errorMessageConverter);
        }

        private T PerformQuery<T>(Func<NpgsqlConnection, T> queryAction, Func<Exception, string> errorMessageConverter)
        {
            T result = default(T);
            using (NpgsqlConnection sqlConnection = new NpgsqlConnection(Program.UrlResolver.GetSqlConnectionString()))
            {
                sqlConnection.Open();

                try
                {
                    result = queryAction(sqlConnection);
                }
                catch (Exception ex)
                {
                    logger.LogData("SQLError", Guid.Empty.ToString(), new { Error = errorMessageConverter(ex) });
                }
                finally
                {
                    sqlConnection.Close();
                }
            }

            return result;
        }
    }
}
