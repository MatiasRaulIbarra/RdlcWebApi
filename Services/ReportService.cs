using AspNetCore.Reporting;
using RdlcWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RdlcWebApi.Services
{
    public interface IReportService
    {
        byte[] GenerateReportAsync(string reportName, string reportType);
    }
    public class ReportService : IReportService
    {
        public byte[] GenerateReportAsync(string reportName, string reportType)
        {

            string fileDirPath = Assembly.GetExecutingAssembly().Location.Replace("RdlcWebApi.dll",string.Empty);
            string rdlcFilePath = string.Format("{0}ReportFiles\\{1}.rdlc", fileDirPath, reportName);

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding.GetEncoding("utf-8");

            LocalReport report = new LocalReport(rdlcFilePath);

            //prepare data for report

            List<UserDto> userList = new List<UserDto>();

            var user1 = new UserDto { FirstName = "Matías", LastName = "Ibarra", Email = "matiasibarra96@hotmail.com", Phone = "3547563985" };

            userList.Add(user1);

            report.AddDataSource("dsUsers", userList);

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            var result = report.Execute(GetRenderType(reportType), 1, parameters);

            return result.MainStream;
        }

        private RenderType GetRenderType(string reportType)
        {
            var renderType = RenderType.Pdf;
            switch (reportType.ToUpper())
            {
                default:
                case "PDF":
                    renderType = RenderType.Pdf;
                    break;
                case "XLS":
                    renderType = RenderType.Excel;
                    break;
                case "WORD":
                    renderType = RenderType.Word;
                    break;
            }

            return renderType;
        }
    }
}
