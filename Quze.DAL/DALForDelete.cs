using Microsoft.EntityFrameworkCore;
using Quze.Infrastruture.Extensions;
using Quze.Infrastruture.Types;
using Quze.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Quze.Models.Models.Alerts;

namespace Quze.DAL
{
    /// <summary>
    /// General DAL methods
    /// </summary>
    public class DALForDelete
    {
        protected User CurrentUser { get; set; }
        protected QuzeContext ctx { get; set; }
        public string ConnectionString { get; set; }

        public DALForDelete(QuzeContext ctx) 
        {
            this.ctx = ctx;
        }


        public async Task<List<IAlert>> GetUnsentAlertsAsync()//                async Task<List<IAlert>> GetUnsentAlertsAsync()
        {
            //var list = await ctx.Alerts.Where(x => x.TimeToSend == null).ToListAsync();
            //return list.ForAll<Alert, IAlert>().ToList();

            // var list = ctx.Alerts.Where(x => x.TimeToSend == null).ToList();
            var list = await ctx.Alerts.ToListAsync();

            return list.ForAll<Alert, IAlert>().ToList();
        }

        /// <summary>
        /// Fetch Appointments with RequriedTasks and docs including all AlertRules
        /// </summary>
        /// <param name="now"></param>
        /// <returns></returns>
        public async Task<List<Appointment>> GetUpcomingAppointmentsAsync(DateTime now)
        {

            try
            {
                var result = await ctx.Appointments.Where(x => x.BeginTime > now && !x.Served)
                                   .Include(x => x.AppointmentDocs)
                                   .ThenInclude(at => at.RequiredDocument)//.ThenInclude(rd => rd.AlertRules)
                                   .Include(x => x.AppointmentTasks)
                                   .ThenInclude(at => at.RequiredTask)//.ThenInclude(rt => rt.AlertRules)
                                   .Include(x => x.Fellow).ToListAsync();
                return result;
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public async void SaveAlertsAsync(IEnumerable<Alert> alerts)
        {
            await ctx.Alerts.AddRangeAsync(alerts);
            await ctx.SaveChangesAsync();
        }



       
        //}

        public async Task<List<Branch>> GetAllBranchesAsync(int organizationId)
        {
            var list = ctx.Branches
                            .Where(b => b.OrganizationId == organizationId)
                            .ToListAsync();

            return await list;
        }

        public async Task<Branch> GetBranchByIdAsync(int id)
        {
            return await ctx.Branches.FindAsync(id);
        }

        public async Task<ServiceProvider> GetServiceProviderByIdAsync(int id)
        {
            return await ctx.ServiceProviders.FindAsync(id);
        }

        public async Task<Fellow> GetFellowIdAsync(int id)
        {
            return await ctx.Fellows.FirstOrDefaultAsync(x => x.Id == id);
        }

        internal async Task<List<TimeTable>> GetTimeTableAsync(int? serviceProviderId, DateTime validityDate)
        {
            if (serviceProviderId.IsNull()) return null;
            var serviceProvider = await ctx.ServiceProviders.FirstOrDefaultAsync(sp => sp.Id == serviceProviderId);
            var providesServesInThoseOrganizations = await ctx.ServiceProviders.Where(sp => sp.IdentityNumber == serviceProvider.IdentityNumber).Select(sp => sp.Id).ToListAsync();
            var timeTables = await ctx.TimeTables
                .Where(tt => providesServesInThoseOrganizations.Contains(tt.ServiceProviderId) &&
                                     tt.ValidFromDate <= validityDate
                                    &&
                                    tt.ValidUntilDate >= validityDate
                 )
                .Include(tt => tt.TimeTableLines).ToListAsync();
            //var list =Mapper.Map<List<TimeTableVM>>(timeTables);


            return timeTables;
        }

        /// <summary>
        /// Gets the nearest appointment for a given SP
        /// </summary>
        /// <param name="spId"></param>
        /// <returns></returns>
        public async Task<Appointment> GetNearestAppointmentBySPAsync(int spId)
        {
            var timeTables = await GetTimeTableAsync(spId, DateTime.Today);
            if (timeTables.IsNull()) return null;
            var lines = timeTables.SelectMany(tt => tt.TimeTableLines).OrderBy(l => l.WeekDay);
            var todaysWeekDay = (QuzeDayOfWeek)((int)DateTime.Today.DayOfWeek + 1);



            return null;
        }

        /// <summary>
        ///     saving entities that are already in the contex to the Data EntityBase
        /// </summary>
        /// <param name="alerts"></param>
        public async void SaveAlertsChangesAsync(List<Alert> alerts)
        {
            await ctx.Alerts.AddRangeAsync(alerts);
            await ctx.SaveChangesAsync();
        }

        public async void SaveTimeTablesAsync(TimeTable timeTable)
        {
            //TimeTable timeTable = Mapper.Map<TimeTable>(timeTableVM);
            await ctx.TimeTables.AddAsync(timeTable);
            await ctx.SaveChangesAsync();
        }

        public async void SaveTimeTableExceptionAsync(TimeTableException timeTableException)
        {
            ////ServiceQueue sq = AutoMapper.Map<ServiceQueue>(timeTableException);

            //await ctx.TimeTableExceptions.AddAsync(timeTableException);
            //await ctx.ServiceQueues.AddAsync(sq);
            //await ctx.SaveChangesAsync();
        }

    }


}
