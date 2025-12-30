using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuditSystem.Contract.Models.Reports.DriftReport;

namespace AuditSystem.Contract.Interfaces.ModelServices.ReportsServices.DriftReportServices;

public interface IControlDataFetcher
{
    Task<ControlData> FetchAsync(GenerateDriftReportRequest request);
}
