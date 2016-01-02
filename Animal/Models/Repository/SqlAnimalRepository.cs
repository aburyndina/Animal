using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc.Html;
using Dapper;

namespace Animal.Models.Repository
{
    public class SqlAnimalRepository : IRepositoryEx<Animal>, IRepository<Region>, IRepository<Location>, 
                                       IRepository<FellColor>, IRepository<AnimalType>
    {
        private static string _DBConnectionString =
            ConfigurationManager.ConnectionStrings["AnimalConnection"].ToString();

        #region Животные
        public void Add(Animal entity)
        {
            try
            {
                //Проверка на отсутствие такого животного 
                if (GetAll().Any(item => item == entity))
                    return;

                using (SqlConnection connection = new SqlConnection(_DBConnectionString))
                {
                    connection.Open();

                    var query = "insert into dbo.Animal(Name, TypeId, ColorId, LocationId)"
                                + "values(@Name, @TypeId, @ColorId, @LocationId) "
                                + " select scope_identity() as animalId"
                ;

                    entity.Id = connection.Query<int>(query, new {Name = entity.Name, TypeId = entity.AnimalType.Id, ColorId = entity.FellColor.Id, LocationId = entity.Location.Id }).FirstOrDefault();

                    connection.Close(); 
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Edit(Animal entity)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_DBConnectionString))
                {
                    connection.Open();

                    var query = "update dbo.Animal" +
                                  "set Name = @Name" +
                                    ", TypeId = TypeId" +
                                    ", ColorId = ColorId" +
                                    ", LocationId = LocationId" +
                                  "where id = @id";

                    connection.Execute(query, entity);//new { animal.Name, animal.AnimalType, animal.FellColor, animal.Location });

                    connection.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Delete(int id)
        {
            try
            {
                using ( SqlConnection connection = new SqlConnection(_DBConnectionString))
                {
                    connection.Open();

                    var query = "delete from dbo.Animal " +
                                  "where id = @Id";

                    connection.Execute(query, new{Id = id});

                    connection.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<Animal> FindAnimals(int? typeId, int? fellColorId, List<Region> regionList)
        {
            var animal = GetAll().Where(item => (item.AnimalType.Id == typeId || typeId == null)
                                            && (item.FellColor.Id == fellColorId || fellColorId == null));
            if(regionList.Count > 0)
                animal.Join(regionList, a=>a.Location.Region.Id, r=>r.Id, (a,r) => new {a.Name, a.Id});

            
            return animal.ToList();
        }

        public Animal GetbyId(int id)
        {
            return GetAll().SingleOrDefault(item => item.Id == id);

        }

        public IEnumerable<Animal> GetAll()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_DBConnectionString))
                {
                    connection.Open();

                    var sql = "select * from dbo.Animal a "
                              + "inner join dbo.AnimalType at on at.Id = a.TypeId "
                              + "inner join dbo.FellColor fc on fc.Id = a.ColorId "
                              + "inner join dbo.Location l on l.Id = a.LocationId "
                              + "inner join dbo.Region r on r.Id = l.RegionId";

                    var animal = connection.Query<Animal, AnimalType, FellColor, Location, Region, Animal>(sql,
                        (a, at, fc, l, r) =>
                        {
                            a.AnimalType = at;
                            a.FellColor = fc;
                            a.Location = l;
                            a.Location.Region = r;
                            return a;
                        },
                        commandType: CommandType.Text
                        );
                    
                    return animal;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Справочники
        private static IEnumerable<AnimalType> _animalTypes;
        private static IEnumerable<FellColor> _fellColors;
        private static IEnumerable<Location> _locations;
        private static IEnumerable<Region> _regions;

        IEnumerable<AnimalType> IRepository<AnimalType>.GetAll()
        {
            try
            {
                if (_animalTypes == null)
                {
                    using (SqlConnection connection = new SqlConnection(_DBConnectionString))
                    {
                        connection.Open();
                        var animalType = connection.Query<AnimalType>("select * from dbo.AnimalType").ToList();
                        connection.Close();
                        _animalTypes = animalType;
                    }
                }

                return _animalTypes;
            }
            catch (Exception)
            {
                throw;
            }
        }

        AnimalType IRepository<AnimalType>.GetbyId(int id)
        {
            try
            {
                if (_animalTypes == null)
                {
                    using (SqlConnection connection = new SqlConnection(_DBConnectionString))
                    {
                        connection.Open();
                        var animalType = connection.Query<AnimalType>("select * from dbo.AnimalType where Id = @Id", new{Id = id}).FirstOrDefault();
                        connection.Close();
                        return animalType;
                    }
                }

                return _animalTypes.FirstOrDefault(a=>a.Id == id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        IEnumerable<FellColor> IRepository<FellColor>.GetAll()
        {
            try
            {
                if (_fellColors != null)
                    return _fellColors;

                using (SqlConnection connection = new SqlConnection(_DBConnectionString))
                {
                    connection.Open();
                    var fellColor = connection.Query<FellColor>("select * from dbo.FellColor").ToList();
                    connection.Close();
                    _fellColors = fellColor;
                    return _fellColors;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        FellColor IRepository<FellColor>.GetbyId(int id)
        {
            try
            {
                if (_fellColors == null)
                {
                    using (SqlConnection connection = new SqlConnection(_DBConnectionString))
                    {
                        connection.Open();
                        var fellColor = connection.Query<FellColor>("select * from dbo.FellColor where Id = @Id", new { Id = id }).FirstOrDefault();
                        connection.Close();
                        return fellColor;
                    }
                }

                return _fellColors.FirstOrDefault(a => a.Id == id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        IEnumerable<Location> IRepository<Location>.GetAll()
        {
            try
            {
                if (_locations != null)
                    return _locations;

                using (SqlConnection connection = new SqlConnection(_DBConnectionString))
                {
                    connection.Open();
                    var location = connection.Query<Location, Region, Location>("select * from dbo.Location l inner join dbo.Region r on r.Id = l.RegionId"
                                                                                , (l, r) =>
                                                                                {
                                                                                    l.Region = r;
                                                                                    return l;
                                                                                },
                                                                                  commandType: CommandType.Text
                                                                                );
                    connection.Close();
                    _locations = location;
                    return location;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        Location IRepository<Location>.GetbyId(int id)
        {
            try
            {
                if (_locations == null)
                {
                    using (SqlConnection connection = new SqlConnection(_DBConnectionString))
                    {
                        connection.Open();
                        var location = connection.Query<Location, Region, Location>("select * from dbo.Location l inner join dbo.Region r on r.Id = l.RegionId where l.Id = @Id"
                                                                                    , (l, r) =>
                                                                                    {
                                                                                        l.Region = r;
                                                                                        return l;
                                                                                    }
                                                                                    , new { Id = id }
                                                                                    ).FirstOrDefault();
                                                                                   
                        connection.Close();
                        return location;
                    }
                }

                return _locations.FirstOrDefault(a => a.Id == id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        IEnumerable<Region> IRepository<Region>.GetAll()
        {
            try
            {
                if (_regions != null)
                    return _regions;

                using (SqlConnection connection = new SqlConnection(_DBConnectionString))
                {
                    connection.Open();
                    var region = connection.Query<Region>("select * from dbo.Region").ToList();
                    connection.Close();
                    _regions = region;
                    return region;
                }
                
            }
            catch (Exception)
            {
                throw;
            }
        }

        Region IRepository<Region>.GetbyId(int id)
        {
            try
            {
                if (_regions == null)
                {
                    using (SqlConnection connection = new SqlConnection(_DBConnectionString))
                    {
                        connection.Open();
                        var region = connection.Query<Region>("select * from dbo.Region where Id = @Id", new { Id = id }).FirstOrDefault();
                        connection.Close();
                        return region;
                    }
                }

                return _regions.FirstOrDefault(a => a.Id == id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        
        #endregion
    }
}