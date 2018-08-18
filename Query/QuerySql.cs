
namespace Legendary.Query
{
    public abstract class QuerySql
    {
        public Sql Sql;
        public static string connectionString;

        public QuerySql()
        {
            Sql = new Sql(connectionString);
        }

    }
}
