using Quze.Models.Entities;


namespace CreateAlerts.Test
{
    class InitAppointmentTaskAndDocuments
    {
        public AppointmentTask GetAppointmentTask1()
        {
            var requiredTasksAndDocs = new InitRequiredTaskAndDocuments();
            return new AppointmentTask()
            {
                RequiredTask = requiredTasksAndDocs.GetRequiredTask1(),
                AppointmentId = 123,
                RequiredTaskId = 1
            };
        }

        public AppointmentDocument GetAppointmentDocs1()
        {
            var requiredTasksAndDocs = new InitRequiredTaskAndDocuments();
            return new AppointmentDocument()
            {
                RequiredDocument = requiredTasksAndDocs.GetRequiredDocuments1(),
                AppointmentId = 123,
                RequiredDocumentId = 1
            };
        }
    }
}
