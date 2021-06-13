using Quze.Infrastruture.Extensions;
using Quze.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Quze.CreateAlerts
{
    public enum FieldTypes
    {
        ServiceProviderName,
        ServiceType,
        FellowName,
        AppointmentDate,
        AppointmentTime,
        OrganizationName,
        AppointmentAddress
    }

    public class SpecialFieldsUtil
    {
        StringBuilder template;
        Appointment appointment;

        public SpecialFieldsUtil(string template, Appointment appointment)
        {
            this.template = new StringBuilder(template);
            this.appointment = appointment;

            string missingData = wrongAppointmentData();
            if (missingData.IsNotNullOrEmpty())
                throw new Exception(missingData);
        }

        private string wrongAppointmentData()
        {
            if (appointment.IsNull())
                return "missing appointment";

            if (appointment.Fellow.IsNull())
                return "missing fellow";

            if (appointment.ServiceQueue.IsNull())
                return "missing service queue";

            if(appointment.ServiceQueue.Branch.IsNull())
                return "missing branch";

            if (appointment.ServiceQueue.ServiceProvider.IsNull())
                return "missing service provider";

            if (appointment.ServiceQueue.Branch.Organization.IsNull())
                return "missing organization";

            return string.Empty;
        }

        public string ReplaceSpecialFields()
        {
            foreach (var field in (FieldTypes[])Enum.GetValues(typeof(FieldTypes)))
                ReplaceField(field);

            return template.ToString();
        }

        private void ReplaceField(FieldTypes field)
        {
            switch (field)
            {
                case FieldTypes.AppointmentAddress:
                    template.Replace(Enum.GetName(typeof(FieldTypes), field), appointment.ServiceQueue.Branch.Address);
                    break;

                case FieldTypes.AppointmentDate:
                    template.Replace(Enum.GetName(typeof(FieldTypes), field), appointment.BeginTime.Date.ToString());
                    break;

                case FieldTypes.AppointmentTime:
                    template.Replace(Enum.GetName(typeof(FieldTypes), field), appointment.BeginTime.TimeOfDay.ToString());
                    break;

                case FieldTypes.FellowName:
                    template.Replace(Enum.GetName(typeof(FieldTypes), field), appointment.Fellow?.FullName);
                    break;

                case FieldTypes.OrganizationName:
                    template.Replace(Enum.GetName(typeof(FieldTypes), field), appointment.ServiceQueue?.Branch?.Organization?.Name);
                    break;

                case FieldTypes.ServiceProviderName:
                    template.Replace(Enum.GetName(typeof(FieldTypes), field), appointment.ServiceQueue?.ServiceProvider?.FullName);
                    break;

                case FieldTypes.ServiceType:
                    template.Replace(Enum.GetName(typeof(FieldTypes), field), appointment.ServiceType?.Description);
                    break;

                default:
                    break;
            }
        }
    }
}
