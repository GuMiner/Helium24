using H24.Definitions.Maps;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using H24.Models;
using H24.Definitions;
using Newtonsoft.Json;
using H24.Definitions.DataStore;
using Microsoft.Extensions.Logging;

namespace H24
{
    class SqlDataStore 
    :  IDocumentationDataStore, IMapsDataStore, INotesDataStore, 
      IStockDataStore, ISystemStatsDataStore, IUserDataStore
    {
        private readonly ILogger logger;
        public SqlDataStore(ILogger logger)
        {
            this.logger = logger;
        }

        public ActivePositions GetActivePositions(string userName)
        {
            return PerformQuery<ActivePositions>((conn) =>
            {
                NpgsqlCommand command = new NpgsqlCommand(
                    $"SELECT Id, ActivePositionsJson FROM ActivePositions WHERE Id = :userName", conn);
                command.Parameters.Add(new NpgsqlParameter("userName", userName));
                NpgsqlDataReader dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    return new ActivePositions()
                    {
                        Id = (String)dataReader[0],
                        Positions = JsonConvert.DeserializeObject<List<ActiveStock>>((String)dataReader[1]),
                    };
                }

                return null;
            },
            (ex) => $"Could not read the active positions for {userName}: {ex.Message}");
        }

        public void SaveActivePositions(ActivePositions activePositions)
        {
            PerformQuery((conn) =>
            {
                NpgsqlCommand command = FormInsertCommand(conn, "ActivePositions", new List<Tuple<string, object>>
                {
                    Tuple.Create<string, object>("Id", activePositions.Id),
                    Tuple.Create<string, object>("ActivePositionsJson", JsonConvert.SerializeObject(activePositions.Positions)),
                });
                command.ExecuteNonQuery();
            },
            (ex) => $"Could not save active positions with id {activePositions.Id}");
        }

        public void UpdateActivePositions(ActivePositions activePositions)
        {
            PerformQuery((conn) =>
            {
                NpgsqlCommand command = FormUpdateCommand(conn, "ActivePositions", new List<Tuple<string, object>>
                {
                    Tuple.Create<string, object>("ActivePositionsJson", JsonConvert.SerializeObject(activePositions.Positions)),
                },
                Tuple.Create<string, string>("Id", activePositions.Id));
                command.ExecuteNonQuery();
            },
                (ex) => $"Could not update active positions with id {activePositions.Id}");
        }

