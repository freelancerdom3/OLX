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
        public int GetOtp(int userId, out string message)
        {
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
            string query = "SELECT lu.LoginOtp FROM Users u JOIN LoginUser lu ON u.userId = lu.userIdByLoginUser WHERE lu.LoginUserId = @id and lu.ExpirationLoginTime>GETDATE()";

            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@id", userId);
            connection.Open();
            int count = (int)(cmd.ExecuteScalar() ?? 0); // Use null coalescing to handle null results
            connection.Close();
            if (count == 0)
            {
                message = "Time Expired";
                return 0;
            }
            else
            {
                message = string.Empty;

                return count;
            }
            // Set the out parameter here

        }
        public int getidfromOtp(int userotp, out string msg)
        {


            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
            string query = "SELECT LoginUserId FROM LoginUser where LoginOtp=@otp";

            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@otp", userotp);
            connection.Open();
            object c = cmd.ExecuteScalar();
            connection.Close();
            if (c == null)
            {
                msg = "Otp not matched";
                return 0;

            }

            msg = null;
            //else { msg = "user found"; return (int)c; }
            return (int)c;
        }
        public bool MobileNumberExists(string MobileNo)
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                connection.Open();

                string query = "SELECT COUNT(*) FROM Users WHERE MobileNo = @mobile";
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@mobile", MobileNo);

                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
        }
        public bool InsertOtp(int userid, int otp, DateTime expiretime)
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                connection.Open();
                string q = "insert into LoginUser(userIdByLoginUser,LoginOtp,ExpirationLoginTime)values(@id,@otp,@expiretime)";
                using (SqlCommand cmd = new SqlCommand(q, connection))
                {
                    cmd.Parameters.AddWithValue("@id", userid);
                    cmd.Parameters.AddWithValue("@otp", otp);
                    cmd.Parameters.AddWithValue("@expiretime", expiretime);
                    int rows = cmd.ExecuteNonQuery();
                    return rows > 0;
                }
            }
        }
        public int GetUserIdByMobileNumber(string mobileNumber)
        {

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                connection.Open();

                string query = "SELECT userId FROM Users WHERE MobileNo = @mobile";
                SqlCommand cmd = new SqlCommand(query, connection);

                cmd.Parameters.AddWithValue("@mobile", mobileNumber);

                object result = cmd.ExecuteScalar();
                if (result != null)
                {
                    return (int)result;
                }
                else
                {
                    return 0;
                }

            }
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

        public IEnumerable<AdvertiseListModel> GetAllAdvertiseList()
        {
            connection();
            List<AdvertiseListModel> lstadv = new List<AdvertiseListModel>();
            SqlCommand cmd = new SqlCommand("spViewtbl_MyAdvertise", con);
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            SqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                AdvertiseListModel product = new AdvertiseListModel();
                product.advertiseId = Convert.ToInt32(rdr["advertiseId"]);
                product.productSubCategoryId = Convert.ToInt32(rdr["productSubCategoryId"]);
                product.advertiseTitle = rdr["advertiseTitle"].ToString();
                product.advertiseDescription = rdr["advertiseDescription"].ToString();
                product.advertisePrice = Convert.ToInt32(rdr["advertisePrice"]);
                product.areaId = Convert.ToInt32(rdr["areaId"]);
                product.advertiseStatus = Convert.ToBoolean(rdr["advertiseStatus"]);
                product.userId = Convert.ToInt32(rdr["userId"]);
                product.createdOn = Convert.ToDateTime(rdr["createdOn"]);
                product.updatedOn = Convert.ToDateTime(rdr["updatedOn"]);
                product.advertiseapproved = Convert.ToBoolean(rdr["advertiseapproved"]);
                lstadv.Add(product);
            }
            con.Close();
            return lstadv;
        }

        public void AddAdvertiseList(AdvertiseListModel product)
        {
            connection();
            SqlCommand cmd = new SqlCommand("spInserttbl_MyAdvertise", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@advertiseId", product.advertiseId);
            cmd.Parameters.AddWithValue("@productSubCategoryId", product.productSubCategoryId);
            cmd.Parameters.AddWithValue("@advertiseTitle", product.advertiseTitle);
            cmd.Parameters.AddWithValue("@advertiseDescription", product.advertiseDescription);
            cmd.Parameters.AddWithValue("@advertisePrice", product.advertisePrice);
            cmd.Parameters.AddWithValue("@areaId", product.areaId);
            cmd.Parameters.AddWithValue("@userId", product.userId);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public void UpdateAdvertiseList(AdvertiseListModel product)
        {
            connection();
            SqlCommand cmd = new SqlCommand("spUpdatetbl_MyAdvertise", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@advertiseId", product.advertiseId);
            cmd.Parameters.AddWithValue("@productSubCategoryId", product.productSubCategoryId);
            cmd.Parameters.AddWithValue("@advertiseTitle", product.advertiseTitle);
            cmd.Parameters.AddWithValue("@advertiseDescription", product.advertiseDescription);
            cmd.Parameters.AddWithValue("@advertisePrice", product.advertisePrice);
            cmd.Parameters.AddWithValue("@addstatus", product.advertiseStatus);
            cmd.Parameters.AddWithValue("@areaId", product.areaId);
            cmd.Parameters.AddWithValue("@userId", product.userId);
            cmd.Parameters.AddWithValue("@advapproved", product.advertiseapproved);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }
        public AdvertiseListModel GetAdvertiseList(int? advertiseId)
        {
            connection();
            AdvertiseListModel product = new AdvertiseListModel();
            string sqlQuery = "SELECT * FROM tbl_MyAdvertise WHERE advertiseId= " + advertiseId;
            SqlCommand cmd = new SqlCommand(sqlQuery, con);
            con.Open();
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                product.advertiseId = Convert.ToInt32(rdr["advertiseId"]);
                product.productSubCategoryId = Convert.ToInt32(rdr["productSubCategoryId"]);
                product.advertiseTitle = rdr["advertiseTitle"].ToString();
                product.advertiseDescription = rdr["advertiseDescription"].ToString();
                product.advertisePrice = Convert.ToInt32(rdr["advertisePrice"]);
                product.areaId = Convert.ToInt32(rdr["areaId"]);
                product.advertiseStatus = Convert.ToBoolean(rdr["advertiseStatus"]);
                product.userId = Convert.ToInt32(rdr["userId"]);
                product.createdOn = Convert.ToDateTime(rdr["createdOn"]);
                product.updatedOn = Convert.ToDateTime(rdr["updatedOn"]);
                product.advertiseapproved = Convert.ToBoolean(rdr["advertiseapproved"]);

            }
            return product;
        }

        public void DeleteAdvertiseList(int? advertiseId)
        {
            connection();
            SqlCommand cmd = new SqlCommand("spDeletetbl_MyAdvertise", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@advertiseId", advertiseId);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }


        public bool IsProductAvailable(int advertiseId)
        {
            connection();
            SqlCommand com = new SqlCommand("SELECT advertiseStatus FROM tbl_MyAdvertise WHERE advertiseId = @AdvertiseId", con);
            com.Parameters.AddWithValue("@AdvertiseId", advertiseId);

            con.Open();
            SqlDataReader sqlDataReader = com.ExecuteReader();

            bool result = false;  // Initialize the result variable as false

            if (sqlDataReader.Read())  // Check if there is a row to read
            {
                result = !sqlDataReader.GetBoolean(0); // Invert the result: true for 0, false for 1
            }

            con.Close();

            return result;
        }


        public bool AddingMoneyToBuyerWallet(int userId, int amountToAdd)
        {
            connection();
            int paymentIdB = 0;
            SqlCommand getpaymentidB = new SqlCommand("select paymentIdB from PaymentdetailsBuyer where userId = @userId", con);
            getpaymentidB.Parameters.AddWithValue("@userId", userId);
            con.Open();

            SqlDataReader reader = getpaymentidB.ExecuteReader();

            if (reader.Read())
            {
                paymentIdB = reader.GetInt32(0);
                reader.Close();

                if (paymentIdB > 0)
                {
                    // User exists, update Buyerwallet
                    SqlCommand updateBuyerwallet = new SqlCommand("UPDATE PaymentdetailsBuyer SET Buyerwallet = Buyerwallet + @amountToAdd WHERE userId = @userId", con);
                    updateBuyerwallet.Parameters.AddWithValue("@amountToAdd", amountToAdd);
                    updateBuyerwallet.Parameters.AddWithValue("@userId", userId);

                    int rowsAffected = updateBuyerwallet.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        con.Close();
                        return true;
                    }
                }
            }
            else
            {
                reader.Close(); // Close the DataReader before proceeding

                // User does not exist, insert a new record
                SqlCommand insertUserIdWallet = new SqlCommand("INSERT INTO PaymentdetailsBuyer (userId, Buyerwallet) VALUES (@userId, @Buyerwallet)", con);
                insertUserIdWallet.Parameters.AddWithValue("@userId", userId);
                insertUserIdWallet.Parameters.AddWithValue("@Buyerwallet", amountToAdd);

                int rowsAffected = insertUserIdWallet.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    con.Close();
                    return true;
                }
            }
            con.Close();
            return false;
        }







        public bool PurchaseProduct(int userId, int advertiseId)
        {
            connection();

            // Check if the product is available
            bool isAvailable = IsProductAvailable(advertiseId);

            if (!isAvailable)
            {
                // Product is not available
                return false;
            }

            // Get the advertise price for the given advertiseId
            decimal advertisePrice = 0;
            SqlCommand getPriceCommand = new SqlCommand("SELECT advertisePrice FROM tbl_MyAdvertise WHERE advertiseId = @AdvertiseId", con);
            getPriceCommand.Parameters.AddWithValue("@AdvertiseId", advertiseId);

            con.Open();
            SqlDataReader priceReader = getPriceCommand.ExecuteReader();
            if (priceReader.Read())
            {
                advertisePrice = priceReader.GetDecimal(0);
            }
            con.Close();

            // Check if the user has a sufficient balance
            SqlCommand checkBalanceCommand = new SqlCommand("SELECT Buyerwallet FROM PaymentdetailsBuyer WHERE userId = @UserId", con);
            checkBalanceCommand.Parameters.AddWithValue("@UserId", userId);

            con.Open();
            SqlDataReader balanceReader = checkBalanceCommand.ExecuteReader();
            decimal buyerWallet = 0;
            if (balanceReader.Read())
            {
                buyerWallet = balanceReader.GetDecimal(0);
            }
            con.Close();

            if (buyerWallet >= advertisePrice)
            {
                // User has sufficient balance, proceed with the purchase
                decimal totalAmountPaid = advertisePrice;

                // Update the TotalamountPaid for the user and advertiseId
                SqlCommand updateTotalAmountCommand = new SqlCommand("UPDATE PaymentdetailsBuyer SET TotalamountPaid = @TotalAmountPaid, advertiseId = @AdvertiseId WHERE userId = @UserId", con);
                updateTotalAmountCommand.Parameters.AddWithValue("@TotalAmountPaid", totalAmountPaid);
                updateTotalAmountCommand.Parameters.AddWithValue("@UserId", userId);
                updateTotalAmountCommand.Parameters.AddWithValue("@AdvertiseId", advertiseId);

                con.Open();
                int rowsAffected = updateTotalAmountCommand.ExecuteNonQuery();
                con.Close();

                if (rowsAffected > 0)
                {
                    // Successfully purchased the product, you can update the product status or perform other actions here
                    return true;
                }
            }

            // Insufficient balance or update failed
            return false;
        }

    }
}