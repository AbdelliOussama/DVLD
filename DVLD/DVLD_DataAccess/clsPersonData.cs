using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Linq.Expressions;

namespace DVLD_DataAccess
{
    public  class clsPersonData
    {
        public static bool GetPersonInfoByPersonID(int PersonID,ref string NationalNo,ref string FirstName,ref string SecondName,ref string ThirdName,ref string LastName
          , ref DateTime DateOfBirth,ref Byte Gendor ,ref string Address,ref string Phone,ref string Email,ref int NationalityCountryID,ref string ImagePath)
        {
            bool IsFound = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string query = "Select * from People Where PersonID = @PersonID";
                    using(SqlCommand command = new SqlCommand(query,connection))
                    {
                        command.Parameters.AddWithValue("@PersonID", PersonID);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if(reader.Read())
                            {
                                IsFound = true;
                                NationalNo = (string)reader["NationalNo"];
                                FirstName = (string)reader["FirstName"];
                                SecondName = (string)reader["SecondName"];
                                if (reader["ThirdName"] != DBNull.Value)
                                {
                                    ThirdName = (string)reader["ThirdName"];
                                }
                                else
                                {
                                    ThirdName = "";
                                }
                                LastName = (string)reader["LastName"];
                                DateOfBirth = (DateTime)reader["DateOfBirth"];
                                Address = (string)reader["Address"];
                                Phone = (string)reader["Phone"];
                                if (reader["Email"] != DBNull.Value)
                                    Email = (string)reader["Email"];
                                else
                                    Email = "";
                                NationalityCountryID = (int)reader["NationalityCountryID"];
                                if (reader["ImagePath"] != DBNull.Value)
                                    ImagePath = (string)reader["ImagePath"];
                                else
                                    ImagePath = "";

                            }
                            else
                            {
                                IsFound = false;
                            }
                        }
                    }
                }
            }
            catch(SqlException ex)
            {
                Console.WriteLine("Error : "+ex.Message);
            }
            return IsFound;
        }

        public static bool GetPersonInfoByPersonNationalNo(string NationalNo, ref int PersonID, ref string FirstName, ref string SecondName, ref string ThirdName, ref string LastName
          , ref DateTime DateOfBirth, ref Byte Gendor, ref string Address, ref string Phone, ref string Email, ref int NationalityCountryID, ref string ImagePath)
        {
            bool IsFound = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string query = "Select * from People Where NationalNo = @NationalNo";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@NationalNo", NationalNo);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                IsFound = true;
                                PersonID = (int)reader["PersonID"];
                                FirstName = (string)reader["FirstName"];
                                SecondName = (string)reader["SecondName"];
                                if (reader["ThirdName"] != DBNull.Value)
                                {
                                    ThirdName = (string)reader["ThirdName"];
                                }
                                else
                                {
                                    ThirdName = "";
                                }
                                LastName = (string)reader["LastName"];
                                DateOfBirth = (DateTime)reader["DateOfBirth"];
                                Address = (string)reader["Address"];
                                Phone = (string)reader["Phone"];
                                if (reader["Email"] != DBNull.Value)
                                    Email = (string)reader["Email"];
                                else
                                    Email = "";
                                NationalityCountryID = (int)reader["NationalityCountryID"];
                                if (reader["ImagePath"] != DBNull.Value)
                                    ImagePath = (string)reader["ImagePath"];
                                else
                                    ImagePath = "";

                            }
                            else
                            {
                                IsFound = false;
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error : " + ex.Message);
            }
            return IsFound;
        }


        public static int AddNewPerson(string NationalNo, string FirstName, string SecondName ,string ThirdName ,string LastName
            ,DateTime DateOfBirth,Byte Gendor,string Address,string Phone ,string Email,int NationalityCountryID,string ImagePath)
        {
            int PersonID = -1;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string query = @"Insert Into People(NationalNo,FirstName,SecondName,ThirdName,LastName,DateOfBirth,Gendor,Address,Phone,Email,NationalityCountryID,ImagePath)
                                Values(@NationalNo ,@FirstName,@SecondName,@ThirdName,@LastName,@DateOfBirth,@Gendor,@Address,@Phone,@Email,@NationalityCountryID,@ImagePath);
                                SELECT SCOPE_IDENTITY();";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@NationalNo", NationalNo);
                        command.Parameters.AddWithValue("@FirstName", FirstName);
                        command.Parameters.AddWithValue("@SecondName", SecondName);
                        if (ThirdName != "")
                        {
                            command.Parameters.AddWithValue("@ThirdName", ThirdName);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@ThirdName", DBNull.Value);
                        }
                        command.Parameters.AddWithValue("@LastName", LastName);
                        command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
                        command.Parameters.AddWithValue("@Gendor", Gendor);
                        command.Parameters.AddWithValue("@Address", Address);
                        command.Parameters.AddWithValue("@Phone", Phone);
                        if (Email != "")
                        {
                            command.Parameters.AddWithValue("@Email", Email);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@Email", DBNull.Value);
                        }
                        command.Parameters.AddWithValue("@NationalityCountryID", NationalityCountryID);
                        if (ImagePath != "")
                        {
                            command.Parameters.AddWithValue("@ImagePath", ImagePath);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@ImagePath", DBNull.Value);
                        }

                        object result = command.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int InsertedID))
                        {
                            PersonID = InsertedID;
                        }

                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error : "+ex.Message);
            }
            return PersonID;
                    
        }

        public static bool UpdatePerson(int  PersonID,string NationalNo,string FirstName,string SecondName,string ThirdName,string LastName,DateTime DateOfBirth,Byte Gendor
            ,string Address,string Phone,string Email,int NationalityCountryID,string ImagePath)
        {
            int RowsAffected = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string query = @"Update People 
                                    set NationalNo = @NationalNo,
                                        FirstName = @FirstName,
                                        SecondName = @SecondName,
                                        ThirdName = @ThirdName,
                                        LastName = @LastName,
                                        DateOfBirth = @DateOfBirth,
                                        Gendor = @Gendor,
                                        Address = @Address,
                                        Phone = @Phone,
                                        Email = @Email,
                                        NationalityCountryID = @NationalityCountryID,
                                        ImagePath = @ImagePath
                                        where PersonID  =@PersonID ";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@NationalNo", NationalNo);
                        command.Parameters.AddWithValue("@FirstName", FirstName);
                        command.Parameters.AddWithValue("@SecondName", SecondName);
                        if (ThirdName != "")
                        {
                            command.Parameters.AddWithValue("@ThirdName", ThirdName);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@ThirdName", DBNull.Value);
                        }
                        command.Parameters.AddWithValue("@LastName", LastName);
                        command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
                        command.Parameters.AddWithValue("@Gendor", Gendor);
                        command.Parameters.AddWithValue("@Address", Address);
                        command.Parameters.AddWithValue("@Phone", Phone);
                        if (Email != "")
                        {
                            command.Parameters.AddWithValue("@Email", Email);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@Email", DBNull.Value);
                        }
                        command.Parameters.AddWithValue("@NationalityCountryID", NationalityCountryID);
                        if (ImagePath != "")
                        {
                            command.Parameters.AddWithValue("@ImagePath", ImagePath);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@ImagePath", DBNull.Value);
                        }

                        RowsAffected = command.ExecuteNonQuery();

                    }
                }

            }   
            catch(Exception ex)
            {
                Console.WriteLine("Error : "+ex.Message);
            }
            return (RowsAffected > 0);
        }   

        public static DataTable GetAllPeople()
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string query =
                        @"SELECT People.PersonID, People.NationalNo,
                        People.FirstName, People.SecondName, People.ThirdName, People.LastName,
			            People.DateOfBirth, People.Gendor,  
				            CASE
                            WHEN People.Gendor = 0 THEN 'Male'

                            ELSE 'Female'

                            END as GendorCaption ,
			            People.Address, People.Phone, People.Email, 
                        People.NationalityCountryID, Countries.CountryName, People.ImagePath
                        FROM            People INNER JOIN
                                   Countries ON People.NationalityCountryID = Countries.CountryID
                        ORDER BY People.FirstName";

                    using (SqlCommand command =new SqlCommand(query,connection))
                    {
                        using(SqlDataReader reader = command.ExecuteReader())
                        {
                            if(reader.HasRows)
                            {
                                dt.Load(reader);
                            }
                        }
                    }

                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error : "+ex.Message );
            }
            return dt;
        }

        public static bool DeletePerson(int PersonID)
        {
            int RowsAffected = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string query = @"Delete From People where PersonID = @PersonID";
                    using(SqlCommand command =new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@PersonID",PersonID);
                        RowsAffected = command.ExecuteNonQuery();
                    }

                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error : " + ex.Message);
            }
            return (RowsAffected > 0);
        }


        public static bool IsPersonExist(int PersonID)
        {
            bool IsFound = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string query = @"Select Found = 1 from People Where PersonID = @PersonID ";
                    using(SqlCommand command = new SqlCommand(query,connection))
                    {
                        command.Parameters.AddWithValue("@PersonID", PersonID);
                        using(SqlDataReader reader = command.ExecuteReader() )
                        {
                            IsFound = reader.HasRows;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error : "+ex.Message);
            }
            return IsFound;
        }


        public static bool IsPersonExist(string NationalNo)
        {
            bool IsFound = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string query = @"Select Found = 1 from People Where NationalNo = @NationalNo ";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@NationalNo",NationalNo);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            IsFound = reader.HasRows;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error : " + ex.Message);
            }
            return IsFound;
        }
    }
}
