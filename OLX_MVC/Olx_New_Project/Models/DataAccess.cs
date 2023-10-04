using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Olx_New_Project.Models
{
    public class DataAccess
    {
        private SqlConnection con;

        //To Handle connection related activities    
        private void connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
            con = new SqlConnection(constr);

        }


        public bool IsAdmin(string userEmail)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
            string query = "select roles from users where userEmail=@userEmail and Roles='Admin'";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@userEmail",userEmail);
            conn.Open();
            string role = (string)cmd.ExecuteScalar();
            conn.Close();
            return role == "Admin";
        }

        public bool authLogin(UserModel model, out string validationmsg)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
            string q = "select count(*) from Users where userEmail=@userEmail";
            SqlCommand cmd = new SqlCommand(q, conn);
            cmd.Parameters.AddWithValue("@userEmail", model.userEmail);

            conn.Open();
            int count = (int)cmd.ExecuteScalar();
            conn.Close();
            if (count == 0)
            {
                validationmsg = "UserEmail doesn't Exist";
                return false;
            }




            string query = "select count(*) from Users where userEmail=@userEmail and userPassword=@userPassword";
            cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@userEmail", model.userEmail);
            cmd.Parameters.AddWithValue("@userPassword", model.userPassword);
            conn.Open();
            count = (int)cmd.ExecuteScalar();
            conn.Close();
            validationmsg = count == 0 ? "invalid credintials" : string.Empty;
            if (count > 0)
            {
                bool isadmin = IsAdmin(model.userEmail);
                if (isadmin)
                {
                    return true;
                }
            }
            return count > 0;
            //if (rows == 0)
            //{  


            //}
            ////return false;
            //validationmsg = "Invalid Credentials ";
            //return rows>0 ;
            //SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            //DataTable dataTable = new DataTable();

        }

        public IEnumerable<ProductSubCategoryModel> GetProductDetailsLists()
        {
            connection();
            List<ProductSubCategoryModel> lstProject = new List<ProductSubCategoryModel>();
            // string query = "select * from tbl_ProductDetails ";
            SqlDataAdapter da = new SqlDataAdapter("spGetAllProductSubCategory", con);
            DataTable dt = new DataTable();
            con.Open();
            da.Fill(dt);
            con.Close();
            foreach (DataRow rdr in dt.Rows)
            {

                lstProject.Add(

                    new ProductSubCategoryModel
                    {
                        productSubCategoryId = Convert.ToInt32(rdr["productSubCategoryId"]),
                        productCategoryId = Convert.ToInt32(rdr["productCategoryId"]),
                        productSubCategoryName = rdr["productSubCategoryName"].ToString(),
                        createdOn = Convert.ToDateTime(rdr["createdOn"]),
                        updatedOn = Convert.ToDateTime(rdr["updatedOn"]),
                    }
                );
            }
            return lstProject;
        }


        public void AddProductDetails(ProductSubCategoryModel productDetails)
        {
            connection();

            SqlCommand cmd = new SqlCommand("AddNewProductDetails", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@productCategoryId", productDetails.productCategoryId);
            cmd.Parameters.AddWithValue("@productSubCategoryName", productDetails.productSubCategoryName);
            //cmd.Parameters.AddWithValue("@createdOn","getdate()");
            //cmd.Parameters.AddWithValue("@updatedOn", "getdate()");
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();

        }

        public void UpdateProductDetails(ProductSubCategoryModel productDetails)
        {
            connection();

            SqlCommand cmd = new SqlCommand("spUpdate", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@productSubCategoryId", productDetails.productSubCategoryId);
            cmd.Parameters.AddWithValue("@productCategoryId", productDetails.productCategoryId);
            cmd.Parameters.AddWithValue("@productSubCategoryName", productDetails.productSubCategoryName);
            //cmd.Parameters.AddWithValue("@createdOn", "getdate()");
            //cmd.Parameters.AddWithValue("@updatedOn", "getdate()");
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();

        }

        public ProductSubCategoryModel GetProductDetails(int? productSubCategoryId)
        {
            connection();
            ProductSubCategoryModel ul = new ProductSubCategoryModel();
            string sqlQuery = "SELECT * FROM tbl_ProductSubCategory WHERE productSubCategoryId= " + productSubCategoryId;
            SqlCommand cmd = new SqlCommand(sqlQuery, con);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                ul.productSubCategoryId = Convert.ToInt32(dr["productSubCategoryId"]);
                ul.productCategoryId = Convert.ToInt32(dr["productCategoryId"]);
                ul.productSubCategoryName = Convert.ToString(dr["productSubCategoryName"]);
                ul.createdOn = Convert.ToDateTime(dr["createdOn"]);
                ul.updatedOn = Convert.ToDateTime(dr["updatedOn"]);
            }
            return ul;
        }

        public void DeleteProductDetails(int? productSubCategoryId)
        {
            connection();

            SqlCommand cmd = new SqlCommand("SpDeleteProductDetails", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@productSubCategoryId", productSubCategoryId);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();

        }

       


            public IEnumerable<UserList> GetAllUser()
            {
                connection();
                List<UserList> lstUser = new List<UserList>();
                SqlCommand cmd = new SqlCommand("GetUser", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    UserList ul = new UserList();
                    ul.UserId = Convert.ToInt32(dr["userId"]);
                    ul.firstName = Convert.ToString(dr["firstName"]);
                    ul.lastName = Convert.ToString(dr["lastName"]);
                    ul.userEmail = Convert.ToString(dr["userEmail"]);
                    ul.Password = Convert.ToString(dr["Password"]);
                    ul.MobileNo = Convert.ToString(dr["MobileNo"]);
                    ul.Gender = Convert.ToString(dr["Gender"]);
                    ul.Address = Convert.ToString(dr["Address"]);
                    ul.City = Convert.ToString(dr["City"]);
                    //createupdate

                    lstUser.Add(ul);
                }
                con.Close();

                return lstUser;
            }

            public void Updateuser(UserList ul)
            {
                connection();

                SqlCommand cmd = new SqlCommand("updateRecord", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@userId", ul.UserId);
                cmd.Parameters.AddWithValue("@firstName", ul.firstName);
                cmd.Parameters.AddWithValue("@lastName", ul.lastName);
                cmd.Parameters.AddWithValue("@userEmail", ul.userEmail);
                cmd.Parameters.AddWithValue("@Password", ul.Password);
                cmd.Parameters.AddWithValue("@MobileNo", ul.MobileNo);
                cmd.Parameters.AddWithValue("@Gender", ul.Gender);
                cmd.Parameters.AddWithValue("@Address", ul.Address);
                cmd.Parameters.AddWithValue("@City", ul.City);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

            }
            public void DeleteUser(int? userId)
            {
                connection();

                SqlCommand cmd = new SqlCommand("deleteRecord", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@userId", userId);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

            }
            public UserList GetUserData(int? userId)
            {
                connection();
                UserList ul = new UserList();
                string sqlQuery = "SELECT * FROM users WHERE userId= " + userId;
                SqlCommand cmd = new SqlCommand(sqlQuery, con);
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    ul.UserId = Convert.ToInt32(dr["userId"]);
                    ul.firstName = Convert.ToString(dr["firstName"]);
                    ul.lastName = Convert.ToString(dr["lastName"]);
                    ul.userEmail = Convert.ToString(dr["userEmail"]);
                    ul.Password = Convert.ToString(dr["Password"]);
                    ul.MobileNo = Convert.ToString(dr["MobileNo"]);
                    ul.Gender = Convert.ToString(dr["Gender"]);
                    ul.Address = Convert.ToString(dr["Address"]);
                    ul.City = Convert.ToString(dr["City"]);
                }
                return ul;
            }
        }
}