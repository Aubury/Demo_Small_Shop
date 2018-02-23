using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = @"Data Source = .\SQLEXPRESS;
                                        Initial Catalog = SMALL_SHOP;
                                        Integrated Security = True";

            //String connectionString = ConfigurationManager.ConnectionStrings["DefaulConnection"].ConnectionString;
            //Console.WriteLine(connectionString);
            //===============================================================================================================

            //Создание подключения
            SqlConnection connection = new SqlConnection(connectionString);


            //try
            //{
            //    //открываем подключение
            //    connection.Open();
            //    Console.WriteLine("Подключение открыто");

            //}
            //catch (SqlException ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}
            //finally
            //{
            //    //закрываем подключение
            //    connection.Close();
            //    Console.WriteLine("Подключение закрыто...");
            //}
            //Console.Read();

            //=============================================================================================================

            using (connection = new SqlConnection(connectionString))
            {
                connection.Open();
                Console.WriteLine("Подключение открыто");

                Console.WriteLine("Свойства подключения :");
                Console.WriteLine("Строка подключения :  {0}", connection.ConnectionString);
                Console.WriteLine("База данных : {0}", connection.Database);
                Console.WriteLine("Сервер : {0}", connection.DataSource);
                Console.WriteLine("Версия сервера : {0}", connection.ServerVersion);
                Console.WriteLine("Состояние : {0}", connection.State);
                Console.WriteLine("Workstationld : {0}", connection.WorkstationId);

            }
            Console.Read();

            //=============================================================================================================
            Console.WriteLine("======================================================");

            SqlDataReader rdr = null;
            string sqlExpression = "Select * from RECEIPT_DETAILS";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand(sqlExpression, conn);
                rdr = command.ExecuteReader();
                if (rdr.HasRows) //если есть данные
                {
                    //выводим название столцов 
                    Console.WriteLine("{0}\t{1}\t{2}\t\t{3}\t\t{4}\t\t{5}", rdr.GetName(0), rdr.GetName(1), rdr.GetName(2), rdr.GetName(3), rdr.GetName(4), rdr.GetName(5));

                    while (rdr.Read())//построчно считываем данные
                    {
                        object RECEIPT_ID = rdr.GetInt32(0);
                        object PRODUCTS_ID = rdr.GetInt32(1);
                        object PRODUCT_NAME = rdr.GetString(2);
                        object AMOUNT = rdr.GetValue(3);
                        object PRICE = rdr.GetDecimal(4);
                        object TOTAL = rdr.GetDecimal(5);

                        Console.WriteLine(" {0} \t\t {1} \t\t {2}\t\t{3}\t\t{4}\t\t{5}", RECEIPT_ID, PRODUCTS_ID, PRODUCT_NAME, AMOUNT, PRICE, TOTAL);
                    }
                }
                rdr.Close();
            }
            Console.Read();

            //-------------------------------------------------------------------------------------------------------------

            //try
            //{
            //    connection.Open();
            //    Console.WriteLine("Подключение открыто");
            //    SqlCommand command = new SqlCommand(sqlExpression, connection);
            //    rdr = command.ExecuteReader();
            //    int line = 0;
            //    while (rdr.Read())
            //    {
            //        if (line == 0)
            //        {
            //            for (int i = 0; i < rdr.FieldCount; i++)
            //            {
            //                Console.Write(rdr.GetName(i).ToString() + " ||   ");
            //            }
            //        }
            //        Console.WriteLine();
            //        line++;
            //        Console.WriteLine(" " + rdr[0] + "\t\t " + rdr[1] + "\t\t " + rdr[2] + "\t\t" + rdr[3] + "\t\t " + rdr[4] + "\t" + rdr[5]);
            //    }
            //    Console.WriteLine("Обработано записей : " + line.ToString());
            //}
            //finally
            //{
            //    if (rdr != null)
            //    {
            //        rdr.Close();
            //    }
            //    if (connection != null)
            //    {
            //        //закрываем подключение
            //        connection.Close();
            //        Console.WriteLine("Подключение закрыто...");
            //    }
            //   }
          //  Console.Read();

            //=========================================================================================================

            //Console.WriteLine("====================================================================");
            //    string sqlExpression_1 = @"Insert Into PRODUCTS (PRODUCT_NAME, PRODUCT_PRICE) 
            //                                            VALUES('ПЕЧЕНЬ',36)";
            //    using (SqlConnection conn = new SqlConnection(connectionString))
            //    {
            //        conn.Open();
            //        SqlCommand command = new SqlCommand(sqlExpression_1, conn);
            //        int number = command.ExecuteNonQuery();
            //        Console.WriteLine("Добавлено обЪектов {0} в таблицу PRODUCTS ", number);

            //    }
            //Console.Read();

            //==============================================================================================================

            //Console.WriteLine("====================================================================");

            //    string sqlExp = @"UPDATE PRODUCTS SET PRODUCT_PRICE=18 WHERE PRODUCT_ID=6";
            //    using (SqlConnection conn = new SqlConnection(connectionString))
            //    {
            //        conn.Open();
            //        SqlCommand command = new SqlCommand(sqlExp, conn);
            //        int number = command.ExecuteNonQuery();
            //        Console.WriteLine("Обновлено обЪектов {0} в таблице PRODUCTS: ", number);
            //    }

            //    //закрываем подключение
            //    connection.Close();
            //    Console.WriteLine("Подключение закрыто...");
            //Console.Read();

            //=======================================================================================================


            string sqlEpr = "Select count(*) from RECEIPT_DETAILS";
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand command = new SqlCommand(sqlEpr, conn);
                    object count = command.ExecuteScalar();

                    command.CommandText = "Select Min(PRICE) FROM RECEIPT_DETAILS";
                    object minPrice = command.ExecuteScalar();

                    Console.WriteLine("В таблице RECEIPT_DETAILS {0} объектов", count);
                    Console.WriteLine("Минимальная цена : {0}", minPrice);

                    connection.Close();
                    Console.WriteLine("Подключение закрыто...");
                }
            Console.Read();

            //================================================================================================================

            decimal price = 32;
            string sqlExpress = @"UPDATE PRODUCTS SET PRODUCT_PRICE = @price WHERE PRODUCT_ID = 8; Set @id=SCOPE_IDENTITY()";


            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand command = new SqlCommand(sqlExpress, conn);
                //Создаем параметр для price

                SqlParameter priceParam = new SqlParameter("@price", price);
                //добавляем параметр к команде 
                command.Parameters.Add(priceParam);

                SqlParameter idParam = new SqlParameter
                {
                    ParameterName = "@id",
                    SqlDbType = System.Data.SqlDbType.Int,
                    Direction = System.Data.ParameterDirection.Output //параметр выходной
                };
                command.Parameters.Add(idParam);

                int number = command.ExecuteNonQuery();
                Console.WriteLine("Обновлено обЪектов: {0}", number);
                //получем значения выходного параметра

                
                command.ExecuteNonQuery();
                Console.WriteLine("Id объекта : {0}", idParam.Value);

                var res = command.ExecuteScalar();
                Console.WriteLine("Id объекта : {0}", res);


                connection.Close();
                Console.WriteLine("Подключение закрыто...");

            }

            Console.Read();

            //============================================================================================================
            //using (SqlConnection conn = new SqlConnection(connectionString))
            //{
            //    conn.Open();
            //    SqlCommand cmd = new SqlCommand("ONE_SALE_RECEIPT", conn);
            //    cmd.CommandType = System.Data.CommandType.StoredProcedure;
            //    cmd.Parameters.Add("@ONE_RECEIPT", System.Data.SqlDbType.Int).Value = 1;
            //    cmd.ExecuteNonQuery();
            //    Console.WriteLine(cmd.Parameters["@ONE_RECEIPT"].Value.ToString());


            //    conn.Close();
            //    Console.WriteLine("Подключение закрыто...");
            //}
            Console.Read();

            }
        }
    }

