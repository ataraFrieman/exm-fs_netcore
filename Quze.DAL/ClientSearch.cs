using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using Quze.Infrastruture.Extensions;
using Quze.Models.Entities;
using System;
using System.Collections.Concurrent;
using System.Data;
using System.Threading.Tasks;

namespace Quze.DAL
{


    public enum SearchResultType
    {
        ServicePriveder,
        Fellow,
        ServiceType,
        Organization
    }

    /// <summary>
    /// helper class to ClientSearch to send to client object type and its id
    /// </summary>
    public class GlobalSearchResultRecord
    {
        public SearchResultType SearchResultType { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
    }

    /// <summary>
    /// this class is to get database objects by client search string
    /// </summary>
    public class GlobalSearch : IDisposable
    {
        private class QuzeSearchContext : DbContext
        {
            public QuzeSearchContext(DbContextOptions options) : base(options)
            {

            }


            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                base.OnModelCreating(modelBuilder);
            }
            public DbSet<ServiceType> ServiceTypes { get; set; }
            public DbSet<Fellow> Fellows { get; set; }
            public DbSet<ServiceProvider> ServiceProviders { get; set; }
        }

        string term;
        int? organizationId;
        string connectionString;

        public ConcurrentBag<GlobalSearchResultRecord> Results { get; } = new ConcurrentBag<GlobalSearchResultRecord>();

        public GlobalSearch(string connString)
        {
            connectionString = connString;
        }

        public async Task<bool> Search(int? organizationId, string term)
        {
            this.organizationId = organizationId;
            this.term = term.Trim().ToUpper();

            Task organizationOrfellowsTask;
            var providersTask = Task.Factory.StartNew(SearchServiceProviders);
            if (organizationId.IsNotNull())
                organizationOrfellowsTask = Task.Factory.StartNew(SearchFellows);
            else
                organizationOrfellowsTask = Task.Factory.StartNew(SearchOrganizations);
            var typesTask = Task.Factory.StartNew(SearchServiceTypes);

            Task.WaitAll(
                providersTask,
                organizationOrfellowsTask,
                typesTask
                );
            return await Task.FromResult(true);
        }

        public async Task<bool> OrganizationsSearch(string term)
        {
            
            this.term = term.Trim().ToUpper();
            Task organizationsTask;
            organizationsTask = Task.Factory.StartNew(SearchOrganizations);
            Task.WaitAll(organizationsTask);
            //SearchOrganizations();
            return await Task.FromResult(true);
        }

        public async Task<bool> FellowsSearch(string term,int? organizationId)
        {
            this.organizationId = organizationId;
            this.term = term.Trim().ToUpper();
            Task fellowsTask;
            fellowsTask = Task.Factory.StartNew(SearchFellows);
            Task.WaitAll(fellowsTask);
            //SearchOrganizations();
            return await Task.FromResult(true);
        }

        public async Task<bool> ServiceTypesSearch(string term)
        {

            this.term = term.Trim().ToUpper();
            var typesTask = Task.Factory.StartNew(SearchServiceTypes);
            Task.WaitAll(typesTask);
            //SearchOrganizations();
            return await Task.FromResult(true);
        }

        public async Task<bool> ServiceProvidersSearch(string term)
        {

            this.term = term.Trim().ToUpper();
            var providersTask = Task.Factory.StartNew(SearchServiceProviders);

            //SearchOrganizations();
            Task.WaitAll(providersTask);
            return await Task.FromResult(true);
        }

