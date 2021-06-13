using Quze.Infrastruture.Extensions;
using Quze.Models.Entities;
using Quze.Models.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using Quze.Models.Models;

namespace Quze.DAL
{
    public class ServiceQueueLogic
    {
        //   private DateTime now = Time.GetNow();
        public ServiceQueue ServiceQueue { get; }
        //  public List<Slot> AvailableSlots { get; set; }
        public ServiceQueueLogic()
        {
        }

        public ServiceQueueLogic(ServiceQueue sq)
        {
            ServiceQueue = sq;
        }

        public int Id => ServiceQueue.Id;

        public IEnumerable<Slot> GetServiceQueueSlots(int arrivalTime, Fellow fellow, int duration = 0)
        {
            var devidedSlots = new List<Slot>();
            //var request = new RequestML
            //{
            //    ServiceProviderId = ServiceQueue.ServiceProviderId,
            //    FellowId = fellow?.Id,
            //    FellowBirthDate = fellow?.BirthDate,
            //    AppointmentTime = ServiceQueue.BeginTime
            //};
            //var mlResponse = await new Duration().GetDurationAsync(request);

            var slots = GetSlots(duration, arrivalTime);
            if (duration == -1)//do not need to divide the slots by duration
                return slots;

            foreach (var slot in slots)
                devidedSlots.AddRange(slot.DevideSlotByDuration(duration));

            //TODO: auto mapping from time table to spBranchSlots
            //if (devidedSlots.Count != 0)
            //{
            //    result.Add(new SP_BranchSlots()
            //    {
            //        ServiceProviderId = sq.ServiceProviderId,
            //        ServiceProviderName = sq.ServiceProvider.Person.FirstName + " " + sq.ServiceProvider.Person.LastName,
            //        BranchId = sq.BranchId,
            //        BranchNmae = sq.Branch.Name,
            //        Slots = devidedSlots,
            //        ServiceQueueId = sq.Id != 0 ? sq.Id : 0
            //    });
            //}
            return devidedSlots;
        }

