using Dapper;
using QLCCVC.Infrastructure.Domain;
using QLCCVC.Infrastructure.Extensions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLCCVC.Infrastructure
{
    public abstract class Repository<T> : IRepository<T> where T : EntityBase
    {
        /// <summary>
        /// Gets the connection string.
        /// </summary>
        protected abstract string ConnectionString { get; }
        /// <summary>
        /// Gets the table name.
        /// </summary>
        protected abstract string TableName { get; }
        /// <summary>
        /// Get Stored Pagination
        /// </summary>
        protected string TablePagination = "[dbo].[CMS_Table_Pagination]";
        /// <summary>
        /// Gets the connection.
        /// </summary>
        protected IDbConnection Connection => !string.IsNullOrEmpty(ConnectionString)
            ? new SqlConnection(ConfigurationManager.ConnectionStrings[ConnectionString].ConnectionString)
            : new SqlConnection(ConfigurationManager.ConnectionStrings["MainConnectionString"].ConnectionString);
        /// <summary>
        /// Insert method
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual long Insert(T entity)
        {
            using (IDbConnection cn = Connection)
            {
                var parameters = (object)Mapping(entity);
                cn.Open();
                return cn.Insert<long>(TableName, parameters);
            }
        }
        /// <summary>
        /// Update method
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Update(T entity)
        {
            using (IDbConnection cn = Connection)
            {
                var parameters = (object)Mapping(entity);
                cn.Open();
                cn.Update(TableName, parameters);
            }
        }
        /// <summary>
        /// Delete method
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Delete(T entity)
        {
            using (IDbConnection cn = Connection)
            {
                cn.Open();
                cn.Execute("DELETE FROM " + TableName + " WHERE Id=@Id", new { Id = entity.Id });
            }
        }
        /// <summary>
        /// Finds by ID.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public virtual T FindById(long id)
        {
            T item = default(T);

            using (IDbConnection cn = Connection)
            {
                cn.Open();
                item = cn.Query<T>("SELECT * FROM " + TableName + " WHERE Id = @Id", new { Id = id }).FirstOrDefault();
            }

            return item;
        }
        /// <summary>
        /// Find by condition
        /// </summary>
        /// <param name="query">Sql query</param>
        /// <param name="param">Parameters</param>
        /// <returns></returns>
        public virtual T FindSingle(string query, dynamic param)
        {
            T item = default(T);

            using (IDbConnection cn = Connection)
            {
                cn.Open();
                item = cn.Query<T>(query, (object)param).FirstOrDefault();
            }

            return item;
        }
        /// <summary>
        /// Select all in table
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<T> FindAll()
        {
            IEnumerable<T> items = null;

            using (IDbConnection cn = Connection)
            {
                cn.Open();
                items = cn.Query<T>("SELECT * FROM " + TableName);
            }

            return items;
        }
        /// <summary>
        /// Select list by dynamic stored with pagination
        /// </summary>
        /// <param name="orderBy"></param>
        /// <param name="fieldList"></param>
        /// <param name="filter"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRecords"></param>
        /// <returns></returns>
        public virtual IEnumerable<T> Pagination(string orderBy, string fieldList, string filter, int pageIndex, int pageSize, out int totalRecords)
        {
            IEnumerable<T> items = null;

            using (IDbConnection cn = Connection)
            {
                cn.Open();

                var parameters = new DynamicParameters();
                parameters.Add("@DataSource", TableName);
                parameters.Add("@OrderBy", orderBy);
                parameters.Add("@FieldList", !string.IsNullOrEmpty(fieldList) ? fieldList : "*");
                parameters.Add("@Filter", filter);
                parameters.Add("@PageIndex", pageIndex);
                parameters.Add("@PageSize", pageSize);
                parameters.Add("@TotalRecord", dbType: DbType.Int32, direction: ParameterDirection.Output);

                items = cn.Query<T>(TablePagination, parameters, commandType: CommandType.StoredProcedure);
                totalRecords = parameters.Get<int>("TotalRecord");
            }

            return items;
        }
        /// <summary>
        /// Mapping the object to the insert/update columns.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>The parameters with values.</returns>
        /// <remarks>In the default case, we take the object as is with no custom mapping.</remarks>
        internal virtual dynamic Mapping(T item)
        {
            item.NullSafeTrimStrings_Initial();
            return item;
        }
    }
}