        public StockSettings GetStockSettings(string userName)
        {
            return PerformQuery<StockSettings>((conn) =>
            {
                NpgsqlCommand command = new NpgsqlCommand(
                    $"SELECT Id, SellStockFee, TradeCommission FROM StockSettings WHERE Id = :userName", conn);
                command.Parameters.Add(new NpgsqlParameter("userName", userName));
                NpgsqlDataReader dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    return new StockSettings()
                    {
                        Id = (String)dataReader[0],
                        SellStockFee = (float)dataReader[1],
                        TradeCommission = (float)dataReader[2]
                    };
                }

                return null;
            },
            (ex) => $"Could not read the stock settings for {userName}: {ex.Message}");
        }

        public void SaveStockSettings(StockSettings settings)
        {
            PerformQuery((conn) =>
            {
                NpgsqlCommand command = FormInsertCommand(conn, "StockSettings", new List<Tuple<string, object>>
                {
                    Tuple.Create<string, object>("Id", settings.Id),
                    Tuple.Create<string, object>("SellStockFee", settings.SellStockFee),
                    Tuple.Create<string, object>("TradeCommission", settings.TradeCommission)
                });
                command.ExecuteNonQuery();
            },
            (ex) => $"Could not add stock settings with ID {settings.Id}");
        }

        public void UpdateStockSettings(StockSettings settings)
        {
            PerformQuery((conn) =>
            {
                NpgsqlCommand command = FormUpdateCommand(conn, "StockSettings", new List<Tuple<string, object>>
                {
                    Tuple.Create<string, object>("SellStockFee", settings.SellStockFee),
                    Tuple.Create<string, object>("TradeCommission", settings.TradeCommission)
                }, Tuple.Create<string, string>("Id", settings.Id));
                command.ExecuteNonQuery();
            },
                (ex) => $"Could not update stock settings with ID {settings.Id}");
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
                    $"SELECT UserName, PasswordHash, Name, RegistrationDate, LastLoginDate, LoginCount FROM Users WHERE UserName = :userName", conn);
                command.Parameters.Add(new NpgsqlParameter("userName", userName));
                NpgsqlDataReader dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    return new User()
                    {
                        UserName = (String)dataReader[0],
                        PasswordHash = (String)dataReader[1],
                        Name = (String)dataReader[2],
                        RegistrationDate = (DateTime)dataReader[3],
                        LastLoginDate = (DateTime)dataReader[4],
                        LoginCount = (int)dataReader[5],
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
                    Tuple.Create<string, object>("RegistrationDate", user.RegistrationDate),
                    Tuple.Create<string, object>("LastLoginDate", user.LastLoginDate),
                    Tuple.Create<string, object>("LoginCount", user.LoginCount),
                    Tuple.Create<string, object>("EnabledFeaturesJson", string.Empty), // Deprecated
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

        public List<PiCachedDocument> GetDocuments()
        {
            return PerformQuery((conn) =>
            {
                List<PiCachedDocument> documents = new List<PiCachedDocument>();
                NpgsqlCommand command = new NpgsqlCommand(
                    "SELECT Title, Source, Format, Category, Association, License, IsPublicallyAvailable FROM Documentation", conn);
                NpgsqlDataReader dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    documents.Add(new PiCachedDocument()
                    {
                        Title = (String)dataReader[0],
                        Source = (String)dataReader[1],
                        Format = (String)dataReader[2],
                        Category = (String)dataReader[3],
                        Association = (String)dataReader[4],
                        License = (String)dataReader[5],
                        IsPublicallyAvailable = (Boolean)dataReader[6]
                    });
                }

                return documents;
            },
            (ex) => $"Could not update the documentation cache: {ex.Message}");
        }

        public Poi AddPoi(int typeId, string latLng)
        {
            Guid poiId = Guid.NewGuid();
            PerformQuery((conn) =>
            {
                NpgsqlCommand command = new NpgsqlCommand(
                    "INSERT INTO Poi (TypeId, PoiId, LatLng) VALUES (:typeIdToInsert, :poiIdToInsert, :latLngToInsert)", conn);
                command.Parameters.Add(new NpgsqlParameter("typeIdToInsert", typeId));
                command.Parameters.Add(new NpgsqlParameter("poiIdToInsert", poiId.ToString()));
                command.Parameters.Add(new NpgsqlParameter("latLngToInsert", latLng));
                command.ExecuteNonQuery();
            },
            (ex) => $"Could not add a new POI of type {typeId} with LatLng {latLng}: {ex.Message}");

            return new Poi()
            {
                TypeId = typeId,
                PoiId = poiId.ToString(),
                LatLng = latLng
            };
        }

        public void DeletePoi(int typeId, string poiId)
        {
            PerformQuery((conn) =>
            {
                NpgsqlCommand command = new NpgsqlCommand(
                    $"DELETE FROM Poi WHERE PoiId = '{poiId}'", conn);
                command.ExecuteNonQuery();
            },
            (ex) => $"Could not delete the POI for type {typeId}, with id {poiId}: {ex.Message}");
        }

        public List<Poi> GetPoi(int typeId)
        {
            return PerformQuery((conn) =>
            {
                List<Poi> poi = new List<Poi>();
                NpgsqlCommand command = new NpgsqlCommand(
                    $"SELECT TypeId, PoiId, LatLng FROM Poi WHERE TypeId = {typeId}", conn);
                NpgsqlDataReader dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    poi.Add(new Poi()
                    {
                        TypeId = (int)dataReader[0],
                        PoiId = (String)dataReader[1],
                        LatLng = (String)dataReader[2]
                    });
                }

                return poi;
            },
            (ex) => $"Could not read the POI for type {typeId}: {ex.Message}");
        }

        public List<PoiType> GetPoiTypes()
        {
            return PerformQuery((conn) =>
            {
                List<PoiType> types = new List<PoiType>();
                NpgsqlCommand command = new NpgsqlCommand(
                        "SELECT Idx, Name FROM PoiTypes", conn);
                NpgsqlDataReader dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    types.Add(new PoiType()
                    {
                        Idx = (int)dataReader[0],
                        Name = (String)dataReader[1]
                    });
                }

                return types;
            },
            (ex) => $"Could not read the POI types: {ex.Message}").OrderBy(type => type.Name).ToList();
        }

        public PoiType AddPoiType(string name)
        {
            return PerformQuery((conn) =>
            {
                List<PoiType> types = new List<PoiType>();
                NpgsqlCommand command = new NpgsqlCommand(
                    "INSERT INTO PoiTypes (Name) VALUES (:nameToInsert)", conn);
                command.Parameters.Add(new NpgsqlParameter("nameToInsert", name));

                command.ExecuteNonQuery();

                command = new NpgsqlCommand("SELECT Idx FROM PoiTypes WHERE Name = :nameToInsert", conn);
                command.Parameters.Add(new NpgsqlParameter("nameToInsert", name));

                NpgsqlDataReader dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    return new PoiType() { Idx = (int)dataReader[0], Name = name };
                }

                return null;
            },
            (ex) => $"Could not add a new POI type: {ex.Message} {name}");
        }

        public List<PoiLine> GetPoiLines(int typeId)
        {
            return PerformQuery((conn) =>
            {
                List<PoiLine> poiLines = new List<PoiLine>();
                NpgsqlCommand command = new NpgsqlCommand(
                        $"SELECT TypeId, PoiId, LatOne, LngOne, LatTwo, LngTwo FROM PoiLines WHERE TypeId = {typeId}", conn);
                NpgsqlDataReader dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    poiLines.Add(new PoiLine()
                    {
                        TypeId = (int)dataReader[0],
                        PoiId = (String)dataReader[1],
                        LatOne = (float)dataReader[2],
                        LngOne = (float)dataReader[3],
                        LatTwo = (float)dataReader[4],
                        LngTwo = (float)dataReader[5]
                    });
                }

                return poiLines;
            },
            (ex) => $"Could not read the POI lines for type {typeId}: {ex.Message}");
        }

        public PoiLine AddPoiLine(int typeId, Tuple<float, float> latLngOne, Tuple<float, float> latLngTwo)
        {
            Guid poiId = Guid.NewGuid();
            PerformQuery((conn) =>
            {
                NpgsqlCommand command = new NpgsqlCommand(
                    "INSERT INTO PoiLines (TypeId, PoiId, LatOne, LngOne, LatTwo, LngTwo) VALUES (:typeIdToInsert, :poiIdToInsert, :latOneToInsert, :lngOneToInsert, :latTwoToInsert, :lngTwoToInsert)", conn);
                command.Parameters.Add(new NpgsqlParameter("typeIdToInsert", typeId));
                command.Parameters.Add(new NpgsqlParameter("poiIdToInsert", poiId.ToString()));
                command.Parameters.Add(new NpgsqlParameter("latOneToInsert", latLngOne.Item1));
                command.Parameters.Add(new NpgsqlParameter("lngOneToInsert", latLngOne.Item2));
                command.Parameters.Add(new NpgsqlParameter("latTwoToInsert", latLngTwo.Item1));
                command.Parameters.Add(new NpgsqlParameter("lngTwoToInsert", latLngTwo.Item2));
                command.ExecuteNonQuery();
            },
            (ex) => $"Could not add a new POI of type {typeId} from {latLngOne} to {latLngTwo}: {ex.Message}");

            return new PoiLine()
            {
                TypeId = typeId,
                PoiId = poiId.ToString(),
                LatOne = latLngOne.Item1,
                LngOne = latLngOne.Item2,
                LatTwo = latLngTwo.Item1,
                LngTwo = latLngTwo.Item2,
            };
        }

        public void DeletePoiLine(int typeId, string poiId)
        {
            PerformQuery((conn) =>
            {
                NpgsqlCommand command = new NpgsqlCommand(
                        $"DELETE FROM PoiLines WHERE PoiId = '{poiId}'", conn);
                command.ExecuteNonQuery();
            },
            (ex) => $"Could not delete the POI line for type {typeId}, with id {poiId}: {ex.Message}");
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
