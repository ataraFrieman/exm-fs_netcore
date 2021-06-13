using Microsoft.Extensions.Configuration;
using Quze.DAL;
using Quze.DAL.Stores;
using Quze.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quze.Models.Logic;
using Quze.Infrastruture.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Quze.BL
{
    public class MinimalKitLogic
    {
        private readonly QuzeContext context;
        private readonly MinimalKitRulesStore minimalKitStore;
        //private readonly AppointmentDocumentStore appointmentDocumentStore;
        //private readonly AppointmentTaskStore appointmentTaskStore;

        public MinimalKitLogic(QuzeContext context,
            IConfiguration configuration
            //AppointmentDocumentStore appointmentDocumentStore,
            //AppointmentTaskStore appointmentTaskStore
            )
        {
            this.context = context;
            minimalKitStore = new MinimalKitRulesStore(context);
            //this.appointmentDocumentStore = appointmentDocumentStore;
            //this.appointmentTaskStore = appointmentTaskStore;
        }

        //public class MinimalKitRequest
        //{
        //    public int AppointmentId { get; set; }
        //    public List<AppointmentDocVM> Docs { get; set; }
        //    public List<AppointmentTaskVM> Task { get; set; }
        //}


        public void GetMinimalKit(Appointment appointment)
        {
            try
            {
                var requiredTasksQuery = context.RequiredTasks.Where(x => x.ServiceTypeID == appointment.ServiceTypeId);
                var requiredDocumentsQuery = context.RequiredDocuments.Where(x => x.ServiceTypeID == appointment.ServiceTypeId);
                var requiredTestQuery = context.RequiredTests.Where(x => x.ServiceTypeId == appointment.ServiceTypeId);
                var appointmentTasksQuery = context.AppointmentTasks.Where(x => x.AppointmentId == appointment.Id); //?
                var appointmentDocumentsQuery = context.AppointmentDocuments.Where(x => x.AppointmentId == appointment.Id); //?
                var appointmentTestQuery = context.AppointmentTests.Where(x => x.AppointmentId == appointment.Id); //?

                //Task.WaitAll(requiredTasksQuery.ToArrayAsync(), requiredDocumentsQuery.ToArrayAsync());
                foreach (var task in requiredTasksQuery)
                {
                    var appointmentTask = new AppointmentTask { AppointmentId = appointment.Id, RequiredTaskId = task.Id };
                    context.AppointmentTasks.Add(appointmentTask);
                }

                foreach (var doc in requiredDocumentsQuery)
                {
                    var appointmentDoc = new AppointmentDocument { AppointmentId = appointment.Id, RequiredDocumentId = doc.Id };
                    context.AppointmentDocuments.Add(appointmentDoc);
                }

                foreach (var test in requiredTestQuery)
                {
                    var appointmentTest = new AppointmentTest { AppointmentId = appointment.Id, RequiredTestId = test.Id };
                    context.AppointmentTests.Add(appointmentTest);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public AppointmentTask AddTaskToAppointment(string description, bool isRequired, int appointmentId)
        {
            AppointmentTask appointmentTask;
            try
            {
                RequiredTask requiredTask = new RequiredTask { Description = description, IsRequired = isRequired };
                appointmentTask = minimalKitStore.AddTaskToAppointment(requiredTask, appointmentId);
                return appointmentTask;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public AppointmentDocument AddDocumentToAppointment(string description, bool isRequired, int appointmentId)
        {
            AppointmentDocument appointmentDocument;
            try
            {
                RequiredDocument requiredDocument = new RequiredDocument { Description = description, IsRequired = isRequired };
                appointmentDocument = minimalKitStore.AddDocumentToAppointment(requiredDocument, appointmentId);
                return appointmentDocument;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //Quze.Models.Entities.MinimalKitRules
        }

        public MinimalKit SaveMinimalKit(MinimalKit mkRequest)
        {
            if (mkRequest.Docs.IsNullOrEmpty() && mkRequest.Tasks.IsNullOrEmpty() && mkRequest.Tests.IsNullOrEmpty())
            {
                //minimalKitResponseVM.AddError(2000, "No docs and tasks and tests");
                return null;
            }

            MinimalKit minimalKit = new MinimalKit();

            var mkDocs = context.AppointmentDocuments
                .Include(ad => ad.RequiredDocument)
                .Where(ad => ad.AppointmentId == mkRequest.AppointmentId).ToList();
            minimalKit.Docs = mkDocs;

            var mkTasks = context.AppointmentTasks
                .Include(at => at.RequiredTask)
                .Where(at => at.AppointmentId == mkRequest.AppointmentId).ToList();
            minimalKit.Tasks = mkTasks;

            var mkTests = context.AppointmentTests
                .Include(ats => ats.RequiredTest)
                .Where(ats => ats.AppointmentId == mkRequest.AppointmentId).ToList();
            minimalKit.Tests = mkTests;

            try
            {
                minimalKit.Docs = SaveDocs(mkRequest.Docs, mkRequest.AppointmentId);
                minimalKit.Tasks = SaveTasks(mkRequest.Tasks, mkRequest.AppointmentId);
                minimalKit.Tests = SaveTests(mkRequest.Tests, mkRequest.AppointmentId);

                return minimalKit;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<AppointmentDocument> SaveDocs(List<AppointmentDocument> minimalKitDocs, int appointmentId)
        {
            try
            {
                var originalDocuments = context.AppointmentDocuments.Where(ad => ad.AppointmentId == appointmentId).ToList();
                if (minimalKitDocs.IsNull() || originalDocuments.Count == 0)
                {
                    return originalDocuments;
                }

                foreach (var doc in minimalKitDocs)
                {
                    var originalDoc = originalDocuments.FirstOrDefault(ad => ad.Id == doc.Id);
                    //if doc.Approved != null means that this doc has changed, and if it's new, Approved = null
                    if (doc.Approved != null)
                    {
                        if (doc.Approved.Value == true || doc.FileContent.IsNotNullOrEmpty())
                        {
                            originalDoc.TimeApproved = Time.GetNow();

                            if (doc.FileContent.IsNotNullOrEmpty())
                            {
                                //TODO: CheckIfItsDiffrentFromTheExsitingContent()
                                //TODO: if it's diffrent, delete the old one before continueing saving the new one
                                var documentContentId = SaveDocumentFile(doc.FileContent);
                                if (documentContentId.IsNull())
                                {
                                    throw new ApplicationException("Saving file content failed");
                                }
                                originalDoc.DocumentContentId = documentContentId;
                            }
                            originalDoc.Approved = true;
                        }
                        else
                        {
                            originalDoc.TimeApproved = Time.GetNow();
                            originalDoc.DocumentContentId = null;
                            originalDoc.Approved = false;
                            //TODO: delete the content itself
                        }
                        context.SaveChanges();
                    }
                }
                return originalDocuments;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private int SaveDocumentFile(string fileContent)
        {
            var bytes = Convert.FromBase64String(fileContent);
            var documentContent = new DocumentsContent() { Content = bytes };
            context.DocumentsContent.Add(documentContent);
            context.SaveChanges();
            return documentContent.Id;
        }

        public List<AppointmentTask> SaveTasks(List<AppointmentTask> minimalKItTask, int appointmentId)
        {
            try
            {
                var originalTasks = context.AppointmentTasks.Where(at => at.AppointmentId == appointmentId).ToList();
                if (minimalKItTask.IsNull() || originalTasks.Count == 0)
                {
                    return originalTasks;
                }
                foreach (var task in minimalKItTask)
                {
                    var originalTask = originalTasks.FirstOrDefault(ta => ta.Id == task.Id);
                    //if task.Approved != null means that this task has changed, and if it's new Approved = null
                    if (task.Approved != null)
                    {
                        if (task.Approved.Value == true)
                        {
                            originalTask.TimeApproved = Time.GetNow();
                            originalTask.Approved = true;
                        }
                        else
                        {
                            originalTask.TimeApproved = Time.GetNow();
                            originalTask.Approved = false;
                        }
                        context.SaveChanges(); //originalTask
                    }
                }
                return originalTasks;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<AppointmentTest> SaveTests(List<AppointmentTest> minimalKItTests, int appointmentId)
        {
            try
            {
                var originalTests = context.AppointmentTests.Where(at => at.AppointmentId == appointmentId).ToList();
                if (minimalKItTests.IsNull() || originalTests.Count == 0)
                {
                    return originalTests;
                }
                foreach (var test in minimalKItTests)
                {
                    var originalTest = originalTests.FirstOrDefault(ts => ts.Id == test.Id);
                    //if test.Approved != null means that this test has changed, and if it's new, Approved = null
                    if (test.Approved != null)
                    {

                        if (test.Approved.Value == true)
                        {
                            originalTest.TimeApproved = Time.GetNow();
                            originalTest.Approved = true;
                            originalTest.ValueOfTest = test.ValueOfTest;
                        }
                        else
                        {
                            originalTest.TimeApproved = Time.GetNow();
                            originalTest.Approved = false;
                        }
                        context.SaveChanges();
                    }
                }
                return originalTests;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string UpdateMkStatus(MinimalKit mkRequest, int operationId)
        {
            string status;
            int allRequireds, allNotRequired, isRequiredApproved, notRequiredApproved;
            allRequireds = allNotRequired = isRequiredApproved = notRequiredApproved = 0;

            //step 1 - get MK length
            var docs = mkRequest.Docs;
            var tasks = mkRequest.Tasks;
            var tests = mkRequest.Tests;
            var allLength = docs.Count + tasks.Count + tests.Count;
            Console.WriteLine("MK all length: " + allLength);

            //step 2 - detect what 'isRequired' and what 'isNOTRequired' & if it's 'Approved' or 'NOTApproved'
            foreach (var doc in docs)
            {
                if (doc.RequiredDocument.IsRequired)
                {
                    allRequireds++;
                    if (doc.Approved.IsNotNull() && doc.Approved == true)
                    {
                        isRequiredApproved++;
                    }
                }
                else
                {
                    allNotRequired++;
                    if (doc.Approved.IsNotNull() && doc.Approved == true)
                    {
                        notRequiredApproved++;
                    }
                }
            }

            foreach (var task in tasks)
            {
                if (task.RequiredTask.IsRequired)
                {
                    allRequireds++;
                    if (task.Approved.IsNotNull() && task.Approved == true)
                    {
                        isRequiredApproved++;
                    }
                }
                else
                {
                    allNotRequired++;
                    if (task.Approved.IsNotNull() && task.Approved == true)
                    {
                        notRequiredApproved++;
                    }
                }
            }

            foreach (var test in tests)
            {
                if (test.RequiredTest.IsRequired)
                {
                    allRequireds++;
                    if (test.Approved.IsNotNull() && test.Approved == true)
                    {
                        isRequiredApproved++;
                    }
                }
                else
                {
                    allNotRequired++;
                    if (test.Approved.IsNotNull() && test.Approved == true)
                    {
                        notRequiredApproved++;
                    }
                }
            }

            //step 4 - Comparison between 'allRequireds' and 'isRequiredApproved' & 'allNotRequired' and 'notRequiredApproved'
            if (isRequiredApproved < allRequireds)
            {
                try
                {
                    Console.WriteLine("Error -> Red");
                    minimalKitStore.UpdateMkStatus(mkRequest.operationId, "Error");
                    status = "Error";
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else if ((isRequiredApproved == allRequireds) && notRequiredApproved < allNotRequired)
            {
                try
                {
                    Console.WriteLine("Warning -> orange");
                    minimalKitStore.UpdateMkStatus(mkRequest.operationId, "Warning");
                    status = "Warning";
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                try
                {
                    Console.WriteLine("Enabled -> green");
                    minimalKitStore.UpdateMkStatus(mkRequest.operationId, "Enabled");
                    status = "Enabled";
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return status;
        }
    }
}