        public void SearchOrganizations()
        {
            try
            {
                var commandText = "GetOrganizationsByTerm";
                using (var connection = new MySqlConnection(connectionString))
                {
                    using (var command = new MySqlCommand(commandText, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        using (var adapter = new MySqlDataAdapter())
                        {

                            command.Parameters.Add(new MySqlParameter()
                            {
                                Direction = ParameterDirection.Input,
                                ParameterName = "term",
                                MySqlDbType = MySqlDbType.VarChar,
                                Value = term
                            });

                            adapter.SelectCommand = command;
                            var ds = new DataSet();
                            adapter.Fill(ds);

                            foreach (DataRow item in ds.Tables[0].Rows)
                            {
                                var record = new GlobalSearchResultRecord()
                                {

                                    SearchResultType = SearchResultType.Organization,
                                    Id = item["id"].ToString(),
                                    Name = item["Name"].ToString()
                                };
                                Results.Add(record);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public void SearchServiceTypes()
        {

            try
            {
                var commandText = "GetServiceTypesByTerm";
                using (var connection = new MySqlConnection(connectionString))
                {
                    using (var command = new MySqlCommand(commandText, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        using (var adapter = new MySqlDataAdapter())
                        {
                            command.Parameters.Add(new MySqlParameter()
                            {
                                Direction = ParameterDirection.Input,
                                ParameterName = "organizationId",
                                Value = organizationId
                            });

                            command.Parameters.Add(new MySqlParameter()
                            {
                                Direction = ParameterDirection.Input,
                                ParameterName = "term",
                                MySqlDbType = MySqlDbType.VarChar,
                                Value = term
                            });

                            adapter.SelectCommand = command;
                            var ds = new DataSet();
                            adapter.Fill(ds);

                            foreach (DataRow item in ds.Tables[0].Rows)
                            {
                                var record = new GlobalSearchResultRecord()
                                {

                                    SearchResultType = SearchResultType.ServiceType,
                                    Id = item["id"].ToString(),
                                    Name = item["Description"].ToString()
                                };
                                Results.Add(record);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public void SearchServiceProviders()
        {

            try
            {
                var commandText = "GetServiceProvidersByTerm";
                using (var connection = new MySqlConnection(connectionString))
                {
                    using (var command = new MySqlCommand(commandText, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        using (var adapter = new MySqlDataAdapter())
                        {
                            command.Parameters.Add(new MySqlParameter()
                            {
                                Direction = ParameterDirection.Input,
                                ParameterName = "organizationId",
                                Value = organizationId
                            });

                            command.Parameters.Add(new MySqlParameter()
                            {
                                Direction = ParameterDirection.Input,
                                ParameterName = "term",
                                MySqlDbType = MySqlDbType.VarChar,
                                Value = term
                            });

                            adapter.SelectCommand = command;
                            var ds = new DataSet();
                            adapter.Fill(ds);

                            foreach (DataRow item in ds.Tables[0].Rows)
                            {
                                var record = new GlobalSearchResultRecord()
                                {
                                    SearchResultType = SearchResultType.ServicePriveder,
                                    Id = item["Id"].ToString(),
                                    Name = string.Format("{0} {1} {2} - {3}", item["Title"], item["FirstName"], item["LastName"], item["Role"])
                                };
                                if (record.Name.EndsWith("- "))
                                    record.Name = record.Name.Substring(0, record.Name.Length - 3);
                                Results.Add(record);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void SearchFellows()
        {
            try
            {
                Console.WriteLine("FEllow " + DateTime.Now);
                var commandText = "GetFellowsByTerm";

                using (var connection = new MySqlConnection(connectionString))
                {
                    using (var command = new MySqlCommand(commandText, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        using (var adapter = new MySqlDataAdapter())
                        {
                            command.Parameters.Add(new MySqlParameter()
                            {
                                Direction = ParameterDirection.Input,
                                ParameterName = "organizationId",
                                Value = organizationId
                            });

                            command.Parameters.Add(new MySqlParameter()
                            {
                                Direction = ParameterDirection.Input,
                                ParameterName = "term",
                                MySqlDbType = MySqlDbType.VarChar,
                                Value = term
                            });

                            adapter.SelectCommand = command;
                            var ds = new DataSet();
                            adapter.Fill(ds);

                            foreach (DataRow item in ds.Tables[0].Rows)
                            {
                                var record = new GlobalSearchResultRecord()
                                {
                                    SearchResultType = SearchResultType.Fellow,
                                    Id = item["Id"].ToString(),
                                    Name = string.Format("{0} {1} {2} ({3})", item["Title"], item["FirstName"], item["LastName"], item["IdentityNumber"])
                                };
                                Results.Add(record);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public void Dispose()
        {

        }
    }
}

