using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Unicode;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using WorkEmployees.Models;
//using WorkEmployees.Models;

namespace WorkEmployees.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            ViewBag.Message = TempData["message"];
            return View();
        }


        public IActionResult UploadFile(IFormFile file)
        {
            List<EmployeeFileData> employeeFileList = new List<EmployeeFileData>();
            string connectionString = _configuration.GetConnectionString("DaysWorkCon");
            if (file == null)
            {
                TempData["message"] = "There is no uploaded file!";
                return RedirectToAction("Index");
            }
            Stream stream = file.OpenReadStream() != null ? file.OpenReadStream() : null;
            StreamReader streamReader = new StreamReader(stream);
            try
            {

                string theWholeFileAsString = streamReader.ReadToEnd();

                string[] lines = theWholeFileAsString.Split(new string[] { Environment.NewLine}, StringSplitOptions.None);

                foreach (var line in lines)
                {
                    var singleLineList = line.Split(',');

                    if (singleLineList.Length == 4)
                    {
                        EmployeeFileData fileData = new EmployeeFileData();

                        fileData.EmployeeId = Convert.ToInt32(singleLineList[0].Trim());
                        fileData.ProjectId = Convert.ToInt32(singleLineList[1].Trim());
                        fileData.DateFrom = singleLineList[2].Trim();
                        fileData.DateTo = (singleLineList[3].Trim() == "NULL") ? DateTime.Now.ToString("yyyy-MM-dd") : singleLineList[3].Trim();

                        employeeFileList.Add(fileData);
                    }

                }
                foreach (var item in employeeFileList)
                {

                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();

                        string query = "INSERT INTO EmployeeDaysWork Values(" + item.EmployeeId + "," + item.ProjectId
                            + ",'" + item.DateFrom + "','" + item.DateTo + "')";
                        using (SqlCommand comm = new SqlCommand(query, conn))
                        {
                            comm.ExecuteScalar();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["message"] = ex.Message;
            }
            finally
            {
                stream.Close();
                streamReader.Close();
            }
            TempData["message"] = "Successfuly saved!";


            return RedirectToAction("Index");

        }

        public IActionResult EmployeesView()
        {
            string connectionString = _configuration.GetConnectionString("DaysWorkCon");
            List<WorkEmployee> empList = new List<WorkEmployee>();
            SqlDataReader dataReader;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("uspGetTotalWorkDays", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    con.Open();
                    dataReader = cmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            WorkEmployee employee = new WorkEmployee();
                            if (dataReader["t1_EmployeeId"] != null)
                            {
                                employee.Employee1 = Convert.ToInt32(dataReader["t1_EmployeeId"].ToString());
                            }
                            else
                            {
                                continue;
                            }
                            if (dataReader["t_EmployeeID"] != null && !(dataReader["t_EmployeeID"] is DBNull))
                            {
                                employee.Employee2 = Convert.ToInt32(dataReader["t_EmployeeID"].ToString());
                            }
                            else
                            {
                                continue;
                            }
                            if (dataReader["t_ProjectID"] != null)
                            {
                                employee.ProjectID = Convert.ToInt32(dataReader["t_ProjectID"].ToString());
                            }
                            else
                            {
                                continue;
                            }
                            if (dataReader["Max_No_Days_Worked_Togather"] != null)
                            {
                                if (Convert.ToInt32(dataReader["Max_No_Days_Worked_Togather"].ToString()) > 0)
                                {
                                    employee.DaysWorked = Convert.ToInt32(dataReader["Max_No_Days_Worked_Togather"].ToString());
                                }
                                else
                                {
                                    continue;
                                }
                            }
                            else
                            {
                                continue;
                            }

                            empList.Add(employee);
                        }
                    }

                }
            }




            DataTable dt = new DataTable();

            dt.Columns.Add("Employee1");
            dt.Columns.Add("Employee2");
            dt.Columns.Add("ProjectId");
            dt.Columns.Add("DaysWork");
            foreach (var item in empList)
            {
                dt.Rows.Add(item.Employee1, item.Employee2, item.ProjectID, item.DaysWorked);
            }


            return View(dt);
        }


    }
}
