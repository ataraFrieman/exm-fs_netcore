CREATE TABLE Equipment (
    Id int NOT NULL IDENTITY(1,583),
	OrganizationId int,
    EqpType varchar(255),
    EqpDescription varchar(255),
    MaximumAmount int,
	PRIMARY KEY (Id)
);

CREATE TABLE AllocationOfEquipment (
    Id int NOT NULL IDENTITY(1,583),
    EqpId int,
	AppointmentId int,
    Amount int,
	BeginTime datetime2(0),
	EndTime datetime2(0),
	PRIMARY KEY (Id),
	CONSTRAINT FK_EquipmentAllocationOfEquipment FOREIGN KEY (EqpId)
    REFERENCES Equipment(Id),
	CONSTRAINT FK_AppointmentAllocationOfEquipment FOREIGN KEY (AppointmentId)
    REFERENCES Appointments(Id),
);
