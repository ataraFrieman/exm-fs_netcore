using Quze.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using Quze.Models;
namespace Quze.DAL.Stores
{
    public class RescheduleStore:StoreBase<Appointment>
    {

        public RescheduleStore(QuzeContext ctx):base(ctx)
        {

        }
        public List<Operation> InitTimeAfterRescheduling(List<Operation> newOperationList, DateTime beginTime, char state='S')
        {
            Dictionary<int, List<Operation>> OpeartionsByRooms = new Dictionary<int, List<Operation>>();
            int[] Rooms = newOperationList.Select(o => o.RoomId).Distinct().ToArray();//every room with operations
            for (int i = 0; i < Rooms.Length; i++)
            { 
                OpeartionsByRooms.Add(Rooms[i], newOperationList.Where(o => o.RoomId == Rooms[i]).ToList());
                OpeartionsByRooms[Rooms[i]]=OpeartionsByRooms[Rooms[i]].OrderBy(x => x.Priority).ToList();
            }
            foreach (var item in OpeartionsByRooms)
            {
                var room = item.Value;
                DateTime time = beginTime;
                int timrTurnOver = 0;
                if (state == 'S') 
                    for (int j = 0; j < room.Count; j++)
                    {
                        //call to ml to know new delay and duration
                        //var CurrOperation = room[j];
                        int AnesthesiaDuration = (int)(room[j].AnesthesiaOrigEndTime - room[j].AnesthesiaOrigBeginTime).TotalSeconds;
                        int SurgeryDuration = (int)(room[j].SurgeryOrigEndTime - room[j].SurgeryOrigBeginTime).TotalSeconds;
                        //int CleanDuration = (int)(room[j].CleanOrigEndTime - room[j].CleanOrigBeginTime).TotalSeconds;
                         timrTurnOver = (int)item.Value[j].Delay;
                        if (timrTurnOver < 15)
                            timrTurnOver = 15*60;
                        else
                            timrTurnOver = (int)item.Value[j].Delay * 60;
                        item.Value[j].AnesthesiaBeginTime = time.AddSeconds(timrTurnOver);
                        item.Value[j].SurgeryBeginTime = time.AddSeconds(timrTurnOver);
                        time= time.AddSeconds(timrTurnOver);
                        time = time.AddSeconds(AnesthesiaDuration);
                        item.Value[j].AnesthesiaEndTime = time;
                        item.Value[j].SurgeryEndTime = time;
                    }
                else
                    for (int j = 0; j < room.Count; j++)
                    {
                        timrTurnOver = (int)item.Value[j].Delay;
                        if (timrTurnOver < 15)
                            timrTurnOver = 15*60;
                        else
                            timrTurnOver = (int)item.Value[j].Delay * 60;
                        item.Value[j].AnesthesiaOrigBeginTime = time.AddSeconds(timrTurnOver);
                        item.Value[j].SurgeryOrigBeginTime = time.AddSeconds(timrTurnOver) ;
                        time = time.AddSeconds(timrTurnOver);
                        time = time.AddSeconds((int)room[j].OperationDuration);
                        item.Value[j].AnesthesiaOrigEndTime = time;
                        item.Value[j].SurgeryOrigEndTime = time;
                    }
            }
            List<Operation> OperationsList = new List<Operation>();
            foreach (var item in OpeartionsByRooms)
            {
                var room = item.Value;
                if (room != null && room.Count > 0)
                    OperationsList.AddRange(room);
            }
            return OperationsList;
        }






        public Operation FindEarlyestAppointment(Operation o1, Operation o2, string type)
        {
            switch (type)
            {
                case "Anesthesia":
                    return o1.AnesthesiaBeginTime < o2.AnesthesiaBeginTime ? o1 : o2;
                case "Clean":
                    return o1.CleanBeginTime < o2.CleanBeginTime ? o1 : o2;
                case "surgery":
                default:
                    return o1.SurgeryBeginTime < o2.SurgeryBeginTime ? o1 : o2;
            }
        }

        public Operation FindLatest(Operation o1, Operation o2, string type)
        {
            switch (type)
            {
                case "Anesthesia":
                    return o1.AnesthesiaBeginTime > o2.AnesthesiaBeginTime ? o1 : o2;
                case "Clean":
                    return o1.CleanBeginTime > o2.CleanBeginTime ? o1 : o2;
                case "surgery":
                default:
                    return o1.SurgeryBeginTime > o2.SurgeryBeginTime ? o1 : o2;
            }

        }

       
    }
}