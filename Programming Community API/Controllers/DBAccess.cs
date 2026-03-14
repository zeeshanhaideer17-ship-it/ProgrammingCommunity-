using System.Data.SqlClient;

namespace Programming_Community_API.Controllers
{
    public class DBAccess
    {
        static private string Constr = "Server=DESKTOP-NK5256L\\SQLEXPRESS;Database=ProgrammingCommunityDB;Trusted_Connection=True;TrustServerCertificate=True;";
        public SqlConnection con = new SqlConnection(Constr);
        public SqlCommand cmd = null;
        public SqlDataReader sdr = null;
        public void OpenCon()
        {
            if (con.State == System.Data.ConnectionState.Closed)
            {
                con.Open();
            }
        }
        public void CloseCon()
        {
            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }
        }
        public void IUD(string query)
        {
            OpenCon();
            cmd = new SqlCommand(query, con);
            cmd.ExecuteNonQuery();
            CloseCon();
        }
        public SqlDataReader GetData(string query)
        {
            cmd = new SqlCommand(query, con);
            sdr = cmd.ExecuteReader();
            return sdr;
        }
    }
}