        /// <summary>
        /// Get slots with minimum duration size as give in the duration parameter
        /// </summary>
        /// <param name="duration"></param>
        /// <param name="arrivalTime"></param>
        /// <returns></returns>
        private List<Slot> GetSlots(int duration, int arrivalTime)
        {
            try
            {
                var slots = GetSlots(arrivalTime).Where(s => s.Duration >= duration).ToList();
                return slots;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }


        /// <summary>
        /// Return all slots of queue
        /// </summary>
        /// <returns></returns>
        private IEnumerable<Slot> GetSlots(int arrivalTime, DateTime? time = null)
        {
            var slots = new List<Slot>();
            Slot slot;

            var beginTime = time.IsNotNull()
                            && time.Value.IsTimeBetween(ServiceQueue.BeginTime.TimeOfDay, ServiceQueue.EndTime.TimeOfDay) ? time.Value : ServiceQueue.BeginTime;
            var sortedApoointments = RelevantAppointments;

            //When no appointments are set in this queue, all the queue is a slot time
            if (sortedApoointments.IsNullOrEmpty())
            {
                slot = new Slot(Id, beginTime, ServiceQueue.EndTime, ServiceQueue.Duration);
                slots.Add(slot);
                return slots;
            }

            var index = 0;
            var duration = sortedApoointments[0].Duration;

            if (duration != null
                && sortedApoointments[0].ActualBeginTime != null
                && sortedApoointments[0].ActualBeginTime.Value.AddSeconds(duration.Value) <= ServiceQueue.BeginTime
                ) index = 1;

            if (sortedApoointments[index].BeginTime > ServiceQueue.BeginTime)
            {
                slot = new Slot(Id, ServiceQueue.BeginTime, sortedApoointments[index].BeginTime);
                slots.Add(slot);
            }

            slot = GetSlotBetweenAppointmentToTheNextOneByDuration(sortedApoointments[index].BeginTime, arrivalTime);
            if (slot != null)
                slots.Add(slot);

            while (slot != null)
            {
                slot = GetSlotBetweenAppointmentToTheNextOneByDuration(slot.EndTime, arrivalTime);
                if (slot != null)
                    slots.Add(slot);
            }
            return slots;
        }


        private Slot GetSlotBetweenAppointmentToTheNextOneByDuration(DateTime beginTime, int arrivalTime)
        {
            var sortedAppointments = RelevantAppointments;

            if (sortedAppointments.IsNullOrEmpty())
                return null;

            var index = sortedAppointments.IndexOf(sortedAppointments.FirstOrDefault(a => a.BeginTime == beginTime));

            if (index == -1)
                return null;

            if (sortedAppointments.Count <= index)
                return null;

            var b = sortedAppointments[index].ActualBeginTime == null ?
                    sortedAppointments[index].BeginTime.AddSeconds(sortedAppointments[index].Duration.Value) :
                    sortedAppointments[index].ActualBeginTime.Value.AddSeconds(sortedAppointments[index].Duration.Value);

            var endTime = b < Time.GetNow() ? Time.GetNow().AddSeconds(sortedAppointments[index].Duration.Value) : b;
            int i;
            for (i = index + 1; i < sortedAppointments.Count && sortedAppointments[i].BeginTime <= endTime; i++)
            {
                endTime = endTime.AddSeconds(sortedAppointments[i].Duration.Value);
            }
            if (endTime < Time.GetNow().AddSeconds(arrivalTime))
                endTime = Time.GetNow().AddSeconds(arrivalTime);
            if (ServiceQueue.EndTime <= endTime) return null;
            return sortedAppointments.Count == i ?
                new Slot(Id, endTime, ServiceQueue.EndTime)
              : new Slot(Id, endTime, sortedAppointments[i].BeginTime);
        }

        //private List<ServiceQueue> SplitServiceQueueByTime()
        //{
        //    var serviceQueues = new List<ServiceQueue>();

        //    if (ServiceQueue.BeginTime.GetTimeInDay() == TimeInDay.Morning && ServiceQueue.EndTime.GetTimeInDay() == TimeInDay.Morning
        //        || ServiceQueue.BeginTime.GetTimeInDay() == TimeInDay.AfterNoon && ServiceQueue.EndTime.GetTimeInDay() == TimeInDay.AfterNoon
        //        || ServiceQueue.BeginTime.GetTimeInDay() == TimeInDay.Evening)
        //    {
        //        serviceQueues.Add(ServiceQueue);
        //        return serviceQueues;
        //    }

        //    if (ServiceQueue.BeginTime.GetTimeInDay() == TimeInDay.Morning)
        //    {
        //        serviceQueues.Add(new ServiceQueue(ServiceQueue, ServiceQueue.BeginTime, ServiceQueue.BeginTime.Date.AddHours(12)));
        //        if (ServiceQueue.EndTime.GetTimeInDay() == TimeInDay.AfterNoon)
        //        {
        //            serviceQueues.Add(new ServiceQueue(ServiceQueue, ServiceQueue.BeginTime.Date.AddHours(12), ServiceQueue.EndTime));
        //            return serviceQueues;
        //        }
        //        serviceQueues.Add(new ServiceQueue(ServiceQueue, ServiceQueue.BeginTime.Date.AddHours(12), ServiceQueue.BeginTime.Date.AddHours(16)));
        //        return serviceQueues;
        //    }
        //    if (ServiceQueue.BeginTime.GetTimeInDay() == TimeInDay.AfterNoon)
        //    {
        //        serviceQueues.Add(new ServiceQueue(ServiceQueue, ServiceQueue.BeginTime, ServiceQueue.BeginTime.Date.AddHours(16)));
        //        serviceQueues.Add(new ServiceQueue(ServiceQueue, ServiceQueue.BeginTime.Date.AddHours(16), ServiceQueue.EndTime));
        //        return serviceQueues;
        //    }

        //    return serviceQueues;
        //}

        public void UpdateCurrentAppointmentServed()
        {
            if (ServiceQueue.CurrentAppointementId == null)
                return;
            var currentAppointment = ServiceQueue.Appointments.FirstOrDefault(
                a => a.Id == ServiceQueue.CurrentAppointement.Id);
            if (currentAppointment != null)
                currentAppointment.Served = true;
        }

        public void UpdateCurrentAppointment()
        {
            if (RelevantAppointments.Count > 0)
            {
                ServiceQueue.CurrentAppointement = RelevantAppointments[0];
                ServiceQueue.Appointments.First(a => a.Id == ServiceQueue.CurrentAppointement.Id).ActualBeginTime = Time.GetNow();
            }
            else
                ServiceQueue.CurrentAppointement = null;
        }

        //TODO: טיפול במי שעבר תורו
        /// <summary>
        /// Appointments that are still not served
        /// </summary>
        public List<Appointment> RelevantAppointments
        {
            get
            {
                var appointments = ServiceQueue.Appointments?.Select(a => a).OrderBy(a => a.BeginTime);
                return appointments?.Where(a => !a.IsDeleted && !a.Served).ToList();
            }
        }


        private Appointment CurrentAppointment => ServiceQueue.CurrentAppointement;

        ///// <summary>
        ///// 
        ///// </summary>
        //private int? CurrentAppointmentIndex => ServiceQueue.CurrentAppointement.IsNull() ? -1 : ServiceQueue.Appointments.IndexOf(ServiceQueue.CurrentAppointement);

        //public DateTime NextPush(DateTime appointmentBeginTime)
        //{
        //    DateTime nextPush = new DateTime();
        //    //if (sq.CurrentAppointement.IsNull())
        //    //{
        //    //    nextPush = sq.BeginTime.AddSeconds(SQDelay());
        //    //    return nextPush;
        //    //}
        //    if (RelevantAppointments.Count == 1)
        //    {
        //        return DateTime.MaxValue;
        //    }
        //    else
        //    {
        //        if (sq.CurrentAppointement.IsNotNull())
        //            nextPush = RelevantAppointments[0].ActualBeginTime.Value.AddSeconds((int)RelevantAppointments[0].Duration);
        //        else
        //            nextPush = RelevantAppointments[0].BeginTime;
        //        var actualDuration = appointmentBeginTime.AddSeconds(CurrentAppointmentsDuration(appointmentBeginTime) * (-1));
        //        nextPush = nextPush < actualDuration ? actualDuration : nextPush;
        //        if (nextPush < Time.GetNow())
        //            nextPush = Time.GetNow();
        //        return nextPush;
        //    }
        //}

        //private int CurrentAppointmentsDuration(DateTime appointmentBeginTime)
        //{
        //    var actualDuration = (int)RelevantAppointments.Where(a => a.BeginTime < appointmentBeginTime).Select(a => a.Duration).Sum();
        //    if (CurrentAppointment == RelevantAppointments[0])
        //    {
        //        var currentAppointmentActualDuration = (int)(Time.GetNow() - CurrentAppointment.ActualBeginTime.Value).TotalSeconds;
        //        currentAppointmentActualDuration = currentAppointmentActualDuration < RelevantAppointments[0].Duration.Value ? currentAppointmentActualDuration : RelevantAppointments[0].Duration.Value;
        //        actualDuration -= currentAppointmentActualDuration;


        //    }
        //    return actualDuration;
        //}

        //public int Delay(DateTime appointmentBeginTime)
        //{
        //    int orginalDuration, actualDuration, currentDelay = 0, delay = 0;
        //    if (ServiceQueue.ActualBeginTime == null)
        //    {
        //        var BegimTime = Time.GetNow() < ServiceQueue.BeginTime ? ServiceQueue.BeginTime : Time.GetNow();
        //        orginalDuration = (int)(appointmentBeginTime - BegimTime).TotalSeconds;
        //        orginalDuration = orginalDuration < 0 ? 0 : orginalDuration;
        //        actualDuration = (int)RelevantAppointments.Where(a => a.BeginTime < appointmentBeginTime).Select(a => a.Duration).Sum();
        //        if (actualDuration > orginalDuration)
        //            delay = actualDuration - orginalDuration;
        //        if (appointmentBeginTime < Time.GetNow())
        //            currentDelay = (int)(Time.GetNow() - appointmentBeginTime).TotalSeconds;
        //        delay += currentDelay;
        //        return delay;

        //    }

        //    if (CurrentAppointment == null)
        //        return 0;
        //    orginalDuration = (int)(appointmentBeginTime - (DateTime)CurrentAppointment.ActualBeginTime).TotalSeconds;
        //    orginalDuration = orginalDuration < 0 ? 0 : orginalDuration;
        //    actualDuration = (int)RelevantAppointments.Where(a => a.BeginTime < appointmentBeginTime).Select(a => a.Duration).Sum();
        //    currentDelay = 0;
        //    ////////currentAppointmentCurrentDelay
        //    var currentAppointmentCurrentDuration = (Time.GetNow() - (DateTime)CurrentAppointment.ActualBeginTime).TotalSeconds;
        //    if (CurrentAppointment.Duration < currentAppointmentCurrentDuration)
        //        currentDelay = (int)(currentAppointmentCurrentDuration - CurrentAppointment.Duration);
        //    currentDelay -= (int)(Time.GetNow() - CurrentAppointment.ActualBeginTime.Value).TotalSeconds;


        //    delay = actualDuration - orginalDuration + currentDelay;
        //    if (appointmentBeginTime < Time.GetNow())
        //        delay += (int)(Time.GetNow() - appointmentBeginTime).TotalSeconds;
        //    delay = delay < 0 ? 0 : delay;
        //    return delay;


        //}

        //public int SQDelay()
        //{
        //    return ServiceQueue.BeginTime < Time.GetNow() ? (int)(DateTime.Now - ServiceQueue.BeginTime).TotalSeconds : 0;
        //}
        /* public int Delay
         {
             get
             {


                 if (sq.ActualBeginTime.IsNull())
                 {
                     return sq.BeginTime < Time.GetNow() ? (int)((DateTime.Now - sq.BeginTime).TotalSeconds) : 0;
                 }
                 else if (CurrentAppointment.IsNotNull())
                 {
                     DateTime currentAppointmentActualBeginTime = new DateTime();
                     if (CurrentAppointment.ActualBeginTime.Value != null)
                         currentAppointmentActualBeginTime = (DateTime)CurrentAppointment.ActualBeginTime;
                     var currentAppointmentBeginTime = CurrentAppointment.BeginTime;
                     var beginTime = currentAppointmentBeginTime > currentAppointmentActualBeginTime ?
                         currentAppointmentBeginTime : currentAppointmentActualBeginTime;
                     int delay;
                     if (currentAppointmentActualBeginTime > currentAppointmentBeginTime)
                     {
                         delay = (int)((currentAppointmentActualBeginTime - currentAppointmentBeginTime).TotalSeconds);

                     }
                     else
                         delay = 0;
                     int currentDuration = CurrentAppointment.Duration == null ? 0 : (int)CurrentAppointment.Duration;
                     if (currentAppointmentActualBeginTime.AddSeconds(currentDuration) < Time.GetNow())
                     {
                         int newDelay = (int)(currentAppointmentActualBeginTime.AddSeconds(currentDuration) - Time.GetNow()).TotalSeconds;
                         delay += newDelay;
                     }
                     return delay;

                 }
                 else
                     return 0;


             }
         }*/

        public ServiceQueue StartQueue()
        {
            ServiceQueue.ActualBeginTime = Time.GetNow();
            return ServiceQueue;

        }

        public void CalculateEtts()
        {
            foreach (var t in RelevantAppointments)
                t.ETTS = t.BeginTime.AddSeconds(t.Delay);
        }

        public void CalculateNextPush()
        {
            if (RelevantAppointments.Count > 0)
                RelevantAppointments[0].NextPush = RelevantAppointments[0].BeginTime < Time.GetNow() ?
               Time.GetNow() : RelevantAppointments[0].BeginTime;
            DateTime nextPush;
            if (RelevantAppointments.Count > 1)
            {

                var beginTime = RelevantAppointments[0].BeginTime;

                if (RelevantAppointments[0].ActualBeginTime != null)
                    beginTime = RelevantAppointments[0].BeginTime < RelevantAppointments[0].ActualBeginTime.Value ?
                        RelevantAppointments[0].ActualBeginTime.Value : RelevantAppointments[0].BeginTime;
                nextPush = beginTime.AddSeconds
                    (RelevantAppointments[0].Duration.Value);

                if (nextPush < RelevantAppointments[1].BeginTime)
                    nextPush = RelevantAppointments[1].BeginTime;
                if (nextPush < Time.GetNow())
                    nextPush = Time.GetNow();

                RelevantAppointments[1].NextPush = nextPush;

            }
            for (var i = 2; i < RelevantAppointments.Count; i++)
            {
                nextPush = RelevantAppointments[i - 1].NextPush.AddSeconds
                    ((RelevantAppointments[i].BeginTime - RelevantAppointments[i - 1].NextPush).TotalSeconds
                    - RelevantAppointments[i - 1].Duration.Value);
                if (nextPush < Time.GetNow())
                    nextPush = Time.GetNow();
                RelevantAppointments[i].NextPush = nextPush;

            }
        }

        public void CalculateDelayOld()
        {
            if (RelevantAppointments.Count == 0)
                return;

            var firstAppDelay = 0;
            var actualBeginTime = RelevantAppointments[0].ActualBeginTime ?? Time.GetNow();

            firstAppDelay = (int)(actualBeginTime - RelevantAppointments[0].BeginTime).TotalSeconds;
            firstAppDelay = firstAppDelay < 0 ? 0 : firstAppDelay;
            RelevantAppointments[0].Delay = firstAppDelay;
            //update all next appoitment duration

            for (var i = 1; i < RelevantAppointments.Count; i++)
            {
                var delay = 0;
                var prevAppointment = RelevantAppointments[i - 1];
                var duration = prevAppointment.Duration.Value;
                if (prevAppointment.ActualBeginTime != null)
                {
                    //set to latest time(BeginTime or ActualBeginTime)
                    var beginTime = prevAppointment.BeginTime > prevAppointment.ActualBeginTime ?
                                              prevAppointment.BeginTime : prevAppointment.ActualBeginTime.Value;
                    if ((Time.GetNow() - beginTime).TotalSeconds > duration)
                    {

                        duration = (int)(Time.GetNow() - beginTime).TotalSeconds;
                    }
                }
                delay = (int)(prevAppointment.BeginTime.AddSeconds(duration)
                            - prevAppointment.BeginTime).TotalSeconds + RelevantAppointments[i - 1].Delay;
                delay = delay < 0 ? 0 : delay;
                RelevantAppointments[i].Delay = delay;
            }
        }

        public void CalculateDelay()
        {
            if (RelevantAppointments.Count == 0 || ServiceQueue.Appointments == null || ServiceQueue.Appointments.Count == 0)
                return;
            int delaySum = 0;
            int i = 0;

            Appointment firstAppointment = ServiceQueue.Appointments[0];
            //assuming ServiceQueue.Appointments[0] is the begin time of Queue
            //SQ still not begin
            if (firstAppointment.ActualBeginTime == null)
            {
                if (ServiceQueue.BeginTime < DateTime.Now)
                    delaySum = (int)(DateTime.Now - firstAppointment.BeginTime).TotalSeconds;
            }
            else
            {
                for (i = 0; i < ServiceQueue.Appointments.Count; i++)
                {
                    var appointment = ServiceQueue.Appointments[i];
                    //sum all delays
                    if (appointment.ActualBeginTime != null)
                    {
                        //ensure actualBeginTome is not null 
                        var actualBeginTime = appointment.ActualBeginTime ?? Time.GetNow();
                        delaySum += (int)(actualBeginTime - appointment.BeginTime).TotalSeconds;
                    }

                }
            }
            //apdate Delay to all next appointments(relevantAppointments)
            for (i = 0; i < ServiceQueue.Appointments.Count - 1; i++)
            {
                var currentApp = ServiceQueue.Appointments[i];
                if (currentApp.ActualBeginTime != null)
                    continue;
                var nextApp = ServiceQueue.Appointments[i + 1];
                if (delaySum > 0)
                {
                    if (currentApp.EndTime.AddSeconds(delaySum) > nextApp.BeginTime)
                    {
                        nextApp.Delay = ((currentApp.EndTime.AddSeconds(delaySum)) - nextApp.BeginTime).IntTotalSeconds();
                    }
                }
                else
                {
                    //טיפול במקרה של קידום הורדה מערך הדיליי
                    if (currentApp.Delay > 0)
                    {
                        currentApp.Delay = currentApp.Delay - delaySum;
                        currentApp.Delay = currentApp.Delay < 0 ? 0 : currentApp.Delay;
                    }

                }

            }

        }

        public void UpdateDelayToAllAppointmentsInQ(int timeDifference)
        {
            for (int i = 0; i < this.RelevantAppointments.Count; i++)
            {
                //עידכון אם יש קידום או שהקידום קטן מהדילי הקיים
                //(בהנחה שאין להקדים תור לפני שעת קביעתו)
                if (timeDifference > 0 || (RelevantAppointments[i].Delay + timeDifference) >= 0)
                    RelevantAppointments[i].Delay += timeDifference;
            }
        }

        public void CalculatePositionInQueue()
        {
            for (var i = 0; i < RelevantAppointments.Count; i++)
            {
                RelevantAppointments[i].PositionInQueue = i;
                if (RelevantAppointments[0].ActualBeginTime.IsNull())
                    RelevantAppointments[i].PositionInQueue++;
            }
            //foreach (var a in RelevantAppointments)
            //{
            //    if (a.Served)
            //        a.NumInServiceQueue = null;
            //    var itenIndex = RelevantAppointments.IndexOf(a);
            //    a.NumInServiceQueue = itenIndex;
            //}


            //    if (CurrentAppointment != null)
            //    {
            //        DateTime currentAppointmentActualBeginTime = DateTime.Now;
            //        DateTime currentAppointmentBeginTime = DateTime.Now;
            //        if (CurrentAppointment.ActualBeginTime != null)
            //            currentAppointmentActualBeginTime = CurrentAppointment.ActualBeginTime.Value;
            //        if (CurrentAppointment.BeginTime != null)
            //            currentAppointmentBeginTime = (DateTime)CurrentAppointment.BeginTime;
            //        a.Delay = (currentAppointmentActualBeginTime - currentAppointmentBeginTime).Minutes + ((currentAppointmentActualBeginTime - currentAppointmentBeginTime).Hours * 60);
            //        a.Delay = a.Delay < 0 ? 0 : a.Delay;
            //    }
            //    else
            //    {
            //        var minBeginTime = a.ServiceQueue.Appointments.Select(x => x.BeginTime).Min();
            //        if (DateTime.Now <= minBeginTime)
            //            a.Delay = 0;
            //        else
            //            a.Delay = (DateTime.Now - minBeginTime).Minutes + (DateTime.Now - minBeginTime).Hours * 60;
            //    }
            //    a.NextPush = a.NextPush.AddMinutes(a.Delay);
            //    if (a.NextPush < DateTime.Now)
            //    {
            //        var delay = (DateTime.Now - a.NextPush).Minutes + (DateTime.Now - a.NextPush).Hours * 60;
            //        a.Delay = a.Delay + delay;
            //    }

        }

    }
}
